using System;
using Opal.Tools;

namespace Opal.Managers
{
  public class AnchorManager : BaseObject
  {
    private Anchor currentAnchor;
    
    /// <summary>
    /// Changes the current anchor to a specified one.
    /// </summary>
    /// <remarks>
    /// De-activates the current anchor; which removes all its chained instances of <see cref="Token"/>.
    /// </remarks>
    /// <param name="anchor">The new anchor.</param>
    public void SetAnchor(Anchor anchor)
    {
      currentAnchor?.Detach(anchor);
      currentAnchor?.DeActivate();

      currentAnchor = anchor;
      anchor.Activate();
    }
    
    /// <summary>
    /// Reloads the current anchor.
    /// </summary>
    /// <remarks>
    /// De-activates the current anchor and sets a new instance of the current one.
    /// De-activating removes all its chained instances of <see cref="Token"/>.
    /// </remarks>
    public void ReloadCurrentAnchor()
    {
      Type t = currentAnchor.GetType();

      currentAnchor?.DeActivate();

      Anchor newAnchor = (Anchor)Activator.CreateInstance(t);

      SetAnchor(newAnchor);
    }
    
    /// <summary>
    /// Gets the current anchor.
    /// </summary>
    /// <remarks>
    /// This *can* sometimes be null.
    /// </remarks>
    public Anchor GetCurrentAnchor()
    {
      return currentAnchor;
    }
  }
}
