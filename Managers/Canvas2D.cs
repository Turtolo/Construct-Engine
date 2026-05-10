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
    private List<IDrawCall> _queue;

    private Matrix _currentMatrix;

    internal Extent RenderSize { get; set; } = new Extent(640, 360);
    internal bool IntScaling { get; set; } = true;
    internal Rectangle Destination { get; set; }
    internal Color CanvasColor { get; set; } = Color.CornflowerBlue;

    internal Effect PostProcessingShader { get; set; }

    public RenderTarget2D RenderTarget { get; internal set; }
    
    public void Submit(IDrawCall call)
    {
      if (call == null) throw new ArgumentNullException(nameof(call));
      _queue.Add(call);
    }

    public void SetMatrix(Matrix matrix)
    {
      _currentMatrix = matrix;
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

      spriteBatch.Draw(RenderTarget, Destination, Color.White);

      spriteBatch.End();
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

        if (currentKey == null || currentKey.Value.Equals(key))
        {
          if (currentKey != null)
            spriteBatch.End();

          spriteBatch.Begin(
              key.SortMode,
              key.BlendState,
              key.SamplerState,
              key.DepthStencilState,
              key.RasterizerState,
              key.Effect,
              _currentMatrix
          );

          currentKey = key;
        }

        call.Draw(spriteBatch);
      }

      spriteBatch.End();

      _queue.Clear();
    } 
  }
}
