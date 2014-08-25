using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using CocosSharp;
using CSExtensionKit.GameLayers;

namespace MetalSlugGameExample
{
    class IntroLayer : CCBeatEmUpGameLayer
    {

        static float GAMESCALE = 1.4f;

        static float[] mapShape = new float[4200];

        static float[] enemysPos = new float[] 
        {
            500, 510, 525, 660, 920, 1030, 1090, 1180, 1315, 1380,
            1570, 1590, 1900, 2150, 2285, 2420, 2610, 2770, 2870, 2970
        };

        // returns a CCScene that contains the HelloWorldLayer as the only child
        public CCLayer _gameBG { get; set; }
        public Hero hero;
        public Truck truck;

        int indextruck;
        float accellY;

        CCSpriteFrame bulletFrame;

        float mapOffset;
        List<EnemyBase> enemys;

        float totalWith;
        int totalHeight;

        public void LoadMap(int index)
        {
            totalWith = 0;
            float xoffset = 0f;
            for (int i = 347; i < 358; i++)
            {
                if (i == 349 || i == 371)
                    i++;
                else if (i == 361)
                    i += 2;

                CCSprite mapcell = new CCSprite(String.Format("Background/Image {0} at frame 10", i));
                mapcell.Scale = GAMESCALE;
                mapcell.Tag = 11169;
                CCSize sizecell = mapcell.Texture.ContentSizeInPixels;
                xoffset += sizecell.Width / 2 * GAMESCALE;
                float y = size.Height / 2;
                if (sizecell.Height != 214.5)
                {
                    y = size.Height / 2f + (sizecell.Height - 214.5f) / 2f * GAMESCALE;
                    if (index == 4)
                    {
                        y += 50 * GAMESCALE;
                    }
                }

                mapcell.Position = new CCPoint(xoffset, 214);
                mapcell.Tag = index;
                index++;

                mapcell.AnchorPoint = new CCPoint(0.5f, 0);

                _gameBG.AddChild(mapcell, 0);

                xoffset += sizecell.Width / 2 * GAMESCALE;
            }

            totalWith = xoffset;

        }

        CCSize size { get { return Director.WindowSizeInPixels; } }

