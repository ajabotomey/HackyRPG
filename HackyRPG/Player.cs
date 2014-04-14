using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackyRPG
{
    public class Player
    {
        private GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One); // So it runs on Vita
        private KeyboardState currentKeyboardState;
        private const float speed = 2.0f;
        private const float tileSize = 32.0f;
        public Texture2D sprite;
        private Vector2 position;
        private Vector2 velocity = Vector2.Zero;

        public Player(Texture2D texture, int x, int y)
        {
            sprite = texture;
            position = new Vector2(x, y);
        }

        public Vector2 Position
        {
            set
            {
                position = value;
            }

            get
            {
                return position;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }

        public Rectangle BoundBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, sprite.Width * 2, sprite.Height * 2);
            }
        }

        public void Update(GameTime gameTime, Level level)
        {
            HandleInput();

            // First update the position
            Position += Velocity;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            // Handle tile collision
            CollidesTile(level);

            // Handle Object Collision
            CollidesObjects(level);

            velocity = Vector2.Zero;
        }

        public void HandleInput()
        {
            if (currentGamePadState.IsConnected)
            {
                HandleGamePadInput();
                return;
            }

            HandleKeyboardInput();
        }

        private void HandleKeyboardInput()
        {
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.W) || currentKeyboardState.IsKeyDown(Keys.Up))
            {
                Velocity = new Vector2(0.0f, -speed);
            }

            if (currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.Left))
            {
                Velocity = new Vector2(-speed, 0.0f);
            }

            if (currentKeyboardState.IsKeyDown(Keys.S) || currentKeyboardState.IsKeyDown(Keys.Down))
            {
                Velocity = new Vector2(0.0f, speed);
            }

            if (currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.Right))
            {
                Velocity = new Vector2(speed, 0.0f);
            }
        }

        private void HandleGamePadInput()
        {
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            if (currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                Velocity = new Vector2(0.0f, -speed);
            }

            if (currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                Velocity = new Vector2(-speed, 0.0f);
            }

            if (currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                Velocity = new Vector2(0.0f, speed);
            }

            if (currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                Velocity = new Vector2(speed, 0.0f);
            }
        }

        public void CollidesObjects(Level level)
        {
            foreach (GameObject g in level.ObjectMap)
            {
                if (g.Type == "static")
                {
                    // Run static collision detection
                    if (BoundBox.Intersects(g.BoundBox))
                    {
                        Position -= Velocity;
                    }
                }
                else if (g.Type == "dynamic")
                {
                    // Run dynamic collision detection
                    // First, cast the object into a DynamicObject
                    DynamicObject d = (DynamicObject)g;
                    
                    if (BoundBox.Intersects(d.BoundBox))
                    {
                        d.Velocity = velocity;

                        d.Position += d.Velocity;
                        d.Position = new Vector2((float)Math.Round(d.Position.X), (float)Math.Round(d.Position.Y));
                        if (d.CollidesTile(level))
                        {
                            position -= d.Velocity;
                        }
                    }
                }
            }
        }

        public void CollidesTile(Level level)
        {
            foreach (Tile t in level.TileMap)
            {
                if (BoundBox.Intersects(t.BoundBox))
                {
                    if (t.Collidable == true)
                    {
                        Position -= Velocity;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, BoundBox, Color.White);
        }
    }
}
