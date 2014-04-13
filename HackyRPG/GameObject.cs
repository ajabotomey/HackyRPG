using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackyRPG
{
    public abstract class GameObject
    {
        public Texture2D sprite;
        public string name;
        private Vector2 position;
        private string type;

        public GameObject(Texture2D texture, int x, int y)
        {
            sprite = texture;
            position = new Vector2(x, y);
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

        public String Type
        {
            get;
            set;
        }

        public virtual void Update(GameTime gameTime, Level level)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, BoundBox, Color.White);
        }
    }
}
