using System.Collections.Generic;
using Amethyst.Tools;
using Microsoft.Xna.Framework;

namespace Amethyst.Geometry
{
    public class ConvexPolygon2D
    {
        private readonly List<Vector2> _vertices;

        public IReadOnlyList<Vector2> Vertices => _vertices;

        public ConvexPolygon2D()
        {
            _vertices = new List<Vector2>();
        }

        public ConvexPolygon2D(IEnumerable<Vector2> vertices)
        {
            _vertices = new List<Vector2>(vertices);
        }

        public void Add(Vector2 v)
        {
            _vertices.Add(v);
        }

        public void Add(IEnumerable<Vector2> points)
        {
            _vertices.AddRange(points);
        }

        public void Add(params Vector2[] points)
        {
          _vertices.AddRange(points);
        }

        public bool Intersects(ConvexPolygon2D other)
        {
            List<Vector2> axes = new();

            for (int i = 0; i < _vertices.Count; i++)
            {
                Vector2 p1 = _vertices[i];
                Vector2 p2 = _vertices[(i + 1) % _vertices.Count];

                Vector2 edge = p2 - p1;
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axes.Add(axis);
            }

            for (int j = 0; j < other._vertices.Count; j++)
            {
                Vector2 p1 = other._vertices[j];
                Vector2 p2 = other._vertices[(j + 1) % other._vertices.Count];

                Vector2 edge = p2 - p1;
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axes.Add(axis);
            }

            foreach (var axis in axes)
            {
                ProjectPolygon(this, axis, out float minA, out float maxA);
                ProjectPolygon(other, axis, out float minB, out float maxB);

                if (maxA < minB || maxB < minA)
                    return false;
            }

            return true;
        }

        private static void ProjectPolygon(
            ConvexPolygon2D polygon,
            Vector2 axis,
            out float min,
            out float max)
        {
            float dot = Vector2.Dot(polygon._vertices[0], axis);
            min = dot;
            max = dot;

            foreach (var v in polygon._vertices)
            {
                dot = Vector2.Dot(v, axis);
                if (dot < min) min = dot;
                if (dot > max) max = dot;
            }
        }

        public void Draw(Color color, int thickness = 1)
        {
          for (int i = 0; i < Vertices.Count - 1; i++)
          {
            Core.Canvas.Submit(GraphicsE.Line(Vertices[i], Vertices[i + 1], color, thickness));
          }
        }
    }
}
