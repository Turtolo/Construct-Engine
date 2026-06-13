using Amethyst.Managers;
namespace Amethyst.Prefs
{
  public class AudioPrefs : IPrefSection
  {
    private readonly DirtyTracker tracker = new();

    public bool IsDirty => tracker.IsDirty;

    public void Apply()
    {

    }
  }
}
