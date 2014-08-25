using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public class CCSpriteAnimationHelper
    {

        public static CCAnimation GetAnimationFromData(string image, int frames, float delay)
        {
            List<CCSpriteFrame> idleFrames = new List<CCSpriteFrame>(frames);
            for (int i = 0; i < frames + 1; i++)
            {
                CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache[string.Format(image, i.ToString("00"))];
                idleFrames.Add(frame);
            }
            return new CCAnimation(idleFrames, delay);
        }

        public static CCAction GetActionFromData(string image, int frames, float delay)
        {
            CCAnimation idleAnimation = GetAnimationFromData(image, frames, delay);
            return new CCRepeatForever(new CCAnimate(idleAnimation));
        }

        public static CCAction GetActionFunctionFromData(string image, int frames, float delay, Action action)
        {

            CCAnimation idleAnimation = GetAnimationFromData(image, frames, delay);
            return new CCSequence(new CCAnimate(idleAnimation), new CCCallFunc(action));

        }

        public static CCAction GetActionBlinkFromData(string image, int frames, float delay, float actionWithDuration, uint blinks)
        {

            CCAnimation idleAnimation = GetAnimationFromData(image, frames, delay);
            return new CCSequence(new CCAnimate(idleAnimation), new CCBlink(actionWithDuration, blinks));

        }

        public static List<CCSpriteFrame> GetFromCacheFrameFromArrayFrames(string name, params int[] frames)
        {
            List<CCSpriteFrame> walkAnimFrames = new List<CCSpriteFrame>();
            for (int i = 0; i < frames.Length; i++)
            {
                walkAnimFrames.Add(
                 CCSpriteFrameCache.SharedSpriteFrameCache[String.Format(name, frames[i])]
                   );
            }
            return walkAnimFrames;

        }

        public static List<CCSpriteFrame> GetFromCacheFrameFromNumToNum(string name, int from, int to)
        {

            List<CCSpriteFrame> walkAnimFrames = new List<CCSpriteFrame>();
            for (int i = from; i <= to; i++)
            {
                walkAnimFrames.Add(
				  CCSpriteFrameCache.SharedSpriteFrameCache[String.Format(name, i)]
                   );
            }
            return walkAnimFrames;

        }

        public static CCAction GetActionRepeteatForever(List<CCSpriteFrame> frames, float delay = 0.2f)
        {
            //List<CCSpriteFrame> walkAnimFrames = SpriteAnimationHelper.GetFromCacheFrameFromNumToNum("{0}.png", 0, 5);
            var framesAnim = new CCAnimation(frames, delay);
            return new CCRepeatForever(new CCAnimate(framesAnim));
        }

        public static CCAction GetActionRepeteatForever(string name, int from, int to, float delay = 0.2f)
        {
            return GetActionRepeteatForever(CCSpriteAnimationHelper.GetFromCacheFrameFromNumToNum(name, from, to), delay);
        }

        //public static CCAnimation GetAnimation(List<CCSpriteFrame> frames, float delay = 0.2f)
        //{
        //    //List<CCSpriteFrame> walkAnimFrames = SpriteAnimationHelper.GetFromCacheFrameFromNumToNum("{0}.png", 0, 5);
        //    return new CCAnimation(frames, delay);
        //}


        public static CCAction GetAction(List<CCSpriteFrame> frames, float delay = 0.2f)
        {
            //List<CCSpriteFrame> walkAnimFrames = SpriteAnimationHelper.GetFromCacheFrameFromNumToNum("{0}.png", 0, 5);
            var framesAnim = new CCAnimation(frames, delay);
            return new CCAnimate(framesAnim);
        }

        public static CCAction GetAction(string name, int from, int to, float delay = 0.2f)
        {
            return GetAction(CCSpriteAnimationHelper.GetFromCacheFrameFromNumToNum(name, from, to), delay);
        }

    }
}
