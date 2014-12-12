
using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit.Helpers
{
    public class TextureHelper
    {

        public static void ChangeTexture(ref CCSprite sprite, string image)
        {

            sprite.Texture = new CCTexture2D();
            sprite.Texture = CCTextureCache.SharedTextureCache.AddImage(image);
			CCTextureCache.SharedTextureCache.RemoveUnusedTextures();

        }

    }
}
