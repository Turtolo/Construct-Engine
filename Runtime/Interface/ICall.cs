using Opal.Managers;

namespace Opal.Runtime
{
  public interface ICall
  {
    /// <summary>
    /// The 'production' function for submitting calls to the current canvas.
    /// </summary>
    /// <remarks>
    /// For production functions you need to call <c>base._Submit(canvas)</c>;
    /// a rule of thumb for production functions vs. regular ones is whether it will be inherited by others, if yes – use this.
    /// </remarks>
    /// <param name="canvas">The current canvas, it it is advised to use this rather than <see cref="Core.Canvas"/>.</param> 
    void _Submit(Canvas2D canvas);
    
    /// <summary>
    /// The 'regular' function for submitting calls to the current canvas.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In opposition to production functions, you do not need to call <c>base.Submit(canvas)</c> – it is intended for classes that do not have children (e.g a player).
    /// </para>
    /// <para>
    /// Note that if you plan to create a system of classes working together (with inheritance), use instead <see cref="_Submit(Canvas2D)"/>.
    /// </para>
    /// </remarks>
    /// <param name="canvas">The current canvas, it it is advised to use this rather than <see cref="Core.Canvas"/>.</param> 
    void Submit(Canvas2D canvas);
  }
}
