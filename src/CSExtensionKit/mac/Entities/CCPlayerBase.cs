using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{
    public struct BoundingBox
    {
        public CCRect original;
        public CCRect actual;

        public CCPoint Position
        {
            get
            {
                return actual.Origin;
            }

            set
            {
                actual.Origin = value;
            }

        }
    }

    public class CCPlayerBase : CCSprite
    {

        public bool HasCollision = false;

        public bool IsShooting { get { return !ShootDelay.HasPassed(); } }

        CCDelayTimeEx ShootDelay;

        public int ID { get; set; }

        public CCPlayerBase(int id, string image)
            : base(image)
        {

            ShootDelay = new CCDelayTimeEx(0.8f, "ShootDelayCount");
            ID = id;
        }

        public void ResetFireTimeout()
        {
            ShootDelay.Update();
        }

        public virtual void Reset()
        {
            // throw new NotImplementedException();
        }

        public virtual void NextFire()
        {
            // throw new NotImplementedException();
        }

        public virtual void Fire()
        {
            // throw new NotImplementedException();
        }


    }

}
