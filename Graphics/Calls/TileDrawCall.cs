using Opal.Managers;
using Opal.Params;
using Opal.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Opal.Graphics
{
  public sealed class TileDrawCall : Layered, IDrawCall, IPoolable
  {
    public int Index { get; set; }

    public Effect Effect { get; set; }

    public CanvasParams Params { get; set; } = CanvasParams.Identity;

    public BatchKey Key { get; set; } = BatchKey.Default;

    public int[,] Tiles { get; set; }

    public Tileset Tileset { get; set; }

    public int Columns { get; set; }
    public int Rows { get; set; }

    public int IndexOffset { get; set; }

    public void Draw(SpriteBatch sb)
    {
      for (int y = 0; y < Rows; y++)
      {
        for (int x = 0; x < Columns; x++)
        {
          int storedIndex = Tiles[x, y];
          if (storedIndex < 0)
            continue;

          int tileSetIndex = storedIndex + IndexOffset;

          MTexture tile = Tileset.GetTile(tileSetIndex);
          if (tile?.Texture == null)
            continue;

          Vector2 localTilePos = new Vector2(
              x * Tileset.TileWidth,
              y * Tileset.TileHeight
          );

          Vector2 worldTilePos = localTilePos + Params.Position;

          Rectangle src = tile.SourceRectangle ?? new Rectangle(0, 0, tile.Texture.Width, tile.Texture.Height);

          sb.Draw(
              tile.Texture,
              worldTilePos,
              src,
              Params.Color,
              Params.Rotation,
              Params.Origin,
              Params.Scale,
              Params.Effects,
              InternalDepth
          );
        }
      }
    }

    public void Reset()
    {
      Tileset = null;
      Tiles = null;

      Columns = 0;
      Rows = 0;

      ObjectPool<TileDrawCall>.Return(this);
    }
  }
}
