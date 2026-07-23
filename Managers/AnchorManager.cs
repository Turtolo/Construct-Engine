using System;
using Opal.Util;

namespace Opal.Managers
{
  public class AnchorManager : BaseObject
  {
    private Anchor currentAnchor;

    public void SetAnchor(Anchor anchor)
    {
      currentAnchor = anchor;
      anchor.Activate();
    }

    public void ReloadCurrentAnchor()
    {
      currentAnchor?.DeActivate();

      Type t = currentAnchor.GetType();

      Anchor newAnchor = (Anchor)Activator.CreateInstance(t);

      SetAnchor(newAnchor);
    }

    public Anchor GetCurrentAnchor()
    {
      return currentAnchor;
    }
  }
}
