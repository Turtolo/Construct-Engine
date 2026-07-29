using Opal.Managers;

#nullable disable

using System;
using Microsoft.Xna.Framework;
using Opal.Params;
using Opal.Tools;

namespace Opal.Hierarchy
{
  public class DynamicBody2D : PhysicsBody2D
  {
    [Export]
    public Vector2 Velocity;

    public DynamicBody2D() { }

    public override void _EnterTree()
    {
      base._EnterTree();
    }

    public override void _PhysicsUpdate(float delta)
    {
      base._PhysicsUpdate(delta);

      Position += Velocity * delta;
    }

    public override void _Process(float delta)
    {
      base._Process(delta);
    }

    public override void _Submit(Canvas2D canvas)
    {
      base._Submit(canvas);
    }

    public override void _ExitTree()
    {
      base._EnterTree();
    }
  }
}
