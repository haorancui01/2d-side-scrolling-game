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
    class JumpGameLevel_2 : RC_GameStateParent
    {
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
        ImageBackground layer0; //sky
        ScrollBackGround cloud, cloud1; //clouds
        ScrollBackGround layer1 = null; //mountains

        Texture2D tex_cloud, tex_cloud1;
        Texture2D texLayer0, texLayer1;

        Sprite3 heart1, heart2, heart3, heart4;//Counts of hearts

        SpriteList list_Bullet = null;
        Sprite3 bullet1;

        SpriteList fireBall_list = null;

        Sprite3 platform;
        Sprite3 door;
        Sprite3 stone;
        Sprite3 bar1;


        Texture2D tex_fireBall;
        Texture2D tex_heart;
        Texture2D tex_bullet1;
        Texture2D tex_platform;
        Texture2D texBar1;

        Texture2D texStone;

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


        //---------------Enemy 2 Fly eye---------------
        SpriteList eyeList;
        Sprite3 eye_fly;
        Texture2D tex_eye_fly;
        Texture2D tex_eye_die;

        //---------------Falling stone----------------
        SpriteList meteor_list;
        Texture2D tex_meteor;

        Vector2 position, velocity;
        const float gravity = 100f;
        float moveSpeed = 350f;
        float jumpSpeed = 1700;
        bool jump = false;  //false: cannot jump, true: can jump

        //Partical system
        Texture2D Particle1;
        Texture2D Particle2;
        Texture2D tex;
       // int ticks = 0;
        ParticleSystem p;
       // int tix = 0;
        Color screenColour = Color.Tan;
        Vector2 pTarget = new Vector2(0, 0);
        WayPointList wl = null;
        Rectangle rectangle = new Rectangle(0, 0, 800, 600);
       // int psys = 1;


        public JumpGameLevel_2()
        {
            position = velocity = Vector2.Zero;
        }


        public override void LoadContent()
        {
            rnd = new Random();

            //Set the screen window
            screenWidth = graphicsDevice.Viewport.Width;
            screenHeight = graphicsDevice.Viewport.Height;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);

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

            //---------------Load ENEMY 2. START---------------
            tex_eye_fly = Content.Load<Texture2D>("eye_fly");
            tex_eye_die = Content.Load<Texture2D>("eye_die");

            //---------------START Load all assets---------------
            //Load the contents of layers
            texLayer0 = Content.Load<Texture2D>("sky_lightened");
            texLayer1 = Content.Load<Texture2D>("glacial_mountains");
            tex_cloud = Content.Load<Texture2D>("clouds1");
            tex_cloud1 = Content.Load<Texture2D>("clouds2");

            //--------------------------------------------------
            Particle1 = Content.Load<Texture2D>("Particle1");
            Particle2 = Content.Load<Texture2D>("Particle2");
            tex_fireBall = Content.Load<Texture2D>("fireBall");
            tex_bullet1 = Content.Load<Texture2D>("bullet1");
            tex_meteor = Content.Load<Texture2D>("fallingStone");
            texBar1 = Content.Load<Texture2D>("bar");
            texTrap1 = Content.Load<Texture2D>("trap1");
            texDoor = Content.Load<Texture2D>("door");
            texStone = Content.Load<Texture2D>("stone");
            tex_platform = Content.Load<Texture2D>("platform");
            tex_heart = Content.Load<Texture2D>("heart");

            layer0 = new ImageBackground(texLayer0, Color.White, graphicsDevice);
            layer1 = new ScrollBackGround(texLayer1, new Rectangle(0, 0, screenWidth, screenHeight),
                                            new Rectangle(0, 0, screenWidth, screenHeight), -0.6f, 2);
            cloud = new ScrollBackGround(tex_cloud, new Rectangle(0, 0, screenWidth, screenHeight),
                                            new Rectangle(0, 0, screenWidth, screenHeight), -0.4f, 2);
            cloud1 = new ScrollBackGround(tex_cloud1, new Rectangle(0, 0, screenWidth, screenHeight),
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

            stone = new Sprite3(true, texStone, 197, 931);
            stone.setMoveSpeed(1.1f);
            stone.setWidthHeight(64, 64);
            stone.setWidthHeightOfTex(64, 64);
            stone.setBB(0, 0, 64, 64);

            door = new Sprite3(true, texDoor, 1686, 870);
            door.setWidthHeight(120, 120);
            door.setWidthHeightOfTex(120, 120);
            door.setBB(0, 0, 120, 120);

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

            //---------------------eye fly-----------------------
            eyeList = new SpriteList();
            Sprite3 eye1;
            for (int i = 0; i < 10; i++)
            {
                eye1 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -100);
                eye1.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble()))); //-1,2
                eye1.setWidthHeight(450, 450);
                eye1.setBB(155, 174, 120, 100);
                eye1.setXframes(6);
                eye1.setYframes(1);
                eye1.setMoveAngleDegrees(0);
                eye1.setMoveSpeed(1f);
                //Set the frame of run pose of droid
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye1.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye1.animationStart();
                eyeList.addSpriteReuse(eye1);
            }

            Sprite3 eye2;
            for (int i = 0; i < 10; i++)
            {
                eye2 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -600);
                eye2.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                eye2.setWidthHeight(450, 450);
                eye2.setBB(155, 174, 120, 100);
                eye2.setXframes(6);
                eye2.setYframes(1);
                eye2.setMoveAngleDegrees(0);
                eye2.setMoveSpeed(1f);
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye2.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye2.animationStart();
                eyeList.addSpriteReuse(eye2);
            }

            Sprite3 eye3;
            for (int i = 0; i < 10; i++)
            {
                eye3 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -1100);
                eye3.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                eye3.setWidthHeight(450, 450);
                eye3.setBB(155, 174, 120, 100);
                eye3.setXframes(6);
                eye3.setYframes(1);
                eye3.setMoveAngleDegrees(0);
                eye3.setMoveSpeed(1f);
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye3.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye3.animationStart();
                eyeList.addSpriteReuse(eye3);
            }

            Sprite3 eye4;
            for (int i = 0; i < 10; i++)
            {
                eye4 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -1600);
                eye4.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                eye4.setWidthHeight(450, 450);
                eye4.setBB(155, 174, 120, 100);
                eye4.setXframes(6);
                eye4.setYframes(1);
                eye4.setMoveAngleDegrees(0);
                eye4.setMoveSpeed(1f);
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye4.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye4.animationStart();
                eyeList.addSpriteReuse(eye4);
            }

            Sprite3 eye5;
            for (int i = 0; i < 10; i++)
            {
                eye5 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -2100);
                eye5.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                eye5.setWidthHeight(450, 450);
                eye5.setBB(155, 174, 120, 100);
                eye5.setXframes(6);
                eye5.setYframes(1);
                eye5.setMoveAngleDegrees(0);
                eye5.setMoveSpeed(1f);
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye5.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye5.animationStart();
                eyeList.addSpriteReuse(eye5);
            }

            Sprite3 eye6;
            for (int i = 0; i < 10; i++)
            {
                eye6 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -2600);
                eye6.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                eye6.setWidthHeight(450, 450);
                eye6.setBB(155, 174, 120, 100);
                eye6.setXframes(6);
                eye6.setYframes(1);
                eye6.setMoveAngleDegrees(0);
                eye6.setMoveSpeed(1f);
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye6.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye6.animationStart();
                eyeList.addSpriteReuse(eye6);
            }

            Sprite3 eye7;
            for (int i = 0; i < 10; i++)
            {
                eye7 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -3100);
                eye7.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                eye7.setWidthHeight(450, 450);
                eye7.setBB(155, 174, 120, 100);
                eye7.setXframes(6);
                eye7.setYframes(1);
                eye7.setMoveAngleDegrees(0);
                eye7.setMoveSpeed(1f);
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye7.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye7.animationStart();
                eyeList.addSpriteReuse(eye7);
            }

            Sprite3 eye8;
            for (int i = 0; i < 10; i++)
            {
                eye8 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -3600);
                eye8.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                eye8.setWidthHeight(450, 450);
                eye8.setBB(155, 174, 120, 100);
                eye8.setXframes(6);
                eye8.setYframes(1);
                eye8.setMoveAngleDegrees(0);
                eye8.setMoveSpeed(1f);
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye8.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye8.animationStart();
                eyeList.addSpriteReuse(eye8);
            }

            Sprite3 eye9;
            for (int i = 0; i < 10; i++)
            {
                eye9 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -4100);
                eye9.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                eye9.setWidthHeight(450, 450);
                eye9.setBB(155, 174, 120, 100);
                eye9.setXframes(6);
                eye9.setYframes(1);
                eye9.setMoveAngleDegrees(0);
                eye9.setMoveSpeed(1f);
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye9.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye9.animationStart();
                eyeList.addSpriteReuse(eye9);
            }

            Sprite3 eye10;
            for (int i = 0; i < 10; i++)
            {
                eye10 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -4600);
                eye10.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                eye10.setWidthHeight(450, 450);
                eye10.setBB(155, 174, 120, 100);
                eye10.setXframes(6);
                eye10.setYframes(1);
                eye10.setMoveAngleDegrees(0);
                eye10.setMoveSpeed(1f);
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye10.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye10.animationStart();
                eyeList.addSpriteReuse(eye10);
            }

            Sprite3 eye11;
            for (int i = 0; i < 10; i++)
            {
                eye11 = new Sprite3(true, tex_eye_fly, 1410 + i * 300, -4600);
                eye11.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                eye11.setWidthHeight(450, 450);
                eye11.setBB(155, 174, 120, 100);
                eye11.setXframes(6);
                eye11.setYframes(1);
                eye11.setMoveAngleDegrees(0);
                eye11.setMoveSpeed(1f);
                Vector2[] eye_flyVector = new Vector2[6];
                eye_flyVector[0].X = 0; eye_flyVector[0].Y = 0;
                eye_flyVector[1].X = 1; eye_flyVector[1].Y = 0;
                eye_flyVector[2].X = 2; eye_flyVector[2].Y = 0;
                eye_flyVector[3].X = 3; eye_flyVector[3].Y = 0;
                eye_flyVector[4].X = 4; eye_flyVector[4].Y = 0;
                eye_flyVector[5].X = 5; eye_flyVector[5].Y = 0;
                eye11.setAnimationSequence(eye_flyVector, 0, 5, 6);
                eye11.animationStart();
                eyeList.addSpriteReuse(eye11);
            }


            //---------------------Meteor list-----------------------
            meteor_list = new SpriteList();
            Sprite3 m1;
            for (int i = 0; i < 100; i++)
            {
                m1 = new Sprite3(true, tex_meteor, i * 300, i*-100);
                m1.setDeltaSpeed(new Vector2((float)(-1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                m1.setWidthHeight(64, 64);
                m1.setBB(0, 0, 320, 320);
                m1.setMoveAngleDegrees(0);
                m1.setMoveSpeed(1.4f);
                meteor_list.addSpriteReuse(m1);
            }



            //---------------------Fireball list 1-----------------------
            fireBall_list = new SpriteList();
            Sprite3 f1;
            for (int i = 0; i < 3; i++)
            {
                f1 = new Sprite3(true, tex_fireBall, 0 + i * 150, -500);
                f1.setDeltaSpeed(new Vector2((float)(1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                f1.setWidthHeight(85, 255);
                f1.setBB(65, 145, 85, 255);
                f1.setXframes(1);
                f1.setYframes(6);
                f1.setMoveAngleDegrees(0);
                f1.setMoveSpeed(1.4f);
                //Set the frame of run pose of fireball
                Vector2[] fireBallVector = new Vector2[8];
                fireBallVector[0].X = 0; fireBallVector[0].Y = 0;
                fireBallVector[1].X = 0; fireBallVector[1].Y = 1;
                fireBallVector[2].X = 0; fireBallVector[2].Y = 2;
                fireBallVector[3].X = 0; fireBallVector[3].Y = 3;
                fireBallVector[4].X = 0; fireBallVector[4].Y = 4;
                fireBallVector[5].X = 0; fireBallVector[5].Y = 5;
                f1.setAnimationSequence(fireBallVector, 0, 5, 6);
                f1.animationStart();
                fireBall_list.addSpriteReuse(f1);
            }


            //---------------------Fireball list 2-----------------------
            fireBall_list = new SpriteList();
            Sprite3 f2;
            for (int i = 0; i < 3; i++)
            {
                f2 = new Sprite3(true, tex_fireBall, 100 + i * 150, 1000);
                f2.setDeltaSpeed(new Vector2((float)(1 + rnd.NextDouble()), (float)(1 + rnd.NextDouble())));
                f2.setWidthHeight(85, 255);
                f2.setBB(65, 145, 85, 255);
                f2.setXframes(1);
                f2.setYframes(6);
                f2.setMoveAngleDegrees(0);
                f2.setMoveSpeed(1.4f);
                //Set the frame of run pose of droid
                Vector2[] fireBallVector = new Vector2[8];
                fireBallVector[0].X = 0; fireBallVector[0].Y = 0;
                fireBallVector[1].X = 0; fireBallVector[1].Y = 1;
                fireBallVector[2].X = 0; fireBallVector[2].Y = 2;
                fireBallVector[3].X = 0; fireBallVector[3].Y = 3;
                fireBallVector[4].X = 0; fireBallVector[4].Y = 4;
                fireBallVector[5].X = 0; fireBallVector[5].Y = 5;
                f2.setAnimationSequence(fireBallVector, 0, 5, 6);
                f2.animationStart();
                fireBall_list.addSpriteReuse(f2);
            }

            //Particle system
            tex = Particle2;
            snow();
        }

        public override void Update(GameTime gameTime)
        {
            preKeyState = keyState;
            keyState = Keyboard.GetState();
            //ticks++;

            in_music_background.Play();
            
            //set bouding box to appear in the screen
            if (keyState.IsKeyDown(Keys.B) && preKeyState.IsKeyUp(Keys.B)) { showbb = !showbb; }

            if (keyState.IsKeyDown(Keys.P) && preKeyState.IsKeyUp(Keys.P)) { gameStateManager.setLevel(7); }

            meteor_list.moveDeltaXY();
            meteor_list.animationTick(gameTime);

            eyeList.moveDeltaXY();
            eyeList.animationTick(gameTime);

            fireBall_list.moveDeltaXY();
            fireBall_list.animationTick(gameTime);


            //Collision Detection
            if (runLeft_hero.collision(stone))
            {
                runLeft_hero.setPosX(stone.getPosX());
                standLeft_hero.setPosX(stone.getPosX());
                standRight_hero.setPosX(stone.getPosX());
            }

            // Bullet collision detection 
            // Shoot to eyeList
            for (int k = 0; k < list_Bullet.count(); k++)
            {
                Sprite3 m = list_Bullet.getSprite(k);
                for (int i = 0; i < eyeList.count(); i++)
                {
                    Sprite3 e = eyeList.getSprite(i);
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

            // Shoot to meteroList 
            for (int k = 0; k < list_Bullet.count(); k++)
            {
                Sprite3 m = list_Bullet.getSprite(k);
                for (int i = 0; i < meteor_list.count(); i++)
                {
                    Sprite3 e = meteor_list.getSprite(i);
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

            //Hero get hit . eyeList
            for (int i = 0; i < eyeList.count(); i++)
            {
                Sprite3 eye1 = eyeList.getSprite(i);
                if (standLeft_hero.collision(eye1))
                {
                    eye1.setActive(false);
                    totalHearts = totalHearts - 1;
                }
            }

            //Hero get hit . meteorlist
            for (int i = 0; i < meteor_list.count(); i++)
            {
                Sprite3 m1 = meteor_list.getSprite(i);
                if (standLeft_hero.collision(m1))
                {
                    m1.setActive(false);
                    totalHearts = totalHearts - 1;
                }
            }

            //Hero get hit . fireball list
            for (int i = 0; i < fireBall_list.count(); i++)
            {
                Sprite3 f1 = fireBall_list.getSprite(i);
                if (standLeft_hero.collision(f1))
                {
                    f1.setActive(false);
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

            if (standLeft_hero.getBoundingBoxAA().Intersects(door.getBoundingBoxAA()))
            {
                in_music_background.Stop();
                gameStateManager.setLevel(3);
            }



            layer1.Update(gameTime); //Scrollbackground
            cloud.Update(gameTime); //Scrollbackground
            cloud1.Update(gameTime); //Scrollbackground
            list_Bullet.moveByAngleSpeed();
            list_Bullet.animationTick(gameTime);
            bullet1.moveByAngleSpeed();
            bullet1.moveByDeltaXY();
            bullet1.Update(gameTime);

            //setSys2();
            //psys = 2;

            p.Update(gameTime); //particle system

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.DeepSkyBlue);
            layer0.Draw(spriteBatch);
            cloud.Draw(spriteBatch);
            layer1.Draw(spriteBatch);
            heart1.Draw(spriteBatch);
            heart2.Draw(spriteBatch);
            heart3.Draw(spriteBatch);
            heart4.Draw(spriteBatch);
            platform.Draw(spriteBatch);
            door.Draw(spriteBatch);
            stone.Draw(spriteBatch);
            p.Draw(spriteBatch);//particle system
            if (wl != null) wl.Draw(spriteBatch, Color.Teal, Color.Red);
            spriteBatch.DrawString(font3, "Your have killed: " + totalKilled, new Vector2(750, 20), Color.Brown);
            spriteBatch.DrawString(font3, "Help key: F1", new Vector2(1600, 15), Color.Brown);
            spriteBatch.DrawString(font3, "Pause key: P ", new Vector2(1600, 65), Color.Brown);
           // spriteBatch.DrawString(font3, "Your hearts:" + totalHearts, new Vector2(1200, 85), Color.Brown);

            bullet1.Draw(spriteBatch);
            eyeList.Draw(spriteBatch);
            meteor_list.Draw(spriteBatch);
            fireBall_list.Draw(spriteBatch);
            list_Bullet.drawActive(spriteBatch);
            standLeft_hero.Draw(spriteBatch);
            //  runLeft_droid.Draw(spriteBatch);
            standRight_hero.Draw(spriteBatch);
            runLeft_hero.Draw(spriteBatch);

            if (showbb) //-------------------BOUDING BOX------------------
            {
                door.drawInfo(spriteBatch, Color.Red, Color.Blue);
                stone.drawInfo(spriteBatch, Color.Red, Color.Blue);
                platform.drawInfo(spriteBatch, Color.Red, Color.Blue);
                eyeList.drawInfo(spriteBatch, Color.Red, Color.Blue);
                meteor_list.drawInfo(spriteBatch, Color.Red, Color.Blue);
                fireBall_list.drawInfo(spriteBatch, Color.Red, Color.Blue);
                list_Bullet.drawInfo(spriteBatch, Color.Red, Color.Blue);
                runLeft_hero.drawInfo(spriteBatch, Color.Red, Color.Blue);

                //hero
                //standLeft_hero.drawInfo(spriteBatch, Color.Red, Color.Blue);
                //standRight_hero.drawInfo(spriteBatch, Color.Red, Color.Blue);
                //runLeft_droid.drawInfo(spriteBatch, Color.Red, Color.Blue);
                //trap1.drawInfo(spriteBatch, Color.Red, Color.Blue);
            }
        }

        void launchBullet_Left()
        {
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
            bullet1 = new Sprite3(true, tex_bullet1, standRight_hero.getPosX(), standRight_hero.getPosY());
            bullet1.setWidthHeight(15, 15);
            bullet1.setBB(0, 0, 15, 15);
            bullet1.setDisplayAngleDegrees(270); //rotate
            bullet1.setMoveAngleDegrees(270);
            bullet1.setMoveSpeed(5f);
            list_Bullet.addSpriteReuse(bullet1);
        }

        //Particle system
        void snow()
        {
            p = new ParticleSystem(new Vector2(300, 100), 40, 10000, 107);
            p.setMandatory1(tex, new Vector2(6, 6), new Vector2(24, 24), Color.White, Color.FloralWhite);
            p.setMandatory2(-1, 1, 1, 3, 0);
            rectangle = new Rectangle(0, 0, 1980, 1080);
            p.setMandatory3(120, rectangle);
            p.setMandatory4(new Vector2(0, 0.1f), new Vector2(0, 0), new Vector2(1, 0));
            p.randomDelta = new Vector2(0.1f, 0.1f);
            p.Origin = 1;
            p.originRectangle = new Rectangle(0, 0, 1980, 10);
            p.moveTowards = 3;
            p.moveTowardsPos = new Vector2(mouse_x, mouse_y);
            p.moveToDrift = 0.1f;
            p.activate();
            wl = null;
        }

    }
}
