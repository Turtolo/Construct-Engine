#nullable enable 

using System;
using System.Collections.Generic;
using System.Linq;
using Amethyst;
using Amethyst.Geometry;
using Amethyst.Graphics;
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
    private List<RenderBucket> _bucketPool = new();
    private List<RenderBucket> _activeBuckets = new();
      
    internal Extent RenderSize { get; set; } = new Extent(640, 360);
    internal bool IntScaling { get; set; } = true;
    internal Rectangle Destination { get; set; }
    internal Color CanvasColor { get; set; } = Color.CornflowerBlue;

    internal Effect? PostProcessingShader { get; set; }

    public RenderTarget2D? RenderTarget { get; internal set; }

    public void Initialize()
    {
      RenderTarget?.Dispose();

      RenderTarget = new RenderTarget2D(
          Core.GraphicsDevice,
          RenderSize.Width,
          RenderSize.Height,
          false,
          SurfaceFormat.Color,
          DepthFormat.None);

      Core.Tracked.Window.ClientSizeChanged += (_, _) => UpdateTransform();
      UpdateTransform();
    }

    public void Submit(IDrawCall call)
    {
      if (call == null) throw new ArgumentNullException(nameof(call));
      _calls.Add(call);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      Core.GraphicsDevice.SetRenderTarget(RenderTarget);
      Core.GraphicsDevice.Clear(CanvasColor);

      Flush(spriteBatch);

      Core.GraphicsDevice.SetRenderTarget(null);
      Core.GraphicsDevice.Clear(Color.Black);

      spriteBatch.Begin(
          SpriteSortMode.Immediate,
          BlendState.AlphaBlend,
          SamplerState.PointClamp,
          effect: PostProcessingShader);

      var dest = new Rectangle(
          (int)MathF.Round(Destination.X),
          (int)MathF.Round(Destination.Y),
          Destination.Width,
          Destination.Height
      );

      spriteBatch.Draw(RenderTarget, dest, Color.White);

      spriteBatch.End();
    }

    public void Flush(SpriteBatch spriteBatch)
    {
      if (_calls.Count == 0)
        return;

      for (int i = 0; i < _calls.Count; i++)
      {
        var call = _calls[i];

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

      _calls.Clear();

      for (int b = 0; b < _activeBuckets.Count; b++)
      {
        var bucket = _activeBuckets[b];
        for (int c = 0; c < bucket.Calls.Count; c++)
        {
          _calls.Add(bucket.Calls[c]);
        }
      }

      Sort(_activeBuckets);
      
      HandleBuckets(spriteBatch, _activeBuckets);
      
      for (int i = 0; i < _calls.Count; i++)
      {
        _calls[i].Reset();
      }
      _calls.Clear();
      
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
