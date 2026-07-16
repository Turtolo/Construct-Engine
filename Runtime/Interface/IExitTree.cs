using Amethyst.Managers;
namespace Amethyst.Runtime
{
  public interface IExitTree
  {

    /// <summary>
    /// The 'production' function called when a <see cref="Token"/> exits the tree.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For production functions you need to call <c>base._ExitTree();</c>;
    /// a rule of thumb for production functions vs. regular ones is whether it will be inherited by others, if yes – use this.
    /// </para>
    /// </remarks>
    void _ExitTree();

    /// <summary>
    /// The 'regular' function called when a <see cref="Token"/> exits the tree.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In opposition to production functions, you do not need to call <c>base.ExitTree()</c> – it is intended for classes that do not have children (e.g a player).
    /// </para>
    /// <para>
    /// Note that if you plan to create a system of classes working together (with inheritance), use instead <see cref="_ExitTree()"/>.
    /// </para>
    /// </remarks>
    void ExitTree();
  }
}
