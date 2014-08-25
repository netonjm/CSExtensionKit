using System;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace PuzzleGameExample
{
    public class IntroLayer : CCLayerColor
    {
        public IntroLayer()
        {

            // create and initialize a Label
            var label = new CCLabelTtf("Hello Cocos2D-XNA", "MarkerFelt", 22);

            // position the label on the center of the screen
            label.Position = Director.WindowSizeInPixels.Center;

            // add the label as a child to this Layer
            AddChild(label);

            // setup our color for the background
            Color = CCColor3B.Blue;
            Opacity = 255;

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

