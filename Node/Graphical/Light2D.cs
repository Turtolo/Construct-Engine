using Opal.Managers;
using Opal.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Opal.Graphics;
using Opal.Tools;
using Opal.Params;
using Opal.Tools;

namespace Opal.Hierarchy
{
  public class PointLight2D : Node2D
  {
    public MTexture Texture { get; set; }
    public Color Tint { get; set; } = Color.White;
    public float Intensity { get; set; } = 1.0f;
    
    public override void _Submit(Canvas2D canvas)
    {
      base._Submit(canvas);

      if (Texture == null || Material.Global.Visible == false)
        return;

      Color finalColor = ColorExtension.Multiply(Material.Global.SelfModulate, Material.Global.Modulate);

      Vector2 pos = Rounded ? Vector2.Floor(Transform.Global.Position) : Transform.Global.Position;
      Vector2 scale = Rounded ? Vector2.Floor(Transform.Global.Scale) : Transform.Global.Scale;
      
      var call = ObjectPool<PointLightDrawCall>.Get();

      call.Texture = Texture;

      call.Intensity = Intensity;

      call.Tint = Tint;

      call.Effect = Material.Global.Shader;
      call.Depth = Ordering.Global.Depth;

      call.Params = CanvasParams.Identity with
      {
        Position = pos,
        Color = finalColor,
        Rotation = Transform.Global.Rotation,
        //Origin = new Vector2(call.Texture.Bounds.Width / 2f, call.Texture.Bounds.Height / 2f),
        Scale = scale,
        Effects = Material.Global.SpriteEffects,
      };

      call.Key = BatchKey.Default with
      {
        Matrix = Seperated ? null : Core.Token.Get<Camera2D>()?.GetTransform()
      };

      Core.Canvas.SubmitLight(call);
    }
  }
}
