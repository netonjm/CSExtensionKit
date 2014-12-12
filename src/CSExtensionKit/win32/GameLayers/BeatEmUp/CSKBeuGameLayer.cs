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
	public class CSKBeuGameLayer : CSKGameLayerBase
	{
		private string TileMapTmxFile;
		private CCTMXTiledMap tileMap;

		public CCSize MapTotalSize { get { return new CCSize(GetMapTotalWidth(), GetMapTotalHeight()); } }

		public CCSize MapSize { get { return tileMap.MapSize; } }
		public CCSize TileSize { get { return tileMap.TileSize; } }

		public override float GetMaxTop()
		{
			return TileSize.Height * 3;
		}

		public override float GetMapTotalHeight()
		{
			return MapSize.Height * TileSize.Height;
		}

		public override float GetMapTotalWidth()
		{
			return MapSize.Width * TileSize.Width;
		}

		public CSKBeuGameLayer(string tileMapTmxFile)
		{
			TileMapTmxFile = tileMapTmxFile;
			//IA Initialization
			IASystem = new CSKBeuIA(this);
		}

		protected override void AddedToScene()
		{
			base.AddedToScene();
			tileMap = new CCTMXTiledMap(TileMapTmxFile); // tileMap = new CCTMXTiledMap("pd_tilemap.tmx");
			AddChild(tileMap, -6);
		}

		public override void Update(float dt)
		{
			base.Update(dt);
			SetViewPointCenter(Player.Position);
		}

		#region OVERWRITE METHODS

		public virtual void OnPlayerHurt(float damage)
		{
		}

		#endregion

		public override void Reset()
		{
			base.Reset();
		}

		protected override void Draw()
		{
			base.Draw();
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}