        public void LoadSprites()
        {
            //Cargamos los plist que utilicemos del nivel
            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("hero.plist");
            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("soldier.plist");
            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("truck.plist");
            CCApplication.SharedApplication.SpriteFrameCache.AddSpriteFrames("tank.plist");
        }

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);
        }

        public IntroLayer()
        {

          
            LoadSprites();

            int index = 0;

            _gameBG = new CCLayer();

            //Cargamos el mapa
            LoadMap(index);

            //Creación de enemigos
            enemys = new List<EnemyBase>();

            for (int i = 0; i < 2; i++)
            {
                Enemy enemy = new Enemy();
                enemys.Add(enemy);
                enemy.ScaleX = ScaleY = GAMESCALE;
                enemy.Position = new CCPoint(enemysPos[i], mapShape[(int)enemysPos[i]]);
                enemy.Tag = i + 10;
                _gameBG.AddChild(enemy);
            }

            Tank tank1 = new Tank();
            tank1.Position = new CCPoint(360, mapShape[3600]);
            tank1.Scale = GAMESCALE;
            _gameBG.AddChild(tank1);
            enemys.Add(tank1);

            //Tank tank2 = new Tank();
            //tank2.Position = new CCPoint(402, mapShape[4020]);
            //_gameBG.AddChild(tank2);
            //enemys.Add(tank2);
            //3600, 4020;

            hero = new Hero();
            hero.Position = new CCPoint(120, 120);
            hero.Opacity = 100;
            hero.Scale = GAMESCALE;
            _gameBG.AddChild(hero, 1);

            truck = new Truck();
            truck.Scale = GAMESCALE;
            truck.Position = new CCPoint(650, 370);
            _gameBG.AddChild(truck, 2);

            //CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic
            //[[SimpleAudioEngine sharedEngine] preloadEffect:@"Sound 1 at frame 10.mp3"];
            //[[SimpleAudioEngine sharedEngine] preloadBackgroundMusic:@"Sound 13 at frame 10.mp3"];
            //[[SimpleAudioEngine sharedEngine] preloadEffect:@"Sound 20 at frame 10.mp3"];
            //[[SimpleAudioEngine sharedEngine] preloadEffect:@"Sound 21 at frame 10.mp3"];
            //[[SimpleAudioEngine sharedEngine] preloadEffect:@"Sound 32 at frame 10.mp3"];
            //GameTouches = [[NSMutableSet alloc]init];

            bulletFrame = new CCSpriteFrame(CCApplication.SharedApplication.TextureCache.AddImage("Image 779 at frame 10.png"), new CCRect(0, 0, 11.5f, 4.5f)); //[CCSpriteFrame frameWithTextureFilename:@"Image 779 at frame 10.png" rect:CGRectMake(0, 0, 11.5, 4.5)];

            ScaleX = ScaleY = 1.7f;
            
        }

        public override void OnFinishedLoad()
        {
            base.OnFinishedLoad();

            if (controlPanelLayer != null)
                controlPanelLayer.SetPlayer(hero);

            kills = new CCLabel("", "MarkerFelt", 22);
            kills.Position = new CCPoint(Director.WindowSizeInPixels.Width * .5f, Director.WindowSizeInPixels.Height * .95f);
            InformationLayer.AddChild(kills);

            life = new CCLabel("", "MarkerFelt", 22);
            life.Position = hero.Position + new CCPoint(0, 100); //new CCPoint( .WindowSizeInPixels.Width * .5f, Director.WindowSizeInPixels.Height * .95f);
            AddChild(life);

           // controlPanelLayer.ScaleX = controlPanelLayer.ScaleY = .7f;

            controlPanelLayer.JoyControl.Position = new CCPoint(-10, -10);

            //SetViewPointCenter(hero.Position);

        }

        public override void Update(float dt)
        {
            base.Update(dt);

            CCPoint p = hero.Position;

            Console.WriteLine("Posicion:{0}, {1}", p.X, p.Y);

            hero.Update(dt);

            ajustmap(p);

            //DSPanel.Update(dt);

            if (accellY > 0.03)
            {
                hero.ScaleX = -GAMESCALE;
                if (accellY > 0.08)
                {
                    if (p.X < mapOffset + 465 && mapShape[(int)p.X + 5] - p.Y < 20)
                    {
                        hero.Position = new CCPoint(p.X + 3, p.Y);
                    }
                    //hero.IsRunning = true;
                    hero.run();
                }
            }
            else if (accellY < -0.03)
            {
                hero.ScaleX = GAMESCALE;
                if (accellY < -0.08)
                {
                    if (p.X > mapOffset + 15 && mapShape[(int)p.X - 5] - p.Y < 20)
                    {
                        hero.Position = new CCPoint(p.X - 3, p.Y);
                    }
                    hero.run();
                }
            }
            if (accellY > -0.08 && accellY < 0.08)
            {
                hero.stop();
            }

            float offsetx = 0;
            p = hero.Position;
            if (p.X - mapOffset > 120 && p.X < 3830)
            {
                offsetx = p.X - mapOffset - 120;

                _gameBG.Position = new CCPoint(_gameBG.Position.X - offsetx, _gameBG.Position.Y);
                mapOffset += offsetx;
            }

            p = _gameBG.Position;


            hero.Position = controlPanelLayer.GetPlayerPosition(dt, new CCSize(totalWith, 429));


            foreach (CCNode sprite in Children)
            {
                if (sprite.Tag == 851137)
                {
                    CCPoint ps = sprite.Position;
                    if (ps.Y + 15 < mapShape[(int)(ps.X - p.X)] + p.Y)
                    {
                        sprite.StopAllActions();
                        sprite.RemoveFromParent(true);
                    }
                    else if (sprite.Position.X < 0 || sprite.Position.X > 480)
                    {
                        sprite.StopAllActions();
                        sprite.RemoveFromParent(true);
                    }
                    else
                    {
                        //for ()
                    }
                }
            }

            foreach (EnemyBase enemy in enemys)
            {
                float absluteX = enemy.Position.X + p.X;
                if (absluteX < -20)
                {
                    enemy.Tag = -1;
                    enemy.RemoveFromParent(true);
                }
                if (absluteX > 0 && absluteX < 500)
                {
                    //[enemy active];
                    enemy.isActive = true;
                }
            }

        }

        public void ajustmap(CCPoint p)
        {

            if (!hero.isJumping)
            {
                p.Y = mapShape[(int)p.X];
            }
            if (p.Y > 180)
            {
                _gameBG.Position = new CCPoint(_gameBG.Position.X, 90 - p.Y + 90);
            }
            else if (p.Y > 90)
            {
                float y = 0;
                if (p.Y + _gameBG.Position.Y < 90)
                {
                    _gameBG.Position = new CCPoint(_gameBG.Position.X, 0);
                }
            }
        }

        public static CCScene Scene
        {
            get
            {

                if (mapShape[0] == 0)
                {
                    for (int i = 0; i < 4200; i++)
                    {
                        if (i > 50 && i < 120)
                            mapShape[i] = 90.0f + Math.Abs(70 - i) / 2.5f;
                        else if (i > 550 && i < 660)
                            mapShape[i] = (90 + (34));
                        else if (i > 1225 && i < 1296)
                            mapShape[i] = (90 + 64);
                        else if (i > 1295 && i < 1350)
                            mapShape[i] = (90 + 130 + i - 1320);
                        else if (i > 1350 && i < 1701)
                            mapShape[i] = (90 + 160);
                        else if (i > 1700 && i < 1761)
                            mapShape[i] = (90 + 200);
                        else if (i > 1760 && i < 1851)
                            mapShape[i] = (90 + 240);
                        else if (i > 1850 && i < 1951)
                            mapShape[i] = (90 + 300.0f + (i - 1910) / 2.0f);
                        else if (i > 1950 && i < 2191)
                            mapShape[i] = (460 + (i - 2075) / 15.0f);
                        else if (i > 2190 && i < 2271)
                            mapShape[i] = (470 + 50.0f);
                        else if (i > 2270 && i < 2501)
                            mapShape[i] = (450 + (2345 - i) / 4.0f);
                        else if (i > 2500 && i < 3221)
                            mapShape[i] = (414 + (2500 - i) / 2.222222f);
                        else
                            mapShape[i] = 90;
                    }
                }

                // 'scene' is an autorelease object.
                var scene = new CCScene();

                var informationLayer = new InformationPanelLayer("");
                scene.AddChild(informationLayer, (int)eTAG_DEPTH.INFORMATION_LAYER);

                // 'layer' is an autorelease object.
                var layer = new IntroLayer();

                layer.IsKeyboardEnabled = true;
                layer.IsTouchEnabled = true;

                layer.Tag = 2;
                layer._gameBG.Tag = 3;

                // add layer as a child to scene
                scene.AddChild(layer, 1);
                scene.AddChild(layer._gameBG, 0);

                layer.InformationLayer = informationLayer;

                //Finished Loaded
                layer.OnFinishedLoad();

                // return the scene
                return scene;

            }

        }


        public CCLabel kills { get; set; }

        public CCLabel life { get; set; }
    }
}

