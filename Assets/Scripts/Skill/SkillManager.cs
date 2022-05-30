using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameSkill
{
    public class SkillManager : Singleton<SkillManager>
    {
        private Dictionary<int, Skill> _dicSkill = new Dictionary<int, Skill>();

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

            return true;
        }
    }
}
