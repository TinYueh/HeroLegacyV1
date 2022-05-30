using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSkill
{
    public class Effect
    {
        internal GameEnum.eSkillEffectType Type { get; private set; } = GameEnum.eSkillEffectType.E_SKILL_EFFECT_TYPE_NA;
        internal GameEnum.eSkillEffectValueType ValueType { get; private set; } = GameEnum.eSkillEffectValueType.E_SKILL_EFFECT_VALUE_TYPE_NA;
        internal int Value { get; private set; } = 0;

        internal bool Init(SkillEffectCsvData csvData)
        {
            Type = (GameEnum.eSkillEffectType)csvData._type;
            ValueType = (GameEnum.eSkillEffectValueType)csvData._effectValueType;
            Value = csvData._effectValue;

            return true;
        }
    }
}