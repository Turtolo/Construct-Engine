using Amethyst.Managers;
using System;
using System.Collections.Generic;

namespace Amethyst.Prefs
{
  public class DirtyTracker
  {
    public bool IsDirty { get; private set; }

    public void Set<T>(ref T field, T value)
    {
      field = value;
      IsDirty = true;
    }

    public void MarkClean() => IsDirty = false;
  }
}
