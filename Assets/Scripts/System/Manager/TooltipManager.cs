using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Tooltip
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        public override bool Init()
        {
            Debug.Log("TooltipManager Init OK");
            return true;
        }
    }
}
