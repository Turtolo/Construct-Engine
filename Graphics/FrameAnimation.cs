#nullable disable

using System;
using System.Collections.Generic;

namespace Amethyst.Graphics
{
  public class FrameAnimation : BaseObject
  {
    public List<MTexture> Frames { get; set; }

    public TimeSpan Delay { get; set; }

    public FrameAnimation()
    {
      Frames = new List<MTexture>();
      Delay = TimeSpan.Zero;
    }

    public FrameAnimation(List<MTexture> frames, TimeSpan delay)
    {
      Frames = frames;
      Delay = delay;
    }
  }
}
