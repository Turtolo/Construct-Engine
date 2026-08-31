using Opal.Managers;
#nullable disable

using System;
using System.Collections.Generic;

namespace Opal.Graphics
{
  public class FrameAnimation : BaseObject
  {
    public List<TextureRegion> Frames { get; set; }

    public TimeSpan Delay { get; set; }

    public FrameAnimation()
    {
      Frames = new List<TextureRegion>();
      Delay = TimeSpan.Zero;
    }

    public FrameAnimation(List<TextureRegion> frames, TimeSpan delay)
    {
      Frames = frames;
      Delay = delay;
    }
  }
}
