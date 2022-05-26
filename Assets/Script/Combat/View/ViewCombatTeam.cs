using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameCombat
{
    public class ViewCombatTeam : MonoBehaviour
    {
        [SerializeField]
        private ViewCombatCircle _vwCombatCircle = null;
        [SerializeField]
        private ViewEnergyBar _vwEnergyBar = null;
        [SerializeField]
        private ViewMemberList _vwMemberList = null;
        [SerializeField]
        private ViewCombatStats _vwCombatStats = null;

        internal bool Init()
        {
            if (_vwCombatCircle.Init() == false)
            {
                Debug.LogError("Init ViewCombatCircle failed");
                return false;
            }

            if (_vwEnergyBar.Init() == false)
            {
                Debug.LogError("Init ViewEnergyBar failed");
                return false;
            }

            if (_vwMemberList.Init() == false)
            {
                Debug.LogError("Init ViewMemberList failed");
                return false;
            }

            if (_vwCombatStats.Init() == false)
            {
                Debug.LogError("Init ViewMemberList failed");
                return false;
            }

            return true;
        }

        internal bool IsStandby()
        {
            return _vwCombatCircle.IsStandby();
        }

        internal bool GetCircleSocket(int posId, out ViewCircleSocket outVwCircleSocket)
        {
            return _vwCombatCircle.GetCircleSocket(posId, out outVwCircleSocket);
        }

        internal bool GetCombatRole(int memberId, out ViewCombatRole outVwCombatRole)
        {
            return _vwMemberList.GetCombatRole(memberId, out outVwCombatRole);
        }

        internal void SetCombatRole(int memberId, CombatRole combatRole)
        {
            _vwMemberList.SetCombatRole(memberId, combatRole);
        }        

        internal void SetEnergyBar(int point)
        {
            _vwEnergyBar.SetEnergyBar(point);
        }

        internal void SetEnergyOrb(int orb)
        {
            _vwEnergyBar.SetEnergyOrb(orb);
        }

        internal void HandleRotation(GameEnum.eRotateDirection direction)
        {
            if (direction == GameEnum.eRotateDirection.E_ROTATE_DIRECTION_RIGHT)
            {
                _vwCombatCircle.RotateAnglePerFrameActual = -_vwCombatCircle.RotateAnglePerFrame;
            }
            else if (direction == GameEnum.eRotateDirection.E_ROTATE_DIRECTION_LEFT)
            {
                _vwCombatCircle.RotateAnglePerFrameActual = _vwCombatCircle.RotateAnglePerFrame;
            }
            else
            {
                return;
            }

            _vwCombatCircle.RotateAngleRemaining = GameConst.COMBAT_CIRCLE_SLOT_ANGLE;
            
            _vwCombatCircle.EnableRotation();
        }

        internal void ShowCombatRole(int memberId)
        {
            _vwMemberList.ShowCombatRole(memberId);
        }

        internal void HideCombatRole(int memberId)
        {
            _vwMemberList.HideCombatRole(memberId);
        }

        internal void ShowFirstToken()
        {
            _vwCombatStats.ShowFirstToken();
        }

        internal void HideFirstToken()
        {
            _vwCombatStats.HideFirstToken();
        }
    }
}