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