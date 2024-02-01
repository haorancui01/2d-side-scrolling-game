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
    class EndScreen : RC_GameStateParent
    {
        //---------------------------------------Game state 5-------------------------------------

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

        ImageBackground bg;
        Texture2D tex_bg;

        public override void LoadContent()
        {
            //Set the screen window
            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);

            font1 = Content.Load<SpriteFont>("spritefont1");
            font2 = Content.Load<SpriteFont>("spritefont3");

            tex_bg = Content.Load<Texture2D>("endBg");
            bg = new ImageBackground(tex_bg, Color.White, graphicsDevice);

        }

        public override void Update(GameTime gameTime)
        {
            preKeyState = keyState;
            keyState = Keyboard.GetState();

            JumpGameLevel_3.in_music_background.Stop();

            bg.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Black);
            bg.Draw(spriteBatch);
            spriteBatch.DrawString(font1, "Game End", new Vector2(700, 400), Color.White);
            spriteBatch.DrawString(font1, "Your scores are: " + (JumpGameLevel_1.totalKilled + JumpGameLevel_2.totalKilled)*123, new Vector2(500, 600), Color.White);

        }
    }
}
