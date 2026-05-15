using Amethyst.Geometry;
using Amethyst.Prefs;
using Microsoft.Xna.Framework;

namespace Amethyst
{

  public class GraphicsPrefs : IPrefSection
  {
    private readonly DirtyTracker tracker = new();

    private bool fullscreen;
    public bool Fullscreen
    {
      get => fullscreen;
      set => tracker.Set(ref fullscreen, value);
    }

    private Extent renderSize = new Extent(640, 360);
    public Extent RenderSize
    {
      get => renderSize;
      set => tracker.Set(ref renderSize, value);
    }

    private Color canvasColor = Color.CornflowerBlue;
    public Color CanvasColor
    {
      get => canvasColor;
      set => tracker.Set(ref canvasColor, value);
    }

    public bool IsDirty => tracker.IsDirty;

    public void Apply()
    {
      Core.Graphics.IsFullScreen = Fullscreen;

      Core.Canvas.RenderSize = RenderSize;
      Core.Canvas.CanvasColor = CanvasColor;

      Core.Canvas.UpdateTransform();
      Core.Graphics.ApplyChanges();
    }
  }
}
