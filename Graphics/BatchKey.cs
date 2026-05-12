using System;
using Microsoft.Xna.Framework.Graphics;

namespace Amethyst.Graphics
{
  public struct BatchKey : IEquatable<BatchKey>
  {
    public SpriteSortMode SortMode { get; set; }
    public BlendState BlendState { get; set; }
    public SamplerState SamplerState { get; set; }
    public DepthStencilState DepthStencilState { get; set; }
    public RasterizerState RasterizerState { get; set; }

    public BatchKey(
        SpriteSortMode sortMode,
        BlendState blendState,
        SamplerState samplerState,
        DepthStencilState depthStencilState,
        RasterizerState rasterizerState)
    {
      SortMode = sortMode;
      BlendState = blendState;
      SamplerState = samplerState;
      DepthStencilState = depthStencilState;
      RasterizerState = rasterizerState;
    }

    public static BatchKey Default => new(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp,
        DepthStencilState.None, RasterizerState.CullCounterClockwise);

    public bool Equals(BatchKey other)
    {
      return SortMode == other.SortMode &&
             BlendState == other.BlendState &&
             SamplerState == other.SamplerState &&
             DepthStencilState == other.DepthStencilState &&
             RasterizerState == other.RasterizerState;
    }

    public override bool Equals(object obj)
        => obj is BatchKey other && Equals(other);

    public override int GetHashCode()
    {
      return HashCode.Combine(
          SortMode,
          BlendState,
          SamplerState,
          DepthStencilState,
          RasterizerState);
    }
  }
}
