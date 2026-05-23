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

    public Camera2D() { }

    /// <summary>
    /// Returns the rectangle of world space currently visible by this camera
    /// </summary>
    public Rectangle GetWorldViewRectangle()
    {
      Matrix inverse = Matrix.Invert(GetTransform());

      Vector2 topLeft = Vector2.Transform(Vector2.Zero, inverse);
      Vector2 bottomRight = Vector2.Transform(
          new Vector2(Core.Canvas.RenderTarget.Width, Core.Canvas.RenderTarget.Height),
          inverse
      );

      return new Rectangle(
          (int)topLeft.X,
          (int)topLeft.Y,
          (int)(bottomRight.X - topLeft.X),
          (int)(bottomRight.Y - topLeft.Y)
      );
    }

    /// <summary>
    /// Returns the camera transform matrix for spritebatch.
    /// Centers the camera so <see cref="Node2D.Position"/> maps to the center of the canvas.
    /// </summary>
    public Matrix GetTransform()
    {
      Vector2 canvasCenter = new(
          Core.Canvas.RenderTarget.Width * 0.5f,
          Core.Canvas.RenderTarget.Height * 0.5f
      );

      return
        Matrix.CreateTranslation(
            new Vector3(-(Transform.Global.Position + Offset), 0f)
        )
          * Matrix.CreateRotationZ(Transform.Global.Rotation)
          * Matrix.CreateScale(Zoom.X, Zoom.Y, 1f)
          * Matrix.CreateTranslation(new Vector3(canvasCenter, 0f));
    }

    public override void _Process(float delta)
    {
      base._Process(delta);
    }
  }
}
