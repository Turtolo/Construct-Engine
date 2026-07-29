using Opal.Params;

namespace Opal.Hierarchy
{
  public readonly record struct ControlAnchor : IProperty<ControlAnchor>
  {
    public float AnchorBottom { get; init; }
    public float AnchorLeft { get; init; }
    public float AnchorRight { get; init; }
    public float AnchorTop { get; init; }

    public float OffsetBottom { get; init; }
    public float OffsetLeft { get; init; }
    public float OffsetRight { get; init; }
    public float OffsetTop { get; init; }

    public static ControlAnchor Combine(in ControlAnchor parent, in ControlAnchor child)
    {
      throw new System.NotImplementedException();
    }
  }

}
