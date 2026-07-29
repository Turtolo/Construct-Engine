using Opal.Managers;
namespace Opal.Tools
{
  public struct KeyFrame<T>
  {
    public float Time { get; set; }
    public T Value { get; set; }
  }
}
