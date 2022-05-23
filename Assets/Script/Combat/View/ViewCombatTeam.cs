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
        private ViewMember _vwMember = null;

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

            if (_vwMember.Init() == false)
            {
                Debug.LogError("Init ViewMember failed");
                return false;
            }

            return true;
        }

        internal bool IsStandby()
        {
            return _vwCombatCircle.IsStandby();
        }

        internal void SetCombatRole(int posId, int memberId, ref CombatRole refCombatRole)
        {
            _vwCombatCircle.SetSocket(posId, ref refCombatRole);
            _vwMember.SetCombatRole(memberId, ref refCombatRole);
        }

        internal void SetEnergyBar(int point)
        {
            _vwEnergyBar.SetEnergyBar(point);
        }

        internal void SetEnergyOrb(int orb)
        {
            _vwEnergyBar.SetEnergyOrb(orb);
        }

        internal void SetRotation(GameEnum.eRotateDirection direction)
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
            
            _vwCombatCircle.EnableRotate();
        }

        internal void ShowCombatRole(int memberId)
        {
            _vwMember.ShowCombatRole(memberId);
        }

        internal void HideCombatRole(int memberId)
        {
            _vwMember.HideCombatRole(memberId);
        }

        internal void SetHealthBar(int memberId, int value, int max)
        {
            ViewCombatRole vwCombatRole = null;
            if (_vwMember.GetCombatRole(memberId, out vwCombatRole) == false)
            {
                return;
            }

            vwCombatRole.SetHealthBar(value, max);

            if (value == 0)
            {
                vwCombatRole.SetStateDying();
            }
            else
            {
                vwCombatRole.SetStateNormal();
            }
        }
    }
}