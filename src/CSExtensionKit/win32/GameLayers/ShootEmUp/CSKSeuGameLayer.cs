using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{
	public class CSKSeuGameLayer : CSKGameLayerBase
	{


		public CCSpriteBatchNode _bachMeteor;

		public CCSpriteBatchNode _bachShips;

		public CCParallaxNode _backgroundNode;
		public CCParallaxNode _foregroundNode;

		//public CCSize _windowSize { get; set; }
		//public CCSize WindowSize { get { return _windowSize; } set { _windowSize = value; } }

		// 1) Update background position
		public CCPoint BackgroundScrollVelocity = new CCPoint(-1000, 0);


		public CCSpriteBatchNode AddBatchSprite(string filename)
		{

			if (filename.EndsWith(".plist"))
				filename = filename.Replace(".plist", "");

			var batch = new CCSpriteBatchNode(filename);
			AddChild(batch);
			CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames(filename + ".plist");

			return batch;
		}


		public void AddParticleSystemQuad(string[] plistFiles, Action<CCParticleSystemQuad> particleInitialization)
		{

			CCParticleSystemQuad starsEffect;
			foreach (var stars in plistFiles)
			{
				starsEffect = new CCParticleSystemQuad(stars);
				if (particleInitialization != null)
					particleInitialization(starsEffect);
				AddChild(starsEffect, 1);
			}

		}

		public override float GetMapTotalHeight()
		{
			return wSize.Height;
		}

		public override float GetMapTotalWidth()
		{
			return wSize.Width;
		}

		public override float GetMaxTop()
		{
			return wSize.Height;
		}

		public CCSprite AddParallaxBackground(string idlayer, string fileName, int z, CCPoint ratio, CCPoint offset, CCParallaxNode container)
		{
			var dev = new CCSprite(fileName);
			container.AddChild(new CCSprite(fileName), 0, ratio, offset);
			return dev;
		}

		protected override void AddedToScene()
		{

			base.AddedToScene();

			//IA Initialization
			IASystem = new CSKSeuIA(this);

			//Map Tile
			// 1) Create the CCParallaxNode
			_backgroundNode = new CCParallaxNode();
			AddChild(_backgroundNode, -3);

			_foregroundNode = new CCParallaxNode();
			AddChild(_foregroundNode, 30);

			_bachMeteor = new CCSpriteBatchNode("meteors");
			AddChild(_bachMeteor);

			_bachShips = new CCSpriteBatchNode("ships");
			AddChild(_bachShips);

		}

		public override void Update(float dt)
		{
			base.Update(dt);

			// 2) Check for background elements moving offscreen
			//List<CCSprite> spaceDusts = new List<CCSprite> { _spacedust1, _spacedust2 }; // [NSArray arrayWithObjects:, nil];

			//foreach (CCSprite spaceDust in spaceDusts)
			//{

			//	if (_backgroundNode.ConvertToWorldSpace(spaceDust.Position).X < -spaceDust.ContentSize.Width)
			//	{
			//		// _backgroundNode. incrementOffset:ccp(2spaceDust.contentSize.width,0) forChild:spaceDust];
			//	}
			//}

			//List<CCSprite> backgrounds = new List<CCSprite> { _planetsunrise, _galaxy, _spacialanomaly, _spacialanomaly2 };
			//foreach (CCSprite background in backgrounds)
			//{
			//	if (_backgroundNode.ConvertToWorldSpace(background.Position).X < -background.ContentSize.Width)
			//	{
			//		// if ([_backgroundNode convertToWorldSpace:background.Position].x < -background.contentSize.width) {
			//		//[_backgroundNode incrementOffset:ccp(2000,0) forChild:background];
			//	}
			//}

			//SetViewPointCenter(Player.Position);
		}

		#region OVERWRITE METHODS

		public override void OnEnemyShoot(CCEventCustom obj)
		{

		}

		public override void OnPlayerShoot(CCEventCustom obj)
		{

		}

		public virtual void OnPlayerHurt(float damage)
		{


		}

		#endregion


		protected override void Draw()
		{
			base.Draw();

		}


		#region SHOOT EVENTS


		public void PlayerNextFire(CCPlayerBase Player1)
		{
			Player1.NextFire();
		}

		public void PlayerFire(CCPlayerBase Player1)
		{
			Player1.Fire();
		}


		#endregion





	}
}
