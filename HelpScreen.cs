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
    //---------------------------------------Game state 8-------------------------------------
    class HelpScreen : RC_GameStateParent
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
        SpriteFont font3;

        public override void LoadContent()
        {
            //Set the screen window
            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);

            font1 = Content.Load<SpriteFont>("spritefont1");
            font2 = Content.Load<SpriteFont>("spritefont3");
            font3 = Content.Load<SpriteFont>("spritefont4");

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
            spriteBatch.DrawString(font1, "HELP", new Vector2(800, 400), Color.Brown);
            spriteBatch.DrawString(font3, "Use arrow key to move to right, left and jump to top", new Vector2(200, 500), Color.Brown);
            spriteBatch.DrawString(font3, "Use Z, X, C to shoot", new Vector2(200, 600), Color.Brown);
            spriteBatch.DrawString(font3, "Enter R back to game", new Vector2(200, 700), Color.Brown);


        }
    }
}
