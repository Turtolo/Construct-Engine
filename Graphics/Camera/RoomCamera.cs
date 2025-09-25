using ConstructEngine.Physics;
using Microsoft.Xna.Framework;
using System;
using ConstructEngine.Util.Tween;

namespace ConstructEngine.Graphics;

public class RoomCamera
{
    public Matrix Transform { get; private set; }
    public float Zoom { get; set; } = 1f;

    public Vector2 cameraPosition = Vector2.Zero;

    public float LerpFactor { get; set; } = 0.1f;
    


    public RoomCamera()
    {
        
    }
    
    
    public void Follow(Rectangle target)
    {

        var targetPosition = new Vector2(target.X + target.Width / 2, target.Y + target.Height / 2);
        

        var position = Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0f);
        var offset = Matrix.CreateTranslation(Core.VirutalWidth / 2f, Core.VirtualHeight / 2f, 0f);
        var scale = Matrix.CreateScale(Zoom, Zoom, 1f);

        Transform = position * scale * offset;
    }
}