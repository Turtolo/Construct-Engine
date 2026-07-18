using Opal.Managers;
using System;

namespace Opal.Tools
{
  public static class ParamTools
  {
    public static TResult With<T, TResult>(T obj, Func<T, TResult> func) => func(obj);
  }
}
