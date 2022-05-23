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

        internal void ChangeHealth(int deltaHealth)
        {
            int tmpHealth = Health + deltaHealth;

            SetHealth(tmpHealth);
        }

        internal void SetHealth(int health)
        {
            if (health < 0)
            {
                Health = 0;
            }
            else if (health > Role.Health)
            {
                Health = Role.Health;
            }
            else
            {
                Health = health;
            }
        }
    }
}
