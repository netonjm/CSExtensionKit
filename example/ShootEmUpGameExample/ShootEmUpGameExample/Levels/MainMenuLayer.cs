using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosDenshion;
using CocosSharp;
namespace ShootEmUpGameExample
{
    public class MainMenuLayer : CCLayerColor
    {

        public CCSimpleAudioEngine audio;

        public MainMenuLayer()
        {
            //CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic("music/menu");
            //CCSimpleAudioEngine.SharedEngine.PreloadBackgroundMusic("music/level1");

            CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic("music/menu", true);
            //CCSimpleAudioEngine.SharedEngine.PlayEffect("DigitalStream");

            Color = CCColor3B.Black;
            Opacity = 255;

            //TouchEnabled = true;

            //CCEventListenerTouchAllAtOnce eTouch = new CCEventListenerTouchAllAtOnce();
            //eTouch.OnTouchesBegan = TouchesBegan;
            //eTouch.OnTouchesBegan = TouchesBegan;
            //eTouch.OnTouchesBegan = TouchesBegan;
            //eTouch.OnTouchesBegan = TouchesBegan;
            //EventDispatcher.AddEventListener(eTouch);


            //CCMenuItemFont tt2 = new CCMenuItemFont("Caracola de chan");
            //tt2.Color = new CCColor3B(Microsoft.Xna.Framework.Color.Blue);
            //tt.SetPosition(200, 300);

            CCMenuItem tt2 = new CCMenuItemImage("menu/nuevo", "menu/nuevo_off", Seleccionado);
            tt2.Scale = 0.7f;

            CCMenuItem tt3 = new CCMenuItemImage("menu/salir", "menu/salir_off", Salir);
            tt3.Scale = 0.7f;

            CCMenu t = new CCMenu(new CCMenuItem[] { tt2 });
            t.Color = CCColor3B.Black;
			t.Position = new CCPoint(Window.WindowSizeInPixels.Width / 2, Window.WindowSizeInPixels.Height / 2);

            AddChild(t);



        }

        public void Seleccionado(object t)
        {

            CCSimpleAudioEngine.SharedEngine.StopAllEffects();
            CCSimpleAudioEngine.SharedEngine.End();

            CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic("music/level1", true);

            Director.ReplaceScene(StartLevelLayer.Scene);

        }

        public void Salir(object t)
        {

        }


        public static CCScene Scene
        {
            get
            {

				var _scene = new CCScene(ShootEmUpGameExample.Mac.GameAppDelegate.Window) ;

                var firstLevel = new MainMenuLayer();

                _scene.AddChild(firstLevel, 0);

                return _scene;

            }

        }
    }
}
