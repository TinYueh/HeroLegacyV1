using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatAI
    {
        internal void GetNextAction(out GameEnum.eRotateDirection outDirection)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                outDirection = GameEnum.eRotateDirection.E_ROTATE_DIRECTION_RIGHT;
            }
            else
            {
                outDirection = GameEnum.eRotateDirection.E_ROTATE_DIRECTION_LEFT;
            }
        }
    }
}
