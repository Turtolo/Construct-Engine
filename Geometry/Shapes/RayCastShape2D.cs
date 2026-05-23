using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Amethyst.Graphics;
using Amethyst.Managers;
using Amethyst.Params;
using Amethyst.Hierarchy;

namespace Amethyst.Geometry
{
  public class RayCastShape2D
  {
    public Vector2 Origin { get; set; }
    public Vector2 TargetOffset { get; set; }

    public Vector2 Direction =>
        TargetOffset == Vector2.Zero
            ? Vector2.Zero
            : Vector2.Normalize(TargetOffset);

    public float Length =>
        TargetOffset.Length();

    public bool _hasHit;
    private Vector2 _hitPoint;

    public bool HasHit => _hasHit;
    public Vector2 HitPoint => _hitPoint;

    public bool CheckIntersections(IEnumerable<(IShape2D shape, Vector2 position)> shapes)
    {
      _hasHit = false;
      float closest = float.MaxValue;

      foreach (var (shape, position) in shapes)
      {
        if (shape.RayIntersect(
            Origin,
            Direction,
            Length,
            position,
            out Vector2 hit,
            out float distance))
        {
          if (distance < closest)
          {
            closest = distance;
            _hasHit = true;
            _hitPoint = hit;
          }
        }
      }

      return _hasHit;
    }

    public void Draw()
    {
      Color color = HasHit ? Color.Yellow : Color.Red;
      int depth = 99;
      int thickness = 2;

      Core.Canvas.Submit(
          new TextureDrawCall
          {
            Params = CanvasParams.Identity with
            {
              Position = Origin,
              Color = color,
              Scale = new Vector2(Length, thickness),
              Rotation = MathF.Atan2(Direction.Y, Direction.X),
              Origin = new Vector2(0f, 0.5f)
            },
            Texture = Core.Pixel,
            Depth = depth,
            Key = BatchKey.Default with
            {
              Matrix = Core.Index.Get<Camera2D>().GetTransform()
            }
          }
      );

      if (HasHit)
      {
        Core.Canvas.Submit(
            new TextureDrawCall
            {
              Params = CanvasParams.Identity with
              {
                Position = HitPoint,
                Color = Color.Blue,
                Scale = new Vector2(4, 4)
              },
              Texture = Core.Pixel,
              Depth = depth + 1
            }
        );
      }
    }

  }
}
