using CocosSharp;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MetalSlugGameExample
{
    class Hero : CCNode
    {

        bool isJump = false;
        bool isRun = false;
        bool isShoot = false;


        enum PersonajeEstado
        {
            Quieto = 0, Derecha = 1, Izquierda = 2,
        }

        PersonajeEstado estado = PersonajeEstado.Quieto;

        CCSprite head;
        CCSprite leg;

        CCAnimation sqIdelAnimation1;
        CCRepeatForever sqIdelAction1;

        CCAnimation sqIdelAnimation2;
        CCRepeatForever sqIdelActionRun;

        CCRepeatForever sqIdelActionStopped;

        CCAnimation sqIdelAnimation3;
        CCRepeatForever sqIdelAction3;

        CCAnimation sqIdelAnimation4;
        CCRepeatForever sqIdelActionSit;

        CCAnimation sqIdelAnimationFireSmall;
        CCRepeatForever sqIdelActionFireSmall;

        //IntroLayer Scene;

        public Hero()
        {

            //Scene = scene;

            head = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 577 at frame 10.png"]);
            head.Scale = 0.75f;
            head.SetPosition(0, 7);

            leg = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 523 at frame 10.png"]);
            leg.Scale = 0.75f;
            leg.SetPosition(12, -23);

            AddChild(leg);
            AddChild(head);

            // CABEZA ESTATICO ============================================================================================

            sqIdelAnimation1 = new CCAnimation(new List<CCSpriteFrame>() {
                CCApplication.SharedApplication.SpriteFrameCache["Image 544 at frame 10.png"], 
                CCApplication.SharedApplication.SpriteFrameCache["Image 545 at frame 10.png"], 
                CCApplication.SharedApplication.SpriteFrameCache["Image 546 at frame 10.png"] 
            }, 0.2f);
            sqIdelAction1 = new CCRepeatForever(new CCAnimate(sqIdelAnimation1));

            //head.RunAction(sqIdelAction1);

            //DISPARANDO ============================================================================================

            sqIdelAnimationFireSmall = new CCAnimation(new List<CCSpriteFrame>() { 
                CCApplication.SharedApplication.SpriteFrameCache["Image 635 at frame 10.png"],
                CCApplication.SharedApplication.SpriteFrameCache["Image 637 at frame 10.png"],
                CCApplication.SharedApplication.SpriteFrameCache["Image 638 at frame 10.png"],
                CCApplication.SharedApplication.SpriteFrameCache["Image 544 at frame 10.png"]
            }, 0.15f);
            sqIdelActionFireSmall = new CCRepeatForever(new CCAnimate(sqIdelAnimationFireSmall));

            //PIERNAS ESTATICO

            // PIERNAS ANDANDO ============================================================================================
            sqIdelActionStopped = new CCRepeatForever(new CCAnimate(new CCAnimation(new List<CCSpriteFrame>() { 
                CCApplication.SharedApplication.SpriteFrameCache["Image 523 at frame 10.png"] },
                0.15f)));

            sqIdelAnimation2 = new CCAnimation(new List<CCSpriteFrame>() { 
                CCApplication.SharedApplication.SpriteFrameCache["Image 524 at frame 10.png"], 
                CCApplication.SharedApplication.SpriteFrameCache["Image 525 at frame 10.png"], 
                CCApplication.SharedApplication.SpriteFrameCache["Image 526 at frame 10.png"], 
                CCApplication.SharedApplication.SpriteFrameCache["Image 527 at frame 10.png"]
            }, 0.15f);
            sqIdelActionRun = new CCRepeatForever(new CCAnimate(sqIdelAnimation2));

            //leg.RunAction(sqIdelActionRun);
            //SALTO ============================================================================================

            sqIdelAnimation3 = new CCAnimation(new List<CCSpriteFrame>() { 
                  CCApplication.SharedApplication.SpriteFrameCache["Image 539 at frame 10.png"],
                  CCApplication.SharedApplication.SpriteFrameCache["Image 540 at frame 10.png"],
                  CCApplication.SharedApplication.SpriteFrameCache["Image 541 at frame 10.png"],
                  CCApplication.SharedApplication.SpriteFrameCache["Image 542 at frame 10.png"],
                  CCApplication.SharedApplication.SpriteFrameCache["Image 543 at frame 10.png"],
                  CCApplication.SharedApplication.SpriteFrameCache["Image 544 at frame 10.png"]
              }, 0.1f);
            sqIdelAction3 = new CCRepeatForever(new CCAnimate(sqIdelAnimation3));

            //leg.RunAction(sqI

            //AGACHADO ==========================================================================
            sqIdelAnimation4 = new CCAnimation(new List<CCSpriteFrame>() { 
               CCApplication.SharedApplication.SpriteFrameCache["Image 533 at frame 10.png"],
               CCApplication.SharedApplication.SpriteFrameCache["Image 534 at frame 10.png"],
               CCApplication.SharedApplication.SpriteFrameCache["Image 535 at frame 10.png"],
               CCApplication.SharedApplication.SpriteFrameCache["Image 536 at frame 10.png"],
               CCApplication.SharedApplication.SpriteFrameCache["Image 537 at frame 10.png"],
               CCApplication.SharedApplication.SpriteFrameCache["Image 538 at frame 10.png"],
               CCApplication.SharedApplication.SpriteFrameCache["Image 544 at frame 10.png"]
           }, 0.2f);

            sqIdelActionSit = new CCRepeatForever(new CCAnimate(sqIdelAnimation4));

            head.RunAction(sqIdelAction1);

            leg.RunAction(sqIdelActionStopped);

            Schedule();
        }

        public byte Opacity
        {
            get
            {
                return head.Opacity;
            }
            set
            {
                head.Opacity = leg.Opacity = value;
            }
        }

        public bool FlipX
        {
            get
            {
                return head.FlipX;
            }
            set
            {
                head.FlipX = leg.FlipX = value;
            }
        }

        //public override void KeyReleased(Keys key)
        //{
        //    if (key == Keys.LeftShift)
        //    {
        //        isRun = false;
        //    } else if (key == Keys.LeftControl)
        //    {
        //        isShoot = false;
        //        head.StopAllActions();
        //        head.RunAction(sqIdelAction1);
        //    }
        //    else
        //    {
        //        stop();
        //    }

        //    base.KeyReleased(key);
        //}

        public void run()
        {
            leg.StopAllActions();
            leg.RunAction(sqIdelActionRun);
        }

        void jumpDownFromTruck()
        {
            StopAllActions();
            this.ScaleX = 1;
        }

        public void agacharse()
        {
            estado = PersonajeEstado.Quieto;
            leg.StopAllActions();
            leg.RunAction(sqIdelActionSit);
        }

        public void disparo()
        {
            isShoot = true;
            head.StopAllActions();
            head.RunAction(sqIdelActionFireSmall);
        }

        public void stop()
        {
            if (!isJump && isRun)
            {
                isRun = false;
                estado = PersonajeEstado.Quieto;
                leg.StopAllActions();
                leg.RunAction(sqIdelActionStopped);

            }
        }

        public float desplazamiento
        {
            get
            {
                return (isRun) ? 7 : 3;
            }
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            //Actualizamos la posición del muñeco
            //if (Scene.ControlPanel!=null) {
            //    Position = DSJoyStickHelper.GetVelocity(Scene.ControlPanel.leftJoystick.velocity, Position, ContentSize , CCDirector.SharedDirector.WinSize, dt);
            //    FlipX = Scene.ControlPanel.leftJoystick.IsRight;
            //}

            //_input.Update();
            //switch (estado)
            //{
            //    case PersonajeEstado.Quieto:

            //        break;
            //    case PersonajeEstado.Derecha:
            //        PositionX += desplazamiento;
            //        break;
            //    case PersonajeEstado.Izquierda:
            //        PositionX -= desplazamiento;
            //        break;
            //    default:
            //        break;
            //}

            /*
            if (_input.IsCurPress(Keys.Up))
                stop();
            if (_input.IsCurPress(Keys.Down))
                stop();
            if (_input.IsCurPress(Keys.Left))
                run();
            if (_input.IsCurPress(Keys.Right))
                run();
           */



        }

        public bool isJumping { get; set; }


    }

}


