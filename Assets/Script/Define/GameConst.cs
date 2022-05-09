using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    // Fire > Wind > Water > Fire
    public enum eRoleClass : byte
    {
        E_ROLE_CLASS_NA = 0,
        E_ROLE_CLASS_POWER,      // Fire(Red)
        E_ROLE_CLASS_SPEED,      // Wind(Green)
        E_ROLE_CLASS_TECHNIQUE,  // Water(Blue)
        E_ROLE_CLASS_LIMIT,
    }

    public const int MAX_TEAM_MEMBER = 6;       // 隊伍成員數
    public const int BAR_ENERGY_POINT = 5;      // 一條 bar 的能量點
    public const int MAX_ENERGY_POINT = 25;     // 最大能量點
}