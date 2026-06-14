#nullable enable 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Amethyst;
using Amethyst.Geometry;
using Amethyst.Graphics;
using Amethyst.Hierarchy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amethyst.Managers
{
  public class Canvas2D : BaseObject
  {
    private class RenderBucket
    {
      public int Depth;
      public BatchKey Key;
      public Effect? Effect;
      public List<IDrawCall> Calls = new List<IDrawCall>();
    }

    private List<IDrawCall> _calls = new();
    private List<IDrawCall> _unLitCalls = new();
    private List<IDrawCall> _lightCalls = new();

    private List<RenderBucket> _bucketPool = new();
    private List<RenderBucket> _activeBuckets = new();

    internal Extent RenderSize { get; set; } = new Extent(640, 360);
    internal bool IntScaling { get; set; } = true;
    internal Rectangle Destination { get; set; }
    
    internal Color AmbientColor { get; set; } = Color.Gray;

    internal Color CanvasColor { get; set; } = Color.CornflowerBlue;

    internal Effect? LightingShader { get; set; }

    internal Effect? PostProcessingShader { get; set; }

    public RenderTarget2D? RenderTarget { get; internal set; }
    public RenderTarget2D? LightRenderTarget { get; internal set; }
    public RenderTarget2D? UnlitRenderTarget { get; internal set; }

    public static readonly BlendState MultiplicativeBlendState = new BlendState
    {
        ColorSourceBlend = Blend.Zero,
        ColorDestinationBlend = Blend.SourceColor,
        AlphaSourceBlend = Blend.Zero,
        AlphaDestinationBlend = Blend.SourceAlpha
    };

    public void Initialize()
    {
      var assembly = typeof(Core).Assembly;
      
      string resourceName = $"Amethyst.Compiled.Lighting.mgfxo";

      using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
      {
        if (stream == null)
          throw new Exception($"Could not find embedded shader resource: {resourceName}");

        using (MemoryStream ms = new MemoryStream())
        {
          stream.CopyTo(ms);
          byte[] shaderCode = ms.ToArray();

          LightingShader = new Effect(Core.GraphicsDevice, shaderCode);
        }
      }

      RenderTarget?.Dispose();
      LightRenderTarget?.Dispose();

      RenderTarget = new RenderTarget2D(
          Core.GraphicsDevice,
          RenderSize.Width,
          RenderSize.Height,
          false,
          SurfaceFormat.Color,
          DepthFormat.None,
          0,
          RenderTargetUsage.PreserveContents);

      LightRenderTarget = new RenderTarget2D(
          Core.GraphicsDevice,
          RenderSize.Width,
          RenderSize.Height,
          false,
          SurfaceFormat.Color,
          DepthFormat.None,
          0,
          RenderTargetUsage.PreserveContents);

      UnlitRenderTarget = new RenderTarget2D(
          Core.GraphicsDevice,
          RenderSize.Width,
          RenderSize.Height,
          false,
          SurfaceFormat.Color,
          DepthFormat.None,
          0,
          RenderTargetUsage.PreserveContents);

      Core.Tracked.Window.ClientSizeChanged += (_, _) => UpdateTransform();
      UpdateTransform();
    }

    public void Submit(IDrawCall call)
    {
      if (call == null) throw new ArgumentNullException(nameof(call));

      _calls.Add(call);
    }

    public void SubmitLight(IDrawCall call)
    {
      if (call == null) throw new ArgumentNullException(nameof(call));
      _lightCalls.Add(call);
    }

    public void SubmitUnLit(IDrawCall call)
    {
      if (call == null) throw new ArgumentNullException(nameof(call));
      _unLitCalls.Add(call);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (LightingShader == null) throw new Exception("LightingShader is null.");

      Core.GraphicsDevice.SetRenderTarget(RenderTarget);
      Core.GraphicsDevice.Clear(Color.Black);
      Flush(spriteBatch, _calls);

      Core.GraphicsDevice.SetRenderTarget(LightRenderTarget);
      Core.GraphicsDevice.Clear(Color.Black);
      Flush(spriteBatch, _lightCalls);

      Core.GraphicsDevice.SetRenderTarget(RenderTarget);
      
      LightingShader.Parameters["MaskTexture"].SetValue(LightRenderTarget);
      spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, effect: LightingShader);
      spriteBatch.Draw(RenderTarget, Vector2.Zero, Color.White); 
      spriteBatch.End();

      Flush(spriteBatch, _unLitCalls);

      Core.GraphicsDevice.SetRenderTarget(null);
      Core.GraphicsDevice.Clear(Color.Black);
      
      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
      spriteBatch.Draw(RenderTarget, Destination, Color.White);
      spriteBatch.End();
    }
    
    public void Flush(SpriteBatch spriteBatch, List<IDrawCall> source)
    {
      if (source.Count == 0)
        return;

      for (int i = 0; i < source.Count; i++)
      {
        var call = source[i];

        RenderBucket? targetBucket = null;

        for (int j = 0; j < _activeBuckets.Count; j++)
        {
          var bucket = _activeBuckets[j];
          if (bucket.Depth == call.Depth &&
              bucket.Key.Equals(call.Key) &&
              ReferenceEquals(bucket.Effect, call.Effect))
          {
            targetBucket = bucket;
            break;
          }
        }

        if (targetBucket == null)
        {
          if (_bucketPool.Count > 0)
          {
            int lastIndex = _bucketPool.Count - 1;
            targetBucket = _bucketPool[lastIndex];
            _bucketPool.RemoveAt(lastIndex);
          }
          else
          {
            targetBucket = new RenderBucket();
          }

          targetBucket.Depth = call.Depth;
          targetBucket.Key = call.Key;
          targetBucket.Effect = call.Effect;
          targetBucket.Calls.Clear();

          _activeBuckets.Add(targetBucket);
        }

        targetBucket.Calls.Add(call);
      }

      _activeBuckets.Sort((a, b) => a.Depth.CompareTo(b.Depth));

      source.Clear();

      for (int b = 0; b < _activeBuckets.Count; b++)
      {
        var bucket = _activeBuckets[b];
        for (int c = 0; c < bucket.Calls.Count; c++)
        {
          source.Add(bucket.Calls[c]);
        }
      }

      Sort(_activeBuckets);

      HandleBuckets(spriteBatch, _activeBuckets);

      for (int i = 0; i < source.Count; i++)
      {
        source[i].Reset();
      }
      source.Clear();

      for (int b = 0; b < _activeBuckets.Count; b++)
      {
        _activeBuckets[b].Calls.Clear();
        _bucketPool.Add(_activeBuckets[b]);
      }
      _activeBuckets.Clear();
    }

    private void Sort(List<RenderBucket> list)
    {
      int count = list.Count;
      for (int i = 1; i < count; i++)
      {
        RenderBucket keyItem = list[i];
        int j = i - 1;

        while (j >= 0 && list[j].Depth > keyItem.Depth)
        {
          list[j + 1] = list[j];
          j--;
        }
        list[j + 1] = keyItem;
      }
    }

    private void HandleBuckets(SpriteBatch spriteBatch, List<RenderBucket> buckets)
    {
      for (int b = 0; b < _activeBuckets.Count; b++)
      {
        var bucket = _activeBuckets[b];
        var key = bucket.Key;

        spriteBatch.Begin(
            key.SortMode,
            key.BlendState,
            key.SamplerState,
            key.DepthStencilState,
            key.RasterizerState,
            bucket.Effect,
            key.Matrix);

        for (int c = 0; c < bucket.Calls.Count; c++)
        {
          bucket.Calls[c].Draw(spriteBatch);
        }

        spriteBatch.End();
      }
    }

    internal void UpdateTransform()
    {
      var pp = Core.GraphicsDevice.PresentationParameters;

      float scale = Math.Min(
          pp.BackBufferWidth / (float)RenderSize.Width,
          pp.BackBufferHeight / (float)RenderSize.Height);

      if (IntScaling)
        scale = Math.Max(1, MathF.Floor(scale));

      int w = (int)(RenderSize.Width * scale);
      int h = (int)(RenderSize.Height * scale);

      int x = (pp.BackBufferWidth - w) / 2;
      int y = (pp.BackBufferHeight - h) / 2;

      Destination = new Rectangle(x, y, w, h);
    }
  }
}
