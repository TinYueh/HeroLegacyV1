using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CircleSocket
    {
        internal int PosId { get; private set; }

        internal GameEnum.eCircleSocketType Type { get; private set; }

        private CombatRole _combatRole;

        internal void Init(int posId)
        {
            PosId = posId;
            Type = GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_SPACE;
        }

        internal void Setup(GameEnum.eCircleSocketType socketType, ref CombatRole refCombatRole)
        {
            Type = socketType;
            _combatRole = refCombatRole;
        }
    }
}
