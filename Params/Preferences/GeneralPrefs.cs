using Opal.Managers;
namespace Opal.Prefs
{
  public class GeneralPrefs : IPrefSection
  {
    private readonly DirtyTracker tracker = new();

    private string title = "Game1";
    public string Title
    {
      get => title;
      set => tracker.Set(ref title, value);
    }

    private bool showCollision = false;
    public bool ShowCollision
    {
      get => showCollision;
      set => tracker.Set(ref showCollision, value);
    }

    public bool IsDirty => tracker.IsDirty;

    public void Apply()
    {
      Core.Instance.Window.Title = title;
    }
  }
}
