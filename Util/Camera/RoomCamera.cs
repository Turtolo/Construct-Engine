using ConstructEngine.Physics;
using Microsoft.Xna.Framework;
using System;
using ConstructEngine.Components.Entity;
using ConstructEngine.Util;
using ConstructEngine.Util.Tween;

namespace ConstructEngine.Graphics;

public class RoomCamera : Camera
{
    public Matrix Transform { get; private set; }
    public float Zoom { get; set; } = 1f;

    private Tween cameraXTween;
    private Tween cameraYTween;
    Vector2 cameraTargetPosition = Vector2.Zero;

    public float LerpFactor { get; set; } = 0.1f;

    public Rectangle CameraRectangle;
    
    private bool Entered = false;







    public RoomCamera(Rectangle target, float zoom)
    {
        Zoom = zoom;


        cameraPosition = new Vector2(Core.VirtualWidth / 2, Core.VirtualHeight / 2);

        CameraRectangle = new Rectangle
        (
            (int)(cameraPosition.X - (Core.VirtualWidth / (2 * Zoom))),
            (int)(cameraPosition.Y - (Core.VirtualHeight / (2 * Zoom))),
            (int)(Core.VirtualWidth / Zoom),
            (int)(Core.VirtualHeight / Zoom)
        );


    }
    
    private void UpdateCameraRectangle()
    {
        CameraRectangle.X = (int)(cameraPosition.X - (Core.VirtualWidth / (2 * Zoom)));
        CameraRectangle.Y = (int)(cameraPosition.Y - (Core.VirtualHeight / (2 * Zoom)));
        CameraRectangle.Width = (int)(Core.VirtualWidth / Zoom);
        CameraRectangle.Height = (int)(Core.VirtualHeight / Zoom);
    }
    
    
    public void Follow(Rectangle target, Entity targetEntity)
    {
        var side = CollisionHelper.GetCameraEdge(target, CameraRectangle);
        
        if (!Entered)
        {


            if (side == CollisionSide.Left)
            {
                
                cameraTargetPosition.X = CameraRectangle.X - CameraRectangle.Width + CameraRectangle.Width / 2;
                CameraRectangle.X -= CameraRectangle.Width;
                
                targetEntity.KinematicBase.Locked = true;
                
                targetEntity.KinematicBase.Collider.Rect.X -= 10;
                
                cameraXTween = new Tween(
                    cameraPosition.X,
                    cameraTargetPosition.X,
                    0.5f,
                    EasingFunctions.Linear
                );
            }

            if (side == CollisionSide.Right)
            {
                
                
                cameraTargetPosition.X = CameraRectangle.X + CameraRectangle.Width + CameraRectangle.Width / 2;
                CameraRectangle.X += CameraRectangle.Width;
                
                targetEntity.KinematicBase.Locked = true;

                targetEntity.KinematicBase.Collider.Rect.X += 10;
                
                
                cameraXTween = new Tween(
                    cameraPosition.X,
                    cameraTargetPosition.X,
                    0.5f,
                    EasingFunctions.Linear
                );
                
                

            }

            if (side == CollisionSide.Top)
            {
                cameraTargetPosition.Y = CameraRectangle.X - CameraRectangle.Height + CameraRectangle.Height / 2;
                CameraRectangle.Y -= CameraRectangle.Height;
                
                targetEntity.KinematicBase.Locked = true;
                
                
                
                cameraYTween = new Tween(
                    cameraPosition.Y,
                    cameraTargetPosition.Y,
                    0.5f,
                    EasingFunctions.Linear
                );
            }

            if (side == CollisionSide.Bottom)
            {
                cameraTargetPosition.Y = CameraRectangle.X + CameraRectangle.Height + CameraRectangle.Height / 2;
                CameraRectangle.Y += CameraRectangle.Height;
                
                targetEntity.KinematicBase.Locked = true;
                
                cameraYTween = new Tween(
                    cameraPosition.Y,
                    cameraTargetPosition.Y,  
                    0.5f,
                    EasingFunctions.Linear
                );

            }
            
        }

        if (cameraXTween != null)
        {
            if (cameraXTween.IsFinished())
            {
                targetEntity.KinematicBase.Locked = false;
            }
        }
        
        if (cameraYTween != null)
        {
            if (cameraYTween.IsFinished())
            {
                targetEntity.KinematicBase.Locked = false;
            }
        }

        

        Entered = side != CollisionSide.None;
        
        if (cameraXTween != null && !cameraXTween.IsFinished())
        {
            cameraXTween.Update(Core.DeltaTime);
            cameraPosition.X = cameraXTween.GetCurrentValue(cameraXTween.Normal);
        }

        if (cameraYTween != null && !cameraYTween.IsFinished())
        {
            cameraYTween.Update(Core.DeltaTime);
            cameraPosition.Y = cameraYTween.GetCurrentValue(cameraYTween.Normal);
        }

        UpdateCameraRectangle();

        var position = Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0f);
        var offset = Matrix.CreateTranslation(Core.VirtualWidth / 2f, Core.VirtualHeight / 2f, 0f);
        var scale = Matrix.CreateScale(Zoom, Zoom, 1f);

        Transform = position * scale * offset;
        
        
    }
}