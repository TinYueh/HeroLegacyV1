using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamCsvData
{
    public int _id = 0;
    public int[] _arrRoleId = new int[GameConst.MAX_TEAM_MEMBER];
}

public class RoleCsvData
{
    public int _id = 0;
    public int _teamType = 0;
    public int _portrait = 0;
    public int _emblem = 0;
    public int _name = 0;
    public int _attribute = 0;
    public int _talent = 0;
    public int _health = 0;
    public int _attack = 0;
    public int _defence = 0;
    public int _ai = 0;
}

public class SkillCsvData
{
    public static readonly int _maxEffect = 2;

    public int _id = 0;
    public int _name = 0;
    public int _pos = 0;
    public int _cost = 0;
    public int _cd = 0;
    public SkillEffectCsvData[] _effect = new SkillEffectCsvData[_maxEffect];
}

public class SkillEffectCsvData
{
    public int _effect = 0;
    public int _range = 0;
    public int _effectValueType = 0;
    public int _effectValue = 0;
}

// 新增 Table: 定義結構