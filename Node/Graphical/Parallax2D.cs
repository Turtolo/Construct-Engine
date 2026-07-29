using System;
using Opal.Geometry;
using Opal.Graphics;
using Opal.Managers;
using Opal.Params;
using Opal.Tools;
using Opal.Tools;
using Microsoft.Xna.Framework;

namespace Opal.Hierarchy
{
  public class Parallax2D : Node2D
  {
    public MTexture Texture { get; set; }

    public int RepeatTimes { get; set; } = 1;

    public Vector2 MotionScale { get; set; }

    public Extent RepeatSize { get; set; }

    private Vector2 cameraPos;
    private Rectangle cameraRect;

    public override void _Process(float delta)
    {
      base._Process(delta);
      
      var cam = Core.Token.Get<Camera2D>();

      cameraRect = cam.GetWorldViewRectangle();
      cameraPos = new Vector2(cam.Transform.Global.Position.X, cam.Transform.Global.Position.Y);
    }

    public override void _Submit(Canvas2D canvas)
    {
      base._Submit(canvas);

      Color finalColor = ColorExtension.Multiply(Material.Global.SelfModulate, Material.Global.Modulate);
      
      Vector2 pos = Rounded ? Vector2.Floor(Transform.Global.Position) : Transform.Global.Position;
      Vector2 scale = Rounded ? Vector2.Floor(Transform.Global.Scale) : Transform.Global.Scale;

      Vector2 viewportSize = new Vector2(cameraRect.Width, cameraRect.Height);      
      Vector2 layerOffset = cameraPos * MotionScale;
      
      float startX, endX, stepX;
      if (RepeatSize.Width <= 0)
      {
        startX = pos.X - layerOffset.X;
        endX = startX + 1;
        stepX = 1f;
      }
      else
      {
        float repeatLeft = (float)MathF.Floor(RepeatTimes / 2f);
        float repeatRight = (float)MathF.Ceiling(RepeatTimes / 2f);

        var baseStartX = -layerOffset.X % RepeatSize.Width;
        if (baseStartX > 0)
          baseStartX -= RepeatSize.Width;
        baseStartX += pos.X;

        startX = baseStartX - (repeatLeft * viewportSize.X);
        endX = baseStartX + (repeatRight * viewportSize.X);
        stepX = RepeatSize.Width;
      }

      float startY, endY, stepY;
      if (RepeatSize.Height <= 0)
      {
        startY = pos.Y - layerOffset.Y;
        endY = startY + 1;
        stepY = 1f;
      }
      else
      {

        float repeatUp = (float)MathF.Floor(RepeatTimes / 2f);
        float repeatDown = (float)MathF.Ceiling(RepeatTimes / 2f);

        var baseStartY = -layerOffset.Y % RepeatSize.Height;
        if (baseStartY > 0)
          baseStartY -= RepeatSize.Height;
        baseStartY += pos.Y;
        
        startY = baseStartY - (repeatUp * viewportSize.Y);
        endY = baseStartY + (repeatDown * viewportSize.Y);
        stepY = RepeatSize.Height;
      }

      for (float x = startX; x < endX; x += stepX)
      {
        for (float y = startY; y < endY; y += stepY)
        {
          TextureDrawCall call = ObjectPool<TextureDrawCall>.Get();
          
          call.Texture = Texture;
          call.Effect = Material.Global.Shader;
          call.Depth = Ordering.Global.Depth;

          call.Params = CanvasParams.Identity with
          {
            Position = new Vector2(x, y),
            Color = finalColor,
            Rotation = Transform.Global.Rotation,
            Origin = new Vector2(Texture.Bounds.Width / 2f, Texture.Bounds.Height / 2f),
            Scale = scale,
            Effects = Material.Global.SpriteEffects,
          };

          call.Key = BatchKey.Default with
          {
            Matrix = Seperated ? null : Core.Token.Get<Camera2D>()?.GetTransform(),
          };

          canvas.Submit(call);
        }
      }
    }
  }
}
