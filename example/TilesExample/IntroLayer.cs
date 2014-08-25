using System;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace TilesExample
{
    public class IntroLayer : CCLayerColor
    {

        CCTMXTiledMap tileMap;
        CCTMXLayer background;
        CCSprite player;

        public IntroLayer()
        {

            tileMap = new CCTMXTiledMap("TileMap.tmx");
            background = tileMap.LayerNamed("Background");
            AddChild(background);
        }

        public static CCScene Scene
        {
            get
            {
                // 'scene' is an autorelease object.
                var scene = new CCScene();

                // 'layer' is an autorelease object.
                var layer = new IntroLayer();

                // add layer as a child to scene
                scene.AddChild(layer);

                // return the scene
                return scene;

            }

        }

    }
}

