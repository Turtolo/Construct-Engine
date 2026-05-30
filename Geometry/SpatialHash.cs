#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Amethyst.Geometry
{
  public class SpatialHash<T> where T : IHashAble
  {
    private readonly Dictionary<long, List<T>> _cells = new();

    private readonly Dictionary<T, List<long>> _objectCells = new();
    private readonly List<T> _queryResults = new(256);

    private readonly List<long> _queryCells = new(32);
    private readonly HashSet<T> _seen = new();

    private readonly float _cellSize;
    private readonly float _inverseCellSize;

    public SpatialHash(float cellSize = 64f)
    {
      _cellSize = cellSize;
      _inverseCellSize = 1f / cellSize;
    }

    private static long GetKey(int x, int y)
    {
      return ((long)x << 32) | (uint)y;
    }

    private int GetCellsForBounds(Rectangle[] bounds, List<long> results)
    {
      int count = 0;

      for (int i = 0; i < bounds.Length; i++)
      {
        Rectangle b = bounds[i];

        int minX = (int)Math.Floor(b.Left * _inverseCellSize);
        int maxX = (int)Math.Floor(b.Right * _inverseCellSize);

        int minY = (int)Math.Floor(b.Top * _inverseCellSize);
        int maxY = (int)Math.Floor(b.Bottom * _inverseCellSize);

        for (int x = minX; x <= maxX; x++)
        {
          for (int y = minY; y <= maxY; y++)
          {
            results.Add(GetKey(x, y));
            count++;
          }
        }
      }

      return count;
    }

    public void Insert(T obj)
    {
      var cells = new List<long>(32);
      GetCellsForBounds(obj.Bounds.ToArray(), cells);

      _objectCells[obj] = cells;

      for (int i = 0; i < cells.Count; i++)
      {
        long key = cells[i];

        if (!_cells.TryGetValue(key, out var list))
        {
          list = new List<T>(4);
          _cells[key] = list;
        }

        list.Add(obj);
      }
    }

    public void Remove(T obj)
    {
      if (!_objectCells.TryGetValue(obj, out var cells))
        return;

      for (int i = 0; i < cells.Count; i++)
      {
        long key = cells[i];

        if (_cells.TryGetValue(key, out var list))
        {
          list.Remove(obj);

          if (list.Count == 0)
            _cells.Remove(key);
        }
      }

      _objectCells.Remove(obj);
    }

    public void Update(T obj)
    {
      if (!_objectCells.TryGetValue(obj, out var oldCells))
      {
        Insert(obj);
        return;
      }

      var newCells = new List<long>(oldCells.Count + 8);
      GetCellsForBounds(obj.Bounds.ToArray(), newCells);

      for (int i = 0; i < oldCells.Count; i++)
      {
        long cell = oldCells[i];

        if (!newCells.Contains(cell))
        {
          if (_cells.TryGetValue(cell, out var list))
          {
            list.Remove(obj);
            if (list.Count == 0)
              _cells.Remove(cell);
          }
        }
      }

      for (int i = 0; i < newCells.Count; i++)
      {
        long cell = newCells[i];

        if (!oldCells.Contains(cell))
        {
          if (!_cells.TryGetValue(cell, out var list))
          {
            list = new List<T>(4);
            _cells[cell] = list;
          }

          list.Add(obj);
        }
      }

      _objectCells[obj] = newCells;
    }

    public List<T> Query(Rectangle[] bounds)
    {
      _queryResults.Clear();
      _queryCells.Clear();
      _seen.Clear();

      GetCellsForBounds(bounds, _queryCells);

      for (int i = 0; i < _queryCells.Count; i++)
      {
        long key = _queryCells[i];

        if (_cells.TryGetValue(key, out var list))
        {
          for (int j = 0; j < list.Count; j++)
          {
            T obj = list[j];

            if (_seen.Add(obj))
              _queryResults.Add(obj);
          }
        }
      }

      return _queryResults;
    }

    public void Clear()
    {
      _cells.Clear();
      _objectCells.Clear();
      _queryResults.Clear();
    }
  }

  public interface IHashAble
  {
    List<Rectangle> Bounds { get; }
  }
}
