using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSkill
{
    public class Effect
    {
        public int Id { get; internal set; } = 0;
        public GameEnum.eSkillEffectValueType Type { get; internal set; } = GameEnum.eSkillEffectValueType.E_SKILL_EFFECT_NA;
        public int Value { get; internal set; } = 0;

        internal bool Init(SkillEffectCsvData csvData)
        {
            Id = csvData._effect;
            Type = (GameEnum.eSkillEffectValueType)csvData._effectValueType;
            Value = csvData._effectValue;

            return true;
        }
    }
}