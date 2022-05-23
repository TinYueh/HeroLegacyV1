using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatRole
    {
        internal ViewCombatRole _viewCombatRole;    // Todo: ≤æ®ÏCombatTeam±±∫ﬁ
        internal int MemberId { get; set; }
        internal Role Role { get; set; }
        internal GameEnum.eCombatRoleState State { get; set; }
        internal int Health { get; private set; }
        internal int NormalDamage { get; set; }

        internal bool Init(int memberId, ref RoleCsvData refCsvData)
        {
            Role = new Role();
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

            _viewCombatRole.SetHealthBar(Health, Role.Health);

            if (State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NORMAL
                && Health == 0)
            {
                State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING;

                _viewCombatRole.SetStateDying();
            }
            else if (State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING
                && Health > 0)
            {
                State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NORMAL;

                _viewCombatRole.SetStateNormal();
            }
        }
    }
}
