using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{
    public class CSKEntityClass
    {

        public const int DEFAULT_PLAYER_INITIAL_HEALTH = 100;

        public const int DEFAULT_PLAYER_INITIAL_KEY_SPEED = 50;
        public const int DEFAULT_PLAYER_INITIAL_SPEED = 5;
        public const int DEFAULT_PLAYER_INITIAL_POWER = 0;
        public const int DEFAULT_PLAYER_SHOOT_DELAY = 30;

        public int PLAYER_SPEED { get; set; }
        public int PLAYER_KEY_SPEED { get; set; }
        public int HEALTH { get; set; }
        public int POWER { get; set; }
        public int MAX_HEALTH { get; set; }

        public int VELOCITY;

        public ActionState actionState;
        public CCDelayTimeEx ShootDelay;

        public bool CheckCollition = false;


        public CSKEntityClass()
            : this(DEFAULT_PLAYER_INITIAL_SPEED, DEFAULT_PLAYER_INITIAL_HEALTH, DEFAULT_PLAYER_SHOOT_DELAY)
        {

        }

        public CSKEntityClass(int velocity, int life, float shoot_delay)
        {
            ShootDelay = new CCDelayTimeEx(shoot_delay, "SHOOT_DELAY");
        }

        public CSKEntityClass(int velocity, int life)
            : this(velocity, life, DEFAULT_PLAYER_SHOOT_DELAY)
        {

        }

    }
}
