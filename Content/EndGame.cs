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

namespace GPT_FinalGame.Content
{
    class EndGame : RC_GameStateParent
    {

        Texture2D startBackground;
        ImageBackground level0 = null;

        // SpriteBatch spriteBatch;

        Texture2D mouseTexture;
        public float mouse_x = 0;
        public float mouse_y = 0;

        SoundEffect music;
        LimitSound limMusic;

        SpriteFont font1;
        SpriteFont font2;


        public KeyboardState keyState;
        public KeyboardState prevKeyState;

        public MouseState currentMouseState;
        public MouseState previousMouseState;



        public override void LoadContent()
        {
            font1 = Content.Load<SpriteFont>("spritefont1");
            font2 = Content.Load<SpriteFont>("spritefont3");
        }

        public override void Update(GameTime gameTime)
        {

            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            //Player Select START button to start the game
            if (keyState.IsKeyDown(Keys.Space) && prevKeyState.IsKeyUp(Keys.Space))
            {
                //gameStateManager.setLevel(3);
                //gameStateManager.setLevel(1);
                gameStateManager.pushLevel(1);
            }

            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Black);
            //level0.Draw(spriteBatch);
            spriteBatch.DrawString(font1, "GAME OVER", new Vector2(100, 200), Color.Brown);
            spriteBatch.DrawString(font1, "ENTER SPACE TO PLAY AGAIN", new Vector2(100, 400), Color.Brown);
        }
    }


}
