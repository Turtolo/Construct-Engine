using Amethyst.Managers;
using System.Collections.Generic;

namespace Amethyst.Util
{
  public sealed class Animation : BaseObject
  {
    public List<ITrack> Tracks { get; private set; } = new();

    public Animation(List<ITrack> tracks)
    {
      Tracks = tracks;
    }

    public Animation(ITrack track)
    {
      Tracks.Add(track);
    }

    public void Add(ITrack track)
    {
      Tracks.Add(track);
    }
  }
}
