using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HackyRPG
{
    public class Level
    {
        private ContentManager contentManager;
        private SpriteSheet groundSprites;

        int rows, columns;

        private List<Tile> tileMap;

        public List<Tile> TileMap
        {
            get
            {
                return tileMap;
            }
        }

        public Level(Game game, string filePath)
        {
            contentManager = new ContentManager(game.Services);
            contentManager.RootDirectory = "Content";

            // Create the tile map
            tileMap = new List<Tile>();

            // Now load in the level file
            LoadLevel(filePath);
        }

        public void LoadLevel(string filePath)
        {
            // Loading the level through XML
            //FileStream fileStream = File.Open(contentManager.RootDirectory + "/" + filePath, FileMode.Open);
            //StreamReader fileStreamReader = new StreamReader(fileStream);
            //string xml = fileStreamReader.ReadToEnd();
            //fileStreamReader.Close();
            //fileStream.Close();
            //XDocument doc = XDocument.Parse(xml);

            // XDocument doc = XDocument.Load("Application/" + contentManager.RootDirectory + "/" + filePath); // Vita version
            XDocument doc = XDocument.Load(contentManager.RootDirectory + "/" + filePath);
            
            // Now to get the raw data from the XML file

            // First the ground tile path
            XElement spritesheet = doc.Element("level").Element("spritesheet");
            string spriteFile = spritesheet.FirstAttribute.Value;

            // Now the specific tiles from the file
            XElement grassTile = (from el in spritesheet.Elements() where (string)el.Attribute("name") == "grass" select el).FirstOrDefault();
            int grassX = int.Parse(grassTile.Element("x").Value);
            int grassY = int.Parse(grassTile.Element("y").Value);
            int grassWidth = int.Parse(grassTile.Element("width").Value);
            int grassHeight = int.Parse(grassTile.Element("height").Value);
            int grassValue = int.Parse(grassTile.Element("value").FirstAttribute.Value);

            XElement waterTile = (from el in spritesheet.Elements() where (string)el.Attribute("name") == "water" select el).FirstOrDefault();
            int waterX = int.Parse(waterTile.Element("x").Value);
            int waterY = int.Parse(waterTile.Element("y").Value);
            int waterWidth = int.Parse(waterTile.Element("width").Value);
            int waterHeight = int.Parse(waterTile.Element("height").Value);
            int waterValue = int.Parse(waterTile.Element("value").FirstAttribute.Value);

            // Now to get the size
            XElement sizeElement = doc.Element("level").Element("size");
            columns = int.Parse(sizeElement.Element("width").Value);
            rows = int.Parse(sizeElement.Element("height").Value);

            // Now to get the level itself
            XElement gridElement = doc.Element("level").Element("grid");
            string[] gridRows = new string[rows];
            int i = 0;
            for (XElement firstRow = (XElement)gridElement.FirstNode; firstRow != null; firstRow = (XElement)firstRow.NextNode, i++)
            {
                gridRows[i] = firstRow.FirstAttribute.Value;
            }

            // Now that we have our raw data, time to load everything
            Texture2D groundFile = contentManager.Load<Texture2D>(spriteFile);
            groundSprites = new SpriteSheet(groundFile);
            groundSprites.AddSourceSprite((int)TileName.Grass, new Rectangle(grassX, grassY, grassWidth, grassHeight));
            groundSprites.AddSourceSprite((int)TileName.Water, new Rectangle(waterX, waterY, waterWidth, waterHeight));

            // Time to populate with tiles
            for (int j = 0; j < rows; j++)
            {
                // Start reading the map
                string text = gridRows[j];
                string[] bits = text.Split(' ');

                for (int k = 0; k < bits.Length; k++)
                {
                    Tile newTile = new Tile(j, k, int.Parse(bits[k]));
                    tileMap.Add(newTile);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void UnloadLevel()
        {
            contentManager.Unload();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile t in tileMap)
            {
                Rectangle source;
                int tileValue = t.TileValue;
                groundSprites.GetRectangle(ref tileValue, out source);
                t.Draw(groundSprites.Texture, source, spriteBatch);
            }
        }
    }
}
