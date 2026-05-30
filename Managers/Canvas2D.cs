#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Amethyst;
using Amethyst.Geometry;
using Amethyst.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.Managers
{
  public class Canvas2D : BaseObject
  {
    private Dictionary<RenderBucketKey, List<IDrawCall>> _buckets = new();

    internal Extent RenderSize { get; set; } = new Extent(640, 360);
    internal bool IntScaling { get; set; } = true;
    internal Rectangle Destination { get; set; }
    internal Color CanvasColor { get; set; } = Color.CornflowerBlue;

    internal Effect PostProcessingShader { get; set; }

    public RenderTarget2D RenderTarget { get; internal set; }

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

      var key = new RenderBucketKey(call.Depth, call.Key, call.Effect);

      if (!_buckets.TryGetValue(key, out var list))
      {
        list = new List<IDrawCall>();
        _buckets[key] = list;
      }

      list.Add(call);
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
      if (_buckets.Count == 0)
        return;

      foreach (var kvp in _buckets
          .OrderBy(x => x.Key.Depth))
      {
        DrawBucket(spriteBatch, kvp.Value);
      }

      _buckets.Clear();
    }

    private void DrawBucket(SpriteBatch spriteBatch, List<IDrawCall> bucket)
    {
      BatchKey? currentKey = null;
      Effect? currentEffect = null;

      for (int i = 0; i < bucket.Count; i++)
      {
        var call = bucket[i];

        var nextKey = call.Key;
        var nextEffect = call.Effect ?? null;

        bool stateChanged =
            currentKey == null ||
            !currentKey.Value.Equals(nextKey) ||
            currentEffect != nextEffect;

        if (stateChanged)
        {
          if (currentKey != null)
            spriteBatch.End();

          spriteBatch.Begin(
              nextKey.SortMode,
              nextKey.BlendState,
              nextKey.SamplerState,
              nextKey.DepthStencilState,
              nextKey.RasterizerState,
              nextEffect,
              nextKey.Matrix
          );

          currentKey = nextKey;
          currentEffect = nextEffect;
        }

        call.Draw(spriteBatch);
      }

      spriteBatch.End();
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
