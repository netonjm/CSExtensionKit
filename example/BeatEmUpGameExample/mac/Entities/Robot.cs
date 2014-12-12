using CocosDenshion;
using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSExtensionKit;
namespace BeatEmUpGameExample.Entities
{
    class Robot : CSKEnemy
    {

        public Robot()
            : base(2, "robot_idle_00.png")
        {

            //idle animation
            idleAction = CCSpriteAnimationHelper.GetActionFromData("robot_idle_{0}.png", 4, 1.0f / 12.0f);

            //attack animation 
            attackAction = CCSpriteAnimationHelper.GetActionFunctionFromData("robot_attack_{0}.png", 2, 1.0f / 24.0f, idle);

            //walk animation
            movementAction = CCSpriteAnimationHelper.GetActionFromData("robot_walk_{0}.png", 7, 1.0f / 12.0f);

            //hurt animation
            hurtAction = CCSpriteAnimationHelper.GetActionFunctionFromData("robot_hurt_{0}.png", 7, 1.0f / 12.0f, idle);

            //knocked out animation
            knockedOutAction = CCSpriteAnimationHelper.GetActionBlinkFromData("robot_knockout_{0}.png", 4, 1.0f / 12.0f, 2.0f, 10);

            //measurements
            movementSpeed = 80;
            centerToBottom = 39.0f;
            centerToSides = 29.0f;
            hitPoints = 100;
            damage = 10;

            //hitBox = createBoundingBoxWithOrigin(
            // new CCPoint(centerToSides, -centerToBottom), new CCSize(centerToSides * 2, centerToBottom * 2));
            //attackBox = createBoundingBoxWithOrigin(new CCPoint(centerToSides, -10), new CCSize(20, 20));

			hitBox = createBoundingBoxWithOrigin(new CCPoint(-ContentSize.Width * .5f, -ContentSize.Height * .5f), ContentSize);
            attackBox = createBoundingBoxWithOrigin(new CCPoint(centerToSides, -10), new CCSize(20, 20));

			SetNextDecisionTime();

        }

		

        public override void knockout()
        {
            base.knockout();
            CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/pd_botdeath");
        }



    }
}
