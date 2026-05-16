using Amethyst.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amethyst.Params
{
  public readonly record struct Material : IProperty<Material>
  {
    public Effect Shader { get; init; }
    public SpriteEffects SpriteEffects { get; init; }

    public bool Visibility { get; init; }
    public Color Modulate { get; init; }

    public static readonly Material Identity =
        new(true, Color.White, null, SpriteEffects.None);

    public Material(bool visibility, Color modulate, Effect shader, SpriteEffects spriteEffects)
    {
      Visibility = visibility;
      Modulate = modulate;
      Shader = shader;
      SpriteEffects = spriteEffects;
    }

    public static Material Combine(in Material parent, in Material child)
    {
      return new Material(
          parent.Visibility && child.Visibility,
          ColorExtension.Multiply(parent.Modulate, child.Modulate),
          child.Shader ?? parent.Shader,
          child.SpriteEffects
      );

    }
  }
}
