using System;
using Microsoft.Xna.Framework;
using CocosSharp;
using CSExtensionKit.GameLayers;
using CocosDenshion;
using System.Linq;
namespace RoleGameExample
{
    public class StartLevelLayer : CCRoleGameLayer
    {
        public int MapWith
        {
            get
            {
                return (int)(_tileMap.MapSize.Width * _tileMap.TileSize.Width - Director.WindowSizeInPixels.Width / 2);
                //  return 700 / 2;
            }
        }
        public int MapHeight
        {
            get
            {
                return (int)((_tileMap.MapSize.Height * _tileMap.TileSize.Height) - Director.WindowSizeInPixels.Height / 2);
            }
        }

        CCTMXTiledMap _tileMap;
        CCTMXLayer _background;
        CCTMXLayer _foreground;

        CCTMXLayer _meta;

        CCSprite _player;

        int _numCollected;

        CCEventListenerTouchAllAtOnce tListener;

        public StartLevelLayer()
        {
        }



        protected override void RunningOnNewWindow(CCSize windowSize)
        {

            base.RunningOnNewWindow(windowSize);

            InitializeAudio();

            InitializeTileMap();

            InitializePlayer();

            tListener = new CCEventListenerTouchAllAtOnce();
            tListener.OnTouchesBegan = OnTouchesBegan;
            tListener.OnTouchesMoved = OnTouchesMoved;
            tListener.OnTouchesCancelled = OnTouchesCancelled;
            tListener.OnTouchesEnded = OnTouchesEnded;
            EventDispatcher.AddEventListener(tListener, this);

        }

        private void OnTouchesEnded(System.Collections.Generic.List<CCTouch> touches, CCEvent e)
        {
            controlPanelLayer.OnTouchesEnded(touches, e);


            CCPoint touchLocation = touches.FirstOrDefault().LocationInView;
            touchLocation = Director.ConvertToGl(touchLocation);
            touchLocation = ConvertToNodeSpace(touchLocation);

            CCPoint playerPos = _player.Position;
            CCPoint diff = touchLocation - playerPos; //  ccpSub(touchLocation, playerPos);

            if (Math.Abs(diff.X) > Math.Abs(diff.Y))
            {
                if (diff.X > 0)
                {
                    playerPos.X += _tileMap.TileSize.Width;
                }
                else
                {
                    playerPos.X -= _tileMap.TileSize.Width;
                }
            }
            else
            {
                if (diff.Y > 0)
                {
                    playerPos.Y += _tileMap.TileSize.Height;
                }
                else
                {
                    playerPos.Y -= _tileMap.TileSize.Height;
                }
            }

            // safety check on the bounds of the map
            if (playerPos.X <= (_tileMap.MapSize.Width * _tileMap.TileSize.Width) &&
                playerPos.Y <= (_tileMap.MapSize.Height * _tileMap.TileSize.Height) &&
                playerPos.Y >= 0 &&
                playerPos.X >= 0)
            {
                //this->setPlayerPosition(playerPos);
                setPlayerPosition(playerPos);
            }

            SetViewPointCenter(_player.Position);

        }

        private void OnTouchesCancelled(System.Collections.Generic.List<CCTouch> arg1, CCEvent arg2)
        {
            controlPanelLayer.OnTouchesCancelled(arg1, arg2);

        }

        private void OnTouchesMoved(System.Collections.Generic.List<CCTouch> arg1, CCEvent arg2)
        {
            controlPanelLayer.OnTouchesMoved(arg1, arg2);
        }

        private void OnTouchesBegan(System.Collections.Generic.List<CCTouch> arg1, CCEvent arg2)
        {
            controlPanelLayer.OnTouchesBegan(arg1, arg2);
        }

        public override void OnExit()
        {
            base.OnExit();

            EventDispatcher.RemoveEventListener(tListener);

        }

        private void InitializePlayer()
        {
            _player = new CCSprite("Player.png");
            _player.Position = GetTilePosition(_tileMap, "SpawnPoint");
            AddChild(_player);
            SetViewPointCenter(_player.Position);
        }


