using Amethyst.Managers;
using Amethyst.Params;
using Microsoft.Xna.Framework;

namespace Amethyst.Hierarchy
{
  ///<summary>
  /// A node that acts as an 'anchor' for <see cref="Canvas2D"/>, this is not in an orientation sense – more so configuration of it.
  ///</summary>
  ///<remarks>
  /// Acts as an interface for the canvas' parameters, such as the ambient color, sky-box, etc.
  ///</remarks>
  public class CanvasAnchor : Node
  {
    ///<summary>
    ///
    ///</summary>
    public Canvas2D Canvas { get; private set; } = Core.Canvas;

    ///<summary>
    /// The color of the 'background', in the sense that where there is nothingness, this color appears.
    ///</summary>
    [Export]
    public Color BackBufferColor
    {
      get => Canvas.CanvasColor;
      set => Canvas.CanvasColor = value;
    }

    ///<summary>
    /// The tint applied to all regular calls, modified by lighting.
    ///</summary>
    ///<remarks>
    /// This system is weird, not my prefered method, but there is a shortage of documentation for HLSL.
    ///</remarks>
    [Export]
    public Color AmbientColor
    {
      get => Canvas.AmbientColor;
      set => Canvas.AmbientColor = value;
    }

    public override void _EnterTree()
    {
      base._EnterTree();
    }

    public override void _Submit(Canvas2D canvas)
    {
      Canvas = canvas;
    }

    public override void _ExitTree()
    {
      BackBufferColor = Color.CornflowerBlue;
      AmbientColor = Color.White;

      Canvas = null;

      base._ExitTree();
    }
  }
}
