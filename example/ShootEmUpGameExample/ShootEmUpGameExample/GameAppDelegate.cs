using System;
using CocosSharp;


namespace ShootEmUpGameExample.Mac
{
	public class GameAppDelegate : CCApplicationDelegate
	{

		public static CCWindow Window;

		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{

			Window = mainWindow;

			//application.AllowUserResizing = true;
			application.PreferMultiSampling = false;
			application.ContentRootDirectory = "Content";
			application.ContentSearchPaths.Add ("animations");
			application.ContentSearchPaths.Add ("fonts");
			application.ContentSearchPaths.Add ("images");
			application.ContentSearchPaths.Add ("sounds");

			CCSize windowSize = mainWindow.WindowSizeInPixels;
			mainWindow.SetDesignResolutionSize (windowSize.Width, windowSize.Height, CCSceneResolutionPolicy.ShowAll);

			CCScene scene = StartLevelLayer.Scene;

			mainWindow.RunWithScene (scene);
		}

		public override void ApplicationDidEnterBackground (CCApplication application)
		{
			application.Paused = true;
		}

		public override void ApplicationWillEnterForeground (CCApplication application)
		{
			application.Paused = false;
		}
	}
}
