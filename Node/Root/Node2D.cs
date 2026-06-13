using Amethyst.Managers;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amethyst.Geometry;
using Amethyst.Graphics;
using Amethyst.Tools;
using Amethyst.Managers;
using Amethyst.Params;
using System.IO.Compression;

namespace Amethyst.Hierarchy
{

  public class Node2D : CanvasNode
  {
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

    public Node2D() { }

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
