using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using CocosDenshion;
using CSExtensionKit.Entities;

namespace CSExtensionKit
{

	public class CSKEnemy : CSKEntity
	{



		public CSKEnemyClass klass;
		public DateTime nextDecisionTime { get { return klass.nextDecisionTime; } }


		public bool HasPassedDecisionTime()
		{
			return DateTime.Now > nextDecisionTime;
		}

		public void SetNextDecisionTime(DateTime nextDecisionTime)
		{
			klass.nextDecisionTime = nextDecisionTime;
		}

		public void SetNextDecisionTime(int fromSec, int toSec)
		{
			SetNextDecisionTime(DateTime.Now.AddSeconds(CCRandom.GetRandomInt(1, 5)));
		}

		public void SetNextDecisionTime()
		{
			SetNextDecisionTime(DateTime.Now);
		}


		public CSKEnemy(int id, string image)
			: base(image)
		{
			klass = new CSKEnemyClass();
			klass.ID = id;
		}

		public void Step(CCSize MapTotalSize, float MapMaxTop, float dt)
		{

			if (!klass.IsIA)
				return;

			Position = new CCPoint(
				Math.Min((MapTotalSize.Width) - centerToSides, Math.Max(centerToSides, desiredPosition.X)),
								   Math.Min((MapMaxTop) + centerToBottom, Math.Max(centerToBottom, desiredPosition.Y))
								   );
		}

		public float DistanceSQ(CSKEntity entity)
		{
			CCPoint pos = entity.Position;
			return Position.DistanceSquared(ref pos);
		}


		public bool DistanceSQ(CSKEntity entity, float distance)
		{
			return DistanceSQ(entity) <= distance;
		}


		public bool IsIA { get { return klass.IsIA; } set { klass.IsIA = value; } }
	}


}
