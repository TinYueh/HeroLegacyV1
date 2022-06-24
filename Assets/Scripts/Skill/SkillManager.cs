using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;
using GameCombat;

namespace GameSkill
{
    public class SkillManager : Singleton<SkillManager>
    {
        private Dictionary<int, Skill> _dicSkill = new Dictionary<int, Skill>();

        private delegate bool DlgSkillExecFunc(Skill skill, Effect effect, CombatRole source, CombatTeam sourceTeam, CombatTeam targetTeam, List<CombatRole> listTarget);
        private Dictionary<GameEnum.eSkillEffectType, DlgSkillExecFunc> _dicSkillExecFunc = new Dictionary<GameEnum.eSkillEffectType, DlgSkillExecFunc>();

        public override bool Init()
        {
            foreach (var csvData in TableManager.Instance._dicSkillCsvData)
            {
                Skill skill = new Skill();
                if (skill.Init(csvData.Value) == false)
                {
                    Debug.LogError("Init Skill failed, SkillId: " + csvData.Value._id);
                    return false;
                }

                _dicSkill.Add(skill.Id, skill);
            }

            RegistSkillExecFunc();

            return true;
        }

        private void RegistSkillExecFunc()
        {
            _dicSkillExecFunc.Add(GameEnum.eSkillEffectType.E_SKILL_EFFECT_TYPE_DAMAGE_PHYSICAL, ExecDamagePhysical);
            _dicSkillExecFunc.Add(GameEnum.eSkillEffectType.E_SKILL_EFFECT_TYPE_DAMAGE_MAGIC, ExecDamageMagic);
            _dicSkillExecFunc.Add(GameEnum.eSkillEffectType.E_SKILL_EFFECT_TYPE_HEAL, ExecHealMtk);
        }

        internal bool ExecSkill(int skillId, CombatRole source, CombatTeam sourceTeam, CombatTeam targetTeam)
        {
            Skill skill = null;
            if (GetSkill(skillId, out skill) == false)
            {
                return false;
            }

            sourceTeam.ChangeEnergyOrb(-skill.Cost);

            source.SetSkillCd(skill.Id, skill.Cd + 1); // 不包含此回合

            List<CombatRole> listTarget = new List<CombatRole>();
            GetRangeTarget(skill.Range, source, sourceTeam, targetTeam, ref listTarget);

            foreach (var effect in skill._listEffect)
            {
                if (effect.Type == GameEnum.eSkillEffectType.E_SKILL_EFFECT_TYPE_NA)
                {
                    break;
                }

                DlgSkillExecFunc dlgFunc = null;
                if (_dicSkillExecFunc.TryGetValue(effect.Type, out dlgFunc) == false)
                {
                    Debug.LogError("Not found DlgSkillExecFunc for " + effect.Type);
                    return false;
                }

                dlgFunc(skill, effect, source, sourceTeam, targetTeam, listTarget);
            }

            return true;
        }

        private bool ExecDamagePhysical(Skill skill, Effect effect, CombatRole source, CombatTeam sourceTeam, CombatTeam targetTeam, List<CombatRole> listTarget)
        {            
            foreach (var target in listTarget)
            {
                int damage = CombatManager.Instance.Formula.GetSkillDamagePhysical(source, target, effect.Value);

                target.ChangeHealth(-damage);
            }

            return true;
        }

        private bool ExecDamageMagic(Skill skill, Effect effect, CombatRole source, CombatTeam sourceTeam, CombatTeam targetTeam, List<CombatRole> listTarget)
        {
            foreach (var target in listTarget)
            {
                int damage = CombatManager.Instance.Formula.GetSkillDamageMagic(source, target, effect.Value);

                target.ChangeHealth(-damage);
            }

            return true;
        }

        private bool ExecHealMtk(Skill skill, Effect effect, CombatRole source, CombatTeam sourceTeam, CombatTeam targetTeam, List<CombatRole> listTarget)
        {
            foreach (var target in listTarget)
            {
                int heal = CombatManager.Instance.Formula.GetSkillHeal(source, target, effect.Value);

                target.ChangeHealth(heal);
            }

            return true;
        }

        internal bool GetSkill(int skillId, out Skill outSkill)
        {
            if (_dicSkill.TryGetValue(skillId, out outSkill) == false)
            {
                Debug.LogError("Not found Skill, SkillId: " + skillId);
                return false;
            }

            return true;
        }

        internal void GetRangeTarget(GameEnum.eSkillRange range, CombatRole source, CombatTeam sourceTeam, CombatTeam targetTeam, ref List<CombatRole> refListTarget)
        {
            switch (range)
            {
                case GameEnum.eSkillRange.E_SKILL_RANGE_SOURCE:
                    {
                        refListTarget.Add(source);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_SOURCE_MATCH:
                    {
                        sourceTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_MATCH, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_SOURCE_WING:
                    {
                        sourceTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_WING, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_SOURCE_FORWARD:
                    {
                        sourceTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_FORWARD, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_SOURCE_GUARD:
                    {
                        sourceTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_GUARD, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_SOURCE_ALL:
                    {
                        sourceTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_ALL, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_TARGET_MATCH:
                    {
                        targetTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_MATCH, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_TARGET_WING:
                    {
                        targetTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_WING, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_TARGET_FORWARD:
                    {
                        targetTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_FORWARD, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_TARGET_GUARD:
                    {
                        targetTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_GUARD, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_TARGET_ALL:
                    {
                        targetTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_ALL, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_BOTH_MATCH:
                    {
                        sourceTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_MATCH, ref refListTarget);
                        targetTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_MATCH, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_BOTH_WING:
                    {
                        sourceTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_WING, ref refListTarget);
                        targetTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_WING, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_BOTH_FORWARD:
                    {
                        sourceTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_FORWARD, ref refListTarget);
                        targetTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_FORWARD, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_BOTH_GUARD:
                    {
                        sourceTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_GUARD, ref refListTarget);
                        targetTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_GUARD, ref refListTarget);
                        break;
                    }
                case GameEnum.eSkillRange.E_SKILL_RANGE_BOTH_ALL:
                    {
                        sourceTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_ALL, ref refListTarget);
                        targetTeam.GetCombatRoleList(GameEnum.ePosType.E_POS_TYPE_ALL, ref refListTarget);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
