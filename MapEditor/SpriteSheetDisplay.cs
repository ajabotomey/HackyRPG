using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{
    class SpriteSheetDisplay : GraphicsDeviceControl
    {
        Editor editor;
        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteSheet spritesheet;

        List<Tile> TileSet;
        int tileCount = 0;

        public SpriteSheetDisplay(ref Editor editor)
        {
            this.editor = editor;
        }

        protected override void Initialize()
        {
            // Point this to the game content folder
            content = new ContentManager(Services, "../../../HackyRPG/Content");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            TileSet = new List<Tile>();

            Application.Idle += delegate { Invalidate(); };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Unload();
            }

            base.Dispose(disposing);
        }

        public void LoadSpriteSheet(string filename)
        {
            // Assume that all tiles are in the sprites folder
            string filePath = "Sprites/" + filename;
            Texture2D texture = content.Load<Texture2D>(filePath);
            spritesheet = new SpriteSheet(texture);

            // Now to iterate through the entire spritesheet and define each individual sprite
            int tilesX = texture.Width / 32;
            int tilesY = texture.Height / 32;

            for (int i = 0; i < tilesY; i++)
            {
                for (int j = 0; j < tilesX; j++)
                {
                    // Figure out a key
                    spritesheet.AddSourceSprite(tileCount, new Rectangle(j * 32, i * 32, 32, 32));
                    Tile tile = new Tile(i, j);
                    TileSet.Add(tile);
                    tileCount++;
                }
            }
        }

        protected override void Draw()
        {
            spriteBatch.Begin();

            Console.WriteLine("Drawing here");

            if (spritesheet != null)
            {
                for (int i = 0; i < tileCount; i++)
                {
                    Rectangle source;
                    spritesheet.GetRectangle(ref i, out source);
                    TileSet[i].Draw(spritesheet.Texture, source, spriteBatch);
                }
            }

            spriteBatch.End();
        }
    }
}
