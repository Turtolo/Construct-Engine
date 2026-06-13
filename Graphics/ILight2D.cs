using Amethyst.Managers;
using Amethyst.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Amethyst.Hierarchy
{
  public interface ILight2D
  {
    MTexture Texture { get; }

    void ProjectLighting(SpriteBatch sb);
  }
}
