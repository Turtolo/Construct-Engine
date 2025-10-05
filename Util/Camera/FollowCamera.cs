using ConstructEngine.Physics;
using Microsoft.Xna.Framework;
using System;
using ConstructEngine.Util.Tween;

namespace ConstructEngine.Graphics;

public class FollowCamera : Camera
{
    public bool XEnabled = true;
    public bool YEnabled = true;
    public Matrix Transform { get; private set; }
    public float Zoom { get; set; } = 1f;
    

    public float LerpFactor { get; set; } = 0.1f;
    
    public Rectangle? Bounds { get; set; } = null;
    
    public int? LimitLeft { get; set; }
    public int? LimitRight { get; set; }
    public int? LimitTop { get; set; }
    public int? LimitBottom { get; set; }

    public bool Locked { get; private set; } = false;
    private Vector2 lockedPosition;
    
    public Rectangle CameraRectangle { get; set; }


    public FollowCamera(float Zoom, bool XEnabled = true, bool YEnabled = true)
    {
        cameraPosition = Vector2.Zero;
        CameraRectangle = new Rectangle(0, 0, Core.VirutalWidth, Core.VirtualHeight);
        this.Zoom = Zoom;
        this.XEnabled = XEnabled;
        this.YEnabled = YEnabled;
    }
    
    public void Lock()
    {
        Locked = true;
        lockedPosition = cameraPosition;
    }
    

    public void Unlock()
    {
        Locked = false;
    }



    public void Follow(Rectangle target)
    {
        
        if (!Locked)
        {

            var targetPosition = new Vector2(target.X + target.Width / 2f, target.Y + target.Height / 2f);

            if (XEnabled)
                cameraPosition.X = float.Lerp(cameraPosition.X, targetPosition.X, LerpFactor);
            if (YEnabled)
                cameraPosition.Y = float.Lerp(cameraPosition.Y, targetPosition.Y, LerpFactor);

            float halfWidth = (Core.VirutalWidth / 2f) / Zoom;
            float halfHeight = (Core.VirtualHeight / 2f) / Zoom;

            if (LimitLeft.HasValue && LimitRight.HasValue)
            {
                float minX = LimitLeft.Value + halfWidth;
                float maxX = LimitRight.Value - halfWidth;
                cameraPosition.X = MathHelper.Clamp(cameraPosition.X, minX, maxX);
            }

            if (LimitTop.HasValue && LimitBottom.HasValue)
            {
                float minY = LimitTop.Value + halfHeight;
                float maxY = LimitBottom.Value - halfHeight;
                cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, minY, maxY);
            }
        }
        else
        {
            cameraPosition = lockedPosition;
        }

        var position = Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0f);
        var offset = Matrix.CreateTranslation(Core.VirutalWidth / 2f, Core.VirtualHeight / 2f, 0f);
        var scale = Matrix.CreateScale(Zoom, Zoom, 1f);
        
        
        
        

        Transform = position * scale * offset;
        
        Console.WriteLine((Core.VirutalWidth / 2f, Core.VirtualHeight / 2f));
    }



}
