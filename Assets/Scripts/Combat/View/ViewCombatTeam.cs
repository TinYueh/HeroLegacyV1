using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameCombat
{
    public class ViewCombatTeam : MonoBehaviour
    {
        internal ViewCombatCircle ViewCombatCircle { get; private set ; } = null;
        internal ViewEnergyBar ViewEnergyBar { get; private set; } = null;
        internal ViewMemberList ViewMemberList { get; private set; } = null;
        internal ViewCombatStats ViewCombatStats { get; private set; } = null;
        internal ViewSkillList ViewSkillList { get; private set; } = null;

        private GameEnum.eCombatTeamType _teamType = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;

        internal bool Init(GameEnum.eCombatTeamType teamType)
        {
            _teamType = teamType;

            ViewCombatCircle = transform.Find("UICombatCircle").GetComponent<ViewCombatCircle>();
            if (ViewCombatCircle.Init(_teamType) == false)
            {
                Debug.LogError("Init ViewCombatCircle failed");
                return false;
            }

            ViewEnergyBar = transform.Find("UIEnergyBar").GetComponent<ViewEnergyBar>();
            if (ViewEnergyBar.Init(_teamType) == false)
            {
                Debug.LogError("Init ViewEnergyBar failed");
                return false;
            }

            ViewMemberList = transform.Find("UIMemberList").GetComponent<ViewMemberList>();
            if (ViewMemberList.Init(_teamType) == false)
            {
                Debug.LogError("Init ViewMemberList failed");
                return false;
            }

            ViewCombatStats = transform.Find("UICombatStats").GetComponent<ViewCombatStats>();
            if (ViewCombatStats.Init(_teamType) == false)
            {
                Debug.LogError("Init ViewMemberList failed");
                return false;
            }

            ViewSkillList = transform.Find("UISkillList").GetComponent<ViewSkillList>();
            if (ViewSkillList.Init(_teamType) == false)
            {
                Debug.LogError("Init ViewSkillList failed");
                return false;
            }

            return true;
        }

        internal void HandleRotation(GameEnum.eRotateDirection direction)
        {
            if (direction == GameEnum.eRotateDirection.E_ROTATE_DIRECTION_RIGHT)
            {
                ViewCombatCircle.RotateAnglePerFrameActual = -ViewCombatCircle.RotateAnglePerFrame;
            }
            else if (direction == GameEnum.eRotateDirection.E_ROTATE_DIRECTION_LEFT)
            {
                ViewCombatCircle.RotateAnglePerFrameActual = ViewCombatCircle.RotateAnglePerFrame;
            }
            else
            {
                return;
            }

            ViewCombatCircle.RotateAngleRemaining = GameConst.COMBAT_CIRCLE_SLOT_ANGLE;
            
            ViewCombatCircle.EnableRotation();
        }           
    }
}