using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnum
{
    public enum eCombatTeamType
    {
        E_COMBAT_TEAM_TYPE_NA = 0,
        E_COMBAT_TEAM_TYPE_PLAYER,   // 玩家
        E_COMBAT_TEAM_TYPE_OPPONENT, // 對手
        E_COMBAT_TEAM_TYPE_LIMIT,
    }

    public enum eCombatRoundAction
    {
        E_COMBAT_ROUND_ACTION_NA = 0,
        E_COMBAT_ROUND_ACTION_ROTATE_RIGHT,  // 順時鐘移動
        E_COMBAT_ROUND_ACTION_ROTATE_LEFT,   // 逆時鐘移動
        E_COMBAT_ROUND_ACTION_CAST,          // 使用技能
        E_COMBAT_ROUND_ACTION_ROTATE_LIMIT,
    }

    public enum eRotateDirection
    {
        E_ROTATE_DIRECTION_NA = 0,
        E_ROTATE_DIRECTION_RIGHT,   // 順時鐘移動
        E_ROTATE_DIRECTION_LEFT,    // 逆時鐘移動
        E_ROTATE_DIRECTION_STAY,    // 停留
        E_ROTATE_DIRECTION_LIMIT,
    }

    public enum eCombatRoundState
    {
        E_COMBAT_ROUND_STATE_NA = 0,
        E_COMBAT_ROUND_STATE_STANDBY,
        E_COMBAT_ROUND_STATE_ROTATE,
        E_COMBAT_ROUND_STATE_MATCH,
        E_COMBAT_ROUND_STATE_FINAL,
        E_COMBAT_ROUND_STATE_LEAVE,
        E_COMBAT_ROUND_STATE_LIMIT,
    }

    public enum eCombatAttributeMatchResult
    {
        E_COMBAT_ATTRIBUTE_MATCH_NA = 0,
        E_COMBAT_ATTRIBUTE_MATCH_WIN,      // 勝
        E_COMBAT_ATTRIBUTE_MATCH_LOSE,     // 負
        E_COMBAT_ATTRIBUTE_MATCH_DRAW,     // 平
        E_COMBAT_ATTRIBUTE_MATCH_LIMIT,
    }

    public enum eCombatRoleState
    {
        E_COMBAT_ROLE_STATE_NA = 0,
        E_COMBAT_ROLE_STATE_LIVING,     // 正常
        E_COMBAT_ROLE_STATE_DYING,      // 瀕死
        E_COMBAT_ROLE_STATE_LIMIT,
    }

    public enum eRoleAttribute
    {
        E_ROLE_ATTRIBUTE_NA = 0,
        E_ROLE_ATTRIBUTE_POWER,      // Fire(Red)
        E_ROLE_ATTRIBUTE_SPEED,      // Wind(Green)
        E_ROLE_ATTRIBUTE_TECHNIQUE,  // Water(Blue)
        E_ROLE_ATTRIBUTE_LIMIT,
    }

    public enum eCircleSocketType
    {
        E_CIRCLE_SOCKET_TYPE_NA = 0,
        E_CIRCLE_SOCKET_TYPE_SPACE,
        E_CIRCLE_SOCKET_TYPE_COMBAT_ROLE,
        E_CIRCLE_SOCKET_TYPE_LIMIT,
    }

    public enum eSkillPos
    {
        E_SKILL_POS_NA = 0,
        E_SKILL_POS_MATCH   = 1,    // 對決位
        E_SKILL_POS_WING    = 2,    // 側翼
        E_SKILL_POS_FORWARD = 3,    // 前排
        E_SKILL_POS_LIMIT,
    }

    public enum eSkillRange
    {
        E_SKILL_RANGE_NA = 0,
        E_SKILL_RANGE_SOURCE            = 1,    // 自己
        E_SKILL_RANGE_SOURCE_MATCH      = 101,  // 已方對決位
        E_SKILL_RANGE_SOURCE_FORWARD    = 102,  // 已方前排
        E_SKILL_RANGE_SOURCE_GUARD      = 103,  // 己方後排
        E_SKILL_RANGE_SOURCE_ALL        = 104,  // 已方全體
        E_SKILL_RANGE_TARGET_MATCH      = 201,  // 對方對決位
        E_SKILL_RANGE_TARGET_FORWARD    = 202,  // 對方前排
        E_SKILL_RANGE_TARGET_GUARD      = 203,  // 對方後排
        E_SKILL_RANGE_TARGET_ALL        = 204,  // 對方全體
        E_SKILL_RANGE_BOTH_MATCH        = 301,  // 雙方對決位
        E_SKILL_RANGE_BOTH_FORWARD      = 302,  // 雙方前排
        E_SKILL_RANGE_BOTH_GUARD        = 303,  // 雙方後排
        E_SKILL_RANGE_BOTH_ALL          = 304,  // 雙方全體
        E_SKILL_RANGE_LIMIT,
    }

    public enum eSkillEffectType
    {
        E_SKILL_EFFECT_TYPE_NA = 0,
        E_SKILL_EFFECT_TYPE_HEAL    = 1,    // 治療
        E_SKILL_EFFECT_TYPE_DAMAGE  = 2,    // 傷害
        E_SKILL_EFFECT_TYPE_LIMIT,
    }

    public enum eSkillEffectValueType
    {
        E_SKILL_EFFECT_VALUE_TYPE_NA = 0,
        E_SKILL_EFFECT_VALUE_TYPE_ACTUAL    = 1,    // 實值
        E_SKILL_EFFECT_VALUE_TYPE_PERCENT   = 2,    // 比例
        E_SKILL_EFFECT_VALUE_TYPE_LIMIT,
    }
}
