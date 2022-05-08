using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class UIRoleList : MonoBehaviour
    {
        [SerializeField]
        internal float initialPosX = 0f;
        [SerializeField]
        internal float deltaPosX = 0f;

        internal Dictionary<int, GameObject> _dicUICombatRole = new Dictionary<int, GameObject>();

        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}