//#region Read from files

//public string GetHeroDirectory(string source)
//{
//    return Path.Combine(StaticVars.HERO_DIRECTORY, source);
//}

//void LoadFromFile()
//{
//    head = new CCSprite("Image 577 at frame 10.png");
//    head.Scale = 0.75f;
//    head.SetPosition(0, 7);
//    leg = new CCSprite("Image 523 at frame 10.png");
//    leg.Scale = 0.75f;
//    leg.SetPosition(12, -23);

//    AddChild(leg);
//    AddChild(head);

//    // CABEZA ESTATICO ============================================================================================
//    frameShouqiang1 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 544 at frame 10.png")), new CCRect(0, 0, 130.5f, 130));
//    frameShouqiang2 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 545 at frame 10.png")), new CCRect(0, 0, 131, 129));
//    frameShouqiang3 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 546 at frame 10.png")), new CCRect(0, 0, 131, 128));

//    sqIdelAnimation1 = new CCAnimation(new List<CCSpriteFrame>() { frameShouqiang1, frameShouqiang2, frameShouqiang3 }, 0.2f);
//    sqIdelAction1 = new CCRepeatForever(new CCAnimate(sqIdelAnimation1));

//    //head.RunAction(sqIdelAction1);

//    //DISPARANDO ============================================================================================
//    f1 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 635 at frame 10.png")), new CCRect(0, 0, 43, 26));
//    f2 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 637 at frame 10.png")), new CCRect(0, 0, 43, 25));
//    f3 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 638 at frame 10.png")), new CCRect(0, 0, 40.5f, 27));

