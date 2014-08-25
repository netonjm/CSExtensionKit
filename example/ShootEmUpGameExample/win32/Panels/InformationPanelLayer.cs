using CocosSharp;
using CSExtensionKit;
using CSExtensionKit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShootEmUpGameExample
{
    public class InformationPanelLayer : CCInformationPanelBase
    {

        CCLabelTtf label;

        public InformationPanelLayer(string initialText)
        {
            ContentSize = Director.WindowSizeInPixels;
            InitializeLabel(initialText);
        }

        public void InitializeLabel(string initialText)
        {
            label = CCLabelFontHelper.GetFont(initialText);
            label.Position = new CCPoint(Director.WindowSizeInPixels.Width / 2, Director.WindowSizeInPixels.Height - (Director.WindowSizeInPixels.Height / 8));
            // add the label as a child to this Layer
            AddChild(label); // --> TOP
        }
        public void SetText(string text)
        {
            label.Text = text;
        }

    }
}
