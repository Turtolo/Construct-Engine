using Amethyst.Managers;
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Amethyst.Geometry;
using Amethyst.Managers;

namespace Amethyst.Hierarchy
{
  public class StaticBody2D : PhysicsBody2D
  {
    public StaticBody2D() { }

    public override void _EnterTree()
    {
      base._EnterTree();
    }

    public override void _PhysicsUpdate(float delta)
    {
      base._PhysicsUpdate(delta);
    }

  }

}
