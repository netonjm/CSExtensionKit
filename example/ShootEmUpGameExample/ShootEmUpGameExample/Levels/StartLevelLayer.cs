using System;
using Microsoft.Xna.Framework;
using CocosDenshion;
using System.Collections.Generic;
using CocosSharp;
using CocosSharp.Extensions.SneakyJoystick;

using CSExtensionKit;

namespace ShootEmUpGameExample
{
	public enum EndReason
	{
		Win = 1,
		Lose = 0
	}
	public class StartLevelLayer : CSKSeuGameLayer
	{

		int kNumAsteroids = 7;
		int kNumLasers = 5;

		CCSprite _spacedust1;

		CCSprite _galaxy;
		CCSprite _spacialanomaly;

		//CCSprite _ship;
		//float _shipPointsPerSecY;

		//List<CCSprite> _asteroids;
		int _nextAsteroid;
		DateTime _nextAsteroidSpawn;

		List<CCSprite> _shipLasers;
		int _nextShipLaser;

		int _lives;

		DateTime _gameOverTime;
		bool _gameOver;

		public StartLevelLayer()
		{

		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			// 3) Determine relative movement speeds for space dust and background
			CCPoint dustSpeed = new CCPoint(0.1f, 0.1f);
			CCPoint bgSpeed = new CCPoint(0.05f, 0.05f);

			// 4) Add children to CCParallaxNode

			AddParticleSystemQuad(new string[] { @"Stars1.plist", @"Stars2.plist", @"Stars3.plist" },
				(starsEffect) =>
				{
					starsEffect.Position = new CCPoint(Window.WindowSizeInPixels.Width, Window.WindowSizeInPixels.Height / 2);
					starsEffect.PositionVar = new CCPoint(0, Window.WindowSizeInPixels.Height / 2);
					starsEffect.StartSize *= 0.2f;
					starsEffect.EndSize *= 0.2f;
					starsEffect.Speed *= CCRandom.GetRandomFloat(0.3f, 0.6f);
				});


			_galaxy = AddParallaxBackground("_galaxy", @"bg_galaxy-hd.png", 0, new CCPoint(0.015f, 0), new CCPoint(Window.WindowSizeInPixels.Width + 200, 0), _backgroundNode);
			_spacialanomaly = AddParallaxBackground("_spacialanomaly", @"bg_spacialanomaly-hd.png", 0, new CCPoint(0.015f, 0), new CCPoint(Window.WindowSizeInPixels.Width + 900, Window.WindowSizeInPixels.Height * 0.3f), _backgroundNode);
			_spacedust1 = AddParallaxBackground("_spacedust1", @"bg_front_spacedust-hd.png", 0, new CCPoint(0.1f, 0), new CCPoint(Window.WindowSizeInPixels.Width, Window.WindowSizeInPixels.Height / 2), _foregroundNode);

			// 6) Add stars

			_bachMeteor = AddBatchSprite("meteors");
			_bachShips = AddBatchSprite("ships");

			// 7) Add space ship
			InitializePlayer(new Hero(), _bachShips, new CCPoint(Window.WindowSizeInPixels.Width * 0.1f, Window.WindowSizeInPixels.Height * 0.5f));
			Player.RotationX += 90;
			Player.RotationY += 90;
			Player.ScaleX = Player.ScaleY = 0.5f;

			// 9) Setup asteroids array
			//_asteroids = new List<CCSprite>(kNumAsteroids);

			CSKEnemy enemy;
			for (int i = 0; i < kNumAsteroids; ++i)
			{
				enemy = InitializeEnemy(new Asteroid(string.Format(@"meteorito{0}.png", CCRandom.GetRandomInt(1, 2) == 1 ? 2 : 4)), _bachMeteor);
				enemy.Visible = false;
				enemy.IsIA = false;
			}

			// 10) Setup lasers array
			_shipLasers = new List<CCSprite>(kNumLasers);
			for (int i = 0; i < kNumLasers; ++i)
			{
				CCSprite shipLaser = new CCSprite(@"shoot.png");
				shipLaser.Visible = false;
				_bachShips.AddChild(shipLaser);
				_shipLasers.Add(shipLaser);
			}

			// 12) Setup lives and game over time
			_lives = 3;

			_gameOverTime = DateTime.Now.AddSeconds(30.0);

			// 13) Setup sounds

			PlayBackgroundMusic(@"snd/SpaceGame", true);
			PreloadEffect(@"snd/explosion_large");
			PreloadEffect(@"snd/laser_ship");

		}

		public override void OnEnter()
		{
			base.OnEnter();

			Schedule();
		}


		public override void OnEnemyShoot(CCEventCustom obj)
		{
			//LOGIC ON ENEMY SHOOT -> Obj.UserData sends the enemy who fired

		}

		public override void OnPlayerShoot(CCEventCustom obj)
		{
			//LOGIC ON PLAYER SHOOT -> Obj.UserData sends the player who fired

		}


