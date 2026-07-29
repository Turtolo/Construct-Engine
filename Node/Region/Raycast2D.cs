using System;
using Opal.Geometry;
using Opal.Graphics;
using Opal.Managers;
using Opal.Params;
using Opal.Tools;
using Microsoft.Xna.Framework;

namespace Opal.Hierarchy
{
  public class Raycast2D : Node2D
  {
    [Export]
    public bool Disabled { get; set; } = false;
    
    [Export]
    public bool ExcludeParent { get; set; } = true;
    
    ///<summary>
    /// The point where the ray ends, think of it as an arrow from the origin to said point.
    ///</summary>
    [Export]
    public Vector2 TargetPosition
    {
      get => Shape.TargetPosition;
      set => Shape.TargetPosition = value;
    }

    ///<summary>
    /// The internal raycast, contains no position logic – that is handled within <see cref="IsColliding()"/>.
    ///</summary>
    [Export]
    public RayCastShape2D Shape { get; set; }

    public Raycast2D() {}

    public bool IsColliding(out Vector2 hitPoint, out float distance)
    {
      hitPoint = Vector2.Zero;
      distance = 0f;

      if (Disabled || Shape == null)
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

        if (GetParent() == body && ExcludeParent)
          continue;
        
        for (int j = 0; j < body.CollisionShapes.Count; j++)
        {
          var shape = body.CollisionShapes[j];

          if (Shape.CheckIntersections(shape.Shape, Transform.Global.Position, shape.Transform.Global.Position, out hitPoint, out distance))
            return true;
        }
      }

      return false;
    }

    public bool IsColliding()
    {
      return IsColliding(out _, out _);
    }

    public override void _Submit(Canvas2D canvas)
    {
      if (!Core.Prefs.General.ShowCollision || Shape == null)
        return;

      var colliding = IsColliding(out Vector2 hitPoint, out float distance);
  
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

      initial.Texture = Core.Resources.Pixel;

      initial.Depth = depth;

      initial.Key = BatchKey.Default with
      {
        Matrix = Core.Token.Get<Camera2D>().GetTransform()
      };

      Core.Canvas.Submit(initial);
    }
  }
}
