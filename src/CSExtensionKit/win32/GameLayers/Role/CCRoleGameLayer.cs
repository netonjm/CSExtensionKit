using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CocosDenshion;
using CocosSharp;
using CocosSharp.Extensions.SneakyJoystick;



#if WINDOWS_PHONE
using System.Windows;
#endif

namespace CSExtensionKit.GameLayers
{

    public class CCRoleGameLayer : CCLevelLayerBase
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

        public BeuIABase IASystem { get; set; }

        public int EnemyKills;

        public List<CCPlayerBase> Players;

        public List<CCShootBase> Shoots;
        public List<CCEnemyBase> Enemies;
        public List<CCShootBase> EnemyShoots;

        public CCRoleGameLayer()
        {
            //Array creation
            Players = new List<CCPlayerBase>();
            Shoots = new List<CCShootBase>();
            Enemies = new List<CCEnemyBase>();
            EnemyShoots = new List<CCShootBase>();

            InitializeIA();  //Initialization of IA 

            InitializeListeners(); //Initialization of Listeners 

            //Some variables
            EnemyKills = 0;

            Schedule();
        }

        private void InitializeIA()
        {
           // IASystem = new CCIABeatEmUpBodyBase();
        }

        public void InitializeListeners()
        {
            //PLAYER SHOOT EVENT =================================================================================
            CCEventListenerCustom eventPlayerShootCustom = new CCEventListenerCustom(EVENT_PLAYER_ID, OnPlayerShoot);
            AddEventListener(eventPlayerShootCustom, 1);

            //PLAYER SHOOT EVENT =================================================================================
            CCEventListenerCustom eventEnemyShootCustom = new CCEventListenerCustom(EVENT_ENEMY_ID, OnEnemyShoot);
            AddEventListener(eventEnemyShootCustom, 1);
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if (Players.Count > 0)
                CheckEnemyCollition(Players[0]);
        }

        #region CREATE PLAYER
        public void AddPlayer(CCPlayerBase newPlayer, CCPoint initialPosition, int depth)
        {

            if (Players.Find((tmp) => tmp == newPlayer) != null)
            {
                Console.WriteLine("CANNOT ADD PLAYER. ITS ON LAYER");
                return;
            }

            CCGameSettingsBase.Instance.Player1 = newPlayer; //TODO: ARRAY DE PLAYERS

            Players.Add(newPlayer); //Add to players array

            AddChild(newPlayer, depth);
            newPlayer.Position = initialPosition;
            Console.WriteLine("PLAYER 1 ADDED TO SHIP LAYER");
        }

        public void AddPlayer(CCPlayerBase newPlayer, CCPoint initialPosition)
        {
            AddPlayer(newPlayer, initialPosition, (int)eTAG_DEPTH.PLAYER_LAYER);
        }

        #endregion

        public void CheckEnemyCollition(CCPlayerBase Player1)
        {

            if (Player1 == null)
                return;

            if (!Player1.HasCollision)
            {

                var deletedShoot = new List<CCShootBase>();
                var deletedEnemy = new List<CCNode>();

                bool enemydeleted = false;

                CCEnemyBase[] cacheEnemies = new CCEnemyBase[Enemies.Count];
                Enemies.CopyTo(cacheEnemies);

                CCShootBase[] cacheShoots = new CCShootBase[Shoots.Count];
                Shoots.CopyTo(cacheShoots);

                foreach (CCEnemyBase tmpEnemy in cacheEnemies) //cacheEnemies.Where(s => (typeof(EnemyMeteor) == s.GetType() && (s as EnemyMeteor).CheckCollition) || (typeof(EnemyShip) == s.GetType() && (s as EnemyShip).CheckCollition)))
                {
                    enemydeleted = false;

                    //COMPROBAMOS LOS DISPAROS QUE HAN IMPACTADO CON EL ENEMIGO
                    foreach (var tmpShoot in cacheShoots)
                    {

                        //TODO: MEJORAR Y PONER UN CIRCULO CON RADIO ALTO Y ANCHO en el caso de los asteorides
                        //AÑADIMOS LA RRAY DE ELEMENTOS PARA BOTTAR
                        if (tmpEnemy.BoundingBox.IntersectsRect(tmpShoot.BoundingBox))
                        {

                            deletedShoot.Add(tmpShoot);
                            deletedEnemy.Add(tmpEnemy);
                            enemydeleted = true; //SOLO ES PARA EL ENEMIGO
                        }
                    }

                    if (enemydeleted) //SI HEMOS IMPACTADO EN ALGUN ENEMIGO LO BORRAMOS
                    {

                        //ELIMINAMOS EL DISPARO
                        foreach (var tmpShoot in deletedShoot)
                            RemoveShoot(tmpShoot);// tmpShoot.DeleteElement(); //Eliminamos el disparo que ha impactado con el enemigo

                        if (tmpEnemy.GetType() == typeof(CCEnemyBase))
                        {
                            if ((tmpEnemy as CCEnemyBase).RemoveLife(1))
                            {
                                EnemyKills++;
                                DeleteEnemy(tmpEnemy, true);
                            }
                        }
                        else if (tmpEnemy.GetType() == typeof(CCEnemyBase))
                        {
                            EnemyKills++;
                            DeleteEnemy(tmpEnemy, true);
                        }


                    }
                    else //SI EL ENEMIGO NO HA SIDO DESTRUIDO
                    {

                        //COMPROBAMOS QUE SALGA FUERA DE PANTALLA
                        if (tmpEnemy.PositionX < -(3 + tmpEnemy.ContentSize.Width))
                        {
                            DeleteEnemy(tmpEnemy, false);
                            enemydeleted = true;
                        }

                        //Comprobamos que esté activada la colision 
                        if ((typeof(CCEnemyBase) == tmpEnemy.GetType() && (tmpEnemy as CCEnemyBase).CheckCollition))
                            //ENEMIGO CHOCA CON NAVE
                            if (!enemydeleted && Player1.BoundingBox.IntersectsRect(tmpEnemy.BoundingBox))
                            {

                                //TODO: COLLISION DETECTED EVENT
                                //   CollitionDetected();
                            }

                    }




                }

            }

        }

