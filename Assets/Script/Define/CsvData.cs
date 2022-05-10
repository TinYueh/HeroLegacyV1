using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamCsvData
{
    public int _id;
    public int[] _arrRoleId = new int[GameConst.MAX_TEAM_MEMBER];
}

public class RoleCsvData
{
    public int _id;
    public int _team;
    public int _portrait;
    public int _emblem;
    public int _name;
    public int _attribute;
    public int _talent;
    public int _life;
    public int _attack;
    public int _defence;
    public int _ai;
}

// 新增 Table: 定義結構