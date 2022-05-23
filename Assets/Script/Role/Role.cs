using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

public class Role
{
    public int Id { get; protected set; }
    public GameEnum.eCombatTeamType TeamType { get; protected set; }
    public int Portrait { get; protected set; }
    public int Emblem { get; protected set; }
    public int Name { get; protected set; }
    public GameEnum.eRoleAttribute Attribute { get; protected set; }
    public int Talent { get; protected set; }
    public int Health { get; protected set; }
    public int Attack { get; protected set; }
    public int Defence { get; protected set; }
    public int Ai { get; protected set; }

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