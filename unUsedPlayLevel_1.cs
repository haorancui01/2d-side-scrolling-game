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
    class unUsedplayLevel_1
    {
    }

    class unUsedPlayLevel_1 : RC_GameStateParent
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

        //Background
        ImageBackground layer0; //sky
        ImageBackground road;
        ScrollBackGround layer1 = null; //mountains
        Texture2D texLayer0;
        Texture2D texLayer1;
        Texture2D texRoad;

        //---------------Sprite3 Nightborne---------------
        Texture2D texNightBorne;
        float nightborne_x = 50; //Initial position X on screen
        float nightborne_y = 800;//Initial position Y on screen
        Sprite3 stand_nightborne; 
        Sprite3 run_nightborne;
        Sprite3 attack_nightborne;
        Sprite3 underAttack_nightborne;
        Sprite3 die_nightborne;

        //Nightborne movement
        int speed_nightborne = 3;
        int lhs = 10;
        int rhs = 10000;
        float mountainSpeed = -0.3f;

        //---------------Enemy 1 Droid---------------
        Texture2D texDroid_attack;
        Texture2D texDroid_demagedAndDeath;
        Texture2D texDroid_run;
        Texture2D texDroid_wake;
        float droid_x = 300; //Initial position X on screen
        float droid_y = 800;//Initial position Y on screen
        Sprite3 droid;
        Sprite3 attack_droid;
        Sprite3 death_droid;
        Sprite3 run_droid;
        Sprite3 wake_droid;

        


        //Sprite3: Monster 

        public override void LoadContent()
        {
            //Set the screen window
            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);

            //Load the contents of layers
            texLayer0 = Content.Load<Texture2D>("sky_lightened");
            texLayer1 = Content.Load<Texture2D>("glacial_mountains");
            texRoad = Content.Load<Texture2D>("Hills Layer 05");
            texNightBorne = Content.Load<Texture2D>("NightBorne_1"); //Load nightborne

            
            texDroid_attack = Content.Load<Texture2D>("droid");
            texDroid_demagedAndDeath = Content.Load<Texture2D>("droid");
            texDroid_run = Content.Load<Texture2D>("droid");
            texDroid_wake = Content.Load<Texture2D>("droid");




            //Create the background of layers
            layer0 = new ImageBackground(texLayer0, Color.White, graphicsDevice);
            road =   new ImageBackground(texRoad, Color.White, graphicsDevice);
            layer1 = new ScrollBackGround(texLayer1, new Rectangle(0, 0, screenWidth, screenHeight),
                                            new Rectangle(0, 0, screenWidth, screenHeight), mountainSpeed, 2);

            //----------------------1. NIGHTBORNE STAND-----------------------
            //Creat the stand pose for nightborne
            stand_nightborne = new Sprite3(true, texNightBorne, nightborne_x, nightborne_y);
            stand_nightborne.setWidthHeight(300, 300);
            stand_nightborne.setBB(20, 30, 35, 40);
            stand_nightborne.setXframes(23);
            stand_nightborne.setYframes(5);
            //stand_nightborne.setBBToTexture();
            stand_nightborne.setMoveAngleDegrees(180);

            //Set the frame of stand postion of nightborne
            Vector2[] stand_nbVector = new Vector2[9];
            stand_nbVector[0].X = 0; stand_nbVector[0].Y = 0;
            stand_nbVector[1].X = 1; stand_nbVector[1].Y = 0;
            stand_nbVector[2].X = 2; stand_nbVector[2].Y = 0;
            stand_nbVector[3].X = 3; stand_nbVector[3].Y = 0;
            stand_nbVector[4].X = 4; stand_nbVector[4].Y = 0;
            stand_nbVector[5].X = 5; stand_nbVector[5].Y = 0;
            stand_nbVector[6].X = 6; stand_nbVector[6].Y = 0;
            stand_nbVector[7].X = 7; stand_nbVector[7].Y = 0;
            stand_nbVector[8].X = 8; stand_nbVector[8].Y = 0;
            stand_nightborne.setAnimationSequence(stand_nbVector, 0, 8, 9);

            stand_nightborne.animationStart();

            //----------------------2. NIGHTBORNE RUN-----------------------
            //Creat the run postion for nightborne
            run_nightborne = new Sprite3(true, texNightBorne, nightborne_x, nightborne_y);
            run_nightborne.setWidthHeight(300, 300);
            run_nightborne.setBB(16,30, 35, 40);
            run_nightborne.setXframes(23);
            run_nightborne.setYframes(5);
            run_nightborne.setMoveAngleDegrees(180);

            //Set the frame of run pose of nightborne
            Vector2[] run_nbVector = new Vector2[6];//// nightborne run
            run_nbVector[0].X = 0; run_nbVector[0].Y = 1;
            run_nbVector[1].X = 1; run_nbVector[1].Y = 1;
            run_nbVector[2].X = 2; run_nbVector[2].Y = 1;
            run_nbVector[3].X = 3; run_nbVector[3].Y = 1;
            run_nbVector[4].X = 4; run_nbVector[4].Y = 1;
            run_nbVector[5].X = 5; run_nbVector[5].Y = 1;
            run_nightborne.setAnimationSequence(run_nbVector, 0, 5, 6);

            run_nightborne.animationStart();

            //---------------------3. NIGHTBORNE ATTACK-----------------------
            //Create the sprite for attack pose of nightborne
            attack_nightborne = new Sprite3(true, texNightBorne, nightborne_x, nightborne_y);
            attack_nightborne.setWidthHeight(300, 300);
            attack_nightborne.setBB(16, 25, 65, 35);
            attack_nightborne.setXframes(23);
            attack_nightborne.setYframes(5);
            attack_nightborne.setMoveAngleDegrees(180);

            Vector2[] attack_nbVector = new Vector2[8]; //Skim the frame position of 3,4,5,6.
            attack_nbVector[0].X = 0; attack_nbVector[0].Y = 2;
            attack_nbVector[1].X = 1; attack_nbVector[1].Y = 2;
            attack_nbVector[2].X = 2; attack_nbVector[2].Y = 2;
            attack_nbVector[3].X = 7; attack_nbVector[3].Y = 2;
            attack_nbVector[4].X = 8; attack_nbVector[4].Y = 2;
            attack_nbVector[5].X = 9; attack_nbVector[5].Y = 2;
            attack_nbVector[6].X = 10; attack_nbVector[6].Y = 2;
            attack_nbVector[7].X = 11; attack_nbVector[7].Y = 2;

            attack_nightborne.setAnimationSequence(attack_nbVector, 0,7, 8);

            attack_nightborne.animationStart();

            //// nightborne die 

            //---------------------Droid RUN-----------------------
            run_droid = new Sprite3(true, texDroid_run, droid_x, droid_y);
            run_droid.setWidthHeightOfTex(160,411);
            run_droid.setWidthHeight(200, 180);
            run_droid.setBB(8,4,20,35);
            run_droid.setXframes(4);
            run_droid.setYframes(10);
            run_droid.setMoveAngleDegrees(180);

            //Set the frame of run pose of droid
            Vector2[] run_droidVector = new Vector2[6];
            run_droidVector[0].X = 1; run_droidVector[0].Y = 0;
            run_droidVector[1].X = 1; run_droidVector[1].Y = 1;
            run_droidVector[2].X = 1; run_droidVector[2].Y = 2;
            run_droidVector[3].X = 1; run_droidVector[3].Y = 3;
            run_droidVector[4].X = 1; run_droidVector[4].Y = 4;
            run_droidVector[5].X = 1; run_droidVector[5].Y = 5;

            run_droid.setAnimationSequence(run_droidVector, 0, 5, 6);

            run_droid.animationStart();

           






        }

        public override void Update(GameTime gameTime)

        {

            preKeyState = keyState;
            keyState = Keyboard.GetState();
            //ticks++;


            //if (keyState.IsKeyDown(Keys.B) && preKeyState.IsKeyUp(Keys.B))
            //{
            //  set bouding box to appear in the screen
            //  Set to always to show bounding box, so these line be commented.
            //    showbb = !showbb;
            //}


            run_droid.moveByDeltaXY();
            run_droid.animationTick(gameTime);

            //die_nightborne.moveByDeltaXY();
            //die.animationTick(gameTime);


            //1. Control the nightborne
            //---------------MOVEMENT PART START-------------
            if (keyState.IsKeyUp(Keys.D))
            {
                stand_nightborne.moveByDeltaXY();
                stand_nightborne.animationTick(gameTime);

                stand_nightborne.setActiveAndVisible(true);
                attack_nightborne.setActiveAndVisible(false);
                run_nightborne.setActiveAndVisible(false);
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                //Set the movement of mountains background would follow the sprite nightborne 
                // mountainSpeed = -(run_nightborne.getMoveSpeed());

                run_nightborne.setActiveAndVisible(true);
                stand_nightborne.setActiveAndVisible(false);
                attack_nightborne.setActiveAndVisible(false);

                run_nightborne.moveByDeltaXY();
                run_nightborne.animationTick(gameTime);

                if (run_nightborne.getPosX() < rhs - screenWidth)
                    run_nightborne.setPosX(run_nightborne.getPosX() + speed_nightborne);
                if(stand_nightborne.getPosX() < rhs - screenWidth)
                    stand_nightborne.setPosX(stand_nightborne.getPosX() + speed_nightborne);
                if (attack_nightborne.getPosX() < rhs - screenWidth)
                    attack_nightborne.setPosX(attack_nightborne.getPosX() + speed_nightborne);
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                if (run_nightborne.getPosX() > lhs - screenWidth)
                    run_nightborne.setPosX(run_nightborne.getPosX() - speed_nightborne);
                if (stand_nightborne.getPosX() > lhs - screenWidth)
                    stand_nightborne.setPosX(stand_nightborne.getPosX() - speed_nightborne);
                if (attack_nightborne.getPosX() > lhs - screenWidth)
                    attack_nightborne.setPosX(attack_nightborne.getPosX() - speed_nightborne);

            }
            //---------------MOVEMENT PART END-------------

            //2. Control the nightborne
            //---------------ATTACK PART START-------------
            
            if (keyState.IsKeyDown(Keys.J)/* && preKeyState.IsKeyUp(Keys.J)*/)
            {
                attack_nightborne.moveByDeltaXY();
                attack_nightborne.animationTick(gameTime);
                attack_nightborne.setActiveAndVisible(true);

                run_nightborne.setActiveAndVisible(false);
                stand_nightborne.setActiveAndVisible(false);
            }


            //Constraint the nightboarn
            if ((run_nightborne.getPosX() <= 0) &&
                (stand_nightborne.getPosX() <=0) &&
                (attack_nightborne.getPosX() <= 0))
            {
                run_nightborne.setPosX(0);
                stand_nightborne.setPosX(0);
                attack_nightborne.setPosX(0);
            }

            if ((run_nightborne.getPosX() + run_nightborne.getWidth() >= screenWidth) &&
                (stand_nightborne.getPosX() + stand_nightborne.getWidth() >= screenWidth) &&
                (attack_nightborne.getPosX() + attack_nightborne.getWidth() >= screenWidth))
            {
                run_nightborne.setPosX(screenWidth - run_nightborne.getWidth());
                stand_nightborne.setPosX(screenWidth - stand_nightborne.getWidth());
                attack_nightborne.setPosX(screenWidth - attack_nightborne.getWidth());
            }

            if ((run_nightborne.getPosY() <= 0) &&
                (stand_nightborne.getPosY() <= 0) &&
                 (attack_nightborne.getPosY() <= 0))
            {
                run_nightborne.setPosY(0);
                stand_nightborne.setPosY(0);
                attack_nightborne.setPosY(0);
            }

            if ((run_nightborne.getPosY() + run_nightborne.getHeight() >= screenHeight) &&
                (stand_nightborne.getPosY() + stand_nightborne.getHeight() >= screenHeight)&&
                (attack_nightborne.getPosY() + attack_nightborne.getHeight() >= screenHeight))

            {
                run_nightborne.setPosY(screenHeight - run_nightborne.getHeight());
                stand_nightborne.setPosY(screenHeight - stand_nightborne.getHeight());
                attack_nightborne.setPosY(screenHeight - attack_nightborne.getHeight());
            }


            layer1.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Aqua);

            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            layer0.Draw(spriteBatch);
            layer1.Draw(spriteBatch);
            road.Draw(spriteBatch);
            
             run_nightborne.Draw(spriteBatch);
            stand_nightborne.Draw(spriteBatch);
            attack_nightborne.Draw(spriteBatch);
            run_droid.Draw(spriteBatch);

            //Show bouding box
            if (!showbb) // !showbb = true (always show bb)
            {
               // stand_nightborne.drawInfo(spriteBatch, Color.Red, Color.Blue);
               // run_nightborne.drawInfo(spriteBatch, Color.Red, Color.Blue);
                attack_nightborne.drawInfo(spriteBatch, Color.Red, Color.Blue);

                run_droid.drawInfo(spriteBatch, Color.Red, Color.Blue);
            }

            //spriteBatch.End();
        }
    }
}
