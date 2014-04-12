using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackyRPG
{
    public class Player:GameObject
    {
        private GamePadState currentGamePadState;
        private KeyboardState lastKeyboardState;
        private KeyboardState currentKeyboardState;
        private Vector2 velocity = Vector2.Zero;
        private const float speed = 32.0f;
        private const float tileSize = 32.0f;
        private float distance;

        public Player(Texture2D texture, int x, int y):base(texture)
        {
            position.X = x;
            position.Y = y;
            name = "Player";
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();

            // Update Position
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Tile Collision first

            // Handle Collisions

            Console.WriteLine("Player Position is at {0},{1}", position.X, position.Y);

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
            lastKeyboardState = Keyboard.GetState();
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

        public void CollideWithTile()
        {
            // Get the tile position and check collisions within 1 tile of the current position
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
