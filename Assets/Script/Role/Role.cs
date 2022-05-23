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

    public bool Init(ref RoleCsvData refCsvData)
    {
        Id = refCsvData._id;
        TeamType = (GameEnum.eCombatTeamType)refCsvData._teamType;
        Portrait = refCsvData._portrait;
        Emblem = refCsvData._emblem;
        Name = refCsvData._name;
        Attribute = (GameEnum.eRoleAttribute)refCsvData._attribute;
        Talent = refCsvData._talent;
        Health = refCsvData._health;
        Attack = refCsvData._attack;
        Defence = refCsvData._defence;
        Ai = refCsvData._ai;

        return true;
    }
}