using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackyRPG
{
    public class Dudette : GameObject
    {
        public Dudette(Texture2D texture, int x, int y) : base(texture)
        {
            position.X = x;
            position.Y = y;
            name = "Dudette";
        }
    }
}
