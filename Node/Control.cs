using Opal.Managers;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Opal.Geometry;
using Opal.Graphics;
using Opal.Tools;
using Opal.Managers;
using Opal.Params;
using System.IO.Compression;

namespace Opal.Hierarchy
{
  public class Control : CanvasNode
  {
    /// <summary>
    /// Signal for when the transform changes.
    /// </summary>
    public event Action<Transform2D>? OnTransformChanged;

    /// <summary>
    /// The self contained position of this node, updates child nodes' position.
    /// </summary>
    [Export]
    public Vector2 Position
    {
      get => Transform.Local.Position;
      set
      {
        Transform.Local = Transform.Local with { Position = value };
      }
    }

    /// <summary>
    /// The self contained rotation of this node, updates child nodes' rotation.
    /// </summary>
    [Export]
    public float Rotation
    {
      get => Transform.Local.Rotation;
      set
      {
        Transform.Local = Transform.Local with { Rotation = value };
      }
    }

    /// <summary>
    /// The self contained scale of this node, updates child nodes' scale.
    /// </summary>
    [Export]
    public Vector2 Scale
    {
      get => Transform.Local.Scale;
      set
      {
        Transform.Local = Transform.Local with { Scale = value };
      }
    }

    public Control() 
    { 
      Ordering.OnChanged += UpdateAttributes;
      Material.OnChanged += UpdateAttributes;
      Transform.OnChanged += UpdateAttributes;

      UpdateAttributes();
      OnParentChanged += (node) =>
      {
        UpdateAttributes();
      };
    }

    /// <summary>
    /// Recalculates global rendering attributes and propagates them to children.
    /// </summary>
    internal void UpdateAttributes()
    {
      if (GetParent() is CanvasNode parent)
      {
        Ordering.Global = Params.Ordering.Combine(parent.Ordering.Global, Ordering.Local);
        Material.Global = Params.Material.Combine(parent.Material.Global, Material.Local);
        Transform.Global = Transform2D.Combine(parent.Transform.Global, Transform.Local);
      }
      else
      {
        Ordering.Global = Ordering.Local;
        Material.Global = Material.Local;
        Transform.Global = Transform.Local;
      }

      OnTransformChanged?.Invoke(Transform.Global);

      foreach (var child in Children)
      {
        switch (child)
        {
          case Node2D n:
            n.UpdateAttributes();
            break;
          case Control c:
            c.UpdateAttributes();
            break;
        }
      }
    }

    public override void _EnterTree()
    {
      base._EnterTree();
    }

    public override void _ExitTree()
    {
      base._ExitTree();
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
