using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem.Tooltip
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Property

        private static LTDescr _delay;

        internal string _header;
        internal string _content;

        internal delegate void DlgHandleTipText(out string outContent, out string outHeader);
        internal DlgHandleTipText _dlgHandleTipText;

        #endregion  // Property

        #region Method

        public void OnPointerEnter(PointerEventData eventData)
        {
            _dlgHandleTipText(out _content, out _header);

            _delay = LeanTween.delayedCall(0.5f, () => {
                TooltipManager.Instance.Show(_content, _header);
            });
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.cancel(_delay.uniqueId);
            TooltipManager.Instance.Hide();
        }

        #endregion  // Method
    }
}