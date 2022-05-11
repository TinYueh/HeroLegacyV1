using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatRole
    {
        internal UICombatRole _uiCombatRole = null;
        internal int MemberId { get; set; } = 0;
        internal Role Role { get; set; } = null;
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

            _uiCombatRole.ChangeViewBar(Life, Role.Life);
        }
    }
}