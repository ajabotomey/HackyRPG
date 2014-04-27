using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackyRPG
{
    public class DynamicObject : GameObject
    {
        private Vector2 velocity = Vector2.Zero;

        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }

        public DynamicObject(Texture2D texture, int x, int y) : base(texture, x, y)
        {
            Type = "dynamic";
        }

        public override void Update(GameTime gameTime, Level level)
        {
            // First update the position
            Position += Velocity;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            // Handle tile collision
            bool collide = CollidesTile(level);

            // Handle Object Collision
            //CollidesObject(level);
        }

        public bool CollidesTile(Level level)
        {
            foreach (Tile t in level.TileMap)
            {
                if (BoundBox.Intersects(t.BoundBox))
                {
                    if (t.Collidable == true)
                    {
                        Position -= Velocity;
                        return true;
                    }
                }
            }

            return false;
        }

        public void CollidesObject(Level level)
        {
            foreach (DynamicObject d in level.ObjectMap)
            {
                if (d == this)
                    continue;

                if (d.BoundBox.Intersects(BoundBox))
                {
                    // Need to figure out a way to stop this if the object is against a collidable tile
                    Velocity = d.Velocity;

                    // Update the position last just in case the tile collision removes the velocity
                    Position += Velocity;
                    Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

                    CollidesTile(level);
                }
            }
            
            //if (name == "Player")
            //    return;

            //Player player = level.CurrentPlayer;

            //if (player.BoundBox.Intersects(BoundBox))
            //{
            //    // Need to figure out a way to stop this if the object is against a collidable tile
            //    Velocity = player.Velocity;

            //    // Update the position last just in case the tile collision removes the velocity
            //    Position += Velocity;
            //    Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            //    CollidesTile(level);
            //}
        }
    }
}
