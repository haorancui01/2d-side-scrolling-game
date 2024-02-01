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
    class splashScreen
    {
    }

    // -------------------------------------------------------- Game level 0 ----------------------------------------------------------------------------------    
    public class SplashScreen : RC_GameStateParent
    {
        Texture2D startBackground;
        ImageBackground level0;

        Texture2D mouseTexture;
        public float mouse_x = 0;
        public float mouse_y = 0;

        SoundEffect music;
        SoundEffectInstance limMusic;

        SpriteFont font1;
        SpriteFont font2;

        // GUI elements
        Frame mainFrame;
        bool flag;
        Texture2D mainFrameTexture;
        TextBox textFrame;
        Texture2D textFrameTexture;
        InputBox editBox1;
        InputBox editBox2;
        Texture2D Button1TextureA;
        Texture2D Button1TextureB;
        Texture2D Button2Texture;
        Texture2D Button3Texture; // close button
        Button2 Button1;
        Button2 Button2;
        Button2 Button3;
        Texture2D whiteTex;


        public KeyboardState keyState;
        public KeyboardState prevKeyState;

        public MouseState currentMouseState;
        public MouseState previousMouseState;

        public override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            // spriteBatch = new SpriteBatch(GraphicsDevice);

            //create Gui frame
            mainFrameTexture = Content.Load<Texture2D>("frame");
            mainFrame = new Frame(mainFrameTexture, new Rectangle(606, 506, 750, 350));

            //mainFrame.setColor(new Color(255,255,255,100),true);
            flag = false;

            Button1TextureA = Content.Load<Texture2D>("play");
            Button1TextureA = Content.Load<Texture2D>("play2");

            Button1 = new Button2(Button1TextureA, Button1TextureB, new Vector2(885, 500));
            //mainFrame.AddControl(Button1);


            font1 = Content.Load<SpriteFont>("spritefont1");
            font2 = Content.Load<SpriteFont>("spritefont3");

            music = Content.Load<SoundEffect>("background0");
            limMusic = music.CreateInstance();

            startBackground = Content.Load<Texture2D>("splashBg");
            level0 = new ImageBackground(startBackground, Color.White, graphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            //Music
            //limMusic.playSound();

            prevKeyState = keyState;
            keyState = Keyboard.GetState();
             
            limMusic.Play();


            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Released)
            {
                // generate the mouse event
                mainFrame.MouseDownEventLeft(mouse_x, mouse_y);
            }
            Keys[] pressedKeys;
            pressedKeys = keyState.GetPressedKeys();
            
            foreach (Keys key in pressedKeys)
            {
                if (prevKeyState.IsKeyUp(key))
                {
                    // generate keyboard event
                    mainFrame.KeyHitEvent(key);
                }
            }
            mainFrame.Update(gameTime);
            if (Button1.wasClicked)
            {
                gameStateManager.setLevel(1); //jump game level 1
                limMusic.Stop();
                Button1.wasClicked = false;
            
            }
            mainFrame.MouseOver(mouse_x, mouse_y);


            //Player Select START button to start the game
            if (keyState.IsKeyDown(Keys.Space) && prevKeyState.IsKeyUp(Keys.Space))
            {
                gameStateManager.setLevel(1); //jump game level 1
                limMusic.Stop();
                //gameStateManager.setLevel(2); //jump game level 2
                //gameStateManager.setLevel(3); //jump game level 2

                //gameStateManager.setLevel(6); //space shooter - test for shooting
                //gameStateManager.setLevel(3); //knightborne - the first game
            }
            level0.Update(gameTime);
              base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Black);

            level0.Draw(spriteBatch);
            mainFrame.Draw(spriteBatch);
            spriteBatch.DrawString(font1, "HERO", new Vector2(850, 400), Color.Brown);
            //spriteBatch.Draw(tex, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.DrawString(font2, "ENTER SPACE TO PLAY !", new Vector2(550, 600), Color.Brown);
        }
    }
}
