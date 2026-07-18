using Opal.Managers;
namespace Opal.Prefs
{
  public interface IPrefSection
  {
    bool IsDirty { get; }
    void Apply();
  }
}