using CocosDenshion;
using CocosSharp;
using CocosSharp.Extensions.SneakyJoystick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{

	public enum eTAG_DEPTH
	{
		GAME_LAYER_TAG = 1,
		INFORMATION_LAYER = 100,
		BACKGROUND_LAYER = 2,
		ITEM_LAYER = 10,
		SHOOT_LAYER = 15,
		PLAYER_LAYER = 20,
		ENEMY_LAYER = 25
	}

	/// <summary>
	/// This is the base GameLayer all GameLayers type inherits this. It has information about all player, enemies, controls and code base project.
	/// In a future sure can split them.
	/// </summary>
	public class CSKGameLayerBase : CCLayerColor
	{

		private string defaultFont = "MarkerFelt";
		private int defaultFontSize = 22;

		public CSKIaSystemBase IASystem { get; set; }

		public int EnemyKills;

		public List<CSKPlayer> Players;

		public CSKPlayer Player;

		public List<CCShootBase> Shoots;
		public List<CSKEnemy> Enemies;
		public List<CCShootBase> EnemyShoots;

		public CCSize wSize
		{
			get
			{
				return Window.WindowSizeInPixels;
			}
		}
		//float scale = 1;
		float setViewPercent = 0.25f;

		public bool IsTouchEnabled { get; set; }
		public bool IsKeyboardEnabled { get; set; }

		public const string EVENT_ENEMY_ID = "CCLevelLayerBase_EVENT_ENEMY";
		public const string EVENT_PLAYER_ID = "CCLevelLayerBase_EVENT_PLAYER";

		public void AddSpriteFrames(params string[] filename)
		{
			foreach (var item in filename)
			{
				CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames(item);
			}

		}

		public SneakyPanelControl controlPanelLayer { get; set; }

		private CCEventListenerCustom SneakyJoystickListener;
		private CCEventListenerCustom SneakyButtonListener;
		//public CCEventListenerCustom PlayerListener;
		public CCEventListenerCustom EnemyListener;
		private CCEventListenerKeyboard KeyListener;

		/// <summary>
		/// This layer is the info screen. All the statics objects like controls or player info goes here.
		/// </summary>
		public CCInformationPanelBase InformationLayer { get; set; }

		/// <summary>
		/// First Initial State. 
		/// </summary>
		public CSKGameLayerBase(CCSize size) :base (size)
		{
            
		   

			Shoots = new List<CCShootBase>();
			Enemies = new List<CSKEnemy>();
			EnemyShoots = new List<CCShootBase>();
			Players = new List<CSKPlayer>();
		}



		/// <summary>
		/// Second Initial State. Window screen objects
		/// </summary>
		/// <param name="windowSize"></param>
		protected override void AddedToScene()
		{
			base.AddedToScene();

            InformationLayer = new CCInformationPanelBase();
			Scene.AddChild(InformationLayer, 99999);

			//Player Shoot events
			//PlayerListener = new CCEventListenerCustom(EVENT_PLAYER_ID, OnPlayerShoot);
			//AddEventListener(PlayerListener, 1);

			//Enemy Shoot events
			EnemyListener = new CCEventListenerCustom(EVENT_ENEMY_ID, OnEnemyShoot);
			AddEventListener(EnemyListener, 1);


			//SNEAKY JOYSTICK
            controlPanelLayer = new SneakyPanelControl(2);
#if DEBUG
			controlPanelLayer.IsDebug = true;
#endif
			InformationLayer.AddChild(controlPanelLayer, 9999);
		}


		/// <summary>
		/// Third Initial State.
		/// </summary>
		public override void OnEnter()
		{
			base.OnEnter();

			//Sneaky eventListener
			SneakyJoystickListener = new CCEventListenerCustom(SneakyPanelControl.JOY_LISTENER_ID, (customEvent) =>
			{
				var response = customEvent.UserData as SneakyJoystickEventResponse;
				if (response != null)
				{
					switch (response.ResponseType)
					{
						case SneakyJoystickMovementStatus.Start:
							OnJoyStickStartMove();
							break;
						case SneakyJoystickMovementStatus.OnMove:
							OnJoyStickMove((CCPoint)response.UserData);
							break;
						case SneakyJoystickMovementStatus.End:
							OnJoyStickEndMove();
							break;
						default:
							break;
					}

				}
			});
			AddEventListener(SneakyJoystickListener, 1);

			SneakyButtonListener = new CCEventListenerCustom(SneakyPanelControl.BUTTON_LISTENER_ID, (customEvent) =>
			{
				var response = customEvent.UserData as SneakyButtonEventResponse;
				if (response != null)
				{
					OnJoyStickButtonPressed(response.ResponseType, response.ID);
				}

			});

			AddEventListener(SneakyButtonListener, 1);
		}

		/// <summary>
		/// Sets the default font for label creation
		/// </summary>
		/// <param name="defaultFont">Default font name. Ex: MarkerFelt</param>
		/// <param name="size">Size of the font</param>
		public void SetDefaultFont(string defaultFont, int size)
		{
			this.defaultFont = defaultFont; defaultFontSize = size;
		}

		/// <summary>
		/// Gets a CCLabel from the default font setted with an optional default text
		/// </summary>
		/// <param name="defaultText"></param>
		/// <returns></returns>
        public CCLabelTtf GetLabel(string defaultText = "")
		{
			return new CCLabelTtf(defaultText, defaultFont, defaultFontSize);
		}

		/// <summary>
		/// Preload an effect sound to skip game flips
		/// </summary>
		/// <param name="filename"></param>
		public void PreloadEffect(string filename)
		{
			CCSimpleAudioEngine.SharedEngine.PreloadEffect(filename);
		}

		/// <summary>
		/// Plays a short sound for sfx
		/// </summary>
		/// <param name="filename"></param>
		public void PlayEffect(string filename)
		{
			CCSimpleAudioEngine.SharedEngine.PlayEffect(filename);
		}

		/// <summary>
		/// Plays the background music level
		/// </summary>
		/// <param name="filename"></param>
		public void PlayBackgroundMusic(string filename, bool loop = false)
		{
			CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic(filename);
		}


		/// <summary>
		/// Precarges background music to skip game flips
		/// </summary>
		/// <param name="filename"></param>
		public void PreloadBackgroundMusic(string filename)
		{
			CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic(filename);
		}

		/// <summary>
		/// Initializes a player on a initial position inside a container
		/// </summary>
		/// <param name="player">Instanciated player</param>
		/// <param name="container">Container when will be dropped</param>
		/// <param name="initialPosition">Player initial position</param>
		public virtual CSKPlayer InitializePlayer(CSKPlayer player, CCNode container, CCPoint initialPosition)
		{

			if (Players.Exists((p) => p == player))
			{
				Console.WriteLine("CANNOT ADD PLAYER. ITS ON LAYER");
				return null;
			}

			CCGameSettingsBase.Instance.Player1 = player.klass; //TODO: ARRAY DE PLAYERS

			player.desiredPosition = player.Position;
			player.idle();

			Players.Add(player); //Add to players array

			Player = player;

			container.AddChild(player);

			//AddChild(player, (int)eTAG_DEPTH.PLAYER_LAYER);

			player.Position = initialPosition;

			Console.WriteLine("PLAYER 1 ADDED TO SHIP LAYER");
			return player;
		}

		/// <summary>
		/// Initializes an enemy on a random position map. This method can be override to add more functionality
		/// </summary>
		/// <param name="robot">Initialized enemy</param>
		/// <param name="container">Container when will be added</param>
		public virtual CSKEnemy InitializeEnemy(CSKEnemy enemy, CCNode container)
		{
			return InitializeEnemy(enemy, container, GetRandomPosition(enemy));
		}

		/// <summary>
		/// Initializes an enemy on a position. This method can be override to add more functionality
		/// </summary>
		/// <param name="enemy">Initialized enemy</param>
		/// <param name="container">Container when will be added</param>
		/// <param name="initialPosition">Position inside the layer</param>
		public virtual CSKEnemy InitializeEnemy(CSKEnemy enemy, CCNode container, CCPoint initialPosition)
		{
			enemy.Position = initialPosition;
			enemy.ScaleX = -1;
			enemy.actionState = ActionState.Idle;

			enemy.desiredPosition = enemy.Position;
			enemy.idle();

			container.AddChild(enemy);
			Enemies.Add(enemy);
			return enemy;
		}

		/// <summary>
		/// Gets a random position in the level from a actor
		/// </summary>
		/// <param name="actor"></param>
		/// <returns></returns>
		public CCPoint GetRandomPosition(CSKEntity actor)
		{
			int minX = (int)(wSize.Width + actor.centerToSides);
			int maxX = (int)(GetMapTotalWidth() - actor.centerToSides);
			int minY = (int)actor.centerToBottom; // ContentSize.Height;
			int maxY = (int)(actor.Texture.ContentSizeInPixels.Height);

			return CCPointExHelper.GetRandomPosition(wSize, minX, maxX, minY, maxY);
		}


		/// <summary>
		/// Is executed when the player press a SneakyJoystick button.
		/// </summary>
		/// <param name="status">Indiques button status Pressed/Released</param>
		/// <param name="id">Button Id pressed</param>
		public virtual void OnJoyStickButtonPressed(SneakyButtonStatus status, int id)
		{

		}

		/// <summary>
		/// Indiques when a user stops movement on SneakyJoystick
		/// </summary>
		public virtual void OnJoyStickEndMove()
		{

		}

		/// <summary>
		/// Indiques when a user is moving the SneakyJoystick
		/// </summary>
		/// <param name="direction">Gets users point direction</param>
		public virtual void OnJoyStickMove(CCPoint direction)
		{

		}

		/// <summary>
		/// Indiques when a user starts movement on SneakyJoystick
		/// </summary>
		public virtual void OnJoyStickStartMove()
		{

		}

		public void InitializeKeyboard()
		{

			KeyListener = new CCEventListenerKeyboard();
			KeyListener.OnKeyPressed = OnKeyPressed;
			KeyListener.OnKeyReleased = OnKeyReleased;

		}

		/// <summary>
		/// This is fired by the IA System
		/// </summary>
		/// <param name="obj"></param>
		public virtual void OnEnemyShoot(CCEventCustom obj)
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		public virtual void OnPlayerShoot(CCEventCustom obj)
		{

		}

		public virtual float GetMapTotalHeight()
		{
			throw new NotImplementedException();
		}

		public virtual float GetMapTotalWidth()
		{
			throw new NotImplementedException();
		}
		public virtual float GetMaxTop()
		{
			throw new NotImplementedException();
		}

		public CCSize GetMapTotalSize()
		{
			return new CCSize(GetMapTotalWidth(), GetMapTotalHeight());
		}

		public int MapWith
		{
			get
			{
				return (int)(GetMapTotalWidth() - wSize.Width / 2);
			}
		}
		public int MapHeight
		{
			get
			{
				return (int)(GetMapTotalHeight() - wSize.Height / 2);
			}
		}


		public override void Update(float dt)
		{
			base.Update(dt);

			var mapTotalSize = GetMapTotalSize();
			var mapMaxTop = GetMaxTop();

			//Next IA step
			IASystem.Step(Player, Enemies, wSize, dt);

			//Next player step
			Player.Step(controlPanelLayer, mapTotalSize, mapMaxTop, dt);

			//Next robots step
			foreach (CSKEnemy robot in Enemies)
				robot.Step(mapTotalSize, mapMaxTop, dt);

			//Check shots collitions
			//if (Players.Count > 0)
			//    CheckEnemyCollition(Players[0]);

			//Reorder position of actors
			reorderActors();
		}

		public virtual void OnKeyReleased(CCEventKeyboard obj) { }
		public virtual void OnKeyPressed(CCEventKeyboard obj) { }
		// public virtual void OnSneakyButtonListener(SneakyButtonEventResponse e) { }

		public override void OnExit()
		{
			base.OnExit();

			this.RemoveEventListener(SneakyJoystickListener);
			this.RemoveEventListener(SneakyButtonListener);
			this.RemoveEventListener(KeyListener);

			//this.RemoveEventListener(PlayerListener);
			this.RemoveEventListener(EnemyListener);
		}

		public void reorderActors()
		{

			foreach (var item in Players)
				ReorderChild(item, (int)(wSize.Height - item.PositionY));

			foreach (var item in Shoots)
				ReorderChild(item, (int)(wSize.Height - item.PositionY));

			foreach (var item in Enemies)
				ReorderChild(item, (int)(wSize.Height - item.PositionY));

			foreach (var item in EnemyShoots)
				ReorderChild(item, (int)(wSize.Height - item.PositionY));
		}

		public void SetViewPointCenter(CCPoint position)
		{


			int x = (int)Math.Max(position.X, wSize.Width * setViewPercent);
			int y = (int)Math.Max(position.Y, wSize.Height * setViewPercent);
			x = (int)Math.Min(x, MapWith);
			y = (int)Math.Min(y, MapHeight);
			CCPoint actualPosition = new CCPoint(x, y);

			CCPoint centerOfView = new CCPoint(wSize.Width * setViewPercent, wSize.Height * setViewPercent);
			Position = centerOfView - actualPosition;
		}

		public string GetInfoPlayerHealth(CSKPlayerClass settings)
		{
			return String.Format("{0}/{1}", settings.HEALTH, settings.MAX_HEALTH);
		}

		public string GetInfoEnemies()
		{
			return String.Format("{0}", Enemies.Count);
		}

		public string GetInfoKills()
		{
			return String.Format("{0}", EnemyKills);
		}


		public virtual void Reset()
		{

			//TODO: IMPLEMENT RESET

			//layers.Reset();
			//lock (Enemies)
			//{
			//    Shoots.Clear();
			//}

			//RemoveAllChildren();

			//lock (Enemies)
			//{
			//    Enemies.Clear();
			//}

			//foreach (var player in Players)
			//{
			//    player.Reset();
			//}

			EnemyKills = 0;

		}



	}
}
