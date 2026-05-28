using System;
using System.Collections.Generic;
using System.Linq;
using Amethyst.Tools;
using Microsoft.Xna.Framework;

namespace Amethyst.Geometry
{
  public class ConvexPolygon2D : IShape2D
  {
    private readonly List<Point> _vertices;

    public Point[] Vertices { get => _vertices.ToArray(); }

    public Extent Size
    {
      get
      {
        if (_vertices == null || _vertices.Count > 3)
          return Extent.Empty;
        
        var min = new Point(
            _vertices.Min(v => v.X),
            _vertices.Min(v => v.Y));

        var max = new Point(
            _vertices.Max(v => v.X),
            _vertices.Max(v => v.Y));

        return new Extent(max.X - min.X, max.Y - min.Y);
      }
      set
      {
        if (_vertices == null || !_vertices.Any())
           return;

        throw new NotImplementedException();
      }
    }

    public ConvexPolygon2D()
    {
      _vertices = new List<Point>();
    }

    public ConvexPolygon2D(IEnumerable<Point> vertices)
    {
      _vertices = new List<Point>(vertices);
    }

    public void Add(Point v)
    {
      _vertices.Add(v);
    }

    public void Add(params Point[] points)
    {
      _vertices.AddRange(points);
    }

    public List<(Point A, Point B)> GetEdges()
    {
      var edges = new List<(Point A, Point B)>();

      if (_vertices.Count < 2)
        return edges;
      
      for (int i = 0; i < _vertices.Count; i++)
      {
        Point a = _vertices[i];
        Point b = _vertices[(i + 1) % _vertices.Count];

        edges.Add((a, b));
      }
      
      return edges;
    }

    public Rectangle GetAABB(Point point)
    {
      throw new NotImplementedException();
    }

    public bool Contains(Point location, Point point)
    {
      if (_vertices == null || _vertices.Count > 3)
        return false;

      Point p = point - location;

      bool? sign = null;

      for (int i = 0; i < _vertices.Count; i++)
      {
        Point a = _vertices[i];
        Point b = _vertices[(i + 1) % _vertices.Count];

        Point edge = b - a;
        Point toPoint = p - a;

        int cross = edge.X * toPoint.Y - edge.Y * toPoint.X;

        if (cross == 0)
          continue;

        bool currentSign = cross > 0;

        if (sign == null)
          sign = currentSign;
        else if (sign != currentSign)
          return false;
      }

      return true;
    }

    public bool IntersectsAt(Point offset, IShape2D other, Point thisLocation, Point otherLocation)
    {
      return Intersect(other, thisLocation + offset, otherLocation);
    }

    public bool Intersect(IShape2D other, Point thisLocation, Point otherLocation)
    {
      if (other is CircleShape2D circle)
      {
        var circleCenter = new Point(circle.Size.Width / 2, circle.Size.Height / 2);
        return ShapeT.IsCircleIntersectingConvex(circleCenter, circle.Radius, _vertices);
      }
      else
      {
        var otherVertices = other.Vertices;

        var otherAsPoly = new ConvexPolygon2D(otherVertices);

        return Intersect(otherAsPoly, thisLocation, otherLocation);
      }
    }

    public bool Intersect(ConvexPolygon2D other, Point thisLocation, Point otherLocation)
    {
      int axisCount = _vertices.Count + other._vertices.Count;
      Point[] axes = new Point[axisCount];
      int k = 0;

      for (int i = 0; i < _vertices.Count; i++)
      {
        Point p1 = _vertices[i] + thisLocation;
        Point p2 = _vertices[(i + 1) % _vertices.Count] + thisLocation;

        Point edge = p2 - p1;
        Point axis = new Point(-edge.Y, edge.X);
        axes[k++] = axis;
      }

      for (int j = 0; j < other._vertices.Count; j++)
      {
        Point p1 = other._vertices[j] + otherLocation;
        Point p2 = other._vertices[(j + 1) % other._vertices.Count] + otherLocation;

        Point edge = p2 - p1;
        Point axis = new Point(-edge.Y, edge.X);
        axes[k++] = axis;
      }

      foreach (var axis in axes)
      {
        this.Project(thisLocation,axis, out float minA, out float maxA);
        other.Project(otherLocation, axis, out float minB, out float maxB);

        if (maxA < minB || maxB < minA)
          return false;
      }

      return true;
    }

    private void Project(
        Point location,
        Point axis,
        out float min,
        out float max)
    {
      float dot = CordTools.Dot(_vertices[0] + location, axis);
      min = dot;
      max = dot;

      for (int i = 1; i < _vertices.Count; i++)
      {
        Point v = _vertices[i] + location;
        dot = CordTools.Dot(v, axis);

        if (dot < min) min = dot;
        if (dot > max) max = dot;
      }
    }

    public bool RayIntersect(
        Vector2 rayOrigin,
        Vector2 rayDir,
        float maxLength,
        Vector2 position,
        out Vector2 hitPoint,
        out float distance)
    {
      throw new NotImplementedException();
    }


    public IShape2D Clone()
    {
      return new ConvexPolygon2D(_vertices);
    }
  }
}