//    sqIdelAnimationFireSmall = new CCAnimation(new List<CCSpriteFrame>() { f1, f2, f3, frameShouqiang1 }, 0.15f);
//    sqIdelActionFireSmall = new CCRepeatForever(new CCAnimate(sqIdelAnimationFireSmall));

//    //PIERNAS ESTATICO

//    // PIERNAS ANDANDO ============================================================================================
//    l1 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 523 at frame 10.png")), new CCRect(0, 0, 116.5f, 116));

//    sqIdelActionStopped = new CCRepeatForever(new CCAnimate(new CCAnimation(new List<CCSpriteFrame>() { l1 }, 0.15f)));

//    l2 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 524 at frame 10.png")), new CCRect(0, 0, 12.5f, 16.5f));
//    l3 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 525 at frame 10.png")), new CCRect(0, 0, 26.5f, 20f));
//    l4 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 526 at frame 10.png")), new CCRect(0, 0, 30.5f, 16.5f));
//    l5 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 527 at frame 10.png")), new CCRect(0, 0, 21, 18));
//    l6 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 528 at frame 10.png")), new CCRect(0, 0, 15.5f, 17));
//    l7 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 529 at frame 10.png")), new CCRect(0, 0, 12.5f, 16.5f));
//    l8 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 530 at frame 10.png")), new CCRect(0, 0, 25.5f, 18));
//    l9 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 531 at frame 10.png")), new CCRect(0, 0, 32.5f, 16.5f));
//    l10 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 532 at frame 10.png")), new CCRect(0, 0, 1.5f, 18.5f));


//    sqIdelAnimation2 = new CCAnimation(new List<CCSpriteFrame>() { l2, l3, l4, l5 }, 0.15f);
//    sqIdelActionRun = new CCRepeatForever(new CCAnimate(sqIdelAnimation2));

//    //leg.RunAction(sqIdelActionRun);

//    //SALTO ============================================================================================

//    j1 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 539 at frame 10.png")), new CCRect(10, 0, 19, 22.5f));
//    j2 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 540 at frame 10.png")), new CCRect(10, 0, 19.3f, 22));
//    j3 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 541 at frame 10.png")), new CCRect(10, 0, 20.5f, 22));
//    j4 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 542 at frame 10.png")), new CCRect(10, 0, 22, 19.5f));
//    j5 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage(GetHeroDirectory("Image 543 at frame 10.png")), new CCRect(10, 0, 21.5f, 20));


//    sqIdelAnimation3 = new CCAnimation(new List<CCSpriteFrame>() { j1, j2, j3, j4, j5, l1 }, 0.1f);
//    sqIdelAction3 = new CCRepeatForever(new CCAnimate(sqIdelAnimation3));

//    //leg.RunAction(sqIdelAction3);

//    //AGACHADO ============================================================================================

//    sit1 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage("Image 533 at frame 10.png"), new CCRect(0, 0, 17, 16.5f));
//    sit2 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage("Image 534 at frame 10.png"), new CCRect(0, 0, 17.5f, 16.5f));
//    sit3 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage("Image 535 at frame 10.png"), new CCRect(0, 0, 17, 16.5f));
//    sit4 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage("Image 536 at frame 10.png"), new CCRect(0, 0, 17.5f, 17));
//    sit5 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage("Image 537 at frame 10.png"), new CCRect(0, 0, 17, 16.5f));
//    sit6 = new CCSpriteFrame(CCTextureCache.SharedTextureCache.AddImage("Image 538 at frame 10.png"), new CCRect(0, 0, 19, 16.5f));

//    sqIdelAnimation4 = new CCAnimation(new List<CCSpriteFrame>() { sit1, sit2, sit3, sit4, sit5, sit6, j1 }, 0.2f);
//    sqIdelActionSit = new CCRepeatForever(new CCAnimate(sqIdelAnimation4));


//    head.RunAction(sqIdelAction1);

//    leg.RunAction(sqIdelActionStopped);

//}


//#endregion

