#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Amethyst.Tools;
using Microsoft.Xna.Framework;

namespace Amethyst.Geometry
{
  public struct PolygonShape2D : IShape2D
  {
    private ConvexSegment2D[] _segments;

    public Extent Size
    {
      get
      {
        var vertices = GetVertices().ToArray();

        if (vertices == null || vertices.Length < 3)
          return Extent.Empty;
        
        var min = new Point(
            vertices.Min(v => v.X),
            vertices.Min(v => v.Y));

        var max = new Point(
            vertices.Max(v => v.X),
            vertices.Max(v => v.Y));

        return new Extent(max.X - min.X, max.Y - min.Y);
      }
    }

    private Point[]? _vertices;
    
    ///<summary>
    /// Returns this polygon's vertices, as a combined product of the segments.
    ///</summary>
    ///<remarks>
    /// This is not a true representation of this polygon -- and should not be used for intersection, as it could form both a concave and a convex.
    ///</remarks>
    public ReadOnlySpan<Point> GetVertices()
    {
      if (_vertices != null)
        return _vertices;

      int count = 0;
      foreach (var seg in _segments)
        count += seg.Vertices.Length;

      _vertices = new Point[count];

      int offset = 0;
      foreach (var segment in _segments)
      {
          segment.Vertices.CopyTo(_vertices.AsSpan(offset));
          offset += segment.Vertices.Length;
      }
      
      return _vertices;
    }
    
    ///<summary>
    /// Creates a new polygon with params for <see cref="Point"/>.
    ///</summary>
    ///<remarks>
    /// In contrast to a segment, the inputted points to not have to be convex.
    /// If they are, for example in a concave formation -- it will be divided into convexes.
    ///</remarks>
    public PolygonShape2D(params Point[] vertices)
    {
      if (IsConvex(vertices))
        _segments = new[] { new ConvexSegment2D(vertices) };
      else
        _segments = Triangulate(vertices).ToArray();
    }

    ///<summary>
    /// Creates a new polygon with an <see cref="IEnumerable{T}"/> for <see cref="Point"/>.
    ///</summary>
    ///<remarks>
    /// In contrast to a segment, the inputted points to not have to be convex.
    /// If they are, for example in a concave formation -- it will be divided into convexes.
    ///</remarks>
    public PolygonShape2D(IEnumerable<Point> vertices)
    {
      var vert = vertices.ToArray();

      if (IsConvex(vert))
        _segments = new[] { new ConvexSegment2D(vert) };
      else
        _segments = Triangulate(vert).ToArray();
    }

    ///<summary>
    /// Creates a new polygon with explicitly set <see cref="ConvexSegment2D"/>.
    ///</summary>
    ///<remarks>
    /// As the user is the one providing the segments, not the engine -- these have to be convex.
    /// That is to say, each interior angle has to be less than π-rad (180° degrees).
    ///</remarks>
    public PolygonShape2D(IEnumerable<ConvexSegment2D> segments)
    {
      _segments = segments.ToArray();
    }
    
    ///<summary>
    /// Clones this polygon, the cloned polygon will have the same segments.
    ///</summary>
    public IShape2D Clone()
    {
      return new PolygonShape2D(_segments.ToArray());
    }
    
    ///<summary>
    /// Checks if any of the segments contain the specified point, at a specified location.
    ///</summary>
    ///<remarks>
    /// <see cref="point"/> and <see cref="location"/>, are both expected as world-space.
    ///</remarks>
    public bool Contains(Point point, Point location)
    {
      int n = _segments.Length;

      for (int i = 0; i < n; i++)
      {
        var seg = _segments[i];

        if (seg.Contains(location, point))
          return true;
        else
          continue;
      }

      return false;
    }
    
