using System;
using Amethyst.Geometry;
using Amethyst.Graphics;
using Amethyst.Hierarchy;
using Amethyst.Managers;
using Amethyst.Params;
using Amethyst.Util;
using Microsoft.Xna.Framework;

namespace Amethyst.Hierarch
{
  public class Raycast2D : Node2D
  {
    [Export]
    public bool Disabled { get; set; }
    
    ///<summary>
    /// The point where the ray ends, think of it as an arrow from the origin to said pont.
    ///</summary>
    public Vector2 TargetPosition { get; set; }
    
    public RayCastShape2D Shape { get; set; } 

    public Raycast2D() {}


    public bool IsColliding(out Vector2 hitPoint, out float distance)
    {
      hitPoint = Vector2.Zero;
      distance = 0f;

      if (Disabled)
        return false;
      
      Rectangle bounds = new Rectangle(
        (int)Transform.Global.Position.X,
        (int)Transform.Global.Position.Y,
        (int)TargetPosition.X,
        (int)TargetPosition.Y);

      var bodies = Core.Physics.Query([bounds]);

      for (int i = 0; i < bodies.Count; i++)
      {
        var body = bodies[i]; 
        
        for (int j = 0; j < body.CollisionShapes.Count; j++)
        {
          var shape = body.CollisionShapes[j];

          if (Shape.CheckIntersections(shape.Shape, body.Transform.Global.Position, out hitPoint, out distance))
            return true;
        }
      }

      return false;
    }

    public override void _Submit(Canvas2D canvas)
    {
      if (!Core.Prefs.General.ShowCollision)
        return;

      var colliding = IsColliding(out Vector2 hitPoint, out float distance);;
  
      Color color = colliding ? Color.Red : Color.Yellow;
      int depth = 99;
      int thickness = 2;

      TextureDrawCall initial = ObjectPool<TextureDrawCall>.Get();

      initial.Params = CanvasParams.Identity with
      {
        Position = Transform.Global.Position,
        Color = color,
        Scale = new Vector2(Shape.Length, thickness),
        Rotation = MathF.Atan2(Shape.Direction.Y, Shape.Direction.X),
        Origin = new Vector2(0f, 0.5f)
      };

      initial.Texture = Core.Pixel;

      initial.Depth = depth;

      initial.Key = BatchKey.Default with
      {
        Matrix = Core.Token.Get<Camera2D>().GetTransform()
      };

      Core.Canvas.Submit(initial);

      if (colliding)
      {
        TextureDrawCall secondary = ObjectPool<TextureDrawCall>.Get();

        secondary.Params = CanvasParams.Identity with
        {
          Position = hitPoint,
          Color = Color.Blue,
          Scale = new Vector2(Shape.Length, thickness),
          Rotation = MathF.Atan2(Shape.Direction.Y, Shape.Direction.X),
          Origin = new Vector2(0f, 0.5f)
        };

        secondary.Texture = Core.Pixel;

        secondary.Depth = depth + 1;

        secondary.Key = BatchKey.Default with
        {
          Matrix = Core.Token.Get<Camera2D>().GetTransform()
        };
        
        Core.Canvas.Submit(secondary);
      }
    }

  }
}
