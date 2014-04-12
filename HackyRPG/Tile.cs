using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HackyRPG
{
    public enum TileName
    {
        Grass = 0,
        Water = 1
    }

    public class Tile
    {
        private int heuristicCost;
        private Vector2 location;
        private TileName tileValue;

        public int TileValue
        {
            get
            {
                return (int)tileValue;
            }

            set
            {
                tileValue = (TileName)Enum.ToObject(typeof(TileName), value);
            }
        }

        public Tile(int x, int y, int tileType)
        {
            tileValue = (TileName)Enum.ToObject(typeof(TileName), tileType);

            if (tileValue == TileName.Water)
                heuristicCost = 100;
            else
                heuristicCost = 1;

            location = new Vector2(y * 32, x * 32);
        }

        public int GetCost()
        {
            return heuristicCost;
        }

        public void Draw(Texture2D texture, Rectangle source, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, location, source, Color.White);
        }
    }
}
