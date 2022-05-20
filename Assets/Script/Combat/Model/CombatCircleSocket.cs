using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatCircleSocket
    {
        internal int SocketId { get; private set; }

        internal GameEnum.eCircleSocketType Type { get; private set; }

        internal void Init(int socketId)
        {
            SocketId = socketId;
        }
    }
}
