using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CSExtensionKit.Helpers
{
    public class CCLabelFontHelper
    {

        public static CCLabelTtf GetFont(string text)
        {
            return new CCLabelTtf(text, "MarkerFelt", 22)
            {
                HorizontalAlignment = CCTextAlignment.Right
            };
        }

    }
}
