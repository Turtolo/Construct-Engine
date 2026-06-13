using Amethyst.Managers;
namespace Amethyst.Util
{
  public struct KeyFrame<T>
  {
    public float Time { get; set; }
    public T Value { get; set; }
  }
}
