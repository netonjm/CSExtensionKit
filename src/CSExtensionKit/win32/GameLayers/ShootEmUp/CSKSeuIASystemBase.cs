using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosDenshion;
using CocosSharp;


#if WINDOWS_PHONE
using System.Windows;
#endif

namespace CSExtensionKit
{



	public class CSKSeuIA : CSKIaSystemBase
	{
		public CSKSeuGameLayer layer { get; set; }

		public const string EVENT_IA_PLAYER_HURTS = "IA_PLAYER_HURTS";

		public CSKSeuIA(CSKSeuGameLayer layer)
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

//else if (tipo == ShootType.MeteoroFuego)
//{
//    var meteorof = new ShootFire();
//    meteorof.Position = Player.Position;
//    meteorof.Deleted += (s) =>
//    {
//        Shoots.Remove(s);
//        RemoveChild(s);
//    };
//    Shoots.Add(meteorof);
//    AddChild(meteorof);
//    //meteorof.RunAction(new CCMoveTo(1.0f, new CCPoint(CCDirector.SharedDirector.WinSize.Width + 10, ContentSize.Height / 2)));
//}
//else if (tipo == ShootType.MeteoroFlower)
//{
//    var meteorof = new ShootFlower();
//    meteorof.Deleted += (s) =>
//    {
//        Shoots.Remove(s);
//        RemoveChild(s);
//    };
//    meteorof.Position = Player.Position;
//    Shoots.Add(meteorof);
//    AddChild(meteorof);
//    //meteorof.RunAction(new CCMoveTo(1.0f, new CCPoint(CCDirector.SharedDirector.WinSize.Width + 10, ContentSize.Height / 2)));
//}
//else if (tipo == ShootType.MeteoroFireworks)
//{
//    var meteorof = new ShootFireworks();
//    meteorof.Deleted += (s) =>
//    {
//        Shoots.Remove(s);
//        RemoveChild(s);
//    };
//    meteorof.Position = Player.Position;
//    Shoots.Add(meteorof);
//    AddChild(meteorof);
//    //meteorof.RunAction(new CCMoveTo(1.0f, new CCPoint(CCDirector.SharedDirector.WinSize.Width + 10, ContentSize.Height / 2)));
//}


//public void CheckEnemyMeteorsCollition()
//{
//    if (CollitionDetected != null && !Player.HasCollision)
//    {
//        bool colision = false;

//        var deletedShoot = new List<ShootMeteor>();
//        var deletedEnemyMeteor = new List<EnemyMeteor>();

//        bool enemydeleted = false;

//        foreach (var tmpEnemy in EnemyMeteors)
//        {
//            //Comprobamos que no matemos al enemigo
//            foreach (var tmpShoot in Shoots)
//            {
//                if (tmpEnemy.BoundingBox.IntersectsRect(tmpShoot.BoundingBox))
//                {
//                    deletedShoot.Add(tmpShoot);
//                    deletedEnemyMeteor.Add(tmpEnemy);
//                    enemydeleted = true;
//                }
//            }

//            if (!enemydeleted)
//            {
//                //Comprobamos que no esté en la pantalla
//                if (tmpEnemy.PositionX < -(3 + tmpEnemy.ContentSize.Width))
//                {
//                    DeleteEnemyMeteor(tmpEnemy);
//                    enemydeleted = true;
//                }
//            }

//            if (!enemydeleted && Player.BoundingBox.IntersectsRect(tmpEnemy.BoundingBox))
//                colision = true;
//        }

//        if (enemydeleted)
//        {
//            foreach (var tmpShoot in deletedShoot)
//                tmpShoot.DeleteElement();

//            foreach (var tmpEnemy in EnemyMeteors)
//                DeleteEnemyMeteor(tmpEnemy);
//        }

//        if (!enemydeleted && colision)
//            CollitionDetected();
//    }


//}

//public void CheckEnemyShipsCollition()
//{
//    if (CollitionDetected != null && !Player.HasCollision)
//    {
//        bool colision = false;

//        var deletedShoot = new List<ShootMeteor>();
//        var deletedEnemyMeteor = new List<EnemyShip>();

//        bool enemydeleted = false;

//        foreach (var tmpEnemy in EnemyShips)
//        {
//            //Comprobamos que no matemos al enemigo
//            foreach (var tmpShoot in Shoots)
//            {
//                if (tmpEnemy.BoundingBox.IntersectsRect(tmpShoot.BoundingBox))
//                {
//                    deletedShoot.Add(tmpShoot);
//                    deletedEnemyMeteor.Add(tmpEnemy);
//                    enemydeleted = true;
//                }
//            }

//            if (!enemydeleted)
//            {
//                //Comprobamos que no esté en la pantalla
//                if (tmpEnemy.PositionX < -(3 + tmpEnemy.ContentSize.Width))
//                {
//                    DeleteEnemyShip(tmpEnemy);
//                    enemydeleted = true;
//                }
//            }

//            if (!enemydeleted && Player.BoundingBox.IntersectsRect(tmpEnemy.BoundingBox))
//                colision = true;
//        }

//        if (enemydeleted)
//        {
//            foreach (var tmpShoot in deletedShoot)
//                tmpShoot.DeleteElement();

//            foreach (var tmpEnemy in EnemyMeteors)
//                DeleteEnemyMeteor(tmpEnemy);
//        }

//        if (!enemydeleted && colision)
//            CollitionDetected();
//    }


//}


//public void CheckEnemiesOutScreen()
//{

//    foreach (var enemy in EnemyMeteors)
//    {
//        if (enemy.PositionX < -(3 + enemy.ContentSize.Width))
//        {
//            DeleteEnemy(enemy);
//        }
//    }

//    for (int i = EnemyMeteors.Count - 1; i >= 0; i--)
//    {
//        if (EnemyMeteors[i].PositionX < -(3 + EnemyMeteors[i].ContentSize.Width))
//        {
//            DeleteEnemy(EnemyMeteors[i]);
//        }
//    }
//}