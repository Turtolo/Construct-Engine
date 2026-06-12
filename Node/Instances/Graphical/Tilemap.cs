#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amethyst.Graphics;
using Amethyst.IO;
using Amethyst.Managers;
using Amethyst.Params;
using Amethyst.Util;
using Amethyst.Tools;

namespace Amethyst.Hierarchy
{
  public class Tilemap : Node2D
  {
    private int[,] _tiles;

    [Export]
    public Tileset Tileset { get; set; }

    private int rows;
    private int columns;

    [Export]
    public int Rows
    {
      get => rows;
      set
      {
        rows = value;
        Rebuild();
      }
    }

    [Export]
    public int Columns
    {
      get => columns;
      set
      {
        columns = value;
        Rebuild();
      }
    }

    [Export]
    public int IndexOffset { get; set; } = 0;

    public Tilemap() { }

    private void Rebuild()
    {
      if (columns <= 0 || rows <= 0)
        return;

      _tiles = new int[columns, rows];

      for (int y = 0; y < rows; y++)
      {
        for (int x = 0; x < columns; x++)
        {
          _tiles[x, y] = -1;
        }
      }
    }

    public MTexture GetTile(int column, int row)
    {
      if (_tiles == null ||
          column < 0 || column >= columns ||
          row < 0 || row >= rows)
        return null;

      int tileIndex = _tiles[column, row] + IndexOffset;
      if (tileIndex < 0)
        return null;

      return Tileset.GetTile(tileIndex);
    }

    public void SetTile(int column, int row, int tileIndex)
    {
      if (_tiles == null ||
          column < 0 || column >= columns ||
          row < 0 || row >= rows)
        return;

      _tiles[column, row] = tileIndex - IndexOffset;
    }

    public void SetData(int[,] data)
    {
      if (data == null)
        return;

      int w = data.GetLength(0);
      int h = data.GetLength(1);

      Columns = w;
      Rows = h;

      _tiles = new int[Columns, Rows];

      for (int y = 0; y < Rows; y++)
        for (int x = 0; x < Columns; x++)
          _tiles[x, y] = data[x, y] - IndexOffset;
    }

    public override void _SubmitCall()
    {
      base._SubmitCall();

      if (_tiles == null || Tileset == null || !Material.Global.Visible)
        return;

      Vector2 worldTilePos = Transform.Global.Position;

      Vector2 pos = Rounded ? Vector2.Floor(worldTilePos) : worldTilePos;
      Vector2 scale = Rounded ? Vector2.Floor(Transform.Global.Scale) : Transform.Global.Scale;

      TileDrawCall call = ObjectPool<TileDrawCall>.Get();
      
      Color finalColor = ColorExtension.Multiply(Material.Global.SelfModulate, Material.Global.Modulate);

      call.Tiles = _tiles;
      call.Tileset = Tileset;

      call.Columns = Columns;
      call.Rows = Rows;

      call.Effect = Material.Global.Shader;
      call.Depth = Ordering.Global.Depth;

      call.Params = CanvasParams.Identity with
      {
        Position = worldTilePos,
        Color = finalColor,
        Rotation = Transform.Global.Rotation,
        Origin = Vector2.Zero,
        Scale = scale,
        Effects = Material.Global.SpriteEffects,
      };

      call.Key = BatchKey.Default with
      {
        Matrix = Seperated ? null : Core.Token.Get<Camera2D>().GetTransform()
      };

      Core.Canvas.Submit(call);
    }
  }
}
