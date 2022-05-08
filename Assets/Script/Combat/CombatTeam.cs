using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatTeam : MonoBehaviour
    {
        [SerializeField]
        internal CombatCore.eCombatTeam _team = CombatCore.eCombatTeam.E_COMBAT_TEAM_NA;
        [SerializeField]
        internal UICombatCircle _uiCombatCircle = null;
        [SerializeField]
        internal UIEnergyBar _uiEnergyBar = null;
        [SerializeField]
        internal UIRoleList _uiRoleList = null;

        internal int EnergyPoint { get; private set; } = 0;

        private void Start()
        {

        }

        private void Update()
        {

        }

        internal void ChangeEnergyPoint(int deltaPoint)
        {
            int tmpPoint = EnergyPoint + deltaPoint;

            SetEnergyPoint(tmpPoint);
        }

        internal void SetEnergyPoint(int point)
        {
            if (point < 0)
            {
                EnergyPoint = 0;
            }
            else if (point > GameConst.MAX_ENERGY_POINT)
            {
                EnergyPoint = GameConst.MAX_ENERGY_POINT;
            }
            else
            {
                EnergyPoint = point;
            }

            int showPoint = EnergyPoint % GameConst.BAR_ENERGY_POINT;
            int showCube = EnergyPoint / GameConst.BAR_ENERGY_POINT;

            _uiEnergyBar.ShowBar(showPoint);
            _uiEnergyBar.ShowCube(showCube);

            if (EnergyPoint == GameConst.MAX_ENERGY_POINT)
            {
                // Todo: Lock EnergyBar
            }
        }
    }
}
