using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{
	public enum EnemyDesiredActionState
	{
		SearchingTarget = 1, Firing = 2, OutOfRange = 3
	}

	public enum EnemyPersonalityState
	{
		Passive = 1, Attacking = 2, Suicide = 3
	}

	public class CSKIaSystemBase
	{

		public static int DELAY_SHOOT = 100;
		public int ACTUAL_DELAY = 0;

		public CSKIaSystemBase()
		{
			Console.WriteLine("IA System active");
		}

		public virtual void Step(CSKPlayer player, CSKEnemy enemy, CCSize wSize, float dt)
		{
			throw new NotImplementedException("Custom IA its not instanciated on the Game Layer");
		}

		public virtual void Step(CSKPlayer player, List<CSKEnemy> enemies, CCSize wSize, float dt)
		{
			throw new NotImplementedException("Custom IA its not instanciated on the Game Layer");
		}

		public virtual string GetEnemyIAInfo(CSKEnemy enemy)
		{
			throw new NotImplementedException();
		}

	}
}
