using System;
using CocosSharp;


namespace BeatEmUpGameExample.Mac
{
	public class GameAppDelegate : CCApplicationDelegate
	{
		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{
			//application.AllowUserResizing = true;
			application.PreferMultiSampling = false;
			application.ContentRootDirectory = "Content";
			application.ContentSearchPaths.Add ("animations");
			application.ContentSearchPaths.Add ("fonts");
			application.ContentSearchPaths.Add ("images");
			application.ContentSearchPaths.Add ("sounds");

			CCSize windowSize = new CCSize (600, 320);
			mainWindow.SetDesignResolutionSize (windowSize.Width, windowSize.Height, CCSceneResolutionPolicy.FixedHeight);

			CCScene scene = new CCScene (mainWindow);
			StartLevelLayer gameLayer = new StartLevelLayer ();
			scene.AddChild (gameLayer);

			mainWindow.RunWithScene (scene);
			//Hack to see all objects in right position ¿? 
			scene.Position = -windowSize.Center;

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
