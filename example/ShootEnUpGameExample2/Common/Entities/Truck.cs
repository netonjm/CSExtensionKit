using Cocos2D;
using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalSlugGameExample
{
    public class Truck : CCSprite
    {

        CCSprite caster1, caster2;
        CCSprite curtain1, curtain2, curtain3;
        CCSprite door, body, head, light;

        public Truck()
        {

            //[self setTextureRect:CGRectMake(0, 0, 308, 185)];
            caster1 = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 906 at frame 10.png"]);
            caster1.Position = new CCPoint(50, -38);
            caster2 = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 906 at frame 10.png"]);
            caster2.Position = new CCPoint(-63, -38);

            CCSpriteFrame fc1 = CCApplication.SharedApplication.SpriteFrameCache["Image 904 at frame 10.png"];
            CCSpriteFrame fc2 = CCApplication.SharedApplication.SpriteFrameCache["Image 904 at frame 10.png"];
            CCSpriteFrame fc3 = CCApplication.SharedApplication.SpriteFrameCache["Image 904 at frame 10.png"];

            var curtainanimation = new CCAnimation(new List<CCSpriteFrame>() { fc1, fc2, fc3 }, 0.2f);

            var curtainaction1 = new CCRepeatForever(new CCAnimate(curtainanimation));
            var curtainaction2 = new CCRepeatForever(new CCAnimate(curtainanimation));
            var curtainaction3 = new CCRepeatForever(new CCAnimate(curtainanimation));

            curtain1 = new CCSprite(fc1);
            curtain2 = new CCSprite(fc1);
            curtain3 = new CCSprite(fc1);
            curtain1.Position = new CCPoint(-10, -5);
            curtain2.Position = new CCPoint(-40, -5);
            curtain3.Position = new CCPoint(-70, -5);

            head = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 907 at frame 10.png"]);
            body = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 915 at frame 10.png"]);
            body.Position = new CCPoint(-8, 5);
            door = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 908 at frame 10.png"]);
            door.Position = new CCPoint(50, -5);
            light = new CCSprite(CCApplication.SharedApplication.SpriteFrameCache["Image 909 at frame 10.png"]);
            light.Position = new CCPoint(165, -30);

            AddChild(caster1);
            AddChild(caster2);
            AddChild(head);
            AddChild(curtain1);
            RunAction(curtainaction1);
            AddChild(curtain2);
            RunAction(curtainaction2);
            AddChild(curtain3);
            RunAction(curtainaction3);

            AddChild(body);
            AddChild(door);
            AddChild(light);

        }

        public void stopAllActions()
        {
            StopAllActions();

            curtain1.StopAllActions();
            curtain2.StopAllActions();
            curtain3.StopAllActions();
        }

        ~Truck()
        {
            caster1.RemoveFromParent(true);
            caster2.RemoveFromParent(true);
            curtain1.RemoveFromParent(true);
            curtain2.RemoveFromParent(true);
            curtain3.RemoveFromParent(true);
            door.RemoveFromParent(true);
            body.RemoveFromParent(true);
            head.RemoveFromParent(true);
            light.RemoveFromParent(true);
        }

    }
}
