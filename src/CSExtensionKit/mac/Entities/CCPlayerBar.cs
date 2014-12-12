using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{
    class PlayerBar : CCNode
    {

        CSKPlayerClass PlayerSettings;

        CCSprite Portrait;
        CCProgressTimer LifeBar;

        public PlayerBar(CSKEntity player)
        {
            Portrait = player.Portrait;
            Portrait.AnchorPoint = new CCPoint(0, 0);
            AddChild(Portrait);

            LifeBar = new CCProgressTimer("bar/green_health_bar.png");
            LifeBar.Color = CCColor3B.Blue;
            LifeBar.PositionX = Portrait.BoundingBox.Size.Width + 10;
            LifeBar.Type = CCProgressTimerType.Bar;

            LifeBar.AnchorPoint = new CCPoint(0, 0);
            AddChild(LifeBar);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            LifeBar.Percentage = PlayerSettings.HEALTH;
        }

    }
}
