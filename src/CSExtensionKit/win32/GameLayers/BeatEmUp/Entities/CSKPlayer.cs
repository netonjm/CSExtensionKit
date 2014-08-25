using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using CocosDenshion;
using CocosSharp.Extensions.SneakyJoystick;

namespace CSExtensionKit
{

	public enum ActionState
	{
		Idle = 0,
		Walk = 1,
		Attack = 2,
		KnockedOut = 3,
		Hurt = 4
	}

	public class CSKPlayer : CSKEntity
	{

		public CSKPlayerClass klass;

		public CSKPlayer(string image)
			: base(image)
		{
			klass = new CSKPlayerClass();

		}

		public void Step(SneakyPanelControl controlPanelLayer, CCSize MapTotalSize, float MapMaxTop, float dt)
		{
			if (actionState == ActionState.Walk)
				desiredPosition = Position + (velocity * dt);

			//Update player position
			CCPoint tmpPosition = SneakyPanelControl.GetPlayerPosition(this, dt, new CCSize(MapTotalSize.Width, MapTotalSize.Height));

			float posX = Math.Min((MapTotalSize.Width) - centerToSides, Math.Max(centerToSides, tmpPosition.X));
			float posY = Math.Min(MapMaxTop + centerToBottom, Math.Max(centerToBottom, tmpPosition.Y));

			Position = new CCPoint(posX, posY);

		}


		

	}


}
