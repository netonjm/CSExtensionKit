/*
 
 * IA SYSTEM
 
 */
using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{



	public class CSKBeuIA : CSKIaSystemBase
	{

		public CSKBeuGameLayer layer { get; set; }

		public const string EVENT_IA_PLAYER_HURTS = "IA_PLAYER_HURTS";

		public CSKBeuIA(CSKBeuGameLayer layer)
		{
			this.layer = layer;
		}

		public int GetRandomChoice(int from, int to)
		{
			return CCRandom.GetRandomInt(from, to);
		}

		public void RandomChoice(Action<bool> action)
		{
			if (action != null)
				action(CCRandom.GetRandomInt(0, 1) == 0 ? false : true);
		}

		public void RandomChoice(int from, int to, Action<int> action)
		{
			if (action != null)
				action(CCRandom.GetRandomInt(from, to));
		}

		public override void Step(CSKPlayer player, CSKEnemy enemy, CCSize wSize, float dt)
		{

			if (!enemy.klass.IsIA)
				return;

			//int alive = 0;
			float distanceSQ;
			int randomChoice = 0;
			enemy.Update(dt);

			if (enemy.actionState != ActionState.KnockedOut)
			{

				if (enemy.HasPassedDecisionTime())
				{

					CCPoint pos = player.Position;
					//distanceSQ = enemy.Position.DistanceSQ(ref pos);

					distanceSQ = enemy.DistanceSQ(player);

					//3
					if (enemy.DistanceSQ(player, 50 * 50))
					{
						enemy.SetNextDecisionTime(1, 5);

						RandomChoice(rnd =>
						{


							if (rnd)
								enemy.idle();
							else
							{

								if (player.IsOnRight(enemy))
									enemy.SetFlip(true);
								else
									enemy.SetFlip(false);

								//4
								enemy.attack();

								if (enemy.actionState == ActionState.Attack)
								{
									if (Math.Abs(player.PositionY - enemy.PositionY) < 10)
									{
										if (player.AbsoluteHitBoxRect.IntersectsRect(enemy.AbsoluteAttackBoxRect))
										{
											CCEventCustom eventHurts = new CCEventCustom(EVENT_IA_PLAYER_HURTS, enemy);
											if (layer != null)
												layer.DispatchEvent(eventHurts);

										}
									}
								}

							}


						});

						randomChoice = CCRandom.GetRandomInt(0, 1);



					}
					else if (distanceSQ <= wSize.Width * wSize.Width)
					{
						//5
						enemy.SetNextDecisionTime(1, 5);
						randomChoice = CCRandom.GetRandomInt(0, 2);
						if (randomChoice == 0)
						{
							CCPoint moveDirection = CCPoint.Normalize(player.Position - enemy.Position);
							enemy.moveWithDirection(moveDirection);
						}
						else
						{
							enemy.idle();
						}
					}
				}
			}
		}

		public override void Step(CSKPlayer player, List<CSKEnemy> enemies, CCSize wSize, float dt)
		{
			// int alive = 0;
			foreach (CSKEnemy enemy in enemies)
				Step(player, enemy, wSize, dt);
		}


		public override string GetEnemyIAInfo(CSKEnemy enemy)
		{
			// return string.Format("{0}{1}", (enemy.IsIA) ? "IA ENABLED" : "IA DISABLED", enemy.IA_INFO);
			return "";
		}

	}
}
