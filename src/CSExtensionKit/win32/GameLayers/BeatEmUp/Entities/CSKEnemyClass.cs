using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{
    public class CSKEnemyClass : CSKEntityClass
    {

        public float SPEED_MODIFICATION = .5f;
        public DateTime nextDecisionTime;

        public EnemyDesiredActionState IADesiredActionState;
        public EnemyPersonalityState IAPersonalityState;

        public string IA_INFO = "SEARCHING TARGET";
        public CCPoint IATarget;

        public bool IsIA = false;

        public float DEFAULT_SHOOT_DELAY = 0.8f;
        public int ID { get; set; }

        public CSKEnemyClass()
        {
            ShootDelay = new CCDelayTimeEx(DEFAULT_SHOOT_DELAY, "ShootDelayCount");
        }

    }
}
