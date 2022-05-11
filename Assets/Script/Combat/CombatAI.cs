using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatAI
    {
        internal bool GetNextAction()
        {
            int rand = Random.Range(0, 2);

            return (rand != 0);
        }
    }
}
