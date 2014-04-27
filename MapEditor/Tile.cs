using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{
    public enum TileName
    {
        Grass = 0,
        Water = 1
    }

    public class TileDetails
    {
        private string name;
        private int tileValue;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int TileValue
        {
            get
            {
                return tileValue;
            }

            set
            {
                tileValue = value;
            }
        }
    }

    public class Tile
    {
        private int heuristicCost;
        private Vector2 location;
        private TileName tileValue;
        private TileDetails tileDetails;
        private const int tileSize = 32;
        private bool collidable;

        public int Cost
        {
            get
            {
                return heuristicCost;
            }
        }

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

        public bool Collidable
        {
            get
            {
                return collidable;
            }

            set
            {
                collidable = value;
            }
        }

        public Tile(int x, int y, int tileType)
        {
            tileValue = (TileName)Enum.ToObject(typeof(TileName), tileType);

            if (tileValue == TileName.Water)
            {
                heuristicCost = 100;
                collidable = true;
            }
            else
            {
                heuristicCost = 1;
                collidable = false;
            }

            location = new Vector2(y * 32, x * 32);
        }

        public Tile(int x, int y)
        {
            location = new Vector2(y * 32, x * 32);
        }

        public void Draw(Texture2D texture, Rectangle source, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, location, source, Color.White);
        }
    }
}
