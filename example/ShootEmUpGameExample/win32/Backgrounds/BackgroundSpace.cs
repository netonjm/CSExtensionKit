using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShootEmUpGameExample.Backgrounds
{
    public class BackgroundSpace : CCInfiniteBackground
    {
        public BackgroundSpace(int speed)
            : base("fondos/fondo1", speed)
        {
        }

        public BackgroundSpace()
            : this(4)
        {
        }
    }
}