    ///<summary>
    /// Returns this polygon as an axis-aligned-bounding-box.
    ///</summary>
    public Rectangle GetAABB(Point position)
    {
      if (GetVertices() == null || GetVertices().Length == 0)
        return Rectangle.Empty;

      int minX = int.MaxValue;
      int minY = int.MaxValue;
      int maxX = int.MinValue;
      int maxY = int.MinValue;

      foreach (var v in GetVertices())
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
    /// Checks if this polygon intersects with another generic <see cref="IShape2D"/>, with an offset applied to <see cref="thisLocation"/>. 
    ///</summary>
    ///<param name="offset"> The offset applied to <see cref="thisLocation"/>.</param>
    ///<param name="other"> The shape this polygon checks against, <see cref="offset"/> is not applied to this.</param>
    ///<param name="thisLocation"> The location for this shape. </param>
    ///<param name="otherLocation"> The location for <see cref="other"/>.</param>
    public bool IntersectsAt(Point offset, IShape2D other, Point thisLocation, Point otherLocation)
    {
      return Intersect(other, thisLocation + offset, otherLocation);
    }
    
    ///<summary>
    /// Checks if this polygon intersects with another generic <see cref="IShape2D"/>. 
    ///</summary>
    ///<param name="other"> The shape this polygon checks against.</param>
    ///<param name="thisLocation"> The location for this shape. </param>
    ///<param name="otherLocation"> The location for <see cref="other"/>.</param>
    public bool Intersect(IShape2D other, Point thisLocation, Point otherLocation)
    {
      int n = _segments.Length;

      for (int i = 0; i < n; i++)
      {
        var seg = _segments[i];

        if (other is CircleShape2D circle)
        {
          List<Vector2> vectorList = new List<Vector2>(seg.Vertices.Length);

          for (int j = 0; j < seg.Vertices.Length; j++)
          {
            var ver = seg.Vertices[j] + thisLocation;
              vectorList.Add(ver.ToVector2());
          }

          var circleCenter = new Vector2(circle.Size.Width / 2, circle.Size.Height / 2) + otherLocation.ToVector2();
          return ShapeT.IsCircleIntersectingConvex(circleCenter, circle.Radius, vectorList);   
        }
        else
        {
          var otherVertices = other.GetVertices();

          var otherAsPoly = new ConvexSegment2D(otherVertices);
        
          if (seg.Intersect(otherAsPoly, thisLocation, otherLocation))
            return true;
          else
            continue;
        }
      }

      return false;
    }

    public bool RayIntersect(Vector2 rayOrigin, Vector2 rayDir, float maxLength, Vector2 shapePosition, out Vector2 hitPoint, out float distance)
    {
      throw new System.NotImplementedException();
    }
    
    ///<summary>
    /// Checks if the provided <see cref="vertices"/> -- make a convex polygon.
    /// It does this by checking how an edge trails off.
    ///</summary>
    ///<remarks>
    /// A convex polygon is a shape with no interior angles more π-rad (180° degrees).
    ///</remarks>
    public bool IsConvex(Point[] vertices)
    {
      int n = vertices.Length;
      if (vertices.Length < 4)
        return true;
      
      float sign = 0;

      for (int i = 0; i < n; i++)
      {
        Point a = vertices[i];
        Point b = vertices[(i + 1) % n];
        Point c = vertices[(i + 2) % n];

        float cross = Cross(a, b, c);

        if (Math.Abs(cross) < 1e-6f)
          continue;

        if (sign == 0)
          sign = cross;
        else if (cross * sign < 0)
          return false;
      }

      return true;
    }
    
    public static bool PointInTriangle(Point p, Point a, Point b, Point c)
    {
      float d1 = Cross(a, b, p);
      float d2 = Cross(b, c, p);
      float d3 = Cross(c, a, p);

      bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
      bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

      return !(hasNeg && hasPos);
    }

    private static List<ConvexSegment2D> Triangulate(Point[] vertices)
    {
      List<ConvexSegment2D> triangles = new();
      List<Point> remainingVertices = vertices.ToList();

      while (remainingVertices.Count > 3)
      {
        bool earFound = false;

        int n =  remainingVertices.Count;

        for (int i = 0; i < n; i++)
        {
          int prevIndex = (i - 1 + n) % n;
          int nextIndex = (i + 1) % n;
          
          Point a = remainingVertices[prevIndex];
          Point b = remainingVertices[i];
          Point c = remainingVertices[nextIndex];

          float cross = Cross(a, b, c);

          if (cross <= 0)
            continue;

          bool isEar = true;
          for (int j = 0; j < n; j++)
          {
            if (j == prevIndex || j == i || j == nextIndex)
              continue;

            if (PointInTriangle(remainingVertices[j], a, b, c))
            {
              isEar = false;
              break;
            } 
          }

          if (isEar)
          {
            triangles.Add(new ConvexSegment2D(a, b, c));
            remainingVertices.RemoveAt(i);
            earFound = true;
            break;
          }
        }

        if (!earFound)
          break;
      }

      return triangles;
    }

    private static float Cross(Point a, Point b, Point c)
    {
        return (b.X - a.X) * (c.Y - b.Y)
             - (b.Y - a.Y) * (c.X - b.X);
    }

  }
}
