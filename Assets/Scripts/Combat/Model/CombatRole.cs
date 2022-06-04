using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatRole
    {
        private ViewCombatRole _viewCombatRole = null;    // View

        internal int MemberId { get; private set; } = 0;
        internal int PosId { get; private set; } = 0;
        internal Role Role { get; private set; } = new Role();
        internal GameEnum.eCombatRoleState State { get; private set; } = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NA;
        internal int Health { get; private set; } = 0;
        internal int NormalDamage { get; set; } = 0;

        internal bool Init(int memberId, int posId, RoleCsvData csvData, ViewCombatRole viewCombatRole)
        {
            if (Role.Init(csvData) == false)
            {
                Debug.LogError("Init Role failed, RoleId: " + csvData._id);
                return false;
            }

            MemberId = memberId;
            PosId = posId;
            Health = Role.Health;
            State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NORMAL;
            _viewCombatRole = viewCombatRole;    // Attach View

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

            if (Health == 0)
            {
                State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING;
                _viewCombatRole.SetStateDying();
            }
            else
            {
                State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NORMAL;
                _viewCombatRole.SetStateNormal();
            }
        }
    }
}
