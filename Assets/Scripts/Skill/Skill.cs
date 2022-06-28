using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSkill
{
    public class Skill
    {
        #region Property

        internal int Id { get; private set; }
        public string Name { get; internal set; }
        internal int UIName { get; private set; }
        internal GameEnum.ePosType PosType { get; private set; }
        internal int Cost { get; private set; }
        internal int Cd { get; private set; }
        internal GameEnum.eSkillRange Range { get; private set; }

        internal List<Effect> _listEffect = new List<Effect>();

        #endregion  // Property

        #region Method

        internal bool Init(SkillCsvData csvData)
        {
            Id = csvData._id;
            Name = csvData._name;
            UIName = csvData._uiName;
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

        #endregion  // Method
    }
}