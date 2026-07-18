using Opal.Managers;
namespace Opal.Runtime
{
  public interface IProcess
  {
    /// <summary>
    /// The 'production' function called every frame with a dynamic delta.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For production functions you need to call <c>base._Process(delta);</c>;
    /// a rule of thumb for production functions vs. regular ones is whether it will be inherited by others, if yes – use this.
    /// </para>
    /// </remarks>
    /// <param name="delta">The space between frames.</param>
    void _Process(float delta);
    
    /// <summary>
    /// The 'regular' function called every frame with a dynamic delta.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In opposition to production functions, you do not need to call <c>base.Process(delta)</c> – it is intended for classes that do not have children (e.g a player).
    /// </para>
    /// <para>
    /// Note that if you plan to create a system of classes working together (with inheritance), use instead <see cref="_Process(float)"/>.
    /// </para>
    /// </remarks>
    /// <param name="delta">The space between frames.</param>
    void Process(float delta);
  }
}
