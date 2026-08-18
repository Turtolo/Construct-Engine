using System;
using Opal.Tools;

namespace Opal.Managers
{
  public class AnchorManager : BaseObject
  {
    private Anchor currentAnchor;
    
    /// <summary>
    /// Changes the current anchor, with a provided type. Does not use reflection.
    /// </summary>
    /// <remarks>
    /// De-activates the current anchor; which removes all its chained instances of <see cref="Token"/>.
    /// </remarks>
    /// <param name="T">The type of the new anchor, similar to <see cref="TokenIndex">' Get functions.</param>
    public T SetAnchor<T>() where T : Anchor, new()
    {
      currentAnchor?.Dispose();

      var newAnchor = new T();
      currentAnchor = newAnchor;
      newAnchor.Activate();

      return newAnchor;
    }

    /// <summary>
    /// Changes the current anchor, with a provided type. Uses reflection -- though -- as <see cref="Anchor"/>s are usually transitional, this is negligible.
    /// </summary>
    /// <remarks>
    /// De-activates the current anchor; which removes all its chained instances of <see cref="Token"/>.
    /// </remarks>
    /// <param name="anchorType">The type of the new anchor.</param>
    public Anchor SetAnchor(Type anchorType)
    {
      if (!typeof(Anchor).IsAssignableFrom(anchorType))
      {
        throw new ArgumentException($"{anchorType.Name} must inherit from Anchor.");
      }

      currentAnchor?.Dispose();

      var newAnchor = (Anchor)Activator.CreateInstance(anchorType);
      currentAnchor = newAnchor;
      newAnchor.Activate();

      return newAnchor;
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

      SetAnchor(t);
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
