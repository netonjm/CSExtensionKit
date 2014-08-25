using Cocos2D;
using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalSlugGameExample
{
    class Tank : EnemyBase
    {
        CCSprite s1, s2, s3, s4, s5, s6;
        CCSequence seqFire;
        CCSpriteFrame bf1, bf2;

        public Tank()
        {
            s1 = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 320 at frame 10.png"]);
            s2 = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 321 at frame 10.png"]);
            s4 = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 323 at frame 10.png"]);
            s1.Position = new CCPoint(0, -10);
            s2.Position = new CCPoint(0, 0);
            s4.Position = new CCPoint(-15, 18);
            AddChild(s1);
            AddChild(s2);
            AddChild(s4);
        }

        void Active()
        {

            if (isActive)
                return;

            isActive = true;

            if (type == 0)
            {
                bf1 = CCApplication.SharedApplication.SpriteFrameCache["Image 904 at frame 10.png"];//, new CCRect(0, 0, 46, 21));
                bf2 = CCApplication.SharedApplication.SpriteFrameCache["Image 905 at frame 10.png"];//, new CCRect(0, 0, 46, 22));

                Schedule(TankFire, 2);
            }

        }

        bool beenAttack(int type)
        {
            return true;
        }

        public void TankFire(float t)
        {
            Console.WriteLine("Shoot");
            CCSprite bullet = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 904 at frame 10.png"]);
            CCLayer layer = (CCLayer)GetChildByTag(2); //[self.parent.parent getChildByTag:2];

            CCPoint hp = Position;
            CCPoint sp = Parent.Position;
            CCPoint position = new CCPoint(hp.X + sp.X - 80, hp.Y + sp.Y + 25);

            bullet.Position = position;
            layer.AddChild(bullet, 2);
            bullet.Tag = 851137;
            bullet.RunAction(new CCMoveBy(1.5f, new CCPoint(-400, 0)));
        }
    }
}
