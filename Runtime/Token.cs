using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Amethyst.Managers;
using Amethyst.Params;
using Amethyst.Runtime;

namespace Amethyst
{
  ///<summary>
  /// The absolute abstract class for other classes.
  ///</summary>
  public abstract class Token : BaseObject
  {
    ///<summary>
    /// The name of this instance.
    ///</summary>
    ///<remarks>
    /// Since names are not always unique, same named instances are grouped together.
    ///</remarks>
    [Export]
    public string Name { get; set; }

    public Token()
    {
      Core.Index.QueueAdd(this);
    }

    ///<summary>
    /// Queues this instance to be removed from <see cref="Index"/>.
    ///</summary>
    ///<remarks>
    /// It will be removed at the end of this frame.
    ///</remarks>
    public void QueueFree()
    {
      Core.Index.QueueRemove(this);
    }

    ///<summary>
    /// Immediately removes this instance from <see cref="Index">.
    ///</summary>
    public void FreeImmediate()
    {
      Core.Index.RemoveNow(this);
    }

    ///<summary>
    /// Removes all data associated with this instance.
    ///</summary>
    internal virtual void ClearData() { }
  }
}
