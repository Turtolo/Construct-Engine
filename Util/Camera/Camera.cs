using System.Numerics;
using System.Runtime.Intrinsics.X86;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ConstructEngine.Graphics;

public class Camera
{
    public Vector2 cameraPosition = Vector2.Zero;
    public float Zoom;
    public static Camera CurrentCamera;
    public Camera()
    {
        CurrentCamera = this;
    }

    public (Vector2 TopLeft, Vector2 TopRight, Vector2 BottomLeft, Vector2 BottomRight) GetScreenEdges()
    {
        float halfWidth = (Core.VirtualWidth / 2f) / Zoom;
        float halfHeight = (Core.VirtualHeight / 2f) / Zoom;

        Vector2 topLeft = new Vector2(cameraPosition.X - halfWidth, cameraPosition.Y - halfHeight);
        Vector2 topRight = new Vector2(cameraPosition.X + halfWidth, cameraPosition.Y - halfHeight);
        Vector2 bottomLeft = new Vector2(cameraPosition.X - halfWidth, cameraPosition.Y + halfHeight);
        Vector2 bottomRight = new Vector2(cameraPosition.X + halfWidth, cameraPosition.Y + halfHeight);

        return (topLeft, topRight, bottomLeft, bottomRight);
    }
    
}