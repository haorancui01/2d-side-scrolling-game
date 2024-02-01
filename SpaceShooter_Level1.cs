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
    class SpaceShooter_Level_1 : RC_GameStateParent
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

        SpriteList list_Bullet = null;
        Sprite3 bullet1;

        Sprite3 ship_ide;
        public Sprite3 s;



        Texture2D texShip_ide;
        Texture2D tex_bullet1;




    

        bool swap = true;
     

        int ticks = 0;

        Random rnd = new Random();
        Random RandomClass;

        //variables of the car movement
        int carSpeed = 3;
        int lhs = 10;
        int rhs = 10000;

        int cnt = 0;
        
        int score = 0;
        int timer = 1;

        bool exp = false;

        public override void LoadContent()
        {
            //Set the screen window
            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);

            RandomClass = new Random();
           

            //Load all art work

            //Load the ship
            texShip_ide = Content.Load<Texture2D>("ship_ide");

            //Load the bullet
            tex_bullet1 = Content.Load<Texture2D>("bullet1");

            
            //Sprite ----------------update the ship -----END
            ship_ide = new Sprite3(true, texShip_ide, 300, 1190);
            ship_ide.setWidthHeight(110, 89);
            ship_ide.setWidthHeight(110, 89);
            ship_ide.setBB(0, 0, 110, 89);

            list_Bullet = new SpriteList();

            bullet1 = new Sprite3(true, tex_bullet1, -9000, 0);
            list_Bullet.addSpriteReuse(bullet1);

            //Sprite ----------------update the ship -----END



            //ship.setXframes(1);
            //ship.setYframes(5);

            ////Set the frame of run pose of nightborne
            //Vector2[] ship_anim = new Vector2[5];//// nightborne run
            //ship_anim[0].X = 0; ship_anim[0].Y = 0;
            //ship_anim[1].X = 0; ship_anim[1].Y = 1;
            //ship_anim[2].X = 0; ship_anim[2].Y = 2;
            //ship_anim[3].X = 0; ship_anim[3].Y = 3;
            //ship_anim[4].X = 0; ship_anim[4].Y = 4;

            //ship.setAnimationSequence(ship_anim, 0, 4, 10);

            //ship.animationStart();
            //Sprite ----------------update the ship -----END

            list_Bullet = new SpriteList();
        }

        void launchBullet()
        {
           // bullet1.setVisible(true);
            bullet1 = new Sprite3(true, tex_bullet1, ship_ide.getPosX(), ship_ide.getPosY());
            bullet1.setWidthHeight(15, 15);
            bullet1.setBB(0, 0, 15, 15);
            bullet1.setDisplayAngleDegrees(270); //rotate
            bullet1.setMoveAngleDegrees(270);
            bullet1.setMoveSpeed(4.1f);
            list_Bullet.addSpriteReuse(bullet1);
        }


        public override void Update(GameTime gameTime)

        {

            preKeyState = keyState;
            keyState = Keyboard.GetState();


            //For the ship to move
            //for the car moving(sprite3 with bounding Box)
            if (keyState.IsKeyDown(Keys.Right))
            {
                if (ship_ide.getPosX() < rhs - screenWidth)
                    ship_ide.setPosX(ship_ide.getPosX() + carSpeed);
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                if (ship_ide.getPosX() > lhs - screenWidth)
                    ship_ide.setPosX(ship_ide.getPosX() - carSpeed);
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                if (ship_ide.getPosY() < rhs - screenHeight)
                    ship_ide.setPosY(ship_ide.getPosY() - carSpeed);
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                if (ship_ide.getPosY() < rhs - screenHeight)
                    ship_ide.setPosY(ship_ide.getPosY() + carSpeed);
            }

            //Update the shoot bullet
            if (keyState.IsKeyDown(Keys.Z) /*&& !prevKeyState.IsKeyUp(Keys.Z)*/)
            {
               
                launchBullet();
            }



            if (ship_ide.getPosX() <= 0)
            {
                ship_ide.setPosX(0);
                
            }

            if (ship_ide.getPosX() + ship_ide.getWidth() >= screenWidth)
            {
                ship_ide.setPosX(screenWidth - ship_ide.getWidth());

            }

            if (ship_ide.getPosY() <= 0)
            {
                ship_ide.setPosY(0);
            }

            if (ship_ide.getPosY() + ship_ide.getHeight() >= screenHeight)
            {
                ship_ide.setPosY(screenHeight - ship_ide.getHeight());
            }

            list_Bullet.moveByAngleSpeed();
            list_Bullet.animationTick(gameTime);
            bullet1.moveByAngleSpeed();
            bullet1.moveByDeltaXY();
            bullet1.Update(gameTime);

            base.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.White);


            ship_ide.Draw(spriteBatch);
            bullet1.Draw(spriteBatch);
            list_Bullet.drawActive(spriteBatch);
            //Show bouding box
            //if (!showbb) // !showbb = true (always show bb)
            //{ }
            // list_Bullet.drawInfo(spriteBatch, Color.Red, Color.Blue);

            ship_ide.drawInfo(spriteBatch, Color.Red, Color.Blue);
            list_Bullet.drawInfo(spriteBatch, Color.Red, Color.Blue);



        }
    }
}
