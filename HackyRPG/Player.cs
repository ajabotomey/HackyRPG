using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackyRPG
{
    public class Player:GameObject
    {
        private GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One); // So it runs on Vita
        private KeyboardState currentKeyboardState;
        private Vector2 velocity = Vector2.Zero;
        private const float speed = 2.0f;
        private const float tileSize = 32.0f;
        private float distance;

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

        public Player(Texture2D texture, int x, int y):base(texture)
        {
            position.X = x;
            position.Y = y;
            name = "Player";
        }

        public void Update(GameTime gameTime, Level level)
        {
            HandleInput();

            // Update Position
            //position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity;

            CollidesTile(level);

            // Handle Collisions - Tile Collision is handled in the Tile Class

            //Console.WriteLine("Player Position is at {0},{1}", position.X, position.Y);

            position.X = (float)Math.Round(position.X);
            position.Y = (float)Math.Round(position.Y);

            // Must be cleared or else the player will keep moving even though a key has not been pressed
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
                velocity = new Vector2(0.0f, -speed);
            }

            if (currentKeyboardState.IsKeyDown(Keys.A) || currentKeyboardState.IsKeyDown(Keys.Left))
            {
                velocity = new Vector2(-speed, 0.0f);
            }

            if (currentKeyboardState.IsKeyDown(Keys.S) || currentKeyboardState.IsKeyDown(Keys.Down))
            {
                velocity = new Vector2(0.0f, speed);
            }

            if (currentKeyboardState.IsKeyDown(Keys.D) || currentKeyboardState.IsKeyDown(Keys.Right))
            {
                velocity = new Vector2(speed, 0.0f);
            }
        }

        private void HandleGamePadInput()
        {
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            if (currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                velocity = new Vector2(0.0f, -speed);
            }

            if (currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                velocity = new Vector2(-speed, 0.0f);
            }

            if (currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                velocity = new Vector2(0.0f, speed);
            }

            if (currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                velocity = new Vector2(speed, 0.0f);
            }
        }

        public void CollidesWith(GameObject gameObject)
        {
            
        }

        public void CollidesTile(Level level)
        {
            foreach (Tile t in level.TileMap)
            {
                if (BoundBox.Intersects(t.BoundBox))
                {
                    if (t.GetTileValue() == TileName.Water)
                    {
                        position -= velocity;
                    }
                }
            }
        }

        public void SnapToGrid()
        {
            distance += Math.Abs(velocity.X) + Math.Abs(velocity.Y);

            //test if we've travelled far enough
            if (distance >= tileSize)
            {
                //reset distance
                distance = 0.0f;

                //stop
                velocity = Vector2.Zero;

                int tileX = (int)((position.X / tileSize) * tileSize);
                int tileY = (int)((position.Y / tileSize) * tileSize);
            }
        }
    }
}
