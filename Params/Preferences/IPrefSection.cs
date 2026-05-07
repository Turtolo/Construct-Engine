namespace Amethyst.Prefs
{
  public interface IPrefSection
  {
    bool IsDirty { get; }
    void Apply();
  }
}