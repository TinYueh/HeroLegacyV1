using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCsvData
{
    public int _id;
    public int _portrait;
    public int _emblem;
    public int _name;
    public int _talent;
    public int _life;
    public int _attack;
    public int _defence;
}

public class MobCsvData
{
    public int _id;
    public int _portrait;
    public int _emblem;
    public int _name;
    public int _life;
    public int _attack;
    public int _defence;
    public int _ai;
}

public class TeamCsvData
{
    public int _id;
    public int[] _arrRoleId = new int[GameConst.MAX_TEAM_MEMBER];
}

// 新增 Table: 定義結構