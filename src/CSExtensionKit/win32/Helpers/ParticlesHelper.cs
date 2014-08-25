
using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit.Helpers
{
    public class ParticlesHelper
    {
        public static CCParticleRain GetPar(CCPoint position)
        {
            return new CCParticleRain(position)
            {
                BlendFunc = new CCBlendFunc(1, 1),
                //EmitterMode = CCEmitterMode.Gravity,
                EndSize = 60,
                StartSize = 60,
                Gravity = new CCPoint(-500, 0)
            };
        }
        public enum PlayerShipEngineFireType
        {
            Superior = 1, Inferior = 2
        }
        public static CCParticleSun GetPlayerShipEngineFire(PlayerShipEngineFireType tipo)
        {

            var position = new CCPoint((tipo == PlayerShipEngineFireType.Superior) ? 50 : 110, 0);
            var explosion = new CCParticleSun(position, 250);
            explosion.BlendFunc = new CCBlendFunc(770, 772);
            explosion.EmissionRate = 83.3334f;
            //explosion.AutoRemoveOnFinish = true;
            //explosion.StartSize = 15.0f;
            explosion.Speed = 30.0f;
            explosion.AnchorPoint = new CCPoint(0.5f, 0.5f);
            explosion.Gravity = new CCPoint(0, -200);
            explosion.StartSize = 60;
            explosion.Rotation = 270f;
            return explosion;
        }

    }
}
