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
    

    class PlayLevel_3 : RC_GameStateParent
    {
        //Keyboard control
        KeyboardState keyState;
        KeyboardState preKeyState;

        //Variable for screen
        int screenWidth;
        int screenHeight;
        Rectangle screenRect;
        Rectangle rect1;

        //Bounding box boolean value
        bool showbb = false;

        //Hero movement
        int spped_hero = 3;
        int lhs = 10;
        int rhs = 10000;

        SpriteList tileList1;
        SpriteList tileList2; //green one

        Sprite3 startTile;
        Sprite3 rightSide;
        Sprite3 midTile;
        Sprite3 leftSide;
        Sprite3 trap1;
        Sprite3 square1; //leftSquare
        Sprite3 block1; //right sequre at left screen
        Sprite3 block2;
        Sprite3 door;
        Sprite3 stone;
        Sprite3 bar1;
        Sprite3 bar2;

        Texture2D texBar2;
        Texture2D texBar1;
        Texture2D texTwoBlock;
        Texture2D texStone;
        Texture2D texMidTile;
        Texture2D texStartTile;
        Texture2D texRightSide;
        Texture2D texTile1;
        Texture2D texTile2;
        Texture2D texSquare1; // leftSquare
        Texture2D texLeftSide;
        Texture2D texDoor;
        Texture2D texTrap1;

        //The hereo
        Sprite3 standLeft_hero;
        Sprite3 standRight_hero;
        Sprite3 runLeft_hero;
        Sprite3 runLRight_hero;


        Texture2D tex_standLeft_hero;
        Texture2D tex_runLeft_hero;
        Texture2D tex_standRight_hero;
        Texture2D tex_runRight_hero;



        Vector2 position, velocity;
        const float gravity = 100f;
        float moveSpeed = 500f;
        float jumpSpeed = 1700;
        bool jump = false;  //false: cannot jump, true: can jump

        float velocity_hero = 0;

      

        public PlayLevel_3()
        {
            position = velocity = Vector2.Zero;
        }

        public override void LoadContent()
        {
            //---------------START Load all assets---------------

            texBar1 = Content.Load<Texture2D>("bar");
            texBar2 = Content.Load<Texture2D>("bar");



            texTile1 = Content.Load<Texture2D>("sideTile1");


            texTile2 = Content.Load<Texture2D>("tile2");
            texStartTile = Content.Load<Texture2D>("startTile");
            texRightSide = Content.Load<Texture2D>("rightSide");
            texMidTile = Content.Load<Texture2D>("mid");
            texSquare1 = Content.Load<Texture2D>("leftSquare");
            texTwoBlock = Content.Load<Texture2D>("twoBlock");
            texLeftSide = Content.Load<Texture2D>("leftSide");
            texTrap1 = Content.Load<Texture2D>("trap1");
            texDoor = Content.Load<Texture2D>("door");
            texStone = Content.Load<Texture2D>("stone");

            //Load hero
            tex_standLeft_hero = Content.Load<Texture2D>("spriteToLeft");
            tex_runLeft_hero = Content.Load<Texture2D>("spriteToLeft");

            tex_standRight_hero = Content.Load<Texture2D>("spriteToRight");
            tex_runRight_hero = Content.Load<Texture2D>("spriteToRight");


            //---------------END Load all assets---------------


            //---------------START Create the stuff---------------
            bar1 = new Sprite3(true, texBar1, 356, 754);
            bar1.setWidthHeight(144, 49);
            bar1.setWidthHeightOfTex(144, 49);
            bar1.setBB(0, 0, 144, 49);

            //bar1 = new Sprite3(true, texBar1, 350, 678);
            //bar1.setWidthHeight(192, 64);
            //bar1.setWidthHeightOfTex(192, 64);
            //bar1.setBB(0, 0, 192, 64);


            stone = new Sprite3(true, texStone, 1467, 369);
            stone.setWidthHeight(64, 64);
            stone.setWidthHeightOfTex(64, 64);
            stone.setBB(0, 0, 64, 64);


            startTile = new Sprite3(true, texStartTile, 316, 984);
            startTile.setWidthHeight(1664, 92);
            startTile.setWidthHeightOfTex(1664, 92);
            startTile.setBB(0, 0, 1664, 92);

            rightSide = new Sprite3(true, texRightSide, 1887, 121);
            rightSide.setWidthHeight(93, 865);
            rightSide.setWidthHeightOfTex(93, 865);
            rightSide.setBB(0, 0, 93, 865);

            midTile = new Sprite3(true, texMidTile, 601, 431);
            midTile.setWidthHeight(1286, 316);
            midTile.setWidthHeightOfTex(1286, 316);
            midTile.setBB(0, 0, 1286, 316);



            leftSide = new Sprite3(true, texLeftSide, 0, 195);
            leftSide.setWidthHeight(125, 884);
            leftSide.setWidthHeightOfTex(125, 884);
            leftSide.setBB(0, 0, 125, 884);

            trap1 = new Sprite3(true, texTrap1, 519, 936);
            trap1.setWidthHeight(146, 48);
            trap1.setWidthHeightOfTex(146, 48);
            trap1.setBB(0, 0, 146, 48);


            square1 = new Sprite3(true, texSquare1, 126, 893);
            square1.setWidthHeight(190, 190);
            square1.setWidthHeightOfTex(190, 190);
            square1.setBB(0, 0, 190, 190);

            block1 = new Sprite3(true, texTwoBlock, 410, 639);
            block1.setWidthHeight(190, 94);
            block1.setWidthHeightOfTex(190, 94);
            block1.setBB(0, 0, 190, 94);

            block2 = new Sprite3(true, texTwoBlock, 126, 419);
            block2.setWidthHeight(190, 94);
            block2.setWidthHeightOfTex(190, 94);
            block2.setBB(0, 0, 190, 94);

            door = new Sprite3(true, texDoor, 1686, 865);
            door.setWidthHeight(120, 120);
            door.setWidthHeightOfTex(120, 120);
            door.setBB(0, 0, 120, 120);

            //---------------END Create the stuff---------------

            rect1 = new Rectangle(1135, 987, 93, 865);

            //public Rectangle(int x, int y, int width, int height);
            

            tileList1 = new SpriteList();
            Sprite3 t1;
            for (int i = 0; i < 22; i++)
            {
                t1 = new Sprite3(true, texTile1, 0, i * 50);
                // t1.setWidthHeight(50, 50);
                t1.setWidthHeight(95, 95);
                t1.setWidthHeightOfTex(95, 95);
                t1.setBB(0, 0, 95, 95);
                tileList1.addSpriteReuse(t1);
            }


            //----------------------1. Hero STAND-----------------------
            //Creat the stand LEFT pose for hero
            standLeft_hero = new Sprite3(true, tex_standLeft_hero, 1607, 910);

            standLeft_hero.setWidthHeight(57, 80);
            standLeft_hero.setBB(10, 5, 57, 75);
            standLeft_hero.setXframes(8);
            standLeft_hero.setYframes(15);
            standLeft_hero.setMoveAngleDegrees(180);

            //Set the frame of stand pose of hero
            Vector2[] standLeft_heroVector = new Vector2[4];
            standLeft_heroVector[0].X = 7; standLeft_heroVector[0].Y = 5;
            standLeft_heroVector[1].X = 6; standLeft_heroVector[1].Y = 5;
            standLeft_heroVector[2].X = 5; standLeft_heroVector[2].Y = 5;
            standLeft_heroVector[3].X = 4; standLeft_heroVector[3].Y = 5;
            standLeft_hero.setAnimationSequence(standLeft_heroVector, 0, 3, 4);

            standLeft_hero.animationStart();

            //Creat the stand RIGHT pose for hero
            standRight_hero = new Sprite3(true, tex_standRight_hero, 1607, 910);

            standRight_hero.setWidthHeight(57, 80);
            standRight_hero.setBB(10, 5, 57, 75);
            standRight_hero.setXframes(8);
            standRight_hero.setYframes(15);
            standRight_hero.setMoveAngleDegrees(180);

            //Set the frame of stand pose of hero
            Vector2[] standRight_heroVector = new Vector2[4];
            standRight_heroVector[0].X = 0; standRight_heroVector[0].Y = 5;
            standRight_heroVector[1].X = 1; standRight_heroVector[1].Y = 5;
            standRight_heroVector[2].X = 2; standRight_heroVector[2].Y = 5;
            standRight_heroVector[3].X = 3; standRight_heroVector[3].Y = 5;
            standRight_hero.setAnimationSequence(standRight_heroVector, 0, 3, 4);

            standRight_hero.animationStart();


            //Run to left
            runLeft_hero = new Sprite3(true, tex_standLeft_hero, 1607, 910);

            runLeft_hero.setWidthHeight(57, 80);
            runLeft_hero.setBB(10, 5, 57, 75);
            runLeft_hero.setXframes(8);
            runLeft_hero.setYframes(15);
            runLeft_hero.setMoveAngleDegrees(180);

            //Set the frame of run postion of hero
            Vector2[] runLeft_heroVector = new Vector2[6];
            runLeft_heroVector[0].X = 7; runLeft_heroVector[0].Y = 2;
            runLeft_heroVector[1].X = 6; runLeft_heroVector[1].Y = 2;
            runLeft_heroVector[2].X = 5; runLeft_heroVector[2].Y = 2;
            runLeft_heroVector[3].X = 4; runLeft_heroVector[3].Y = 2;
            runLeft_heroVector[4].X = 3; runLeft_heroVector[4].Y = 2;
            runLeft_heroVector[5].X = 2; runLeft_heroVector[5].Y = 2;

            runLeft_hero.setAnimationSequence(runLeft_heroVector, 0, 5, 6);

            runLeft_hero.animationStart();

            

        }

        public override void Update(GameTime gameTime)
        {
            tileList1.moveDeltaXY();
            tileList1.animationTick(gameTime);

      

            preKeyState = keyState;
            keyState = Keyboard.GetState();
            //ticks++;


            //1.Control the hero
            //---------------MOVEMENT PART START-------------

            //------------------TEST START---------------------
            if (keyState.IsKeyUp(Keys.Right))
            {
                standLeft_hero.moveByDeltaXY();
                standLeft_hero.animationTick(gameTime);

                standLeft_hero.setActiveAndVisible(true);
                runLeft_hero.setActiveAndVisible(false);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                standRight_hero.moveByDeltaXY();
                standRight_hero.animationTick(gameTime);

                runLeft_hero.setActiveAndVisible(false);
                standLeft_hero.setActiveAndVisible(false);
                standRight_hero.setActiveAndVisible(true);

                if (runLeft_hero.getPosX() < rhs - screenWidth)
                    runLeft_hero.setPosX(runLeft_hero.getPosX() + spped_hero);
                if (standLeft_hero.getPosX() < rhs - screenWidth)
                    standLeft_hero.setPosX(standLeft_hero.getPosX() + spped_hero);

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                runLeft_hero.setActiveAndVisible(true);
                standLeft_hero.setActiveAndVisible(false);
                runLeft_hero.moveByDeltaXY();
                runLeft_hero.animationTick(gameTime);
                if (runLeft_hero.getPosX() > lhs - screenWidth)
                    runLeft_hero.setPosX(runLeft_hero.getPosX() - spped_hero);
                if (standLeft_hero.getPosX() > lhs - screenWidth)
                    standLeft_hero.setPosX(standLeft_hero.getPosX() - spped_hero);
            }
            else
            {
                velocity.X = 0;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Up) && jump)
            {
                velocity.Y = -jumpSpeed * ((float)gameTime.ElapsedGameTime.TotalSeconds);

                jump = false;
               
                if (standLeft_hero.getBoundingBoxAA().Intersects(midTile.getBoundingBoxAA()) 
                   && runLeft_hero.getBoundingBoxAA().Intersects(midTile.getBoundingBoxAA()))
                {
                    // standLeft_hero.setPos(startTile.getPosX()+1, startTile.getPosY()+1);
                    standLeft_hero.setPosY(746);//floor
                    standRight_hero.setPosY(746);//floor

                    runLeft_hero.setPosY(746);// floor
                }
            }

            if (jump)
            {
                position.Y = 910;

              //  standLeft_hero.setPosY(position.Y);
                //standRight_hero.setPosY(position.Y);
                //runLeft_hero.setPosY(position.Y);

                //standLeft_hero.setPosX(1130);//floor
                //runLeft_hero.setPosX(1130);// floor

                
            }

            if (!jump)
            {
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                velocity_hero += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;


            }
            else
            {
                velocity.Y = 0;

                
            }

            if (position.Y >= 910)
            {
                jump = true;
            }
            else
            {
                jump = false;
            }

            //OR//
            position += velocity;
           standLeft_hero.setPos(position.X+1607, position.Y); //1607, 910 initial position
            //jump = position.Y >= 900;




            //------------------TEST END---------------------


            if ((standLeft_hero.collision(square1))
               && (standLeft_hero.getPosY() > 891))
            {
                standLeft_hero.setPosY(810);
                standRight_hero.setPosY(810);
                runLeft_hero.setPosY(810);
                jump = true;
            }

            if ((standLeft_hero.collision(bar1))
               && (standLeft_hero.getPosY() > 753))
            {
                standLeft_hero.setPosY(673);
               // jump = false;
            }
            //if ((standLeft_hero.collision(bar1))
            //   && (standLeft_hero.getPosY() > 753) && jump)
            //{
            //    standLeft_hero.setPosY(753);
            //    jump = false;
            //}



            if (standLeft_hero.collision(midTile) 
                /*&& (standLeft_hero.getPosY() > 746)*/)
            {
                standLeft_hero.setPosY(746);//floor
                standRight_hero.setPosY(746);//floor

                runLeft_hero.setPosY(746);// floor

            }


            //if ((standLeft_hero.getPosX()>1135)&&(standLeft_hero.getPosY()>986))
            //{
            //    standLeft_hero.setPosY(995);//floor
            //}

            




            //---------------MOVEMENT PART END-------------

            //2. Control the nightborne
            //---------------ATTACK PART START-------------



            ////Constraint the nightboarn
            //if ((runLeft_hero.getPosX() <= 0) &&
            //    (standLeft_hero.getPosX() <= 0)
            //    /*&&(attack_nightborne.getPosX() <= 0)*/)
            //{
            //    runLeft_hero.setPosX(0);
            //    standLeft_hero.setPosX(0);
            //    //attack_nightborne.setPosX(0);
            //}

            //if ((runLeft_hero.getPosX() + runLeft_hero.getWidth() >= screenWidth) &&
            //    (standLeft_hero.getPosX() + standLeft_hero.getWidth() >= screenWidth) 
            //  /*  &&(attack_nightborne.getPosX() + attack_nightborne.getWidth() >= screenWidth)*/)
            //{
            //    runLeft_hero.setPosX(screenWidth - runLeft_hero.getWidth());
            //    standLeft_hero.setPosX(screenWidth - standLeft_hero.getWidth());
            //    //attack_nightborne.setPosX(screenWidth - attack_nightborne.getWidth());
            //}

            //if ((runLeft_hero.getPosY() <= 0) &&
            //    (standLeft_hero.getPosY() <= 0) /*&& (attack_nightborne.getPosY() <= 0)*/)
            //{
            //    runLeft_hero.setPosY(0);
            //    standLeft_hero.setPosY(0);
            //    //attack_nightborne.setPosY(0);
            //}

            //if ((runLeft_hero.getPosY() + runLeft_hero.getHeight() >= screenHeight) &&
            //    (standLeft_hero.getPosY() + standLeft_hero.getHeight() >= screenHeight) /*&& (attack_nightborne.getPosY() + attack_nightborne.getHeight() >= screenHeight)*/)
            //{
            //    runLeft_hero.setPosY(screenHeight - runLeft_hero.getHeight());
            //    standLeft_hero.setPosY(screenHeight - standLeft_hero.getHeight());
            //    //attack_nightborne.setPosY(screenHeight - attack_nightborne.getHeight());
            //}

            if (standLeft_hero.getBoundingBoxAA().Intersects(door.getBoundingBoxAA()))
            {
                gameStateManager.setLevel(4);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.White);

            //tileList1.Draw(spriteBatch);

            startTile.Draw(spriteBatch);
            rightSide.Draw(spriteBatch);
            midTile.Draw(spriteBatch);
            leftSide.Draw(spriteBatch);
            trap1.Draw(spriteBatch);
            square1.Draw(spriteBatch);
            //block1.Draw(spriteBatch);
            //block2.Draw(spriteBatch);
            door.Draw(spriteBatch);
            stone.Draw(spriteBatch);
            bar1.Draw(spriteBatch);
            //Draw hero
            standLeft_hero.Draw(spriteBatch);
            //spriteBatch.Draw(texStone, position, Color.White);

            //standRight_hero.Draw(spriteBatch);

            //runLeft_hero.Draw(spriteBatch);


            //show bb

            //tileList1.drawInfo(spriteBatch, Color.Red, Color.Blue);
            bar1.drawInfo(spriteBatch, Color.Red, Color.Blue);

            startTile.drawInfo(spriteBatch, Color.Red, Color.Blue);
            rightSide.drawInfo(spriteBatch, Color.Red, Color.Blue);
            midTile.drawInfo(spriteBatch, Color.Red, Color.Blue);
            leftSide.drawInfo(spriteBatch, Color.Red, Color.Blue);
            trap1.drawInfo(spriteBatch, Color.Red, Color.Blue);
            square1.drawInfo(spriteBatch, Color.Red, Color.Blue);
            //block1.drawInfo(spriteBatch, Color.Red, Color.Blue);
            //block2.drawInfo(spriteBatch, Color.Red, Color.Blue);
            door.drawInfo(spriteBatch, Color.Red, Color.Blue);
            stone.drawInfo(spriteBatch, Color.Red, Color.Blue);



            //hero
            standLeft_hero.drawInfo(spriteBatch, Color.Red, Color.Blue);
           // standRight_hero.drawInfo(spriteBatch, Color.Red, Color.Blue);


           // runLeft_hero.drawInfo(spriteBatch, Color.Red, Color.Blue);

        }
    }
}
