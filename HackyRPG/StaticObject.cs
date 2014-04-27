using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackyRPG
{
    // Static objects have no physics what so ever. Some can be destroyed but that will 
    // be a separate subclass of StaticObject
    public class StaticObject : GameObject
    {
        public StaticObject(Texture2D texture, int x, int y) : base(texture, x, y)
        {
            Type = "static";
        }

        public override void Update(GameTime gameTime, Level level)
        {
            foreach (GameObject go in level.ObjectMap)
            {
                if (go.Type == "static")
                {
                    continue;
                }

                DynamicObject d = (DynamicObject)go;

                if (BoundBox.Intersects(d.BoundBox))
                {
                    d.Position -= d.Velocity;
                }
            }

            base.Update(gameTime, level);
        }


    }
}
