using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConstructEngine.Physics;

namespace ConstructEngine.Components.Physics
{
    public class KinematicBase
    {
        public Rectangle Hitbox;


        public Vector2 Velocity;
        private float remainderX = 0;
        private float remainderY = 0;

    

        public bool Locked;

        public List<Collider> Colliders;

        public KinematicBase()
        {
            Colliders = Collider.ColliderList;
        }

        public void UpdateCollider(GameTime gameTime)
        {
            if (!Locked)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                Move(Velocity.X * deltaTime, Velocity.Y * deltaTime);

                foreach (var collider in Colliders)
                {
                    if (!collider.Enabled || collider.Velocity == Vector2.Zero)
                        continue;

                    Rectangle feetCheck = Hitbox;
                    feetCheck.Y += 1;

                    if (feetCheck.Intersects(collider.Rect))
                    {
                        Hitbox.X += (int)collider.Velocity.X;
                        Hitbox.Y += (int)collider.Velocity.Y;
                    }
                }
            }

            else
            {
                Velocity.X = 0;
                Velocity.Y = 0;
            }
        }


        public void Move(float moveX, float moveY)
        {
            remainderX += moveX;
            int mx = (int)MathF.Round(remainderX);
            remainderX -= mx;
            MoveX(mx);

            remainderY += moveY;
            
            int my = (int)MathF.Round(remainderY);
            remainderY -= my;
            MoveY(my);
            
        }

        public void MoveX(int amount)
        {
            int sign = Math.Sign(amount);
            
            while (amount != 0)
            {
                Rectangle test = Hitbox;
                
                test.X += sign;
                
                

                if (!IsColliding(test))
                    Hitbox = test;
                else
                {
                    Velocity.X = 0;
                    break;
                }

                amount -= sign;
                
                
            }
        }

        public void MoveY(int amount)
        {
            int sign = Math.Sign(amount);
            while (amount != 0)
            {
                Rectangle test = Hitbox;
                test.Y += sign;

                if (!IsColliding(test))
                    Hitbox = test;
                else
                {
                    Velocity.Y = 0;
                    break;
                }

                amount -= sign;
            }
        }

        public bool IsOnGround()
        {
            Rectangle test = Hitbox;
            test.Y += 1;
            return IsColliding(test);
        }

        public bool IsOnWall()
        {
            Rectangle leftCheck = Hitbox;
            leftCheck.X -= 1;

            Rectangle rightCheck = Hitbox;
            rightCheck.X += 1;

            foreach (var collider in Colliders)
            {
                if (!collider.Enabled)
                    continue;

                if (leftCheck.Intersects(collider.Rect) || rightCheck.Intersects(collider.Rect))
                    return true;
            }

            return false;
        }

        public bool IsColliding(Rectangle rect)
        {
            foreach (var collider in Colliders)
            {
                if (!collider.Enabled)
                {
                    continue;
                }
                if (rect.Intersects(collider.Rect))
                    return true;
            }
            return false;
        }
    }
}