using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem;
public class Role
{
    public int Id { get; protected set; } = 0;
    public GameEnum.eCombatTeam Team { get; protected set; } = 0;
    public int Portrait { get; protected set; } = 0;
    public int Emblem { get; protected set; } = 0;
    public int Name { get; protected set; } = 0;
    public GameEnum.eRoleAttribute Attribute { get; protected set; } = 0;
    public int Talent { get; protected set; } = 0;
    public int Life { get; protected set; } = 0;
    public int Attack { get; protected set; } = 0;
    public int Defence { get; protected set; } = 0;
    public int Ai { get; protected set; } = 0;

    public bool Init(int roleId)
    {
        RoleCsvData csvData = new RoleCsvData();
        if (TableManager.Instance.GetRoleCsvData(roleId, out csvData) == false)
        {
            Debug.LogError("Not found RoleCsvData, Id: " + roleId);
            return false;
        }

        Id = csvData._id;
        Team = (GameEnum.eCombatTeam)csvData._team;
        Portrait = csvData._portrait;
        Emblem = csvData._emblem;
        Name = csvData._name;
        Attribute = (GameEnum.eRoleAttribute)csvData._attribute;
        Talent = csvData._talent;
        Life = csvData._life;
        Attack = csvData._attack;
        Defence = csvData._defence;
        Ai = csvData._ai;

        return true;
    }
}