        private void InitializeTileMap()
        {
            _tileMap = new CCTMXTiledMap("TileMap.tmx");
            _background = _tileMap.LayerNamed("Background");
            _foreground = _tileMap.LayerNamed("Foreground");

            _meta = _tileMap.LayerNamed("Meta");

            AddChild(_tileMap, -5);

            CCTMXObjectGroup objectGroup = _tileMap.ObjectGroupNamed("Objects");
            if (objectGroup == null)
            {
                CCLog("tile map has no objects object layer");
            }

            var spawnPoint = objectGroup.ObjectNamed("SpawnPoint");

            int x = int.Parse(spawnPoint["x"]);//.intValue();
            int y = int.Parse(spawnPoint["y"]); //spawnPoint->valueForKey("y")).intValue();

        }

        private void InitializeAudio()
        {
            CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic("snd/TileMap");
            CCSimpleAudioEngine.SharedEngine.PreloadEffect("snd/hit");
            CCSimpleAudioEngine.SharedEngine.PreloadEffect("snd/move");
            CCSimpleAudioEngine.SharedEngine.PreloadEffect("snd/pickup");
            CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic("snd/TileMap");
        }



        public void SetViewPointCenter(CCPoint position)
        {

            CCSize winSize = Director.WindowSizeInPixels;
            int x = (int)Math.Max(position.X, winSize.Width / 2f);
            int y = (int)Math.Max(position.Y, winSize.Height / 2f);

            x = (int)Math.Min(x, MapWith);
            y = (int)Math.Min(y, MapHeight);
            CCPoint actualPosition = new CCPoint(x, y);

            CCPoint centerOfView = new CCPoint(winSize.Width / 2, winSize.Height / 2);
            Position = centerOfView - actualPosition;
        }

        #region STATIC METHODS
        public static CCPoint GetTilePosition(CCTMXTiledMap tileMap, string tileName)
        {

            CCTMXObjectGroup objectGroup = tileMap.ObjectGroupNamed("Objects");
            if (objectGroup == null)
            {
                CCLog("tile map has no objects object layer");
                return CCPoint.Zero;
            }

            var spawnPoint = objectGroup.ObjectNamed(tileName);
            string x;
            spawnPoint.TryGetValue("x", out x);
            string y;
            spawnPoint.TryGetValue("y", out y);

            return new CCPoint((float)Convert.ToDouble(x), (float)Convert.ToDouble(y));
        }


        CCPoint tileCoordForPosition(CCPoint position)
        {
            int x = (int)(position.X / _tileMap.TileSize.Width);
            int y = (int)(((_tileMap.MapSize.Height * _tileMap.TileSize.Height) - position.Y) / _tileMap.TileSize.Height);
            return new CCPoint(x, y);
        }

        private static void CCLog(string p)
        {
            // throw new NotImplementedException();
        }


        void setPlayerPosition(CCPoint position)
        {
            CCPoint tileCoord = tileCoordForPosition(position);
            uint tileGid = _meta.TileGIDAt(tileCoord);

         
            if (tileGid != 0)
            {

                var properties = _tileMap.PropertiesForGID(tileGid);
                if (properties != null)
                {
                    // CCString collision = new CCString();
                    string collision;
                    if (!properties.TryGetValue("Collidable",out collision))
                        return;

                    if (collision.CompareTo("True") == 0)
                    {
                        CCSimpleAudioEngine.SharedEngine.PlayEffect("snd/hit");
                        return;
                    }

                    //CCString *collectible = new CCString();
                    string collectible;
                    if (!properties.TryGetValue("Collectable",out collectible))
                        return;

                    if (collectible.CompareTo("True") == 0)
                    {
                        _meta.RemoveTileAt(tileCoord);
                        _foreground.RemoveTileAt(tileCoord);
                        _numCollected++;

                        ((InformationPanelLayer)InformationLayer).SetText(_numCollected.ToString());
                        CCSimpleAudioEngine.SharedEngine.PlayEffect("snd/pickup");

                    }
                }
            }
            _player.Position = position;
            CCSimpleAudioEngine.SharedEngine.PlayEffect("snd/move");


        }


        #endregion

        public static CCScene Scene
        {
            get
            {
                // 'scene' is an autorelease object.
                var scene = new CCScene();
                //add information layer
                var informationLayer = new InformationPanelLayer("");
                scene.AddChild(informationLayer, (int)eTAG_DEPTH.INFORMATION_LAYER);

                var firstLevel = new StartLevelLayer();
                scene.AddChild(firstLevel, (int)eTAG_DEPTH.GAME_LAYER_TAG);

                //Add information layer
                firstLevel.InformationLayer = informationLayer;

                //Finished Loaded
                firstLevel.OnFinishedLoad();

                // return the scene
                return scene;

            }

        }

    }
}

