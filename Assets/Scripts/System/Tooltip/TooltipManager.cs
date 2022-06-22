using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Tooltip
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        internal Tooltip _tooltip;

        public override bool Init()
        {
            _tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
            if (_tooltip == null)
            {
                Debug.LogError("Not found Tooltip");
                return false;
            }

            Debug.Log("TooltipManager Init OK");
            return true;
        }

        public void Show()
        {
            _tooltip.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _tooltip.gameObject.SetActive(false);
        }
    }
}