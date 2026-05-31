#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Amethyst.Tools;
using Microsoft.Xna.Framework;

namespace Amethyst.Geometry
{
  public struct ConvexSegment2D
  {
    private readonly Point[] _vertices;

    public ReadOnlySpan<Point> Vertices { get => _vertices; }
    
    ///<summary>
    /// The size of this polygon, in integer values.
    ///</summary>
    ///<remarks>
    /// Since a polygon is inherited dynamic, this returns more so a 'bounding-box' of this polygon.
    ///</remarks>
    public Extent Size
    {
      get
      {
        if (_vertices == null || _vertices.Length < 3)
          return Extent.Empty;
        
        var min = new Point(
            _vertices.Min(v => v.X),
            _vertices.Min(v => v.Y));

        var max = new Point(
            _vertices.Max(v => v.X),
            _vertices.Max(v => v.Y));

        return new Extent(max.X - min.X, max.Y - min.Y);
      }
    }

    ///<summary>
    /// Creates a new convex-segment with params of <see cref="Point"/>.
    ///</summary>
    ///<remarks>
    /// Since this is a convex-segment it requires that no interior angles are above π-rad (180° degrees).
    ///</remarks>
    public ConvexSegment2D(params Point[] vertices)
    {
      _vertices = vertices ?? Array.Empty<Point>();
    }
    
    ///<summary>
    /// Creates a new convex-segment with a read-only-span of <see cref="Point"/>.
    ///</summary>
    ///<remarks>
    /// Since this is a convex-segment it requires that no interior angles are above π-rad (180° degrees).
    ///</remarks>
    public ConvexSegment2D(ReadOnlySpan<Point> vertices)
    {
      _vertices = vertices.ToArray() ?? Array.Empty<Point>();
    }

    ///<summary>
    /// Creates a new convex-segment with an <see cref="IEnumerable{T}"/> of type <see cref="Point"/>.
    ///</summary>
    ///<remarks>
    /// Since this is a convex-segment it requires that no interior angles are above π-rad (180° degrees).
    ///</remarks>
    public ConvexSegment2D(IEnumerable<Point> vertices)
    {
      _vertices = vertices?.ToArray() ?? Array.Empty<Point>();
    }
    
    ///<summary>
    /// Returns a list of the edges this polygon contains, instead of the regular pure points.
    ///</summary>
    public List<(Point A, Point B)> GetEdges()
    {
      var edges = new List<(Point A, Point B)>();

      if (_vertices.Length < 2)
        return edges;
      
      for (int i = 0; i < _vertices.Length; i++)
      {
        Point a = _vertices[i];
        Point b = _vertices[(i + 1) % _vertices.Length];

        edges.Add((a, b));
      }
      
      return edges;
    }
    
    ///<summary>
    /// Returns this polygon as an axis-aligned-polygon.
    ///</summary>
    public Rectangle GetAABB(Point position)
    {
      if (Vertices == null || _vertices.Length == 0)
        return Rectangle.Empty;

      int minX = int.MaxValue;
      int minY = int.MaxValue;
      int maxX = int.MinValue;
      int maxY = int.MinValue;

      foreach (var v in Vertices)
      {
        var world = v + position;

        if (world.X < minX) minX = world.X;
        if (world.Y < minY) minY = world.Y;
        if (world.X > maxX) maxX = world.X;
        if (world.Y > maxY) maxY = world.Y;
      }

      return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }
    
    ///<summary>
    /// Checks if this polygon contains a specified point, at a certain location.
    ///</summary>
    ///<param name="location">The location which this polygon is at, acts as a sort of 'offset'.</param>
    public bool Contains(Point location, Point point)
    {
      if (_vertices == null || _vertices.Length < 3)
        return false;

      Point p = point - location;

      bool? sign = null;

      for (int i = 0; i < _vertices.Length; i++)
      {
        Point a = _vertices[i];
        Point b = _vertices[(i + 1) % _vertices.Length];

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
    
    ///<summary>
    /// Checks if this segmet intersects with another.
    ///</summary>
    ///<remarks>
    /// This used the seperating-axis-theorem, projecting the segments onto a one dimensional plane.
    ///</remarks>
    public bool Intersect(ConvexSegment2D other, Point thisLocation, Point otherLocation)
    {
      int axisCount = _vertices.Length + other._vertices.Length;
      Point[] axes = new Point[axisCount];
      int k = 0;

      for (int i = 0; i < _vertices.Length; i++)
      {
        Point p1 = _vertices[i] + thisLocation;
        Point p2 = _vertices[(i + 1) % _vertices.Length] + thisLocation;

        Point edge = p2 - p1;
        Point axis = new Point(-edge.Y, edge.X);
        axes[k++] = axis;
      }

      for (int j = 0; j < other._vertices.Length; j++)
      {
        Point p1 = other._vertices[j] + otherLocation;
        Point p2 = other._vertices[(j + 1) % other._vertices.Length] + otherLocation;

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
    
    ///<summary>
    /// Checks if this segment intersects with another.
    ///</summary>
    public bool IntersectsAt(Point offset, ConvexSegment2D other, Point thisLocation, Point otherLocation)
    {
      return Intersect(other, thisLocation + offset, otherLocation);
    }

    /// <summary>
    /// Projects the shape onto the specified axis and returns the interval
    /// occupied by the shape on that axis.
    /// </summary>
    public void Project(
        Point location,
        Point axis,
        out float min,
        out float max)
    {
      float dot = CordTools.Dot(_vertices[0] + location, axis);
      min = dot;
      max = dot;

      for (int i = 1; i < _vertices.Length; i++)
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
    
    ///<summary>
    /// Clones this segment.
    ///</summary>
    public ConvexSegment2D Clone()
    {
      return new ConvexSegment2D(_vertices.ToArray());
    }
  }
}
