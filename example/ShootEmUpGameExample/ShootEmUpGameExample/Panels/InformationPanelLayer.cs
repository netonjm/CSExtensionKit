using CocosSharp;
using CSExtensionKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSExtensionKit.Helpers;

namespace ShootEmUpGameExample
{
    public class InformationPanelLayer : CCInformationPanelBase
    {

        CCLabelTtf label;

        public InformationPanelLayer(string initialText)
        {
            ContentSize = Window.WindowSizeInPixels;
            InitializeLabel(initialText);
        }

        public void InitializeLabel(string initialText)
        {
            label = CCLabelFontHelper.GetFont(initialText);
            label.Position = new CCPoint(Window.WindowSizeInPixels.Width / 2, Window.WindowSizeInPixels.Height - (Window.WindowSizeInPixels.Height / 8));
            // add the label as a child to this Layer
            AddChild(label); // --> TOP
        }
        public void SetText(string text)
        {
            label.Text = text;
        }

    }
}
