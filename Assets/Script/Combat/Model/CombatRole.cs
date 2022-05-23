using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatRole
    {
        internal int MemberId { get; set; } = 0;
        internal Role Role { get; set; } = new Role();
        internal GameEnum.eCombatRoleState State { get; set; } = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NA;
        internal int Health { get; private set; } = 0;
        internal int NormalDamage { get; set; } = 0;

        internal bool Init(int memberId, ref RoleCsvData refCsvData)
        {
            if (Role.Init(ref refCsvData) == false)
            {
                Debug.LogError("Init Role failed, Id: " + refCsvData._id);
                return false;
            }

            MemberId = memberId;
            Health = Role.Health;
            State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NORMAL;

            return true;
        }
    }
}
