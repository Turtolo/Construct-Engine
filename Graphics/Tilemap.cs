using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using ConstructEngine.Util;
using ConstructEngine;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Graphics;

public class Tilemap
{
    private readonly Tileset _tileset;
    private readonly int[] _tiles;
    
    public static List<Tilemap> Tilemaps = new List<Tilemap>();

    public int Rows { get; }
    public int Columns { get; }
    public int Count { get; }
    
    public float LayerDepth { get; set; }
    



    public float TileWidth => _tileset.TileWidth;
    public float TileHeight => _tileset.TileHeight;


    public Tilemap(Tileset tileset, int columns, int rows, float layerDepth)
    {
        LayerDepth = layerDepth;
        _tileset = tileset;
        Rows = rows;
        Columns = columns;
        Count = Columns * Rows;
        _tiles = new int[Count];
    }



    public static void DrawTilemaps(SpriteBatch spriteBatch)
    {
        foreach (Tilemap tilemap in Tilemaps)
        {
            tilemap.Draw(spriteBatch, tilemap.LayerDepth);
        }
    }
    
    
    public void SetTile(int index, int tilesetID)
    {
        _tiles[index] = tilesetID;
    }

    public void SetTile(int column, int row, int tilesetID)
    {
        int index = row * Columns + column;
        SetTile(index, tilesetID);
    }

    public TextureRegion GetTile(int index)
    {
        return _tileset.GetTile(_tiles[index]);
    }

    public TextureRegion GetTile(int column, int row)
    {
        int index = row * Columns + column;
        return GetTile(index);
    }
    
    

    public Rectangle GetNonZeroBounds()
    {
        int minRow = Rows;
        int maxRow = -1;
        int minCol = Columns;
        int maxCol = -1;

        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                int index = row * Columns + col;
                if (_tiles[index] != 0)
                {
                    if (row < minRow) minRow = row;
                    if (row > maxRow) maxRow = row;
                    if (col < minCol) minCol = col;
                    if (col > maxCol) maxCol = col;
                }
            }
        }

        if (maxRow == -1)
        {
            return new Rectangle(0, 0, 0, 0);
        }

        int width = maxCol - minCol + 1;
        int height = maxRow - minRow + 1;

        return new Rectangle(minCol, minRow, width, height);
    }

    public void Draw(SpriteBatch spriteBatch, float layerDepth)
    {
        for (int i = 0; i < Count; i++)
        {
            int tileSetIndex = _tiles[i];
            TextureRegion tile = _tileset.GetTile(tileSetIndex);

            int x = i % Columns;
            int y = i / Columns;

            Vector2 position = new Vector2(x * TileWidth, y * TileHeight);
            tile.Draw(spriteBatch, position, Color.White, layerDepth);
        }
    }
    
}
