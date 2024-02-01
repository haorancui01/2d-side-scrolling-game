using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using RC_Framework;


namespace GPT_FinalGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static KeyboardState keyState;     // must use or keystate can be unstable on level change
        public static KeyboardState prevKeyState; // must use or keystate can be unstable on level change

        public static MouseState currentMouseState;  // must use or mousestate can be unstable on level change
        public static MouseState previousMouseState;

        RC_GameStateManager levelManager;

        Texture2D mouseTexture;
        public float mouse_x = 0;
        public float mouse_y = 0;

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
        SpriteFont font1;
        SpriteFont font2;

        int timerTicks = 300; //add a timer to flip you to the play level after a few seconds

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

           graphics.PreferredBackBufferWidth = 1980;
           graphics.PreferredBackBufferHeight = 1080;

            // graphics.SynchronizeWithVerticalRetrace = true;
            //this.IsFixedTimeStep = false; this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60f); //target 100FPS
            //graphics.ApplyChanges(); //Now required to modify the screen/window resolution
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LineBatch.init(GraphicsDevice);

            font1 = Content.Load<SpriteFont>("spritefont1");
            font2 = Content.Load<SpriteFont>("spritefont2");

            //mouseTexture = Util.texFromFile(GraphicsDevice, Dir.dir + "Mouse.png");

            mouseTexture = Content.Load<Texture2D>("Mouse");
            // initialise GUI system

            whiteTex = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color); // one pixel white texture
            whiteTex.SetData(new[] { Color.White });

            GUI_Globals.tTfont = font1;
            GUI_Globals.whiteTex = whiteTex;
            GUI_Globals.defaultText = Color.Black;

            // TODO: use this.Content to load your game content here
            levelManager = new RC_GameStateManager();

            levelManager.AddLevel(0, new SplashScreen());
            levelManager.getLevel(0).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(0).LoadContent();
            levelManager.setLevel(0);

            //levelManager.AddLevel(2, new SpaceShooter_Level_2());
            //levelManager.getLevel(2).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            //levelManager.getLevel(2).LoadContent();

            //levelManager.AddLevel(1, new unUsedPlayLevel_1());
            //levelManager.getLevel(1).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            //levelManager.getLevel(1).LoadContent();

            //levelManager.AddLevel(3, new PlayLevel_3()); //METROIDVANIA play level 1
            //levelManager.getLevel(3).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            //levelManager.getLevel(3).LoadContent();

            //levelManager.AddLevel(4, new PlayLevel_4()); //METROIDVANIA play level 2
            //levelManager.getLevel(4).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            //levelManager.getLevel(4).LoadContent();

           
            levelManager.AddLevel(1, new JumpGameLevel_1()); //level 1
            levelManager.getLevel(1).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(1).LoadContent();

            levelManager.AddLevel(2, new JumpGameLevel_2()); //level 2
            levelManager.getLevel(2).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(2).LoadContent();

            levelManager.AddLevel(3, new JumpGameLevel_3()); //level 3
            levelManager.getLevel(3).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(3).LoadContent();

            levelManager.AddLevel(5, new EndScreen()); //End Screen
            levelManager.getLevel(5).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(5).LoadContent();

            levelManager.AddLevel(6, new SpaceShooter_Level_1());
            levelManager.getLevel(6).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(6).LoadContent();

            levelManager.AddLevel(7, new PauseScreen());
            levelManager.getLevel(7).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(7).LoadContent();

            levelManager.AddLevel(8, new HelpScreen());
            levelManager.getLevel(8).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(8).LoadContent();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Update for mouse
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            mouse_x = currentMouseState.X;
            mouse_y = currentMouseState.Y;

           


            if (keyState.IsKeyDown(Keys.Escape)) this.Exit();
           

            //mainFrame.Update(gameTime);

            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            //Add a timer to flip you to the play level after a few seconds
            
            //timerTicks--;
            //if (timerTicks <= 0)
            //{
            //    levelManager.setLevel(1);
            //}


            levelManager.getCurrentLevel().Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            levelManager.getCurrentLevel().Draw(gameTime);

            spriteBatch.Draw(mouseTexture, new Vector2(mouse_x, mouse_y), Color.White);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
