﻿using System;
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

    public class TileDetails
    {
        private string name;
        private int heuristicCost;
        private bool collidable;

        public string Name
        {
            get;
            set;
        }

        public int Cost
        {
            get;
            set;
        }

        public bool Collidable
        {
            get;
            set;
        }
    }

    public class Tile
    {
        private int heuristicCost;
        private Vector2 location;
        private TileName tileValue;
        private const int tileSize = 32;
        private bool collidable;

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

        public Rectangle BoundBox
        {
            get
            {
                return new Rectangle((int)location.X, (int)location.Y, tileSize, tileSize);
            }
        }

        public Tile(int x, int y, int tileType, int cost, bool collidable)
        {
            tileValue = (TileName)Enum.ToObject(typeof(TileName), tileType);

            //if (tileValue == TileName.Water)
            //{
            //    heuristicCost = 100;
            //    collidable = true;
            //}
            //else
            //{
            //    heuristicCost = 1;
            //    collidable = false;
            //}

            heuristicCost = cost;
            this.collidable = collidable;

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
