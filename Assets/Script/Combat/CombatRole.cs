using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatRole
    {
        internal GameObject UICombatRole { get; set; } = null;

        internal int MemberId { get; set; } = 0;
        internal Role Role { get; set; } = null;

        internal bool Init(int memberId, int roleId)
        {
            Role = new Role();
            if (Role.Init(roleId) == false)
            {
                Debug.LogError("Init Role failed, Id: " + roleId);
                return false;
            }

            MemberId = memberId;

            return true;
        }
    }
}
