using System;
using System.Collections.Generic;
using Amethyst;
using Amethyst.Geometry;
using Amethyst.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monolith.Managers
{
  public class Canvas2D : BaseObject
  {
    private List<IDrawCall> _queue = new();

    private Matrix _currentMatrix;

    internal Extent RenderSize { get; set; } = new Extent(640, 360);
    internal bool IntScaling { get; set; } = true;
    internal Rectangle Destination { get; set; }
    internal Color CanvasColor { get; set; } = Color.CornflowerBlue;

    internal Effect PostProcessingShader { get; set; }

    public RenderTarget2D RenderTarget { get; internal set; }

    private int _beginCount;
    private int _endCount;
    private int _drawCount;
    private BatchKey? _lastKey;

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
      _queue.Add(call);
    }

    public void SetMatrix(Matrix matrix)
    {
      _currentMatrix = matrix;
    }

    /// <summary>
    /// Returns the rectangle of world space currently visible by this camera
    /// </summary>
    public Rectangle GetWorldViewRectangle()
    {
      Matrix inverse = Matrix.Invert(_currentMatrix);

      Vector2 topLeft = Vector2.Transform(Vector2.Zero, inverse);
      Vector2 bottomRight = Vector2.Transform(
          new Vector2(RenderTarget.Width, RenderTarget.Height),
          inverse
      );

      return new Rectangle(
          (int)topLeft.X,
          (int)topLeft.Y,
          (int)(bottomRight.X - topLeft.X),
          (int)(bottomRight.Y - topLeft.Y)
      );
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      _beginCount = 0;
    _endCount = 0;
      _drawCount = 0;
      _lastKey = null;

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

      spriteBatch.Draw(RenderTarget, Destination, Color.White);

      spriteBatch.End();

      Console.WriteLine(
        $"[Canvas2D] Batches={_beginCount}, Ends={_endCount}, Draws={_drawCount}, Queue={_queue.Count}");

      _queue.Clear();
    }

    public void Flush(SpriteBatch spriteBatch)
    {
      if (_queue.Count == 0)
        return;

      BatchKey? currentKey = null;

      for (int i = 0; i < _queue.Count; i++)
      {
        var call = _queue[i];
        var key = call.Key;

        if (!_lastKey.HasValue || !_lastKey.Value.Equals(key))
        {
          Console.WriteLine($"[Canvas2D] BatchKey switch at {i}: {key.SortMode}");

          _lastKey = key;
        }

        if (currentKey == null || !currentKey.Value.Equals(key))
        {
          if (currentKey != null)
          {
            spriteBatch.End();
            _endCount++;
          }

          spriteBatch.Begin(
              key.SortMode,
              key.BlendState,
              key.SamplerState,
              key.DepthStencilState,
              key.RasterizerState,
              key.Effect,
              _currentMatrix
          );

          _beginCount++;
          currentKey = key;
        }

        _drawCount++;
        call.Draw(spriteBatch);
      }

      spriteBatch.End();
      _endCount++;

      _queue.Clear();
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
