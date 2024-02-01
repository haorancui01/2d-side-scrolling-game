using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using RC_Framework;

namespace GPT_FinalGame
{
    class PauseScreen : RC_GameStateParent  //level 7
    {
            //Keyboard control
            KeyboardState keyState;
            KeyboardState preKeyState;

            //Variable for screen
            int screenWidth;
            int screenHeight;
            Rectangle screenRect;

            //Bounding box boolean value
            bool showbb = false;

            SpriteFont font1;
            SpriteFont font2;

            public override void LoadContent()
            {
                //Set the screen window
                screenWidth = graphicsDevice.Viewport.Width;
                screenHeight = graphicsDevice.Viewport.Height;
                screenRect = new Rectangle(0, 0, screenWidth, screenHeight);

                font1 = Content.Load<SpriteFont>("spritefont1");
                font2 = Content.Load<SpriteFont>("spritefont3");
            }

        public override void Update(GameTime gameTime)
        {
            preKeyState = keyState;
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.R) && preKeyState.IsKeyUp(Keys.R))
            {
                gameStateManager.pushLevel(1);
            }

        }
        public override void Draw(GameTime gameTime)
            {
                graphicsDevice.Clear(Color.Black);
                spriteBatch.DrawString(font1, "You have paused the game", new Vector2(100, 200), Color.Brown);
            spriteBatch.DrawString(font1, "Press 'R' back to the game", new Vector2(100, 300), Color.Brown);


        }
    }
    }
