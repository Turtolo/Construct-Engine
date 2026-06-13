using Amethyst.Managers;
using System.Collections.Generic;

namespace Amethyst.Util
{
  public static class ObjectPool<T> where T : class, IPoolable, new()
  {
    private static readonly Stack<T> _pool = new Stack<T>(50000);

    ///<summary>
    /// Returns a fresh copy from the pool of drawcalls.
    ///</summary>
    public static T Get()
    {
      return _pool.Count > 0 ? _pool.Pop() : new T();
    }

    public static void Return(T item)
    {
      if (item == null) return;
      _pool.Push(item);
    }
  }
}
