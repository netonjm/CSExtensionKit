using System;
using Microsoft.Xna.Framework;
using CocosSharp;
using System.Collections.Generic;
using CocosDenshion;
using BeatEmUpGameExample.Entities;
using CocosSharp.Extensions.SneakyJoystick;
using BeatEmUpGameExample.Layers;
using CSExtensionKit;

namespace BeatEmUpGameExample
{
	public class StartLevelLayer : CSKBeuGameLayer
	{

		public int ROBOTS = 10;

		CCSpriteBatchNode actors;

		Hero hero;

		CCLabel kills;
		CCLabel life;

		bool IsLevelCompleted;

		#region INIT

		public StartLevelLayer(CCSize size)
			: base("level1.tmx",size)
		{
			InitializeMusic();
		}

		protected override void AddedToScene()
		{
			base.AddedToScene();

			SetDefaultFont("MarkerFelt", 22); //Add de default font

			//Add sprite batch
			CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames("pd_sprites.plist");

			//Create a spite batch node with all sprites
			actors = new CCSpriteBatchNode("pd_sprites.png");
			AddChild(actors, -5);

			//Add our custom information layer
			//InformationLayer = new CustomInfoLayer("");

			//Some window information
			kills = GetLabel("0");
			kills.Position = new CCPoint(wSize.Width * .5f, wSize.Height * .95f);
			InformationLayer.AddChild(kills);
		}



		public override void OnEnter()
		{
			base.OnEnter();

			//Configure control panel
			//Our hero initialization
			hero = new Hero();
			InitializePlayer(hero, actors, new CCPoint(100, 100));

			//Hero information
			life = GetLabel("");
			life.Position = hero.GetLifePosition(LifePosition.Up); //new CCPoint( .WindowSizeInPixels.Width * .5f, Director.WindowSizeInPixels.Height * .95f);
			AddChild(life);

			//Initialization Enemies

			for (int i = 0; i < ROBOTS; i++)
				InitializeEnemy(new Robot(), actors);

			controlPanelLayer.ScaleX = controlPanelLayer.ScaleY = .7f;
			controlPanelLayer.JoyControl.Position = new CCPoint(-10, -10);

			SetViewPointCenter(hero.Position);

			Schedule();
		}

		public void InitializeMusic()
		{
			PreloadBackgroundMusic("sounds/latin_industries");
			PlayBackgroundMusic("sounds/latin_industries");
			PreloadEffect("sounds/pd_hit0");
			PreloadEffect("sounds/pd_hit1");
			PreloadEffect("sounds/pd_herodeath");
			PreloadEffect("sounds/pd_botdeath");
		}

		#endregion

		public override void Update(float dt)
		{
			base.Update(dt);

			//Life 
			life.Position = hero.GetLifePosition(LifePosition.Up);
			life.Text = hero.hitPoints.ToString();

			if (!IsLevelCompleted)
				kills.Text = string.Format("Enemies: {0}", Enemies.Count);
			else
				kills.Text = "LEVEL FINISHED";

		}

		public override void OnPlayerHurt(float damage)
		{
			base.OnPlayerHurt(damage);

			hero.hurtWithDamage(damage);

			if (hero.actionState == ActionState.KnockedOut && GetChildByTag(5) == null)
			{
				Endgame();
			}

		}

		public override void OnEnemyShoot(CCEventCustom obj)
		{
			//Se debería pasar el ataque del monstruo más el CCEnemy
			//LOGIC ON ENEMY SHOOT -> Obj.UserData sends the enemy who fired

		}

		private void HeroAttack()
		{
			//Hero attack
			hero.attack();

			if (hero.actionState == ActionState.Attack)
			{
				CCRect attackBox = hero.AbsoluteAttackBoxRect;

				foreach (var robot in Enemies)
				{

					if (robot.actionState == ActionState.KnockedOut)
						continue;

					if (Math.Abs(hero.PositionY - robot.PositionY) < 10)
					{
						if (attackBox.IntersectsRect(robot.AbsoluteHitBoxRect))
						{
							robot.hurtWithDamage(hero.damage);

							if (robot.actionState == ActionState.KnockedOut)
							{
								DeadRobot();
							}
						}
					}
				}
			}
		}

