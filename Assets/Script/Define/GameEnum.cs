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

    public enum eCombatRoundState
    {
        E_COMBAT_ROUND_STATE_NA = 0,
        E_COMBAT_ROUND_STATE_STANDBY,
        E_COMBAT_ROUND_STATE_ROTATE,
        E_COMBAT_ROUND_STATE_MATCH,
        E_COMBAT_ROUND_STATE_LIMIT,
    }

    public enum eCombatMatchResult
    {
        E_COMBAT_MATCH_RESULT_NA = 0,
        E_COMBAT_MATCH_RESULT_WIN,      // 勝
        E_COMBAT_MATCH_RESULT_LOSE,     // 負
        E_COMBAT_MATCH_RESULT_DRAW,     // 平
        E_COMBAT_MATCH_RESULT_LIMIT,
    }

    public enum eCombatRoleState
    {
        E_COMBAT_ROLE_STATE_NA = 0,
        E_COMBAT_ROLE_STATE_NORMAL,     // 正常
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
}
