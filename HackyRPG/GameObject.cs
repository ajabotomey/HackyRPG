using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackyRPG
{
    public class GameObject
    {
        public Texture2D sprite;
        public string name;
        public Vector2 position;

        public GameObject(Texture2D texture)
        {
            sprite = texture;
        }

        public Vector2 Position
        {
            set
            {
                position = value;
            }

            get
            {
                return position;
            }
        }

        public Rectangle BoundBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, sprite.Width * 2, sprite.Height * 2);
            }
        }

        public Vector2 bbCenter
        {
            get
            {
                return new Vector2(position.X - sprite.Width / 2, position.Y - sprite.Height / 2);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, BoundBox, Color.White);
        }

        public bool AABBCollisionText(GameObject gameObject)
        {
            

            return false;
        }
    }
}
