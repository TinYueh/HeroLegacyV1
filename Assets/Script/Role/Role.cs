using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

public class Role
{
    public int Id { get; protected set; } = 0;
    public GameEnum.eCombatTeamType TeamType { get; protected set; } = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;
    public int Portrait { get; protected set; } = 0;
    public int Emblem { get; protected set; } = 0;
    public int Name { get; protected set; } = 0;
    public GameEnum.eRoleAttribute Attribute { get; protected set; } = GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_NA;
    public int Talent { get; protected set; } = 0;
    public int Health { get; protected set; } = 0;
    public int Attack { get; protected set; } = 0;
    public int Defence { get; protected set; } = 0;
    public int Ai { get; protected set; } = 0;

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
        Attack = csvData._attack;
        Defence = csvData._defence;
        Ai = csvData._ai;

        return true;
    }
}