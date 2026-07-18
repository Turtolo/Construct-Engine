using Opal.Managers;
using Opal.Geometry;
using Opal.Prefs;
using Microsoft.Xna.Framework;
using System;

namespace Opal
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

    private bool mouseVisible = true;
    public bool MouseVisible
    {
      get => mouseVisible;
      set => tracker.Set(ref mouseVisible, value);
    }

    public bool IsDirty => tracker.IsDirty;

    public void Apply()
    {
      Core.Instance.Graphics.IsFullScreen = Fullscreen;

      Core.Canvas.RenderSize = RenderSize;
      Core.Canvas.CanvasColor = CanvasColor;

      Core.Instance.IsMouseVisible = MouseVisible;

      Core.Canvas.UpdateTransform();
      Core.Instance.Graphics.ApplyChanges();
    }
  }
}
