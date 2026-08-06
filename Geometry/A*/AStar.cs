using Opal.Managers;
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Opal.Tools;

namespace Opal.Geometry
{
  public static class AStar
  {
    /// <summary>
    /// Gets the fastest path towards a goal.
    /// </summary>
    /// <summary>
    /// Uses heuristic, moves in eight directions.
    /// </summary>
    /// <param name="grid"> The grid which will be used, takes the form of a csv, you can turn collision into a csv. </param>
    /// <param name="start"> The start point; should not be in real-world coordinates nor go past the <see cref="grid"/> boundry. </param>
    /// <param name="goal"> The end point, the goal; should not be in real-world cooridnates nor go past the <see cref="grid"/> boundry. </param>
    public static List<Point> GetPath(int[,] grid, Point start, Point goal)
    {
      var allCells = new Dictionary<Point, Cell>();

      int rows = grid.GetLength(0);
      int cols = grid.GetLength(1);

      var closedList = new HashSet<Cell>();

      Cell startCell = new Cell(start);
      allCells[start] = startCell;
      var openList = new List<Cell> { startCell };

      startCell.G = 0;
      startCell.H = Heuristic(startCell, new Cell(goal));

      while (openList.Any())
      {
        Cell currentCell = openList.OrderBy(n => n.F).First();
        openList.Remove(currentCell);
        closedList.Add(currentCell);

        if (currentCell.Location == goal)
        {
          return GetPath(currentCell);
        }

        foreach (var direction in CordTools.Directions)
        {
          Point newLocation = currentCell.Location + direction;
          if (newLocation.IsValidLocation(grid))
          {

            Cell neighbor;

            if (!allCells.TryGetValue(newLocation, out neighbor))
            {
              neighbor = new Cell(newLocation);
              allCells[newLocation] = neighbor;
            }

            bool isDiagonal = direction.X != 0 && direction.Y != 0;
            int moveCost = isDiagonal ? 14 : 10;

            int tentativeG = currentCell.G + moveCost;

            if (isDiagonal)
            {
              Point p1 = new Point(currentCell.Location.X + direction.X, currentCell.Location.Y);
              Point p2 = new Point(currentCell.Location.X, currentCell.Location.Y + direction.Y);

              if (!p1.IsValidLocation(grid) || !p2.IsValidLocation(grid))
                continue;
            }

            if (tentativeG < neighbor.G)
            {
              neighbor.G = tentativeG;
              neighbor.H = Heuristic(neighbor, new Cell(goal));
              neighbor.Parent = currentCell;
              if (!openList.Contains(neighbor))
                openList.Add(neighbor);
            }
          }
        }
      }

      return null;
    }

    private static int Heuristic(Cell a, Cell b)
    {
      int dx = Math.Abs(a.Location.X - b.Location.X);
      int dy = Math.Abs(a.Location.Y - b.Location.Y);
      return 14 * Math.Min(dx, dy) + 10 * (Math.Max(dx, dy) - Math.Min(dx, dy));
    }

    private static List<Point> GetPath(Cell goal)
    {
      var path = new List<Point>();
      Cell current = goal;

      while (current != null)
      {
        path.Add(current.Location);
        current = current.Parent;
      }

      path.Reverse();
      return path;
    }
  }
}
