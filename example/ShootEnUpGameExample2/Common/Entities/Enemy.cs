using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalSlugGameExample
{
    class Enemy : EnemyBase
    {

        CCSprite s1;
        CCAnimation stillAnimation;
        CCRepeatForever stillPositionAction;

        public Enemy()
        {
            //Cargamos animaciones
            LoadAnimations();

            //Añadimos el sprite
            s1 = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 138 at frame 10.png"]);
            s1.Position = new CCPoint(0, 0);
            AddChild(s1);
            Active();
        }

        void LoadAnimations()
        {
            stillPositionAction = new CCRepeatForever(new CCAnimate(new CCAnimation(new List<CCSpriteFrame>() { 
               CCApplication.SharedApplication.SpriteFrameCache["Image 138 at frame 10.png"], 
                 CCApplication.SharedApplication.SpriteFrameCache["Image 139 at frame 10.png"]
            }, 0.2f))); //restoreOriginalFrame:NO

        }

        void Active()
        {
            //Activamos
            if (isActive)
                return;

            isActive = true;

            runActiveAnimation();
            Schedule(checkUpdate, 1 / 20);
        }

        void runActiveAnimation()
        {
            s1.RunAction(stillPositionAction);
        }

        bool beenAttack(int type)
        {
            if (!isActive)
                return false;
            if (type < 4)
                runDeadAnimation();
            return false;
        }

        public void runDeadAnimation()
        {
            s1.RunAction(new CCAnimate(new CCAnimation(new List<CCSpriteFrame>() {
              CCApplication.SharedApplication.SpriteFrameCache["Image 231 at frame 10.png"], 
              CCApplication.SharedApplication.SpriteFrameCache["Image 232 at frame 10.png"], 
              CCApplication.SharedApplication.SpriteFrameCache["Image 233 at frame 10.png"], 
              CCApplication.SharedApplication.SpriteFrameCache["Image 234 at frame 10.png"], 
              CCApplication.SharedApplication.SpriteFrameCache["Image 235 at frame 10.png"]
            }, 0.2f)));
        }

        void checkUpdate(float f)
        {
            if (!isFiring)
            {

            }
        }

        public void fire()
        {
            isFiring = true;
        }

        ~Enemy()
        {

        }
    }
}
