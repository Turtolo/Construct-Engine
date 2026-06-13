using Amethyst.Managers;
#nullable disable

using Amethyst.Hierarchy;
using Amethyst.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amethyst.Params
{
  public readonly record struct Material : IProperty<Material>
  {
    public Effect Shader { get; init; }
    public SpriteEffects SpriteEffects { get; init; }

    public bool Visible { get; init; }

    public Color Modulate { get; init; }
    public Color SelfModulate { get; init; }

    public bool Separated { get; init; }

    public static readonly Material Identity =
        new(true, Color.White, Color.White, null, SpriteEffects.None, false);

    public Material(bool visible, Color modulate, Color selfModulate, Effect shader, SpriteEffects spriteEffects, bool separated)
    {
      Visible = visible;
      SelfModulate = selfModulate;
      Modulate = modulate;
      Shader = shader;
      SpriteEffects = spriteEffects;
      Separated = separated;
    }

    public static Material Combine(in Material parent, in Material child)
    {
      return new Material(
          parent.Visible && child.Visible,
          child.Modulate,
          ColorExtension.Multiply(parent.SelfModulate, child.SelfModulate),
          child.Shader ?? parent.Shader,
          child.SpriteEffects,
          child.Separated
      );
    }
  }
}
