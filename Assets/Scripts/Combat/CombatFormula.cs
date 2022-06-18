using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatFormula
    {
        internal GameEnum.eCombatAttributeMatchResult CheckAttributeMatch(GameEnum.eRoleAttribute player, GameEnum.eRoleAttribute opponent)
        {
            if (player == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
            {
                if (opponent == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
                {
                    return GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_WIN;
                }
                else if (opponent == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
                {
                    return GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_LOSE;
                }
            }
            else if (player == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
            {
                if (opponent == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
                {
                    return GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_LOSE;
                }
                else if (opponent == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
                {
                    return GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_WIN;
                }
            }
            else if (player == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
            {
                if (opponent == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
                {
                    return GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_WIN;
                }
                else if (opponent == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
                {
                    return GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_LOSE;
                }
            }

            return GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_DRAW;
        }

        internal void GetNormalAttackDamage(CombatRole source, CombatRole target, bool isCriticalHit, out int outValue)
        {
            if (source.Role.AttackType == GameEnum.eRoleAttackType.E_ROLE_ATTACK_TYPE_PHYSICAL)
            {
                outValue = source.Role.Ptk - target.Role.Pef;
            }
            else
            {
                outValue = source.Role.Mtk - target.Role.Mef;
            }

            outValue = (outValue < 1) ? 1 : outValue;

            if (isCriticalHit)
            {
                outValue = (int)(outValue * GameConst.CRITICAL_HIT_DAMAGE_RATIO);
            }
        }
    }
}

