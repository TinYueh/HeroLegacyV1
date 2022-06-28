using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    public const int MAX_TEAM_MEMBER = 6;                       // 隊伍成員數
    public const int BAR_ENERGY_POINT = 10;                     // 一條 bar 的能量點
    public const int MAX_ENERGY_POINT = BAR_ENERGY_POINT * 5;   // 最大能量點
    public const int COMBAT_MATCH_WIN_ENERGY_POINT = 5;         // 屬性勝獲得能量點
    public const int COMBAT_MATCH_LOSE_ENERGY_POINT = 1;        // 屬性敗獲得能量點
    public const int COMBAT_MATCH_DRAW_ENERGY_POINT = 3;        // 屬性平獲得能量點
    public const int MAX_ROLE_SKILL = 5;                        // 角色技能數
    public const int MAX_SKILL_EFFECT = 2;                      // 技能效果數
    public const int CRITICAL_HIT_DAMAGE_PERCENT = 150;         // 爆擊傷害倍率

    public const float COMBAT_CIRCLE_SLOT_ANGLE = 60f;          // 戰圓每個 slot 間距角度
}