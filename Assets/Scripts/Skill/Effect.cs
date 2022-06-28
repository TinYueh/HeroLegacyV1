using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSkill
{
    public class Effect
    {
        #region Property

        internal GameEnum.eSkillEffectType Type { get; private set; }
        internal GameEnum.eSkillEffectValueType ValueType { get; private set; }
        internal int Value { get; private set; }

        #endregion  // Property

        #region Init

        internal bool Init(SkillEffectCsvData csvData)
        {
            Type = (GameEnum.eSkillEffectType)csvData._type;
            ValueType = (GameEnum.eSkillEffectValueType)csvData._effectValueType;
            Value = csvData._effectValue;

            return true;
        }

        #endregion  // Init
    }
}