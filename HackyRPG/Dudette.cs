using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackyRPG
{
    // Example of a dynamic collision object
    public class Dudette : DynamicObject
    {
        public Dudette(Texture2D texture, int x, int y) : base(texture, x, y)
        {
            name = "Dudette";
        }

        public override void Update(GameTime gameTime, Level level)
        {
            base.Update(gameTime, level);
        }
    }
}