        public void Reset()
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

        #region OVERWRITE METHODS

        public virtual void OnEnemyShoot(CCEventCustom obj)
        {

        }

        public virtual void OnPlayerShoot(CCEventCustom obj)
        {

        }

        #endregion


        #region METEOR LAYER

        //public void CreateMeteorsOnLayer(CCInfiniteTile layer, int number)
        //{
        //    for (int i = 0; i < number; i++)
        //    {
        //        CreateMeteorsOnLayer(layer);
        //    }
        //}

        //public void CreateMeteorsOnLayer(CCInfiniteTile layer)
        //{
        //    CCPoint point = CCPointExHelper.GetAleatoryPositionBoundingX(this.BoundingBox, 100, Director.WindowSizeInPixels);
        //    point.X += layer.PositionX;

        //    CreateMeteorsOnLayer(layer, point);
        //}

        //public void CreateMeteorsOnLayer(CCInfiniteTile layer, CCPoint point)
        //{
        //    var creador = new EnemyMeteor();

        //    if (point != null)
        //        creador.Position = point;

        //    AddEnemy(creador);
        //}

        #endregion

        #region ENEMY LAYER

        public void AddEnemy(CCEnemyBase enemy)
        {
            lock (Enemies)
            {
                Enemies.Add(enemy);
            }

            AddChild(enemy, 2);
        }


        public int GetEnemyCount()
        {
            return Enemies.Count;
        }

        public void DeleteEnemy(CCEnemyBase enemy, bool sound)
        {
            RemoveChild(enemy);

            if (typeof(CCEnemyBase) == enemy.GetType())
                (enemy as CCEnemyBase).Destroy(sound);

            lock (Enemies)
            {
                Enemies.Remove(enemy);
            }
        }


        //public void CreateEnemyShipOnLayer(CCInfiniteTile tile, int number)
        //{
        //    for (int i = 0; i < number; i++)
        //        CreateEnemyShipOnLayer(tile);
        //}

        //public void CreateEnemyShipOnLayer(CCInfiniteTile tile)
        //{
        //    CCPoint Position = CCPointExHelper.GetAleatoryPositionBoundingX(this.BoundingBox, 100, Director.WindowSizeInPixels);
        //    //Position.X += tile.PositionX;
        //    CreateEnemyShipOnLayer(tile, Position);
        //}

        public void AddEnemyShipOnRandomPositionLayer(CCInfiniteTile tile, CCEnemyBase enemy)
        {
            enemy.Position = CCPointExHelper.GetAleatoryPositionBoundingX(BoundingBox, 100, Director.WindowSizeInPixels);
            AddEnemyShipOnLayer(tile, enemy);
        }

        /// <summary>
        /// Adds and positions aleatory a enemy on a tile
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="enemy"></param>
        public void AddEnemyShipOnLayer(CCInfiniteTile tile, CCEnemyBase enemy)
        {
            enemy.PositionX = tile.PositionX + enemy.PositionX;
            AddEnemy(enemy);
        }

        #endregion

        #region SHOOT LAYER

        private void CreateShoot(CCShootBase m)
        {
            lock (Shoots)
            {
                Shoots.Add(m);
            }
            AddChild(m);
        }

        private void CreateEnemyShoot(CCShootBase m)
        {
            lock (EnemyShoots)
            {
                EnemyShoots.Add(m);
            }
            AddChild(m);
        }

        public void RemoveEnemyShoot(CCShootBase m)
        {
            lock (EnemyShoots)
            {
                EnemyShoots.Remove(m);
            }
            RemoveChild(m);
        }

        public void RemoveShoot(CCShootBase m)
        {
            lock (Shoots)
            {
                Shoots.Remove(m);
            }
            RemoveChild(m);
        }

        #endregion


        protected override void Draw()
        {
            base.Draw();
            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawRect(new CCRect(0, 0, ContentSize.Width, ContentSize.Height), CCColor4B.Red);
            CCDrawingPrimitives.End();
        }

        #region GET DATA INFORMATION

        public string GetInfoPlayerHealth(CCPlayerSettings settings)
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

        #endregion

        #region SHOOT EVENTS

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

        public void PlayerNextFire(CCPlayerBase Player1)
        {
            Player1.NextFire();
        }

        public void PlayerFire(CCPlayerBase Player1)
        {
            Player1.Fire();
        }

        //public void Shoot(ShootType shootType, EnemyShip enemy)
        //{
        //    Shoot(shootType, enemy.Position, false);
        //}

        //public void Shoot(ShootType shootType, Player player)
        //{
        //    Shoot(shootType, player.Position, true);
        //}

        #endregion




        //public void SetPlayerPosition(SneakyPanelControl controller, float dt)
        //{
        //    Player1.Position = controller.GetPlayerPosition(dt);
        //}

        //public void SetPlayerController(SneakyPanelControl controller)
        //{
        //    controller.SetPlayer(Player1);
        //}

        public override void OnExit()
        {
            base.OnExit();



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