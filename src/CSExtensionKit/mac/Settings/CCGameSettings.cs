using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{

	public enum ePlayers
	{
		One = 1, Two = 2, Three = 3, Four = 4

	}

	public class CCGameSettingsBase
	{

		public CSKPlayerClass Player1 { get; set; }
		public CSKPlayerClass Player2 { get; set; }
		public CSKPlayerClass Player3 { get; set; }
		public CSKPlayerClass Player4 { get; set; }


		public int GameSpeed;
		public bool GameIsFinished = false;

		public ePlayers Players;

		public CCGameSettingsBase(ePlayers players)
		{
			Players = players;
			Player1 = new CSKPlayerClass();
			//Player2 = new PlayerClass();
			//Player3 = new PlayerClass();
			//Player4 = new PlayerClass();
		}

		#region Static instance

		private static CCGameSettingsBase _instance;

		public static CCGameSettingsBase Instance
		{
			get
			{
				if (_instance == null)
					_instance = new CCGameSettingsBase(ePlayers.One);
				return _instance;
			}
			set { _instance = value; }
		}

		#endregion

	}

}
