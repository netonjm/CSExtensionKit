using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using CocosDenshion;
using CSExtensionKit;

namespace BeatEmUpGameExample.Entities
{
	class Hero : CSKPlayer
	{
		public Hero()
			: base("hero_idle_00.png")
		{

			

			//idle animation
			idleAction = CCSpriteAnimationHelper.GetActionFromData("hero_idle_{0}.png", 5, 1.0f / 12.0f);

			//attack animation 
			attackAction = CCSpriteAnimationHelper.GetActionFunctionFromData("hero_attack_00_{0}.png", 2, 1.0f / 24.0f, idle);

			//walk animation
			movementAction = CCSpriteAnimationHelper.GetActionFromData("hero_walk_{0}.png", 7, 1.0f / 12.0f);

			//hurt animation
			hurtAction = CCSpriteAnimationHelper.GetActionFunctionFromData("hero_hurt_{0}.png", 7, 1.0f / 12.0f, idle);

			//knocked out animation
			knockedOutAction = CCSpriteAnimationHelper.GetActionBlinkFromData("hero_knockout_{0}.png", 4, 1.0f / 12.0f, 2.0f, 10);

			centerToBottom = 39.0f;
			centerToSides = 29.0f;
			hitPoints = 100.0f;
			damage = 20.0f;
			movementSpeed = 80f;

			hitBox = createBoundingBoxWithOrigin(new CCPoint(-ContentSize.Width * .5f, -ContentSize.Height * .5f), ContentSize);

			attackBox = createBoundingBoxWithOrigin(new CCPoint(centerToSides, -10), new CCSize(20, 20));
		}

		public override void knockout()
		{
			base.knockout();
			CCSimpleAudioEngine.SharedEngine.PlayEffect("sounds/pd_herodeath");
		}

	}
}
