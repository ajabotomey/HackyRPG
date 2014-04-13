using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HackyRPG
{
    public class Player : DynamicObject
    {
        private GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One); // So it runs on Vita
        private KeyboardState currentKeyboardState;
        private const float speed = 2.0f;
        private const float tileSize = 32.0f;

        public Player(Texture2D texture, int x, int y) : base(texture, x, y)
        {
            name = "Player";
        }

        public override void Update(GameTime gameTime, Level level)
        {
            HandleInput();

            base.Update(gameTime, level);
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

        public void CollidesWith(GameObject gameObject)
        {
            
        }
    }
}
