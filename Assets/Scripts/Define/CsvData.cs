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
    public string _name = null;
    public int _uiName = 0;
    public int _portrait = 0;
    public int _emblem = 0;
    public int _attribute = 0;  // eRoleAttribute
    public int _talent = 0;
    public int _health = 0;
    public int _attackType = 0; // eRoleAttackType
    public int _ptk = 0;
    public int _mtk = 0;
    public int _pef = 0;
    public int _mef = 0;
    public int _ai = 0;
    public int[] _skillId = new int[GameConst.MAX_ROLE_SKILL];
}

public class SkillCsvData
{
    public int _id = 0;
    public string _name = null;
    public int _uiName = 0;
    public int _posType = 0;    // ePosType
    public int _cost = 0;
    public int _cd = 0;
    public int _range = 0;      // eSkillRange
    public SkillEffectCsvData[] _effect = new SkillEffectCsvData[GameConst.MAX_SKILL_EFFECT];
}

public class SkillEffectCsvData
{
    public int _type = 0;               // eSkillEffectType
    public int _effectValueType = 0;    // eSkillEffectValueType
    public int _effectValue = 0;
}

// 新增 Table: 定義結構