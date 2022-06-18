using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

public class Role
{
    public int Id { get; internal set; } = 0;
    public GameEnum.eCombatTeamType TeamType { get; internal set; } = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;
    public int Portrait { get; internal set; } = 0;
    public int Emblem { get; internal set; } = 0;
    public int Name { get; internal set; } = 0;
    public GameEnum.eRoleAttribute Attribute { get; internal set; } = GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_NA;
    public int Talent { get; internal set; } = 0;
    public int Health { get; internal set; } = 0;
    public GameEnum.eRoleAttackType AttackType { get; internal set; } = GameEnum.eRoleAttackType.E_ROLE_ATTACK_TYPE_NA;
    public int Ptk { get; internal set; } = 0;
    public int Mtk { get; internal set; } = 0;
    public int Pef { get; internal set; } = 0;
    public int Mef { get; internal set; } = 0;
    public int Ai { get; internal set; } = 0;
    public List<int> ListSkill { get; private set; } = new List<int>();

    public bool Init(RoleCsvData csvData)
    {
        Id = csvData._id;
        TeamType = (GameEnum.eCombatTeamType)csvData._teamType;
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
}