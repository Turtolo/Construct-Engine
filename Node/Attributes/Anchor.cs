using Amethyst.Params;

namespace Amethyst.Hierarchy
{
  public readonly record struct Anchor : IProperty<Anchor>
  {
    public float AnchorBottom { get; init; }
    public float AnchorLeft { get; init; }
    public float AnchorRight { get; init; }
    public float AnchorTop { get; init; }

    public float OffsetBottom { get; init; }
    public float OffsetLeft { get; init; }
    public float OffsetRight { get; init; }
    public float OffsetTop { get; init; }

    public static Anchor Combine(in Anchor parent, in Anchor child)
    {
      throw new System.NotImplementedException();
    }
  }

}
