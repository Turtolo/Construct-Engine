#nullable disable

using System;
using Microsoft.Xna.Framework;
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
    public Matrix? Matrix { get; set; }

    public BatchKey(
        SpriteSortMode sortMode,
        BlendState blendState,
        SamplerState samplerState,
        DepthStencilState depthStencilState,
        RasterizerState rasterizerState,
        Matrix? matrix)
    {
      SortMode = sortMode;
      BlendState = blendState;
      SamplerState = samplerState;
      DepthStencilState = depthStencilState;
      RasterizerState = rasterizerState;
      Matrix = matrix;
    }

    public static BatchKey Default => new(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp,
        DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

    public bool Equals(BatchKey other)
    {
      return SortMode == other.SortMode &&
             BlendState == other.BlendState &&
             SamplerState == other.SamplerState &&
             DepthStencilState == other.DepthStencilState &&
             RasterizerState == other.RasterizerState &&
             Matrix == other.Matrix;
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
          RasterizerState,
          Matrix);
    }
  }
}
