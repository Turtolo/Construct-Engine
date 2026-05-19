using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amethyst.Managers;
using Amethyst.Params;

namespace Amethyst.Hierarchy
{
  public class Camera2D : Node2D
  {
    [Export]
    public Vector2 Zoom { get; set; } = Vector2.One;

    [Export]
    public Rectangle Bounds
    {
      get
      {
        float width = Core.Canvas.RenderTarget.Width / Zoom.X;
        float height = Core.Canvas.RenderTarget.Height / Zoom.Y;

        float left = Transform.Global.Position.X - width * 0.5f;
        float top = Transform.Global.Position.Y - height * 0.5f;

        return new Rectangle(
            (int)left,
            (int)top,
            (int)width,
            (int)height
        );
      }
    }

    [Export]
    public Vector2 Offset { get; set; } = Vector2.Zero;

    public Camera2D()
    {
      Core.Canvas.SetMatrix(GetTransform());
    }

    /// <summary>
    /// Returns the camera transform matrix for spritebatch.
    /// Centers the camera so <see cref="Node2D.Position"/> maps to the center of the canvas.
    /// </summary>
    public Matrix GetTransform()
    {
      Vector2 canvasCenter =
        new Vector2(
            Core.Canvas.RenderTarget.Width / Zoom.X,
            Core.Canvas.RenderTarget.Height / Zoom.Y
        ) * 0.5f;

      Matrix transform =
          Matrix.CreateScale(Zoom.X, Zoom.Y, 1f)
          * Matrix.CreateRotationZ(Transform.Global.Rotation)
          * Matrix.CreateTranslation(
              new Vector3(-(Transform.Global.Position + Offset), 0f)
          )
          * Matrix.CreateTranslation(new Vector3(canvasCenter, 0f));

      return transform;
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      Core.Canvas.SetMatrix(GetTransform());
    }
  }
}
