using Opal.Managers;

using System;
using System.Collections.Generic;
using System.IO.Compression;
using Microsoft.Xna.Framework;
using Opal.Geometry;
using Opal.Params;
using Opal.Tools;
using System.Linq;

namespace Opal.Hierarchy
{

  public class CollisionNode2D : Node2D
  {
    public List<CollisionShape2D> CollisionShapes { get => GetAll<CollisionShape2D>().ToList(); }

    ///<summary>
    /// The max layer.
    ///</summary>
    [Export]
    public int MaxLayer { get; set; } = 30;

    ///<summary>
    /// The layers for this collision node; used while checking intersection.
    /// If shapes share one or more layers they can intersect, zero layered shapes intersect with eachother.
    ///</summary>
    [Export]
    public HashSet<int> Layers { get; set; } = new();

    ///<summary>
    /// The bounds of this node's shapes, represented in the form of a rectangle.
    /// e.g a circle's bounds are the top, bottom, left and right.
    ///</summary>
    [Export]
    public List<Rectangle> Bounds
    {
      get
      {
        if (CollisionShapes.Count == 0)
          return [Rectangle.Empty];

        var b = new List<Rectangle>();

        foreach (var c in CollisionShapes)
          b.Add(c.Shape.GetAABB(c.Transform.Global.Position.ToPoint()));

        return b;
      }
    }

    public CollisionNode2D() { }

    ///<summary>
    /// Adds a layer which will be used for checking intersection.
    ///</summary>
    ///<param name="layer">The layer, if it surpases the max value (default: 30); it will be clamped down.</param>
    public int AddLayer(int layer)
    {
      var finVal = Math.Clamp(layer, 0, MaxLayer);
      Layers.Add(finVal);

      return finVal;
    }

    ///<summary>
    /// Adds layers which will be used for checking intersection.
    ///</summary>
    ///<param name="layers">The layers, if they surpass the max value (default: 30); they will be clamped down.</param>
    public int[] AddLayers(params int[] layers)
    {
      var finVals = layers.ClampArray(0, MaxLayer);
      foreach (var l in finVals)
        Layers.Add(l);

      return finVals;
    }

    ///<summary>
    /// Removes a layer.
    ///</summary>
    ///<param name="layer">The layer in question, if it surpasses the max value (default: 30), it will be clamped down</param>
    public int RemoveLayer(int layer)
    {
      var finVal = Math.Clamp(layer, 0, MaxLayer);
      Layers.Remove(finVal);

      return finVal;
    }

    ///<summary>
    /// Removes multiple layers.
    ///</summary>
    ///<param name="layers">The layers in question, if they surpass the max value (default: 30), they will be clamped down</param>
    public int[] RemoveLayers(params int[] layers)
    {
      var finVals = layers.ClampArray(0, MaxLayer);
      Layers.ExceptWith(finVals);

      return finVals;
    }

    ///<summary>
    /// A check for whether the shape is valid for intersection.
    ///</summary>
    ///<param name="shape">The shape in question.</param>
    private bool IsValid(CollisionShape2D shape)
    {
      return shape.Disabled == false && shape?.Shape != null;
    }

    ///<summary>
    /// Checks whether this shape intersects with another specified shape.
    ///</summary>
    ///<param name="other">The other shape.</param>
    public bool Intersects(CollisionNode2D other)
    {
      if ((this.Layers.Count > 0 || other.Layers.Count > 0)
          && !this.Layers.Overlaps(other.Layers))
      {
        return false;
      }

      int myCount = this.CollisionShapes.Count;
      int otherCount = other.CollisionShapes.Count;

      for (int i = 0; i < myCount; i++)
      {
        var thisShape = this.CollisionShapes[i];

        if (!IsValid(thisShape))
          continue;

        for (int j = 0; j < otherCount; j++)
        {
          var otherShape = other.CollisionShapes[j];

          if (IsValid(otherShape) && thisShape.Intersects(otherShape))
            return true;
        }
      }

      return false;
    }

    ///<summary>
    /// Checks whether this shape intersects with a given shape at a specified offset.
    ///</summary>
    ///<param name="offset">The offset; applied to this shape's global position </param>
    ///<param name="other">The other shape, the offset is not applied to it.</param>
    public bool IntersectsAt(Vector2 offset, CollisionNode2D other)
    {
      if ((this.Layers.Count > 0 || other.Layers.Count > 0)
          && !this.Layers.Overlaps(other.Layers))
      {
        return false;
      }

      int myCount = this.CollisionShapes.Count;
      int otherCount = other.CollisionShapes.Count;

      for (int i = 0; i < myCount; i++)
      {
        var thisShape = this.CollisionShapes[i];

        if (!IsValid(thisShape))
          continue;

        for (int j = 0; j < otherCount; j++)
        {
          var otherShape = other.CollisionShapes[j];

          if (IsValid(otherShape) && thisShape.IntersectsAt(offset, otherShape))
            return true;
        }
      }

      return false;
    }

    ///<summary>
    /// Checks if this shape contains a specified position.
    ///</summary>
    ///<param name="position"> </param>
    public bool Contains(Vector2 p)
    {
      int thisCount = this.CollisionShapes.Count;

      for (int i = 0; i < thisCount; i++)
      {
        var thisShape = this.CollisionShapes[i];
        if (thisShape.Contains(p))
          return true;
      }

      return false;
    }

    public override void _EnterTree()
    {
      base._EnterTree();
    }

    public override void _ExitTree()
    {
      base._ExitTree();
    }

    public override void _PhysicsUpdate(float delta)
    {
      base._PhysicsUpdate(delta);
    }

    public override void _Process(float delta)
    {
      base._Process(delta);
    }

    public override void _Submit(Canvas2D canvas)
    {
      base._Submit(canvas);
    }
  }
}
