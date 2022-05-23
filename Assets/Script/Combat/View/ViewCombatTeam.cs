using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameCombat
{
    public class ViewCombatTeam : MonoBehaviour
    {
        [SerializeField]
        private ViewCombatCircle _vwCombatCircle;
        [SerializeField]
        private ViewEnergyBar _vwEnergyBar;
        [SerializeField]
        private ViewMember _vwMember;

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

        internal void SetCombatRole(int posId, int memberId, CombatRole combatRole)
        {
            _vwCombatCircle.SetSocket(posId, combatRole);
            _vwMember.SetCombatRole(memberId, combatRole);
        }

        internal void SetEnergyBar(int point)
        {
            _vwEnergyBar.SetEnergyBar(point);
        }

        internal void SetEnergyOrb(int orb)
        {
            _vwEnergyBar.SetEnergyOrb(orb);
        }

        internal void ShowCombatRole(int memberId)
        {
            _vwMember.ShowCombatRole(memberId);
        }

        internal void HideCombatRole(int memberId)
        {
            _vwMember.HideCombatRole(memberId);
        }
    }
}