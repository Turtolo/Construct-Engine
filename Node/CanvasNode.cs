using Opal.Managers;
#nullable enable

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Opal.Params;

namespace Opal.Hierarchy
{
  public class CanvasNode : Node
  {
    [Export]
    public Dual<Ordering> Ordering { get; private set; }

    [Export]
    public Dual<Material> Material { get; private set; }

    [Export]
    public Dual<Transform2D> Transform { get; private set; }

    /// <summary>
    /// The self contained visibility of this node. 
    /// </summary>
    [Export]
    public bool Visible
    {
      get => Material.Local.Visible;
      set
      {
        Material.Local = Material.Local with { Visible = value };
      }
    }

    /// <summary>
    /// The self contained modulate of this node.
    /// </summary>
    [Export]
    public Color Modulate
    {
      get => Material.Local.Modulate;
      set
      {
        Material.Local = Material.Local with { Modulate = value };
      }
    }

    /// <summary>
    /// The self contained depth of this node.
    /// </summary>
    [Export]
    public int Depth
    {
      get => Ordering.Local.Depth;
      set
      {
        Ordering.Local = Ordering.Local with { Depth = value };
      }
    }

    /// <summary>
    /// The self contained shader of this node.
    /// </summary>
    [Export]
    public Effect Shader
    {
      get => Material.Local.Shader;
      set
      {
        Material.Local = Material.Local with { Shader = value };
      }
    }

    [Export]
    public bool Rounded { get; set; }

    /// <summary>
    /// Whether this node should be one a seperated plane.
    /// In layman-terms–this could regarded as a top-level ordering.
    /// </summary>
    [Export]
    public bool Seperated
    {
      get => Material.Global.Separated;
      set
      {
        Material.Local = Material.Local with { Separated = value };
      }
    }

    /// <summary>
    /// The self contained sprite effects of this node.
    /// </summary>
    [Export]
    public SpriteEffects SpriteEffects
    {
      get => Material.Local.SpriteEffects;
      set
      {
        Material.Local = Material.Local with { SpriteEffects = value };
      }
    }

    public CanvasNode()
    {
      Ordering = new Dual<Ordering>(Params.Ordering.Identity);
      Material = new Dual<Material>(Params.Material.Identity);
      Transform = new Dual<Transform2D>(Params.Transform2D.Identity);
    }
  }
}
