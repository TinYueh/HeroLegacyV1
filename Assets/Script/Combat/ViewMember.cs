using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class ViewMember : MonoBehaviour
    {
        [SerializeField]
        internal float initPosX = 0f;
        [SerializeField]
        internal float deltaPosX = 0f;

        internal Dictionary<int, GameObject> _dicUICombatRole = new Dictionary<int, GameObject>();
    }
}