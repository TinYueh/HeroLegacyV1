using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameProperty
{
    public class Role
    {
        #region Property

        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public int UIName { get; internal set; }
        public int Portrait { get; internal set; }
        public int Emblem { get; internal set; }
        public GameEnum.eRoleAttribute Attribute { get; internal set; }
        public int Talent { get; internal set; }
        public int Health { get; internal set; }
        public GameEnum.eRoleAttackType AttackType { get; internal set; }
        public int Ptk { get; internal set; }
        public int Mtk { get; internal set; }
        public int Pef { get; internal set; }
        public int Mef { get; internal set; }
        public int Ai { get; internal set; }
        public List<int> ListSkill { get; private set; } = new List<int>();

        #endregion  // Property

        #region Init

        public bool Init(RoleCsvData csvData)
        {
            Id = csvData._id;
            Name = csvData._name;
            UIName = csvData._uiName;
            Portrait = csvData._portrait;
            Emblem = csvData._emblem;
            Name = csvData._name;
            Attribute = (GameEnum.eRoleAttribute)csvData._attribute;
            Talent = csvData._talent;
            Health = csvData._health;
            AttackType = (GameEnum.eRoleAttackType)csvData._attackType;
            Ptk = csvData._ptk;
            Mtk = csvData._mtk;
            Pef = csvData._pef;
            Mef = csvData._mef;
            Ai = csvData._ai;

            foreach (var skillId in csvData._skillId)
            {
                if (skillId == 0)
                {
                    break;
                }

                ListSkill.Add(skillId);
            }

            return true;
        }

        #endregion  // Init
    }
}