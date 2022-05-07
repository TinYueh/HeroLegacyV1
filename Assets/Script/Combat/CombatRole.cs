using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatRole : MonoBehaviour
    {
        [SerializeField]
        internal CombatCore.eCombatRole _role = CombatCore.eCombatRole.E_COMBAT_ROLE_NA;
        [SerializeField]
        internal UICombatCircle _uiCombatCircle = null;
        [SerializeField]
        internal UIEnergyBar _uiEnergyBar = null;
        [SerializeField]
        internal GameObject _roleList = null;

        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}
