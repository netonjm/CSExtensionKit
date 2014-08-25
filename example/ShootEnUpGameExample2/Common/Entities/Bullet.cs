using Cocos2D;
using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalSlugGameExample
{
    class Bullet : CCSprite
    {

        CCSprite spriteWithSpriteFrame(CCSpriteFrame frame, bool forward)
        {
            CCSprite sel = new CCSprite(frame);
            if (sel != null)
            {
                RunAction(new CCMoveBy(0.8f, new CCPoint((forward) ? 600 : -600, 0)));
            }
            return sel;
        }
    }
}
