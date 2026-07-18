using Opal.Managers;
namespace Opal.Util
{
  public struct KeyFrame<T>
  {
    public float Time { get; set; }
    public T Value { get; set; }
  }
}
