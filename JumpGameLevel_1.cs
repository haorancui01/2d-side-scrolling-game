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
    class JumpGameLevel_1 : RC_GameStateParent
    {
        //---------------------------------------Game Level 2-------------------------------------
        //Keyboard control
        KeyboardState keyState;
        KeyboardState preKeyState;

        Random rnd = new Random();
        Random RandomClass;

        SoundEffect music_background;
        SoundEffectInstance in_music_background;
        SoundEffect music_shoot;
        SoundEffectInstance in_music_shoot;

        SpriteFont font1;
        SpriteFont font2;
        SpriteFont font3;

        //Variable for screen
        int screenWidth;
        int screenHeight;
        Rectangle screenRect;

        bool showbb = false;
        //Hero movement
        int spped_hero = 1;
        int lhs = 10;
        int rhs = 10000;
        Rectangle rect1;

        public static int totalHearts = 4;//Get hit count
        public static int totalKilled = 0; //Number of Killed enemy

        //Background
        ImageBackground layer0; //hill
        ScrollBackGround layer1; //tree
        ScrollBackGround layer2; //tree1

        Texture2D texLayer0, texLayer1, texLayer2;

        Sprite3 heart1, heart2, heart3, heart4;//Counts of hearts

        SpriteList list_Bullet = null;
        Sprite3 bullet1;

        SpriteList fireBall_list = null;

        Sprite3 platform;
        Sprite3 door;
        Sprite3 stone;
        Sprite3 bar1;
        Sprite3 bar2;

        Texture2D tex_fireBall;
        Texture2D tex_heart;
        Texture2D tex_bullet1;
        Texture2D tex_platform;
        Texture2D texBar1;
        Texture2D texTwoBlock;
        Texture2D texStone;
        Texture2D texMidTile;

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

        //---------------Enemy 1 Slime---------------
        SpriteList slime_Right_List;
        SpriteList slime_Left_List;
        Sprite3 slime_walk_left;
        Sprite3 slime_walk_right;
        Sprite3 slime_die;
        Texture2D tex_slime_walk_right;
        Texture2D tex_slime_walk_left;
        Texture2D tex_slime_die;

        Vector2 position, velocity;
        const float gravity = 100f;
        float moveSpeed = 350f;
        float jumpSpeed = 1700;
        bool jump = false;  //false: cannot jump, true: can jump

        public JumpGameLevel_1()
        {
            position = velocity = Vector2.Zero;
        }

        public override void LoadContent()
        {
            //Set the screen window
            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);

            RandomClass = new Random();
            
            //Load music 
            music_background = Content.Load<SoundEffect>("background");
            in_music_background = music_background.CreateInstance();

            music_shoot = Content.Load<SoundEffect>("sound_shoot");
            in_music_shoot = music_shoot.CreateInstance();

            font1 = Content.Load<SpriteFont>("spritefont1");
            font2 = Content.Load<SpriteFont>("spritefont3");
            font3 = Content.Load<SpriteFont>("spritefont4");

            //---------------Load HERO START---------------
            tex_standLeft_hero = Content.Load<Texture2D>("spriteToLeft");
            tex_runLeft_hero = Content.Load<Texture2D>("spriteToLeft");
            tex_standRight_hero = Content.Load<Texture2D>("spriteToRight");
            tex_runRight_hero = Content.Load<Texture2D>("spriteToRight");
            //---------------Load HERO END---------------

            //---------------Load ENEMY 1. START---------------
            tex_slime_walk_right = Content.Load<Texture2D>("slime_walk_right");
            tex_slime_walk_left = Content.Load<Texture2D>("slime_walk_left");

            //---------------START Load all assets---------------
            //Load the contents of layers
            texLayer0 = Content.Load<Texture2D>("layer0");
            texLayer1 = Content.Load<Texture2D>("layer1");
            texLayer2 = Content.Load<Texture2D>("layer2");

            //--------------------------------------------------
            tex_fireBall = Content.Load<Texture2D>("fireBall");
            tex_bullet1 = Content.Load<Texture2D>("bullet1");
            texBar1 = Content.Load<Texture2D>("bar");
            texTrap1 = Content.Load<Texture2D>("trap1");
            texDoor = Content.Load<Texture2D>("door");
            texStone = Content.Load<Texture2D>("stone");
            tex_platform = Content.Load<Texture2D>("platform");
            tex_heart = Content.Load<Texture2D>("heart");

            layer0 = new ImageBackground(texLayer0, Color.White, graphicsDevice);
            layer1 = new ScrollBackGround(texLayer1, new Rectangle(0, 0, screenWidth, screenHeight),
                                            new Rectangle(0, 0, screenWidth, screenHeight), -0.6f, 2);
            layer2 = new ScrollBackGround(texLayer2, new Rectangle(0, 0, screenWidth, screenHeight),
                                            new Rectangle(0, 0, screenWidth, screenHeight), -0.3f, 2);
            //---------------END Load all assets---------------


            //---------------START Create the stuff---------------
            list_Bullet = new SpriteList();

            bullet1 = new Sprite3(true, tex_bullet1, -9000, 0);
            list_Bullet.addSpriteReuse(bullet1);

            platform = new Sprite3(true, tex_platform, 0, 991);
            platform.setWidthHeight(1980, 89);
            platform.setWidthHeightOfTex(1980, 89);
            platform.setBB(0, 0, 1980, 89);

            heart1 = new Sprite3(true, tex_heart, 42, 38);
            heart1.setWidthHeight(64, 64);
            heart1.setWidthHeightOfTex(64, 64);
            heart1.setBB(0, 0, 64, 64);

            heart2 = new Sprite3(true, tex_heart, 112, 38);
            heart2.setWidthHeight(64, 64);
            heart2.setWidthHeightOfTex(64, 64);
            heart2.setBB(0, 0, 64, 64);

            heart3 = new Sprite3(true, tex_heart, 192, 38);
            heart3.setWidthHeight(64, 64);
            heart3.setWidthHeightOfTex(64, 64);
            heart3.setBB(0, 0, 64, 64);

            heart4 = new Sprite3(true, tex_heart, 272, 38);
            heart4.setWidthHeight(64, 64);
            heart4.setWidthHeightOfTex(64, 64);
            heart4.setBB(0, 0, 64, 64);

            bar1 = new Sprite3(true, texBar1, 356, 754);
            bar1.setWidthHeight(144, 49);
            bar1.setWidthHeightOfTex(144, 49);
            bar1.setBB(0, 0, 144, 49);


            rect1 = new Rectangle(1135, 987, 93, 865);

            //----------------------1. Hero STAND-----------------------
            //Creat the stand LEFT pose for hero
            standLeft_hero = new Sprite3(true, tex_standLeft_hero, 990, 910);
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

            //---------------------Slime walke to right-----------------------
            slime_Right_List = new SpriteList();
            Sprite3 s1;
            for (int i = 0; i < 10; i++)
            {
                s1 = new Sprite3(true, tex_slime_walk_right, -3000 + i * 350, 925);
                s1.setDeltaSpeed(new Vector2((float)(0.5 + rnd.NextDouble()), 0));
                s1.setWidthHeight(40, 70);
                s1.setBB(0, 0, 40, 70);
                s1.setXframes(15);
                s1.setYframes(1);
                s1.setMoveAngleDegrees(0);
                Vector2[] slime_walkVector = new Vector2[15];
                slime_walkVector[0].X = 0; slime_walkVector[0].Y = 0;
                slime_walkVector[1].X = 1; slime_walkVector[1].Y = 0;
                slime_walkVector[2].X = 2; slime_walkVector[2].Y = 0;
                slime_walkVector[3].X = 3; slime_walkVector[3].Y = 0;
                slime_walkVector[4].X = 4; slime_walkVector[4].Y = 0;
                slime_walkVector[5].X = 5; slime_walkVector[5].Y = 0;
                slime_walkVector[6].X = 6; slime_walkVector[6].Y = 0;
                slime_walkVector[7].X = 7; slime_walkVector[7].Y = 0;
                slime_walkVector[8].X = 8; slime_walkVector[8].Y = 0;
                slime_walkVector[9].X = 9; slime_walkVector[9].Y = 0;
                slime_walkVector[10].X = 10; slime_walkVector[10].Y = 0;
                slime_walkVector[11].X = 11; slime_walkVector[11].Y = 0;
                slime_walkVector[12].X = 12; slime_walkVector[12].Y = 0;
                slime_walkVector[13].X = 13; slime_walkVector[13].Y = 0;
                slime_walkVector[14].X = 14; slime_walkVector[14].Y = 0;
                s1.setAnimationSequence(slime_walkVector, 0, 14, 15);
                s1.animationStart();
                slime_Right_List.addSpriteReuse(s1);
            }

            //---------------------Slime walke to left-----------------------
            slime_Left_List = new SpriteList();
            Sprite3 s2;
            for (int i = 0; i < 10; i++)
            {
                s2 = new Sprite3(true, tex_slime_walk_left, 3000 + -i * 350, 925);
                s2.setDeltaSpeed(new Vector2((float)(-0.5 + rnd.NextDouble()), 0));
                s2.setWidthHeight(40, 70);
                s2.setBB(0, 0, 40, 70);
                s2.setXframes(15);
                s2.setYframes(1);
                s2.setMoveAngleDegrees(0);
                Vector2[] slime_walkVector = new Vector2[15];
                slime_walkVector[0].X = 14; slime_walkVector[0].Y = 0;
                slime_walkVector[1].X = 13; slime_walkVector[1].Y = 0;
                slime_walkVector[2].X = 12; slime_walkVector[2].Y = 0;
                slime_walkVector[3].X = 11; slime_walkVector[3].Y = 0;
                slime_walkVector[4].X = 10; slime_walkVector[4].Y = 0;
                slime_walkVector[5].X = 9; slime_walkVector[5].Y = 0;
                slime_walkVector[6].X = 8; slime_walkVector[6].Y = 0;
                slime_walkVector[7].X = 7; slime_walkVector[7].Y = 0;
                slime_walkVector[8].X = 6; slime_walkVector[8].Y = 0;
                slime_walkVector[9].X = 5; slime_walkVector[9].Y = 0;
                slime_walkVector[10].X = 4; slime_walkVector[10].Y = 0;
                slime_walkVector[11].X = 3; slime_walkVector[11].Y = 0;
                slime_walkVector[12].X = 2; slime_walkVector[12].Y = 0;
                slime_walkVector[13].X = 1; slime_walkVector[13].Y = 0;
                slime_walkVector[14].X = 0; slime_walkVector[14].Y = 0;
                s2.setAnimationSequence(slime_walkVector, 0, 14, 15);
                s2.animationStart();
                slime_Right_List.addSpriteReuse(s2);
            }

        }

        public override void Update(GameTime gameTime)
        {
            preKeyState = keyState;
            keyState = Keyboard.GetState();
            //ticks++;
            in_music_background.Play();


            slime_Right_List.moveDeltaXY();
            slime_Right_List.animationTick(gameTime);

            slime_Left_List.moveDeltaXY();
            slime_Left_List.animationTick(gameTime);


            if (keyState.IsKeyDown(Keys.B) && preKeyState.IsKeyUp(Keys.B)) { showbb = !showbb; } //set bouding box to appear in the screen

            if (keyState.IsKeyDown(Keys.P) && preKeyState.IsKeyUp(Keys.P)) { gameStateManager.setLevel(7);  } //Pause key

            // Bullet collision detection 
            // Shoot to slime right
            for (int k = 0; k < list_Bullet.count(); k++)
            {
                Sprite3 m = list_Bullet.getSprite(k);
                for (int i = 0; i < slime_Right_List.count(); i++)
                {
                    Sprite3 e = slime_Right_List.getSprite(i);
                    if (e == null) continue;
                    if (!e.active) continue;
                    if (!e.visible) continue;
                    if (e.collision(m))
                    {
                        e.setActive(false);
                        m.setActive(false);
                        totalKilled = totalKilled + 1;
                    }
                }
            }


            // Bullet collision detection 
            // Shoot to slime left
            for (int k = 0; k < list_Bullet.count(); k++)
            {
                Sprite3 m = list_Bullet.getSprite(k);
                for (int i = 0; i < slime_Right_List.count(); i++)
                {
                    Sprite3 e = slime_Right_List.getSprite(i);
                    if (e == null) continue;
                    if (!e.active) continue;
                    if (!e.visible) continue;
                    if (e.collision(m))
                    {
                        e.setActive(false);
                        m.setActive(false);
                        totalKilled = totalKilled + 1;
                    }
                }
            }


            //Hero get hit from slime right
            for (int i = 0; i < slime_Right_List.count(); i++)
            {
                Sprite3 s1 = slime_Right_List.getSprite(i);
                if (standLeft_hero.collision(s1))
                {
                    s1.setActive(false);
                    totalHearts = totalHearts - 1;
                }
            }

            //Hero get hit from slime left
            for (int i = 0; i < slime_Left_List.count(); i++)
            {
                Sprite3 s1 = slime_Left_List.getSprite(i);
                if (standLeft_hero.collision(s1))
                {
                    s1.setActive(false);
                    totalHearts = totalHearts - 1;
                }
            }


            if (totalHearts == 3) heart4.setActiveAndVisible(false);
            else if (totalHearts == 2) heart3.setActiveAndVisible(false);
            else if (totalHearts == 1) heart2.setActiveAndVisible(false);
            else if (totalHearts == 0)
            {
                heart1.setActiveAndVisible(false);
                gameStateManager.pushLevel(5);
            }


            //1.Control the hero
            //---------------MOVEMENT PART START-------------
            if (keyState.IsKeyUp(Keys.Right))
            {
                standLeft_hero.moveByDeltaXY();
                standLeft_hero.animationTick(gameTime);
                standRight_hero.setActiveAndVisible(true);
                standLeft_hero.setActiveAndVisible(false);
                runLeft_hero.setActiveAndVisible(false);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                standRight_hero.moveByDeltaXY();
                standRight_hero.animationTick(gameTime);

                runLeft_hero.setActiveAndVisible(false);
                //  runLRight_hero.setActiveAndVisible(true);
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
                // runLRight_hero.setActiveAndVisible(false);
                standLeft_hero.setActiveAndVisible(false);
                standRight_hero.setActiveAndVisible(false);

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

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && jump)
            {
                velocity.Y = -jumpSpeed * ((float)gameTime.ElapsedGameTime.TotalSeconds);
                jump = false;
            }

            //Update the shoot bullet
            if (keyState.IsKeyDown(Keys.Z)) { in_music_shoot.Stop(); in_music_shoot.Play(); launchBullet_Left(); } //shoot to left                
            if (keyState.IsKeyDown(Keys.C)) { in_music_shoot.Stop(); in_music_shoot.Play(); launchBullet_Right(); } //shoot to Right                
            if (keyState.IsKeyDown(Keys.X)) { in_music_shoot.Stop(); in_music_shoot.Play(); launchBullet_Top(); } //shoot to Top               

            //Pause Screen
            //if (keyState.IsKeyDown(Keys.R)) { gameStateManager. } //shoot to Top               

            //Help Screen
            if (keyState.IsKeyDown(Keys.F1)) { gameStateManager.pushLevel(8); }

            //Jump Logic
            if (jump) //jump is false
            {
                position.Y = 910;
                velocity.Y = 0;
            }
            else //jump is true
            {
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
           
            

            if (position.Y >= 910)
            {
                jump = true;
            }
            else
            {
                jump = false;
            }

            position += velocity;
            standLeft_hero.setPos(position.X + 990, position.Y); //1607, 910 initial position
            standRight_hero.setPos(position.X + 990, position.Y);
            runLeft_hero.setPos(position.X + 990, position.Y);

            if(runLeft_hero.getPosX()>1980) { in_music_background.Stop() ; gameStateManager.setLevel(2); }

            layer1.Update(gameTime); //Scrollbackground
            layer2.Update(gameTime); //Scrollbackground
            list_Bullet.moveByAngleSpeed();
            list_Bullet.animationTick(gameTime);
            bullet1.moveByAngleSpeed();
            bullet1.moveByDeltaXY();
            bullet1.Update(gameTime);

 
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.DeepSkyBlue);

            layer0.Draw(spriteBatch);
            layer1.Draw(spriteBatch);
            layer2.Draw(spriteBatch);

            platform.Draw(spriteBatch);
            heart1.Draw(spriteBatch);
            heart2.Draw(spriteBatch);
            heart3.Draw(spriteBatch);
            heart4.Draw(spriteBatch);
            spriteBatch.DrawString(font3, "Your have killed: " + totalKilled, new Vector2(750, 20), Color.Brown);
            spriteBatch.DrawString(font3, "Help key: F1", new Vector2(1600, 15), Color.Brown);
            spriteBatch.DrawString(font3, "Pause key: P ", new Vector2(1600, 65), Color.Brown);
            //spriteBatch.DrawString(font3, "Your hearts:" + totalHearts, new Vector2(1200, 85), Color.Brown);

            slime_Right_List.Draw(spriteBatch);
            slime_Left_List.Draw(spriteBatch);

            bullet1.Draw(spriteBatch);

            list_Bullet.drawActive(spriteBatch);
            standLeft_hero.Draw(spriteBatch);
            //  runLeft_droid.Draw(spriteBatch);
            standRight_hero.Draw(spriteBatch);
            runLeft_hero.Draw(spriteBatch);

            if (showbb) //-------------------BOUDING BOX------------------
            {
                platform.drawInfo(spriteBatch, Color.Red, Color.Blue);
               
                list_Bullet.drawInfo(spriteBatch, Color.Red, Color.Blue);
                runLeft_hero.drawInfo(spriteBatch, Color.Red, Color.Blue);

                slime_Right_List.drawInfo(spriteBatch, Color.Red, Color.Blue);
                slime_Left_List.drawInfo(spriteBatch, Color.Red, Color.Blue);

                //hero
                //standLeft_hero.drawInfo(spriteBatch, Color.Red, Color.Blue);
                //standRight_hero.drawInfo(spriteBatch, Color.Red, Color.Blue);
                //runLeft_droid.drawInfo(spriteBatch, Color.Red, Color.Blue);
                //trap1.drawInfo(spriteBatch, Color.Red, Color.Blue);
            }
        }

        void launchBullet_Left()
        {
            bullet1 = new Sprite3(true, tex_bullet1, standRight_hero.getPosX(), standRight_hero.getPosY()+50);
            bullet1.setWidthHeight(15, 15);
            bullet1.setBB(0, 0, 15, 15);
            bullet1.setDisplayAngleDegrees(180); //rotate
            bullet1.setMoveAngleDegrees(180);
            bullet1.setMoveSpeed(5f);
            list_Bullet.addSpriteReuse(bullet1);
        }

        void launchBullet_Right()
        {
            bullet1 = new Sprite3(true, tex_bullet1, standRight_hero.getPosX(), standRight_hero.getPosY()+50);
            bullet1.setWidthHeight(15, 15);
            bullet1.setBB(0, 0, 15, 15);
            bullet1.setDisplayAngleDegrees(0); //rotate
            bullet1.setMoveAngleDegrees(0);
            bullet1.setMoveSpeed(5f);
            list_Bullet.addSpriteReuse(bullet1);
        }

        void launchBullet_Top()
        {
            bullet1 = new Sprite3(true, tex_bullet1, standRight_hero.getPosX(), standRight_hero.getPosY());
            bullet1.setWidthHeight(15, 15);
            bullet1.setBB(0, 0, 15, 15);
            bullet1.setDisplayAngleDegrees(270); //rotate
            bullet1.setMoveAngleDegrees(270);
            bullet1.setMoveSpeed(5f);
            list_Bullet.addSpriteReuse(bullet1);
        }
    }
}
