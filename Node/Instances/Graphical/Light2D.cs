using Amethyst.Managers;
using Amethyst.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Amethyst.Hierarchy
{
  public class PointLight2D : Node2D, ILight2D
  {
    public MTexture Texture => throw new System.NotImplementedException();

    public void ProjectLighting(SpriteBatch sb)
    {
      throw new System.NotImplementedException();
    }
  }
}
