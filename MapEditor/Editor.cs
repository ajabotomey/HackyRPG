using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor
{
    public class Editor : GraphicsDeviceControl
    {
        ContentManager content;
        SpriteBatch spriteBatch;
        Level currentLevel;

        public event EventHandler OnInitialize;

        protected override void Initialize()
        {
            // Point this to the game content folder
            content = new ContentManager(Services, "../../../HackyRPG/Content");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Application.Idle += delegate { Invalidate(); };

            if (OnInitialize != null)
            {
                OnInitialize(this, null);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Unload();
            }

            base.Dispose(disposing);
        }

        protected override void Draw()
        {
            spriteBatch.Begin();

            GraphicsDevice.Clear(Color.Black);

            // Draw XNA related stuff here
            //Console.WriteLine("Drawing here");
            if (currentLevel != null)
                currentLevel.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void LoadLevel(string filepath)
        {
            // Obviously it will be in XML for now but I will eventually swap to JSON
            // XML loading will be done in the level class
            Console.WriteLine("Hello World!");
            // Figure out a way to get the content path of the game
            currentLevel = new Level(content, filepath);
        }
    }
}
