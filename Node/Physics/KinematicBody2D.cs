using Opal.Managers;
#nullable disable

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Opal.Params;
using System.Collections.Generic;

namespace Opal.Hierarchy
{
  public class KinematicBody2D : PhysicsBody2D
  {
    [Export]
    public Vector2 Velocity = Vector2.Zero;

    [Export]
    public Vector2 WallNormal { get; private set; } = Vector2.Zero;

    [Export]
    public bool IsOnFloor => _isOnFloor;

    [Export]
    public bool IsOnWall => _isOnWall;

    [Export]
    public bool IsOnRoof => _isOnRoof;

    public CollisionNode2D FloorBody { get; private set; }
    public CollisionNode2D WallBody { get; private set; }

    private Vector2 _lastWallGlobalPosition;
    private Vector2 _lastFloorGlobalPosition;

    private bool _isOnWall = false;
    private bool _isOnFloor = false;
    private bool _isOnRoof = false;

    const float FLOOR_TOLERANCE = 2f;
    const float WALL_TOLERANCE = 2f;

    public KinematicBody2D() { }

    public void MoveAndSlide(float delta)
    {
      if (CollisionShapes.Count == 0)
        return;

      Vector2 movement = Velocity * delta;

      ResolvePlatforms();

      var nearby = Core.Physics.Query(Bounds);

      ResolveStaticPenetration(nearby);

      _isOnFloor = false;
      _isOnRoof = false;
      _isOnWall = false;
      WallNormal = Vector2.Zero;
      FloorBody = null;
      WallBody = null;

      ResolveHorizontal(ref movement, nearby);

      ResolveVertical(ref movement, nearby);
    }

    private void ResolvePlatforms()
    {
      if (_isOnFloor && FloorBody != null)
      {
        Vector2 platformDelta = FloorBody.Transform.Global.Position - _lastFloorGlobalPosition;
        Position += platformDelta;
        _lastFloorGlobalPosition = FloorBody.Transform.Global.Position;
      }

      if (_isOnWall && WallBody != null)
      {
        Vector2 wallDelta = WallBody.Transform.Global.Position - _lastWallGlobalPosition;
        Position += wallDelta;
        _lastWallGlobalPosition = WallBody.Transform.Global.Position;
      }
    }

    private void ResolveHorizontal(ref Vector2 movement, List<PhysicsBody2D> nearby)
    {
      Vector2 horizontalMovement = new Vector2(movement.X, 0);
      Position += horizontalMovement;

      foreach (var other in nearby.Where(b => b != this))
      {
        if (this.Intersects(other))
        {
          _isOnWall = true;
          WallBody = other;
          _lastWallGlobalPosition = other.Transform.Global.Position;

          WallNormal = movement.X > 0 ? new Vector2(-1, 0) : new Vector2(1, 0);

          Position -= horizontalMovement;
          Velocity = new Vector2(0, Velocity.Y);
          break;
        }

        bool nearWall = false;
        Vector2 nearWallNormal = Vector2.Zero;

        if (this.IntersectsAt(new Vector2(WALL_TOLERANCE, 0), other))
        {
          nearWall = true;
          nearWallNormal = new Vector2(-1, 0);
        }
        else if (this.IntersectsAt(new Vector2(-WALL_TOLERANCE, 0), other))
        {
          nearWall = true;
          nearWallNormal = new Vector2(1, 0);
        }

        if (nearWall && movement.X != 0)
        {
          _isOnWall = true;
          WallNormal = nearWallNormal;
          break;
        }
      }
    }

    private void ResolveVertical(ref Vector2 movement, List<PhysicsBody2D> nearby)
    {
      Vector2 verticalMovement = new Vector2(0, movement.Y);
      Position += verticalMovement;

      foreach (var other in nearby.Where(b => b != this))
      {
        if (this.Intersects(other))
        {
          if (movement.Y > 0)
          {
            ResolveVerticalPenetration(other, true);

            _isOnFloor = true;
            FloorBody = other;
            _lastFloorGlobalPosition = other.Transform.Global.Position;
          }
          else if (movement.Y < 0)
          {
            ResolveVerticalPenetration(other, false);
            _isOnRoof = true;
          }

          Velocity = new Vector2(Velocity.X, 0);
          break;
        }

        if (movement.Y >= 0 && this.IntersectsAt(new Vector2(0, FLOOR_TOLERANCE), other))
        {
          _isOnFloor = true;
          FloorBody = other;
          _lastFloorGlobalPosition = other.Transform.Global.Position;
          break;
        }
      }
    }

    private void ResolveVerticalPenetration(PhysicsBody2D other, bool fromTop)
    {
      foreach (var a in this.Bounds)
        foreach (var b in other.Bounds)
        {
        if (!a.Intersects(b)) continue;

        if (fromTop)
        {
          float penetration = (a.Bottom - b.Top);
          Position -= new Vector2(0, penetration);
        }
        else
        {
          float penetration = (b.Bottom - a.Top);
          Position += new Vector2(0, penetration);
        }
        }
    }

    private void ResolveStaticPenetration(List<PhysicsBody2D> nearby)
    {
      foreach (var other in nearby.Where(b => b != this))
      {
        if (!this.Intersects(other)) continue;

        foreach (var a in this.Bounds)
          foreach (var b in other.Bounds)
          {
            if (!a.Intersects(b)) continue;

            float moveRight = b.Right - a.Left;
            float moveLeft = a.Right - b.Left;
            float moveDown = b.Bottom - a.Top;
            float moveUp = a.Bottom - b.Top;

            float minX = Math.Min(moveRight, moveLeft);
            float minY = Math.Min(moveDown, moveUp);

            if (minX < minY)
            {
              Position += new Vector2(
                  moveRight < moveLeft ? moveRight : -moveLeft,
                  0);
            }
            else
            {
              Position += new Vector2(
                  0,
                  moveDown < moveUp ? moveDown : -moveUp);
            }
          }
      }
    }

    public void ApplyImpulse(Vector2 impulse)
    {
      Velocity += impulse;
    }
  }
}