		private void DeadRobot()
		{
			if (Enemies.Count == 0)
				ExecLevelFinished();
		}

		private void ExecLevelFinished()
		{
			IsLevelCompleted = true;
		}

		private void Endgame()
		{
			CCLabel restartLabel = new CCLabel("RESTART", "MarkerFelt", 22);
			CCMenuItemLabel restartItem = new CCMenuItemLabel(restartLabel, RestartGame);
			CCMenu menu = new CCMenu(new CCMenuItem[] { restartItem });
			menu.Position = wSize.Center;
			menu.Tag = 5;
			AddChild(menu, 5);
		}

		private void RestartGame(object obj)
		{
			//Director.ReplaceScene(LoadingLayer.Scene);
		}



		#region JOYSTICK EVENTS

		public override void OnJoyStickButtonPressed(SneakyButtonStatus status, int id)
		{
			if (status == SneakyButtonStatus.Release)
			{
				Console.WriteLine("Button{0} pressed", id);
				if (id == 1)
				{
					HeroAttack();
					return;
				}

				if (id == 2)
				{
					return;
				}
			}
		}

		public override void OnJoyStickStartMove()
		{
			//LOGIC WHEN PLAYER STARTS MOVING Example: Change ship animation
			hero.moveWithDirection(Position);
		}

		public override void OnJoyStickMove(CCPoint direction)
		{
			hero.moveWithDirection(direction);
		}

		public override void OnJoyStickEndMove()
		{
			//LOGIC WHEN PLAYER STOPS MOVING Example: Change ship animation
			if (hero.actionState == ActionState.Walk)
				hero.idle();
		}

		#endregion


#if DEBUG

		protected override void Draw()
		{
			base.Draw();

			CCDrawingPrimitives.Begin();
			//CCDrawingPrimitives.DrawRect(new CCRect(hero.PositionX - (hero.TextureRect.Size.Width * .5f), hero.PositionY - (hero.TextureRect.Size.Height * .5f), hero.TextureRect.Size.Width, hero.TextureRect.Size.Height), CCColor4B.Blue);
			CCDrawingPrimitives.DrawRect(new CCRect(hero.PositionX + hero.hitBox.actual.Origin.X, hero.PositionY + hero.hitBox.actual.Origin.Y, hero.hitBox.actual.Size.Width, hero.hitBox.actual.Size.Height), CCColor4B.Orange);
			CCDrawingPrimitives.DrawRect(new CCRect(hero.PositionX + hero.attackBox.actual.Origin.X, hero.PositionY + hero.attackBox.actual.Origin.Y, hero.attackBox.actual.Size.Width, hero.attackBox.actual.Size.Height), hero.actionState == ActionState.Attack ? CCColor4B.Red : CCColor4B.Yellow);

			foreach (var robot in Enemies)
			{
				if (robot.actionState == ActionState.KnockedOut)
					continue;
				CCDrawingPrimitives.DrawRect(new CCRect(robot.PositionX + robot.hitBox.actual.Origin.X, robot.PositionY + robot.hitBox.actual.Origin.Y, robot.hitBox.actual.Size.Width, robot.hitBox.actual.Size.Height), CCColor4B.Orange);
				CCDrawingPrimitives.DrawRect(new CCRect(robot.PositionX + robot.attackBox.actual.Origin.X, robot.PositionY + robot.attackBox.actual.Origin.Y, robot.attackBox.actual.Size.Width, robot.attackBox.actual.Size.Height), robot.actionState == ActionState.Attack ? CCColor4B.Red : CCColor4B.Yellow);
			}
			CCDrawingPrimitives.End();
		}

#endif

	
	}




}

