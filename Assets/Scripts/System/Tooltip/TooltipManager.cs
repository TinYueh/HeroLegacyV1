using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Tooltip
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        #region Property

        internal Tooltip _tooltip;

        #endregion  // Property

        #region Init

        public override bool Init()
        {
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(AssetsPath.PREFAB_UI_TOOLTIP), Vector2.zero, Quaternion.identity);
            _tooltip = obj.GetComponent<Tooltip>();
            _tooltip.transform.SetParent(GameObject.Find("TooltipCanvas").transform, false);
            _tooltip.gameObject.SetActive(false);

            Debug.Log("TooltipManager Init OK");

            return true;
        }

        #endregion  // Init

        #region Show Hide

        public void Show(string content, string header)
        {
            _tooltip.SetText(content, header);
            _tooltip.gameObject.SetActive(true);
            _tooltip.UpdatePosition();
        }
        
        public void Hide()
        {
            _tooltip.gameObject.SetActive(false);
        }

        #endregion  // Show Hide
    }
}