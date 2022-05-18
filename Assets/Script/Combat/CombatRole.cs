using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatRole
    {
        internal ViewCombatRole _viewCombatRole = null;
        internal int MemberId { get; set; } = 0;
        internal Role Role { get; set; } = null;

        internal GameEnum.eCombatRoleState State { get; set; } = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NA;
        internal int Life { get; private set; } = 0;
        internal int NormalDamage { get; set; } = 0;

        internal bool Init(int memberId, int roleId)
        {
            Role = new Role();
            if (Role.Init(roleId) == false)
            {
                Debug.LogError("Init Role failed, Id: " + roleId);
                return false;
            }

            MemberId = memberId;
            Life = Role.Life;
            State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NORMAL;

            return true;
        }

        internal void ChangeLife(int deltaLife)
        {
            int tmpLife = Life + deltaLife;

            SetLife(tmpLife);
        }

        internal void SetLife(int life)
        {
            if (life < 0)
            {
                Life = 0;
            }
            else if (life > Role.Life)
            {
                Life = Role.Life;
            }
            else
            {
                Life = life;
            }

            _viewCombatRole.ChangeViewBar(Life, Role.Life);

            if (State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NORMAL
                && Life == 0)
            {
                State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING;

                _viewCombatRole.ChangeViewStateDying();
            }
            else if (State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING
                && Life > 0)
            {
                State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NORMAL;

                _viewCombatRole.ChangeViewStateNormal();
            }
        }
    }
}
