
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Amethyst.Geometry 
{
  public struct Polygon2D
  {
    public List<Point> Vertices { get; set; }
    
      
    public Polygon2D(IEnumerable<Point> vertices)
    {
      Vertices = new List<Point>(vertices);
    }
    
    public bool Intersects(Polygon2D other)
    {
      for (int i = 0; i < Vertices.Count; i++)
      {
        Point a1 = Vertices[i];
        Point a2 = Vertices[(i + 1) % Vertices.Count];

        for (int j = 0; j < other.Vertices.Count; j++)
        {
          Point b1 = other.Vertices[j];
          Point b2 = other.Vertices[(j + 1) % other.Vertices.Count];

          if (Segment(a1, a2, b1, b2))
            return true;
        }
      }

      return false;
    }

    public void Add(Point p)
    {
      Vertices.Add(p);
    }

    private static float Cross(Point p1, Point p2)
    {
      return p1.X * p2.Y - p1.Y * p2.X;
    }

    private bool Segment(Point a1, Point a2, Point b1, Point b2)
    {
      Point r = a2 - a1;
      Point s = b2 - b1;

      float rxs = Cross(r, s);

      float qpxr = Cross((b1 - a1), r);

      if (rxs == 0 && qpxr == 0)
        return false;

      if (rxs == 0)
        return false;

      float t = Cross(b1 - a1, s) / rxs;
      float u = Cross(b1 - a1, r) / rxs;

      return t >= 0 && t <= 1 &&
        u >= 0 && u <= 1;
    }

    
    public bool Contains(Point p)
    {
      bool inside = false;

      for (int i = 0, j = Vertices.Count - 1;
          i < Vertices.Count;
          j = i++)
      {
        Point a = Vertices[i];
        Point b = Vertices[j];

        bool intersects =
          ((a.Y > p.Y) != (b.Y > p.Y)) &&
          (p.X < (b.X - a.X) * (p.Y - a.Y) / (b.Y - a.Y) + a.X);

        if (intersects)
            inside = !inside;
      }

      return inside;
    }
  }
}
