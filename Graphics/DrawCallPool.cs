using System.Collections.Generic;

namespace Amethyst.Graphics
{
  public static class DrawCallPool<T> where T : class, IDrawCall, new()
  {
    private static readonly Stack<T> _pool = new Stack<T>(50000);

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
