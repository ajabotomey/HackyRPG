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
        private Player player;
        private Dudette dudette;

        private List<Tile> tileMap;
        private List<GameObject> objectMap;

        public List<Tile> TileMap
        {
            get
            {
                return tileMap;
            }
        }

        public List<GameObject> ObjectMap
        {
            get
            {
                return objectMap;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return player;
            }
        }

        public Level(Game game, string filePath)
        {
            contentManager = new ContentManager(game.Services);
            contentManager.RootDirectory = "Content";

            // Create the tile map
            tileMap = new List<Tile>();
            objectMap = new List<GameObject>();

            // Now load in the level file
            LoadLevel(filePath);
        }

        public void LoadLevel(string filePath)
        {
            // Loading the level through XML
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

            // Read in objects now. Rather than wait until we load the level completely, we need to create these objects now because they have a lot of information

            // Read in the player first
            XElement objectElement = doc.Element("level").Element("objects");
            string playerSprite = objectElement.Element("player").Element("sprite").Attribute("path").Value;
            Texture2D playerTexture = contentManager.Load<Texture2D>(playerSprite);
            int playerPositionX = int.Parse(objectElement.Element("player").Element("position").Element("x").Value);
            int playerPositionY = int.Parse(objectElement.Element("player").Element("position").Element("y").Value);
            player = new Player(playerTexture, playerPositionX, playerPositionY);

            // Read in the dudette for now
            string dudetteSprite = objectElement.Element("dudette").Element("sprite").Attribute("path").Value;
            Texture2D dudetteTexture = contentManager.Load<Texture2D>(dudetteSprite);
            int dudettePositionX = int.Parse(objectElement.Element("dudette").Element("position").Element("x").Value);
            int dudettePositionY = int.Parse(objectElement.Element("dudette").Element("position").Element("y").Value);
            dudette = new Dudette(dudetteTexture, dudettePositionX, dudettePositionY);

            // Read in the other objects
            XElement objectList = objectElement.Element("objectList");
            int objCount = 0;
            for (XElement firstObj = (XElement)objectList.FirstNode; firstObj != null; firstObj = (XElement)firstObj.NextNode, objCount++)
            {
                // Get the sprite path
                string spritePath = firstObj.Element("sprite").Attribute("path").Value;
                Texture2D texture = contentManager.Load<Texture2D>(spritePath);

                // Get the origin position of the object
                int positionX = int.Parse(firstObj.Element("position").Element("x").Value);
                int positionY = int.Parse(firstObj.Element("position").Element("y").Value);

                if (firstObj.Element("object").Attribute("type").Value == "static")
                {
                    StaticObject staticObject = new StaticObject(texture, positionX, positionY);
                    objectMap.Add(staticObject);
                }

                if (firstObj.Element("object").Attribute("type").Value == "dynamic")
                {
                    DynamicObject dynamicObject = new DynamicObject(texture, positionX, positionY);
                    objectMap.Add(dynamicObject);
                }
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
            player.Update(gameTime, this);
            dudette.Update(gameTime, this);

            foreach (GameObject go in objectMap)
            {
                go.Update(gameTime, this);
            }

            // Zero the velocity for each Dynamic Object
            // This is done separately because we need the position to tell our characters to stop in static collision detection
            foreach (GameObject go in ObjectMap)
            {
                if (go.Type == "static")
                {
                    continue;
                }

                DynamicObject d = (DynamicObject)go;
                d.Velocity = Vector2.Zero;
            }
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

            foreach (GameObject go in objectMap)
            {
                go.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);
        }
    }
}
