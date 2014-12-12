using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSExtensionKit.GameLayers
{
    public class CCIARoleBodyBase : CCIASystemBase
    {


        public void ActiveIA(CCEnemyBase enemy, CCPlayerBase player)
        {
            enemy.IsIA = true;
            enemy.IADesiredActionState = EnemyDesiredActionState.SearchingTarget;
            enemy.IAPersonalityState = EnemyPersonalityState.Attacking; //(EnemyPersonalityState)CCRandom.Next(1, 3);
            enemy.IATarget = new CCPoint(enemy.PositionX, (int)CCRandom.GetRandomFloat(player.PositionY - 50f, player.PositionY + 50f));

        }

        public void UpdateShipIA(CCEnemyBase enemy, CCPlayerBase player, CCSize wSize, int PLAYER_SPEED)
        {

            if (enemy.IsIA)
            {

                enemy.PositionX -= (PLAYER_SPEED + enemy.VELOCITY + enemy.VELOCITY + CCRandom.Next(1, 3));

                //Esta dentro de su rango;
                if (enemy.IADesiredActionState == EnemyDesiredActionState.Firing)
                {
                    if (ACTUAL_DELAY <= DELAY_SHOOT)
                        ACTUAL_DELAY++;
                    else
                    {
                        enemy.IADesiredActionState = EnemyDesiredActionState.SearchingTarget;
                        ACTUAL_DELAY = 0;
                    }
                }

                if (enemy.IAPersonalityState == EnemyPersonalityState.Suicide)
                {
                    if (enemy.PositionY > player.PositionY)
                        enemy.PositionY -= CCRandom.Next(1, 3);
                    else if (enemy.PositionY < player.PositionY)
                        enemy.PositionY += CCRandom.Next(1, 3);
                }
                else if (enemy.IAPersonalityState == EnemyPersonalityState.Attacking)
                {

                    //Si la posicion 
                    if (enemy.IATarget.Y == enemy.Position.Y)
                        enemy.IATarget.Y = (int)CCRandom.GetRandomFloat(player.PositionY - 50f, player.PositionY + 50f);

                    if (enemy.PositionY > enemy.IATarget.Y)
                        enemy.PositionY -= CCRandom.Next(1, 3);
                    else if (enemy.PositionY < enemy.IATarget.Y)
                        enemy.PositionY += CCRandom.Next(1, 3);

                }

                if (enemy.PositionY > player.PositionY - 30 && enemy.PositionY < player.PositionY + 30 && enemy.PositionX < wSize.Width - 15)
                {

                    CCEventCustom eventC = new CCEventCustom(CCBeatEmUpGameLayer.EVENT_ENEMY_ID);
                    eventC.UserData = enemy;

                }

                if (player.PositionX > enemy.PositionX - 100)
                {
                    if (enemy.IADesiredActionState != EnemyDesiredActionState.OutOfRange)
                        enemy.IADesiredActionState = EnemyDesiredActionState.OutOfRange;
                }
            }


        }

        public string GetShipIAInfo(CCEnemyBase enemy)
        {
            return string.Format("{0}{1}", (enemy.IsIA) ? "IA ENABLED" : "IA DISABLED", enemy.IA_INFO);
        }

    }
}
