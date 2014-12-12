using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public class CCDelayTimeEx
    {
        CCDelayTime eee = new CCDelayTime(3);
        string Name;

        DateTime lastTimePressed;
        float minSecDelay;

        public CCDelayTimeEx(float minSecondDelay, string name)
        {
            Name = name;
            minSecDelay = minSecondDelay;
            lastTimePressed = DateTime.Now;
        }

        public void Update()
        {
            lastTimePressed = DateTime.Now;
        }


        public bool HasPassed()
        {
            var time = DateTime.Now.Subtract(lastTimePressed).TotalSeconds;

            //if (Name == "BlockDownDelay")
            //Console.WriteLine(String.Format("{0} : {1}",time,minSecDelay) );

            return time > minSecDelay;

        }

        public void SetDelay(float delay)
        {
            minSecDelay = delay;
        }


    }
}
