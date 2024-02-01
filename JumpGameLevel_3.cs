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
    class JumpGameLevel_3 : RC_GameStateParent
    {
        //Keyboard control
        KeyboardState keyState;
        KeyboardState preKeyState;

        int timmer = 0;
        int bossdietimmer = 0;

        SpriteFont font3;

       private int totalHearts = 4;
        
        //Sprite for test
        Sprite3 test;
        Texture2D tex_test;

        Rectangle rectangle = new Rectangle(0, 0, 1980, 1080);
        WayPointList wl = null;
        //Variable for screen
        int screenWidth;
        int screenHeight;
        Rectangle screenRect;

        //Bounding box boolean value
        bool showbb = false;

        ScrollBackGround bg_0;
        ScrollBackGround bg_1;
        Texture2D tex_bg_1;
        Texture2D tex_bg_0;

        Rectangle rect1;

        public static SoundEffect music_background;
        public static SoundEffectInstance in_music_background;



        //-----------------------BOSS-------------------
        Sprite3 boss;
        Sprite3 boss_die;
        Texture2D tex_boss_die;
        Texture2D tex_boss;
        HealthBar boss_healthbar;
        Sprite3 weapon;
        SpriteList weapon_list;
        Texture2D tex_weapon;
        int boss_point = 100;

        //--------------------AI--------------------------
        bool moving = false;
        float speed = 0.1f;
        float pushBackStrength = 3f;
        float pushBackDistance = 15f; // bit above bounding radius

        //Counts of hearts
        Sprite3 heart1, heart2, heart3, heart4;
        Texture2D tex_heart;

        //Counstruct the plates
        Sprite3 plate3;
        Sprite3 plate3a;
        Sprite3 plate2;
        Sprite3 platform;
        Sprite3 block2;
        Sprite3 bar1;
        Sprite3 bar2;

        Texture2D tex_plat2;
        Texture2D tex_plat3;
        Texture2D tex_platform;
        Texture2D texBar2;
        Texture2D texBar1;
        Texture2D texTwoBlock;
        Texture2D texStone;


        //The hereo
        Sprite3 standLeft_hero;
        Sprite3 standRight_hero;
        Sprite3 runLeft_hero;
        Sprite3 runLRight_hero;

        Texture2D tex_standLeft_hero;
        Texture2D tex_runLeft_hero;
        Texture2D tex_standRight_hero;
        Texture2D tex_runRight_hero;

        //Hero movement
        float spped_hero = 0.5f;
        int lhs = 10;
        int rhs = 10000;

        //Bullets
        SpriteList list_Bullet = null;
        Sprite3 bullet1;
        Texture2D tex_bullet1;

        //Jump declaration
        Vector2 position, velocity;
        const float gravity = 100f;
        float moveSpeed = 350f;
        float jumpSpeed = 1700;
        bool jump = false;  //false: cannot jump, true: can jump

        public JumpGameLevel_3()
        {
            position = velocity = Vector2.Zero;
        }

        public override void LoadContent()
        {
            //Set the screen window
            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);

            //Load font
            font3 = Content.Load<SpriteFont>("spritefont4");
            
            //Load music 
            music_background = Content.Load<SoundEffect>("background");
            in_music_background = music_background.CreateInstance();

            tex_test = Content.Load<Texture2D>("testSprite");
            tex_bullet1 = Content.Load<Texture2D>("bullet1");

            //------------------BOSS-----------------
            tex_boss = Content.Load<Texture2D>("boss");
            tex_boss_die = Content.Load<Texture2D>("boss");
            tex_weapon = Content.Load<Texture2D>("weapon");

            //---------------Load HERO START---------------
            tex_standLeft_hero = Content.Load<Texture2D>("spriteToLeft");
            tex_runLeft_hero = Content.Load<Texture2D>("spriteToLeft");
            tex_standRight_hero = Content.Load<Texture2D>("spriteToRight");
            tex_runRight_hero = Content.Load<Texture2D>("spriteToRight");
            //---------------Load HERO END---------------

            //---------------START Load all assets---------------
            tex_bg_0 = Content.Load<Texture2D>("bg0");
            tex_bg_1 = Content.Load<Texture2D>("bg_2");
            tex_plat2 = Content.Load<Texture2D>("plat2");
            tex_plat3 = Content.Load<Texture2D>("plat3");
            texBar1 = Content.Load<Texture2D>("bar");
            texBar2 = Content.Load<Texture2D>("bar");
            tex_platform = Content.Load<Texture2D>("platform");
            tex_heart = Content.Load<Texture2D>("heart");

            //---------------END Load all assets---------------

            //---------------START Create the stuff---------------
            bg_0 = new ScrollBackGround(tex_bg_0, new Rectangle(0, 0, screenWidth, screenHeight),
                                           new Rectangle(0, 0, screenWidth, screenHeight), -0.6f, 2);
            bg_1 = new ScrollBackGround(tex_bg_1, new Rectangle(0, 0, screenWidth, screenHeight),
                                          new Rectangle(0, 0, screenWidth, screenHeight), -0.3f, 2);

            list_Bullet = new SpriteList();

            bullet1 = new Sprite3(true, tex_bullet1, -9000, 0);
            list_Bullet.addSpriteReuse(bullet1);

            plate3a = new Sprite3(true, tex_plat3, 0, 847);
            plate3a.setDeltaSpeed(new Vector2((float)(-0.1), 0));
            plate3a.setWidthHeight(429, 49);
            plate3a.setWidthHeightOfTex(429, 49);
            plate3a.setBB(0, 0, 429, 49);

            plate3 = new Sprite3(true, tex_plat3,572,847 );
            plate3.setDeltaSpeed(new Vector2((float)(-0.1), 0));
            plate3.setWidthHeight(429, 49);
            plate3.setWidthHeightOfTex(429, 49);
            plate3.setBB(0, 0, 429, 49);

            plate2 = new Sprite3(true, tex_plat2, 1114, 847);
            plate2.setDeltaSpeed(new Vector2((float)(-0.1), 0));
            plate2.setWidthHeight(287, 49);
            plate2.setWidthHeightOfTex(287, 49);
            plate2.setBB(0, 0, 287, 49);

            bar1 = new Sprite3(true, texBar1, 1573, 847);
            bar1.setDeltaSpeed(new Vector2((float)(-0.1), 0));
            bar1.setWidthHeight(144, 49);
            bar1.setWidthHeightOfTex(144, 49);
            bar1.setBB(0, 0, 144, 49);

            bar2 = new Sprite3(true, texBar1, 1860, 847);
            bar2.setDeltaSpeed(new Vector2((float)(-0.1), 0));
            bar2.setWidthHeight(144, 49);
            bar2.setWidthHeightOfTex(144, 49);
            bar2.setBB(0, 0, 144, 49);

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

            platform = new Sprite3(true, tex_platform, 0, 991);
            platform.setWidthHeight(1980, 89);
            platform.setWidthHeightOfTex(1980, 89);
            platform.setBB(0, 0, 1980, 89);

            //---------------END Create the stuff---------------

            rect1 = new Rectangle(1135, 987, 93, 865);

            //load the boss healthbar
            boss_healthbar = new HealthBar(Color.Red, Color.White, Color.Black, true);
            boss_healthbar.setPos(750, 100);
            boss_healthbar.setWidthHeight(500, 45);
            boss_healthbar.setHp(boss_point);

            //-------------------------BOSS-------------------------
            boss = new Sprite3(true, tex_boss, 1200, 100);
            boss.setDeltaSpeed(new Vector2((float)(-1), 0));
            boss.setWidthHeight(495, 495);
            boss.setBB(85, 20, 225, 225);
            boss.setXframes(10);
            boss.setYframes(9);
            boss.setMoveAngleDegrees(180);

            //Set the frame of stand pose of BOSS
            Vector2[] boosVector = new Vector2[8];
            boosVector[0].X = 2; boosVector[0].Y = 1;
            boosVector[1].X = 3; boosVector[1].Y = 1;
            boosVector[2].X = 4; boosVector[2].Y = 1;
            boosVector[3].X = 5; boosVector[3].Y = 1;
            boosVector[4].X = 6; boosVector[4].Y = 1;
            boosVector[5].X = 7; boosVector[5].Y = 1;
            boosVector[6].X = 8; boosVector[6].Y = 1;
            boosVector[7].X = 9; boosVector[7].Y = 1;
            boss.setAnimationSequence(boosVector, 0, 7, 8);
            boss.animationStart();

            //-------------------------BOSS DIE-------------------------
            boss_die = new Sprite3(true, tex_boss_die, 1200, 100);
            boss_die.setDeltaSpeed(new Vector2((float)(-1), 0));
            boss_die.setWidthHeight(495, 495);
            boss_die.setBB(85, 20, 225, 225);
            boss_die.setXframes(10);
            boss_die.setYframes(9);
            boss_die.setMoveAngleDegrees(180);

            //Set the frame of stand pose of BOSS
            Vector2[] boos_die_Vector = new Vector2[10];
            boos_die_Vector[0].X = 9; boos_die_Vector[0].Y = 7;
            boos_die_Vector[1].X = 8; boos_die_Vector[1].Y = 7;
            boos_die_Vector[2].X = 7; boos_die_Vector[2].Y = 7;
            boos_die_Vector[3].X = 6; boos_die_Vector[3].Y = 7;
            boos_die_Vector[4].X = 6; boos_die_Vector[4].Y = 7;
            boos_die_Vector[5].X = 6; boos_die_Vector[5].Y = 7;
            boos_die_Vector[6].X = 6; boos_die_Vector[6].Y = 7;
            boos_die_Vector[7].X = 6; boos_die_Vector[7].Y = 7;
            boos_die_Vector[8].X = 6; boos_die_Vector[8].Y = 7;
            boos_die_Vector[9].X = 6; boos_die_Vector[9].Y = 7;
            boss_die.setAnimationSequence(boos_die_Vector, 0, 9, 10);
            //boss_die.animationStart();

            //---------------------Weapon Sprite-----------------------
            weapon = new Sprite3(true, tex_weapon, 1500, 200);
            weapon.setDeltaSpeed(new Vector2(0, 1));
            weapon.setWidthHeight(72, 176);
            weapon.setBB(0, 0, 72, 176);
            weapon.setMoveAngleDegrees(0);
            weapon.setMoveSpeed(0.1f);

            //---------------------Weapon list 1-----------------------
            weapon_list = new SpriteList();

            Sprite3 w1;
            for (int i = 0; i < 5; i++)
            {
                w1 = new Sprite3(true, tex_weapon,  boss.getPosX()+100, boss.getPosY()+50);
                w1.setDeltaSpeed(new Vector2(0, 1));
                w1.setWidthHeight(72, 176);
                w1.setBB(0, 0, 72, 176);
                w1.setMoveAngleDegrees(0);
                w1.setMoveSpeed(1.4f);
                weapon_list.addSpriteReuse(w1);
            }


            //----------------------1. Hero STAND-----------------------
            //Test sprite
            test = new Sprite3(true, tex_test, 990, 847);
            test.setWidthHeight(57, 80);
            test.setBB(10, 5, 57, 75);

            //Creat the stand LEFT pose for hero
            standLeft_hero = new Sprite3(true, tex_standLeft_hero, 990, 847);

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
            timmer++;
            preKeyState = keyState;
            keyState = Keyboard.GetState();

            in_music_background.Play();

           // moveBoss(); //Ai system waypoint


            //Hero get hit . weapon
           if (standLeft_hero.collision(weapon)) { totalHearts = totalHearts - 1; }

            //Hero get hit . weapon list
            //for (int i = 0; i < weapon_list.count(); i++)
            //{
            //    Sprite3 w1 = weapon_list.getSprite(i);
            //    if (standLeft_hero.collision(w1))
            //    {
            //        w1.setActive(false);
            //        totalHearts = totalHearts - 1;
            //    }
            //}


            // Shoot to weapon
            for (int k = 0; k < list_Bullet.count(); k++)
            {
                Sprite3 m = list_Bullet.getSprite(k);
                for (int i = 0; i < weapon_list.count(); i++)
                {
                    Sprite3 e = weapon_list.getSprite(i);
                    if (e == null) continue;
                    if (!e.active) continue;
                    if (!e.visible) continue;
                    if (e.collision(m))
                    {
                        e.setActive(false);
                        m.setActive(false);
                        JumpGameLevel_2.totalKilled = JumpGameLevel_2.totalKilled + 1;
                    }
                }
            }

            // Shoot to boss
            for (int k = 0; k < list_Bullet.count(); k++)
            {
                Sprite3 m = list_Bullet.getSprite(k);
                if (m == null) continue;
                if (!m.active) continue;
                if (!m.visible) continue;
                if (m.collision(boss))
                {
                    m.setActive(false);
                    if (boss_point <= 0)
                    {
                        //bossdietimmer++;
                       // boss_healthbar.setHp(0);
                        boss.setAnimFinished(2);
                        in_music_background.Stop();
                        gameStateManager.setLevel(5);
                    }
                    else
                    {
                        m.setActive(false);
                        boss_point = boss_point - 1;
                        boss_healthbar.setHp(boss_point);
                    }
                }
            }

            //if (bossdietimmer > 120)
            //{
            //    gameStateManager.setLevel(5);
            //}

            //Hero get hit . weapon List
            for (int i = 0; i < weapon_list.count(); i++)
            {
                Sprite3 w1 = weapon_list.getSprite(i);
                if (standLeft_hero.collision(w1))
                {
                    w1.setActive(false);
                    totalHearts = totalHearts - 1;
                }
            }

            //jump to the underground
            if (standLeft_hero.getPosY() == 1000)
            {

            }

            if (totalHearts == 3)
            {
                heart4.setActiveAndVisible(false);
            }
            if (totalHearts == 2)
            {
                heart3.setActiveAndVisible(false);
            }
            if (totalHearts == 1)
            {
                heart2.setActiveAndVisible(false);
            }
            if (totalHearts == 0)
            {
                heart1.setActiveAndVisible(false);
                gameStateManager.pushLevel(5);
            }

            //set bouding box to appear in the screen
            if (keyState.IsKeyDown(Keys.B) && preKeyState.IsKeyUp(Keys.B))
            {
                showbb = !showbb;
            }

            //Pause Screen
            if (keyState.IsKeyDown(Keys.P) && preKeyState.IsKeyUp(Keys.P)) { gameStateManager.setLevel(7); }
            //Help Screen
            if (keyState.IsKeyDown(Keys.F1)) { gameStateManager.pushLevel(8); }

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
            if (keyState.IsKeyDown(Keys.Z)) { launchBullet_Left(); } //shoot to left                
            if (keyState.IsKeyDown(Keys.C)) { launchBullet_Right(); } //shoot to Right                
            if (keyState.IsKeyDown(Keys.X)) { launchBullet_Top(); } //shoot to Top               



            if (jump) //jump is false
            {
                position.Y = 770;
                velocity.Y = 0;

                if (!standLeft_hero.getBoundingBoxAA().Intersects(plate3.getBoundingBoxAA())
                    && !standLeft_hero.getBoundingBoxAA().Intersects(plate3a.getBoundingBoxAA())
                     && !standLeft_hero.getBoundingBoxAA().Intersects(plate2.getBoundingBoxAA())
                     && !standLeft_hero.getBoundingBoxAA().Intersects(bar1.getBoundingBoxAA())
                     && !standLeft_hero.getBoundingBoxAA().Intersects(bar2.getBoundingBoxAA()))
                {
                    position.Y = 1000;
                    standLeft_hero.setPos(position.X + 20, position.Y);
                }
            }
            else //jump is true
            {
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (position.Y >= 770)
            {
                jump = true;
            }
            else // if (position.Y =< 766)
            {
                jump = false;
            }

            position += velocity;
            standLeft_hero.setPos(position.X , position.Y); //1607, 910 initial position
            standRight_hero.setPos(position.X, position.Y);
            runLeft_hero.setPos(position.X , position.Y); //1607, 910 initial position
            test.setPos(standLeft_hero.getPos());

            if (standLeft_hero.getPosY() == 1000)
            {
                //standLeft_hero.setPos(position.X + 990, position.Y+990); //1607, 910 initial position
                //standRight_hero.setPos(position.X + 990, position.Y+990);
                //runLeft_hero.setPos(position.X + 990, position.Y+990); //1607, 910 initial position
                totalHearts = totalHearts - 1;
            }


            if (boss.getPosX() == 0 && boss_die.getPosX() == 0)
            {
                boss.setDeltaSpeed(new Vector2((float)(1), 0));
                boss_die.setDeltaSpeed(new Vector2((float)(1), 0));
            }
            if (boss.getPosX() == 1700 && boss_die.getPosX() == 1700)
            {
                boss.setDeltaSpeed(new Vector2((float)(-1), 0));
                boss_die.setDeltaSpeed(new Vector2((float)(-1), 0));
            }

            if ((runLeft_hero.getPosX() - 60) > boss.getPosX())
            {
                boss.setDeltaSpeed(new Vector2((float)(2), 0));
            }
            if ((runLeft_hero.getPosX() - 60) < boss.getPosX())
            {
                boss.setDeltaSpeed(new Vector2((float)(-2), 0));
            }

            if ((runLeft_hero.getPosX() + 100) > boss.getPosX() && (runLeft_hero.getPosX()-20)< boss.getPosX())
            {
                if (timmer % 30 == 0) { 
                weapon = new Sprite3(true, tex_weapon, boss.getPosX()+200, boss.getPosY()+100);
                weapon.setDeltaSpeed(new Vector2(0, 1));
                weapon.setWidthHeight(72, 176);
                weapon.setBB(0, 0, 72, 176);
                weapon.setMoveAngleDegrees(0);
                weapon.setMoveSpeed(1f);
                weapon_list.addSpriteReuse(weapon);
                }
            }

            weapon_list.moveDeltaXY();
            weapon_list.animationTick(gameTime);
            //BOSS 
            boss.moveByDeltaXY();
            boss.animationTick(gameTime);

            boss_die.moveByDeltaXY();
            boss_die.animationTick(gameTime);
           
            plate3.moveByDeltaXY();
            plate3.animationTick(gameTime);

            plate3a.moveByDeltaXY();
            plate3a.animationTick(gameTime);

            plate2.moveByDeltaXY();
            plate2.animationTick(gameTime);

            bar1.moveByDeltaXY();
            bar1.animationTick(gameTime);

            bar2.moveByDeltaXY();
            bar2.animationTick(gameTime);

            boss_healthbar.Update(gameTime);
            list_Bullet.moveByAngleSpeed();
            list_Bullet.animationTick(gameTime);
            bullet1.moveByAngleSpeed();
            bullet1.moveByDeltaXY();
            bullet1.Update(gameTime);

            plate3.Update(gameTime);
            plate3a.Update(gameTime);
            plate2.Update(gameTime);
            bar1.Update(gameTime);
            bar2.Update(gameTime);

            bg_0.Update(gameTime); //Scrollbackground
            bg_1.Update(gameTime); //Scrollbackground
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Aqua);

            bg_0.Draw(spriteBatch);
            bg_1.Draw(spriteBatch);

            bar1.Draw(spriteBatch);
            bar2.Draw(spriteBatch);
            plate2.Draw(spriteBatch);
            plate3.Draw(spriteBatch);
            plate3a.Draw(spriteBatch);
            heart1.Draw(spriteBatch);
            heart2.Draw(spriteBatch);
            heart3.Draw(spriteBatch);
            heart4.Draw(spriteBatch);

            standLeft_hero.Draw(spriteBatch);
            standRight_hero.Draw(spriteBatch);
            runLeft_hero.Draw(spriteBatch);

            boss.Draw(spriteBatch);
            // boss_die.Draw(spriteBatch);

            //weapon.Draw(spriteBatch);
           weapon_list.Draw(spriteBatch);

            boss_healthbar.Draw(spriteBatch);

            list_Bullet.drawActive(spriteBatch);
            bullet1.Draw(spriteBatch);

            spriteBatch.DrawString(font3, "Your have killed: " + JumpGameLevel_1.totalKilled + JumpGameLevel_2.totalKilled, new Vector2(750, 20), Color.Brown);
            spriteBatch.DrawString(font3, "Help key: F1", new Vector2(1600, 15), Color.Brown);
            spriteBatch.DrawString(font3, "Pause key: P ", new Vector2(1600, 65), Color.Brown);
            //spriteBatch.DrawString(font3, "Your hearts:" + totalHearts, new Vector2(1200, 85), Color.Brown);  //For debug

            //BOUDING BOX
            if (showbb)
            {
                boss.drawInfo(spriteBatch, Color.Red, Color.Blue);
                // boss_die.drawInfo(spriteBatch, Color.Red, Color.Blue);


                weapon_list.drawInfo(spriteBatch, Color.Red, Color.Blue);

                //test.drawInfo(spriteBatch, Color.Red, Color.Blue);
                list_Bullet.drawInfo(spriteBatch, Color.Red, Color.Blue);
                bar1.drawInfo(spriteBatch, Color.Red, Color.Blue);
                bar2.drawInfo(spriteBatch, Color.Red, Color.Blue);
                plate2.drawInfo(spriteBatch, Color.Red, Color.Blue);
                plate3a.drawInfo(spriteBatch, Color.Red, Color.Blue);
                plate3.drawInfo(spriteBatch, Color.Red, Color.Blue);
                platform.drawInfo(spriteBatch, Color.Red, Color.Blue);
                standLeft_hero.drawInfo(spriteBatch, Color.Red, Color.Blue);
            }         
        }

        void launchBullet_Left()
        {
            // bullet1.setVisible(true);
            bullet1 = new Sprite3(true, tex_bullet1, standRight_hero.getPosX(), standRight_hero.getPosY());
            bullet1.setWidthHeight(15, 15);
            bullet1.setBB(0, 0, 15, 15);
            bullet1.setDisplayAngleDegrees(180); //rotate
            bullet1.setMoveAngleDegrees(180);
            bullet1.setMoveSpeed(5f);
            list_Bullet.addSpriteReuse(bullet1);
        }

        void launchBullet_Right()
        {
            // bullet1.setVisible(true);
            bullet1 = new Sprite3(true, tex_bullet1, standRight_hero.getPosX(), standRight_hero.getPosY());
            bullet1.setWidthHeight(15, 15);
            bullet1.setBB(0, 0, 15, 15);
            bullet1.setDisplayAngleDegrees(0); //rotate
            bullet1.setMoveAngleDegrees(0);
            bullet1.setMoveSpeed(5f);
            list_Bullet.addSpriteReuse(bullet1);
        }

        void launchBullet_Top()
        {
            // bullet1.setVisible(true);
            bullet1 = new Sprite3(true, tex_bullet1, standRight_hero.getPosX(), standRight_hero.getPosY());
            bullet1.setWidthHeight(15, 15);
            bullet1.setBB(0, 0, 15, 15);
            bullet1.setDisplayAngleDegrees(270); //rotate
            bullet1.setMoveAngleDegrees(270);
            bullet1.setMoveSpeed(5f);
            list_Bullet.addSpriteReuse(bullet1);
        }
      
        void moveBoss() //waypoint algorithm Not to be used.
        {
            // first compute the  dx,dy vector at 'speed'
            Vector2 weaponPos = weapon.getPos();
            Vector2 baseDelta = runLeft_hero.getPos() - weaponPos;
            Vector2 moveDelta = baseDelta / baseDelta.Length();
            moveDelta = moveDelta * speed;
            // now compute a vector to push back from the trees
            Vector2 pushBack = new Vector2(0, 0);
            Vector2 newBeePos = weaponPos + moveDelta + pushBack;
            weapon.setPos(newBeePos);
            weapon.alignDisplayAngle();
            if ((newBeePos - runLeft_hero.getPos()).Length() <= speed) moving = false;
        }
    }
}
