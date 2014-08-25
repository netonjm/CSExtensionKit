using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit
{

    public class CCShootBase : CCNode
    {

        public static int VELOCITY = 3;
        public static int DELETED_MARGIN_PIXELS = 530;

        public bool IsDeleted = false;
        public bool IsDisapearing = false;

        public bool IsPlayer = true;



        public CCShootBase(CCPoint position)
        {

        }


        public override void Update(float dt)
        {

            base.Update(dt);

            if (CCGameSettingsBase.Instance.GameIsFinished)
                return;

            if (IsPlayer)
                StepPlayer();
            else
                StepEnemy();

        }

        public void StepPlayer()
        {

            //PositionX += (Player.PLAYER_SPEED + VELOCITY);

            //if (PositionX > Director.WindowSizeInPixels.Width + DELETED_MARGIN_PIXELS)
            //    if (TotalParticles > 0)
            //        TotalParticles--;
            //    else
            //        DeleteElement();
        }

        public void StepEnemy()
        {
            //PositionX -= (CCPlayer.PLAYER_SPEED + VELOCITY);
            //if (PositionX < 0 - DELETED_MARGIN_PIXELS)
            //    if (TotalParticles > 0)
            //        TotalParticles--;
            //    else
            //        DeleteElement();
        }

        public void DeleteElement()
        {
            //if (Deleted != null)
            //    Deleted(this);
        }

        protected override void Draw()
        {
            base.Draw();
            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawRect(new CCRect(0, 0, ContentSize.Width, ContentSize.Height), CCColor4B.Red);
            CCDrawingPrimitives.End();
        }

    }
}