		public override void OnJoyStickButtonPressed(SneakyButtonStatus status, int id)
		{
			//LOGIC WHEN PLAYER PRESS BUTTON : Obj.UserData sends the button pressed
			if (status == SneakyButtonStatus.Press)
			{
				if (_gameOver)
					return;

				Console.WriteLine("Button{0} pressed", id);
				if (id == 1)
				{

					PlayEffect(@"snd/laser_ship");

					CCSprite shipLaser = _shipLasers[_nextShipLaser];
					_nextShipLaser++;
					if (_nextShipLaser >= _shipLasers.Count) _nextShipLaser = 0;

					shipLaser.Position = Player.Position + new CCPoint(shipLaser.ContentSize.Width * .5f, 0);
					shipLaser.Visible = true;
					shipLaser.StopAllActions();

					shipLaser.RunAction(new CCSequence(
						new CCMoveBy(0.5f, new CCPoint(Window.WindowSizeInPixels.Width + 50, 0)),
						new CCCallFuncN(SetInvisible)
						)

						);
					return;
				}

				if (id == 2)
				{
					// PlayerFire(player);

					return;
				}
			}

		}



		private void SetInvisible(CCNode obj)
		{
			(obj as CCSprite).Visible = false;
		}

		void EndScene(EndReason endReason)
		{
			if (_gameOver) return;
			_gameOver = true;

			var label = GetLabel(endReason == EndReason.Win ? "You win!" : "You lose!");
			label.Scale = 0.1f;
			label.Position = new CCPoint(wSize.Width / 2, wSize.Height * 0.6f);
			InformationLayer.AddChild(label);

			//var restartItem = GetLabel("Restart");
			//var menu = GetMenu(0.1f, new CCMenuItemLabel(restartItem, restartTapped));
			//menu.Position = new CCPoint(wSize.Width / 2, wSize.Height * 0.4f);

			//restartItem.RunAction(new CCScaleTo(0.5f, 1.0f));
			label.RunAction(new CCScaleTo(0.5f, 1.0f));

		}

		public CCMenu GetMenu(float scale, CCMenuItemLabel item)
		{
			return GetMenu(scale, new CCMenuItemLabel[] { item });
		}

		public CCMenu GetMenu(float scale, CCMenuItemLabel[] items)
		{
			int position = (int)((1f / items.Length) * .5);
			int first = position;
			CCMenuItem[] createdItems = new CCMenuItem[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				items[i].Scale = scale;
				items[i].Position = new CCPoint(wSize.Width / 2, wSize.Height * position);
				position -= first;
			}

			return new CCMenu(createdItems);
		}

		private void restartTapped(object obj)
		{

		}


		void ResetScene()
		{
			CCGameSettingsBase.Instance.GameIsFinished = false;
			Reset();
		}

		#region InGame Menu

		void ShowIngameMenu()
		{
			CCMenuItem tt2 = new CCMenuItemImage("menu/nuevo", "menu/nuevo_off", SelectedIngameMenu);
			tt2.Scale = 0.7f;
		}

		void SelectedIngameMenu(object t)
		{

			ResetScene();
		}

		#endregion

		public override void Update(float dt)
		{

			if (_gameOver)
				return;

			base.Update(dt);

			_backgroundNode.Position = _backgroundNode.Position + (BackgroundScrollVelocity * dt);  //ccpAdd(, ccpMult(backgroundScrollVel, dt));

			_foregroundNode.Position = _foregroundNode.Position + (BackgroundScrollVelocity * dt);  //ccpAdd(, ccpMult(backgroundScrollVel, dt));

			UpdateUserInfo();

			//float newY = _ship.Position.Y + (_shipPointsPerSecY * dt);
			//newY = Math.Min(Math.Max(newY, minY), maxY);
			//_ship.Position = new CCPoint(_ship.Position.X, newY);

			// 4) Spawn asteroids
			DateTime curTime = DateTime.Now;
			if (curTime > _nextAsteroidSpawn)
			{

				float randSecs = CCRandom.GetRandomFloat(0.9f, 1.5f);
				_nextAsteroidSpawn = curTime.AddSeconds(randSecs);

				CCSprite asteroid = Enemies[_nextAsteroid];

				float randY = CCRandom.GetRandomFloat(0.0f + asteroid.ContentSize.Width * .5f, wSize.Height);

				_nextAsteroid++;
				if (_nextAsteroid >= Enemies.Count) _nextAsteroid = 0;

				//asteroid.StopAllActions();
				//asteroid.Position = new CCPoint(wSize.Width + asteroid.ContentSize.Width, randY);

				asteroid.RunAction(new CCSequence(
					   new CCMoveTo(CCRandom.GetRandomFloat(4.5f, 7), new CCPoint(-asteroid.TextureRectInPixels.Size.Width, asteroid.PositionY + CCRandom.GetRandomFloat(-50, 50))),
					   new CCCallFuncN(SetInvisible)
					   )
					   );
				asteroid.Visible = true;

				//asteroid.RunAction(new CCRepeatForever(new CCSequence(
				//	new CCRotateBy(CCRandom.GetRandomFloat(2, 4), 360f)))
				//	);



			}

			//5) Check for collisions
			foreach (CCSprite asteroid in Enemies)
			{
				if (!asteroid.Visible) continue;

				foreach (CCSprite shipLaser in _shipLasers)
				{
					if (!shipLaser.Visible) continue;

					if (shipLaser.BoundingBox.IntersectsRect(asteroid.BoundingBox))
					{
						CCSimpleAudioEngine.SharedEngine.PlayEffect(@"snd/explosion_large");
						shipLaser.Visible = false;
						asteroid.Visible = false;
						continue;
					}
				}

				if (Player.BoundingBox.IntersectsRect(asteroid.BoundingBox))
				{
					CCSimpleAudioEngine.SharedEngine.PlayEffect(@"snd/explosion_large");
					asteroid.Visible = false;
					Player.RunAction(new CCBlink(1.0f, 9));

					_lives--;
				}
			}

			// 6) Check for game over
			if (_lives <= 0)
			{
				Player.StopAllActions();// stopAllActions];
				Player.Visible = false;
				EndScene(EndReason.Lose);

			}
			else if (curTime >= _gameOverTime)
			{
				EndScene(EndReason.Win);
			}

		}



#if DEBUG

