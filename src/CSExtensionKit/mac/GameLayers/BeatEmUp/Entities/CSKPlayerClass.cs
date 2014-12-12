using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{

    public class CSKPlayerClass : CSKEntityClass
    {

        public CSKPlayerClass()
            : base()
        {

        }

        public CSKPlayerClass(int velocity, int life, float shoot_delay)
            : base(velocity, life, DEFAULT_PLAYER_SHOOT_DELAY)
        {

        }

        public CSKPlayerClass(int velocity, int life)
            : this(velocity, life, DEFAULT_PLAYER_SHOOT_DELAY)
        {

        }

    }

}
