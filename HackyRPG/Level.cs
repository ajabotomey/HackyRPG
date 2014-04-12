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

        private int[][] level;
        int rows, columns;

        private List<Tile> tileMap;

        public Level(Game game)
        {
            contentManager = new ContentManager(game.Services);
            contentManager.RootDirectory = "Content";

            //// Load in the spritesheets
            //Texture2D groundFile = contentManager.Load<Texture2D>("Sprites/Tiles");
            //groundSprites = new SpriteSheet(groundFile);
            //groundSprites.AddSourceSprite((int)TileName.Grass, new Rectangle(32, 0, 32, 32));
            //groundSprites.AddSourceSprite((int)TileName.Water, new Rectangle(128, 0, 32, 32));

            // Create the tile map
            tileMap = new List<Tile>();

            // Now load in the level file
            LoadLevel();
        }

        public void LoadLevel()
        {
            //using (TextReader reader = File.OpenText("Content/Levels/test_level.txt"))
            //{
            //    string sizeText = reader.ReadLine();
            //    string[] size = sizeText.Split(' ');

            //    columns = int.Parse(size[0]);
            //    rows = int.Parse(size[1]);

            //    level = new int[rows][];
            //    for (int i = 0; i < rows; i++)
            //    {
            //        level[i] = new int[columns];
            //    }

            //    // Read the empty line
            //    reader.ReadLine();

            //    for (int j = 0; j < rows; j++)
            //    {
            //        // Start reading the map
            //        string text = reader.ReadLine();
            //        string[] bits = text.Split(' ');
            //        Console.WriteLine("Size of line is {0}", bits.Length);
            //        //Console.WriteLine("size of first row is: {0}", level[0].Length);

            //        for (int k = 0; k < bits.Length; k++)
            //        {
            //            level[j][k] = int.Parse(bits[k]);
            //            //Console.Write(level[j][k]);

            //            Tile newTile = new Tile(j, k, level[j][k]);
            //            tileMap.Add(newTile);
            //        }

            //        Console.WriteLine("");
            //    }
            //}

            // Loading the level through XML
            FileStream fileStream = File.Open(contentManager.RootDirectory + "/Levels/test_level.xml", FileMode.Open);
            StreamReader fileStreamReader = new StreamReader(fileStream);
            string xml = fileStreamReader.ReadToEnd();
            fileStreamReader.Close();
            fileStream.Close();
            XDocument doc = XDocument.Parse(xml);
            

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

            // Now to create the level
            level = new int[rows][];
            for (int l = 0; l < rows; l++)
            {
                level[l] = new int[columns];
            }

            // Time to populate with tiles
            for (int j = 0; j < rows; j++)
            {
                // Start reading the map
                string text = gridRows[j];
                string[] bits = text.Split(' ');
                Console.WriteLine("Size of line is {0}", bits.Length);
                //Console.WriteLine("size of first row is: {0}", level[0].Length);

                for (int k = 0; k < bits.Length; k++)
                {
                    level[j][k] = int.Parse(bits[k]);
                    //Console.Write(level[j][k]);

                    Tile newTile = new Tile(j, k, level[j][k]);
                    tileMap.Add(newTile);
                }

                Console.WriteLine("");
            }
        }

        public void UnloadLevel()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw each tile onto the screen from text file
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Rectangle source;
                    groundSprites.GetRectangle(ref level[i][j], out source);
                    spriteBatch.Draw(groundSprites.Texture, new Vector2(j * 32, i * 32), source, Color.White);
                }
            }
        }
    }
}
