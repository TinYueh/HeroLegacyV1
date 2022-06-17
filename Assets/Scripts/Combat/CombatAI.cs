using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatAI
    {
        internal void GetRoundAction(out GameEnum.eCombatRoundAction outAction)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                outAction = GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_RIGHT;
            }
            else
            {
                outAction = GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_LEFT;
            }
        }
    }
}
