#nullable disable

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Amethyst.Geometry;
using Amethyst.Hierarchy;

namespace Amethyst.Managers
{
  public class PhysicsServer2D : BaseObject
  {
    private readonly SpatialHash<PhysicsBody2D> _broadphase;

    public PhysicsServer2D()
    {
      _broadphase = new SpatialHash<PhysicsBody2D>(16);
    }

    /// <summary>
    /// Registers a physics body to the server.
    /// </summary>
    public void RegisterBody(PhysicsBody2D body)
    {
      _broadphase.Insert(body);
    }

    /// <summary>
    /// Removes a physics body from the server.
    /// </summary>
    public void UnregisterBody(PhysicsBody2D body)
    {
      _broadphase.Remove(body);
    }

    /// <summary>
    /// Notify the broadphase that a body moved.
    /// </summary>
    public void NotifyMoved(PhysicsBody2D body)
    {
      _broadphase.Update(body);
    }

    /// <summary>
    /// Queries all bodies intersecting the given area.
    /// </summary>
    public List<PhysicsBody2D> Query(List<Rectangle> area)
    {
      return _broadphase.Query(area.ToArray());
    }
  }
}
