using System;
using Opal.Graphics;
using Opal.Managers;
using Opal.Params;
using Opal.Tools;
using Opal.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Opal.Hierarchy
{
  public class ColorRect : Node2D
  {
    public Color Color { get; set; }

    public override void _Submit(Canvas2D canvas)
    {
      base._Submit(canvas);

      Color surface = ColorExtension.Multiply(Material.Global.SelfModulate, Material.Global.Modulate);
      Color final = ColorExtension.Multiply(Color, surface);

      Vector2 pos = Rounded ? Vector2.Floor(Transform.Global.Position) : Transform.Global.Position;
      Vector2 scale = Rounded ? Vector2.Floor(Transform.Global.Scale) : Transform.Global.Scale;
      
      TextureDrawCall call = ObjectPool<TextureDrawCall>.Get();

      call.Texture = Core.Resources.Pixel;
      call.Effect = Material.Global.Shader;
      call.Depth = Ordering.Global.Depth;

      call.Params = CanvasParams.Identity with
      {
        Position = pos,
        Color = final,
        Rotation = Transform.Global.Rotation,
        Scale = scale,
        Effects = Material.Global.SpriteEffects,
      };

      call.Key = BatchKey.Default with
      {
        Matrix = Seperated ? null : Core.Token.Get<Camera2D>()?.GetTransform(),
      };

      Core.Canvas.Submit(call);
    }
  }
}