		protected override void Draw()
		{
			base.Draw();

			//CCDrawingPrimitives.Begin();
			//CCDrawingPrimitives.DrawRect(new CCRect(0, 0, ContentSize.Width, ContentSize.Height), CCColor4B.Red);
			//CCDrawingPrimitives.End();
		}


#endif
		public void UpdateUserInfo()
		{

			//if (CCGameSettingsBase.Instance.GameIsFinished)
			//InformationLayer.SetText("GAME OVER");
			//else
			//InformationLayer.SetText(string.Format("PLAYER {0} - ENEMIES: {1} - KILLS: {2}", GetInfoPlayerHealth(CCGameSettingsBase.Instance.Player1), GetInfoEnemies(), GetInfoKills()));
		}



		public static CCScene Scene
		{
			get
			{

				// 'scene' is an autorelease object.
				var scene = new CCScene(ShootEmUpGameExample.Mac.GameAppDelegate.Window);
				//add information layer

				var firstLevel = new StartLevelLayer();
				scene.AddChild(firstLevel, (int)eTAG_DEPTH.GAME_LAYER_TAG);

				firstLevel.IsKeyboardEnabled = true;
				firstLevel.IsTouchEnabled = true;

				// return the scene
				return scene;

			}

		}





	}
}

//void Player1Controller_Button3StartPress()
//{
//    //Player1Char.ActionFire();

//}


//void Player1Controller_Button4StartPress()
//{
//    //layer1Char.ActionMagic(AxelMagics.superpunch);
//}


//public void InitializeListeners()
//{

//    //PLAYER SHOOT EVENT =================================================================================
//    CCEventListenerCustom eventPlayerShootCustom = new CCEventListenerCustom(Player.EVENT_PLAYER_SHOOT, (obj) =>
//    {
//        //Receive player Shoot Event
//        CCSimpleAudioEngine.SharedEngine.PlayEffect("music/ship/shoot_light", false);
//        ShootType tipo = (ShootType)obj.UserData;
//        //Shoot(tipo, player);
//    });
//    EventDispatcher.AddEventListener(eventPlayerShootCustom, 1);

//    //PLAYER SHOOT EVENT =================================================================================
//    CCEventListenerCustom eventEnemyShootCustom = new CCEventListenerCustom(EnemyShip.EVENT_ENEMY_SHOOT, (obj) =>
//    {
//        CCEnemyBase enemy = (CCEnemyBase)obj.UserData;
//        if (enemy != null)
//        {
//            if (enemy.IAActionState != EnemyActionState.OutOfRange && enemy.IAActionState != EnemyActionState.Firing)
//            {
//                enemy.IAActionState = EnemyActionState.Firing;
//                enemy.IA_INFO = "FIRE!";
//            }

//        }
//    });
//    EventDispatcher.AddEventListener(eventEnemyShootCustom, 1);
//}
//public void InitializeUserController()
//{
//    controlPanelLayer = new SneakyPanelControl();

//    //controlPanelLayer.Button1StartPress += Player1Controller_Button1StartPress;
//    //controlPanelLayer.Button2StartPress += Player1Controller_Button2StartPress;
//    //controlPanelLayer.Button3StartPress += Player1Controller_Button3StartPress;
//    //controlPanelLayer.Button4StartPress += Player1Controller_Button4StartPress;

//    AddChild(controlPanelLayer, 100);
//}