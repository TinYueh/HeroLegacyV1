using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSkill
{
    public class Skill
    {
        internal int Id { get; private set; } = 0;
        internal int Name { get; private set; } = 0;
        internal GameEnum.ePosType PosType { get; private set; } = GameEnum.ePosType.E_POS_TYPE_NA;
        internal int Cost { get; private set; } = 0;
        internal int Cd { get; private set; } = 0;
        internal GameEnum.eSkillRange Range { get; private set; } = GameEnum.eSkillRange.E_SKILL_RANGE_NA;

        internal List<Effect> _listEffect = new List<Effect>();

        internal bool Init(SkillCsvData csvData)
        {
            Id = csvData._id;
            Name = csvData._name;
            PosType = (GameEnum.ePosType)csvData._posType;
            Cost = csvData._cost;
            Cd = csvData._cd;
            Range = (GameEnum.eSkillRange)csvData._range;

            for (int i = 0; i < GameConst.MAX_SKILL_EFFECT; ++i)
            {
                GameEnum.eSkillEffectType effectType = (GameEnum.eSkillEffectType)csvData._effect[i]._type;

                if (effectType == GameEnum.eSkillEffectType.E_SKILL_EFFECT_TYPE_NA)
                {
                    break;
                }

                Effect effect = new Effect();
                if (effect.Init(csvData._effect[i]) == false)
                {
                    Debug.LogError("Init Effect failed, SkillId: " + Id + " EffectType: " + effectType);
                    return false;
                }

                _listEffect.Add(effect);
            }

            return true;
        }
    }
}
