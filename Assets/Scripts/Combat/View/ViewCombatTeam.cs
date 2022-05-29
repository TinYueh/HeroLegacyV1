using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameCombat
{
    public class ViewCombatTeam : MonoBehaviour
    {
        internal ViewCombatCircle VwCombatCircle { get; private set ; } = null;
        internal ViewEnergyBar VwEnergyBar { get; private set; } = null;
        internal ViewMemberList VwMemberList { get; private set; } = null;
        internal ViewCombatStats VwCombatStats { get; private set; } = null;
        internal ViewSkillList VwSkillList { get; private set; } = null;

        internal bool Init()
        {
            VwCombatCircle = transform.Find("UICombatCircle").GetComponent<ViewCombatCircle>();
            if (VwCombatCircle.Init() == false)
            {
                Debug.LogError("Init ViewCombatCircle failed");
                return false;
            }

            VwEnergyBar = transform.Find("UIEnergyBar").GetComponent<ViewEnergyBar>();
            if (VwEnergyBar.Init() == false)
            {
                Debug.LogError("Init ViewEnergyBar failed");
                return false;
            }

            VwMemberList = transform.Find("UIMemberList").GetComponent<ViewMemberList>();
            if (VwMemberList.Init() == false)
            {
                Debug.LogError("Init ViewMemberList failed");
                return false;
            }

            VwCombatStats = transform.Find("UICombatStats").GetComponent<ViewCombatStats>();
            if (VwCombatStats.Init() == false)
            {
                Debug.LogError("Init ViewMemberList failed");
                return false;
            }

            VwSkillList = transform.Find("UISkillList").GetComponent<ViewSkillList>();
            if (VwSkillList.Init() == false)
            {
                Debug.LogError("Init ViewSkillList failed");
                return false;
            }

            return true;
        }

        internal bool IsStandby()
        {
            return VwCombatCircle.IsStandby();
        }   

        internal void HandleRotation(GameEnum.eRotateDirection direction)
        {
            if (direction == GameEnum.eRotateDirection.E_ROTATE_DIRECTION_RIGHT)
            {
                VwCombatCircle.RotateAnglePerFrameActual = -VwCombatCircle.RotateAnglePerFrame;
            }
            else if (direction == GameEnum.eRotateDirection.E_ROTATE_DIRECTION_LEFT)
            {
                VwCombatCircle.RotateAnglePerFrameActual = VwCombatCircle.RotateAnglePerFrame;
            }
            else
            {
                return;
            }

            VwCombatCircle.RotateAngleRemaining = GameConst.COMBAT_CIRCLE_SLOT_ANGLE;
            
            VwCombatCircle.EnableRotation();
        }
    }
}