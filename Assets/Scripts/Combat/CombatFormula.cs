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

        internal int GetValuePercent(int source, int percent)
        {
            int value = source * percent / 100;

            return value;
        }

        internal int GetNormalAttackDamage(CombatRole source, CombatRole target, bool isCriticalHit)
        {
            int rand = Random.Range(90, 101);
            int sourceAtk = 0;
            int targetDef = 0;

            if (source.Role.AttackType == GameEnum.eRoleAttackType.E_ROLE_ATTACK_TYPE_PHYSICAL)
            {
                sourceAtk = source.Role.Ptk;
                targetDef = target.Role.Pef;
            }
            else if (source.Role.AttackType == GameEnum.eRoleAttackType.E_ROLE_ATTACK_TYPE_MAGIC)
            {
                sourceAtk = source.Role.Mtk;
                targetDef = target.Role.Mef;
            }

            int damage = sourceAtk * rand / 100 - targetDef;

            if (isCriticalHit)
            {
                damage = GetValuePercent(damage, GameConst.CRITICAL_HIT_DAMAGE_PERCENT);
            }

            damage = (damage < 1) ? 1 : damage;

            return damage;
        }

        internal int GetPhysicalSkillDamage(CombatRole source, CombatRole target, int effectValue)
        {
            int rand = Random.Range(90, 101);
            int sourceAtk = GetValuePercent(source.Role.Ptk, effectValue);
            int targetDef = target.Role.Pef;

            int damage = (sourceAtk * rand) / 100 - targetDef;

            damage = (damage < 1) ? 1 : damage;

            return damage;
        }

        internal int GetMagicSkillDamage(CombatRole source, CombatRole target, int effectValue)
        {
            int rand = Random.Range(90, 101);
            int sourceAtk = GetValuePercent(source.Role.Mtk, effectValue);
            int targetDef = target.Role.Mef;

            int damage = sourceAtk * rand / 100 - targetDef;

            damage = (damage < 1) ? 1 : damage;

            return damage;
        }

        internal int GetSkillHeal(CombatRole source, CombatRole target, int effectValue)
        {
            int rand = Random.Range(90, 101);
            int sourceAtk = GetValuePercent(source.Role.Mtk, effectValue);
            int targetDef = (target.Role.Pef > target.Role.Mef) ? target.Role.Pef : target.Role.Mef;

            int heal = (sourceAtk + targetDef) / 2 * rand / 100;

            return heal;
        }
    }
}

