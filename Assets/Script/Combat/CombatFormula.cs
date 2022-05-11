using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatFormula
    {
        internal void GetNormalDamage(CombatRole attacker, CombatRole defender, bool isCriticalHit, out int outValue)
        {
            outValue = attacker.Role.Attack - defender.Role.Defence;
            outValue = (outValue < 1) ? 1 : outValue;
            if (isCriticalHit)
            {
                outValue *= GameConst.CRITICAL_HIT_DAMAGE_RATIO;
            }
        }
    }
}

