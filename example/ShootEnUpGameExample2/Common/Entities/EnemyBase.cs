using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalSlugGameExample
{
    class EnemyBase : CCNode
    {
        public int type = 0;
        public bool isActive;
        public bool isFiring;

        public EnemyBase()
        {

        }

    }
}
