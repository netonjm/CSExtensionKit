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

			//Map Tile
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

//        private virtual void Shoot(ShootType tipo, CCPoint initialPosition, bool isPlayer)
//        {
//            if (tipo == ShootType.Meteoro)
//            {

//#if WINDOWS_PHONE

//                Deployment.Current.Dispatcher.BeginInvoke(() =>
//                {
//                    ShootMeteor meteorof = null;
//                    meteorof = new ShootMeteor(isPlayer);
//                    meteorof.Deleted += (s) =>
//                    {
//                        RemoveShoot(s);
//                    };

//                    meteorof.Position = initialPosition;

//                    if (isPlayer)
//                        AddShoot(meteorof);
//                    else
//                        AddEnemyShoot(meteorof);
//                });
//#else

//                CCShootBase meteorof = null;
//                meteorof = new CCSho(initialPosition, isPlayer);

//                //meteorof.Deleted += (s) =>
//                //{
//                //    RemoveShoot(s);
//                //};

//                if (isPlayer)
//                    AddShoot(meteorof);
//                else
//                    AddEnemyShoot(meteorof);

//#endif

//            }
//        }


//public void Shoot(ShootType shootType, EnemyShip enemy)
//{
//    Shoot(shootType, enemy.Position, false);
//}

//public void Shoot(ShootType shootType, Player player)
//{
//    Shoot(shootType, player.Position, true);
//}

//public void SetPlayerPosition(SneakyPanelControl controller, float dt)
//{
//    Player1.Position = controller.GetPlayerPosition(dt);
//}

//public void SetPlayerController(SneakyPanelControl controller)
//{
//    controller.SetPlayer(Player1);
//}

//public void CheckEnemyCollition(CCPlayerBase Player1)
//{

//    if (Player1 == null)
//        return;

//    if (!Player1.HasCollision)
//    {

//        var deletedShoot = new List<CCShootBase>();
//        var deletedEnemy = new List<CCNode>();

//        bool enemydeleted = false;

//        CCEnemyBase[] cacheEnemies = new CCEnemyBase[Enemies.Count];
//        Enemies.CopyTo(cacheEnemies);

//        CCShootBase[] cacheShoots = new CCShootBase[Shoots.Count];
//        Shoots.CopyTo(cacheShoots);

//        foreach (CCEnemyBase tmpEnemy in cacheEnemies) //cacheEnemies.Where(s => (typeof(EnemyMeteor) == s.GetType() && (s as EnemyMeteor).CheckCollition) || (typeof(EnemyShip) == s.GetType() && (s as EnemyShip).CheckCollition)))
//        {
//            enemydeleted = false;

//            //COMPROBAMOS LOS DISPAROS QUE HAN IMPACTADO CON EL ENEMIGO
//            foreach (var tmpShoot in cacheShoots)
//            {

//                //TODO: MEJORAR Y PONER UN CIRCULO CON RADIO ALTO Y ANCHO en el caso de los asteorides
//                //AÑADIMOS LA RRAY DE ELEMENTOS PARA BOTTAR
//                if (tmpEnemy.BoundingBox.IntersectsRect(tmpShoot.BoundingBox))
//                {

//                    deletedShoot.Add(tmpShoot);
//                    deletedEnemy.Add(tmpEnemy);
//                    enemydeleted = true; //SOLO ES PARA EL ENEMIGO
//                }
//            }

//            if (enemydeleted) //SI HEMOS IMPACTADO EN ALGUN ENEMIGO LO BORRAMOS
//            {

//                //ELIMINAMOS EL DISPARO
//                foreach (var tmpShoot in deletedShoot)
//                    RemoveShoot(tmpShoot);// tmpShoot.DeleteElement(); //Eliminamos el disparo que ha impactado con el enemigo

//                if (tmpEnemy.GetType() == typeof(CCEnemyBase))
//                {
//                    if ((tmpEnemy as CCEnemyBase).RemoveLife(1))
//                    {
//                        EnemyKills++;
//                        DeleteEnemy(tmpEnemy, true);
//                    }
//                }
//                else if (tmpEnemy.GetType() == typeof(CCEnemyBase))
//                {
//                    EnemyKills++;
//                    DeleteEnemy(tmpEnemy, true);
//                }


//            }
//            else //SI EL ENEMIGO NO HA SIDO DESTRUIDO
//            {

//                //COMPROBAMOS QUE SALGA FUERA DE PANTALLA
//                if (tmpEnemy.PositionX < -(3 + tmpEnemy.ContentSize.Width))
//                {
//                    DeleteEnemy(tmpEnemy, false);
//                    enemydeleted = true;
//                }

//                //Comprobamos que esté activada la colision 
//                if ((typeof(CCEnemyBase) == tmpEnemy.GetType() && (tmpEnemy as CCEnemyBase).CheckCollition))
//                    //ENEMIGO CHOCA CON NAVE
//                    if (!enemydeleted && Player1.BoundingBox.IntersectsRect(tmpEnemy.BoundingBox))
//                    {

//                        //TODO: COLLISION DETECTED EVENT
//                        //   CollitionDetected();
//                    }

//            }




//        }

//    }

//}

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