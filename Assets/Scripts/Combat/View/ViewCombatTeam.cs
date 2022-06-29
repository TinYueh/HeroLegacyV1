using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameCombat
{
    public class ViewCombatTeam : MonoBehaviour
    {
        #region Property

        internal ViewCombatCircle ViewCombatCircle { get; private set ; }
        internal ViewEnergyBar ViewEnergyBar { get; private set; }
        internal ViewMemberList ViewMemberList { get; private set; }
        internal ViewCombatStats ViewCombatStats { get; private set; }
        internal ViewSkillList ViewSkillList { get; private set; }

        private GameEnum.eCombatTeamType _teamType;

        #endregion  // Property

        #region Init

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

        #endregion  // Init

        #region Method

        internal void Rotate(GameEnum.eRotateDirection direction)
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
            ViewCombatCircle.SetRotate();
        }

        #endregion  // Method
    }
}