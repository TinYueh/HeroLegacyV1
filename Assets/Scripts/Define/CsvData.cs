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
    public string _name;
    public int _uiName;
    public int _portrait;
    public int _emblem;
    public int _attribute;  // eRoleAttribute
    public int _talent;
    public int _health;
    public int _attackType; // eRoleAttackType
    public int _ptk;
    public int _mtk;
    public int _pef;
    public int _mef;
    public int _ai;
    public int[] _skillId = new int[GameConst.MAX_ROLE_SKILL];
}

public class SkillCsvData
{
    public int _id;
    public string _name;
    public int _uiName;
    public int _posType;    // ePosType
    public int _cost;
    public int _cd;
    public int _range;      // eSkillRange
    public SkillEffectCsvData[] _effect = new SkillEffectCsvData[GameConst.MAX_SKILL_EFFECT];
}

public class SkillEffectCsvData
{
    public int _type;               // eSkillEffectType
    public int _effectValueType;    // eSkillEffectValueType
    public int _effectValue;
}

// 新增 Table: 定義結構