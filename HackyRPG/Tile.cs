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
        private TileName value;

        public Tile(int x, int y, int tileValue)
        {
            value = (TileName)Enum.ToObject(typeof(TileName), tileValue);

            if (value == TileName.Water)
                heuristicCost = 100;
            else
                heuristicCost = 1;

            location = new Vector2(x, y);
        }

        public int GetCost()
        {
            return heuristicCost;
        }
    }
}
