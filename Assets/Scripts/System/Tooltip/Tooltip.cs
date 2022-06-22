using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Tooltip
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField]
        internal TextMeshProUGUI _header;
        [SerializeField]
        internal TextMeshProUGUI _content;
        [SerializeField]
        public LayoutElement _layoutElement;
        [SerializeField]
        private int _charWrapLimit;

        private void Update()
        {
            int headerLength = _header.text.Length;
            int contentLength = _content.text.Length;

            _layoutElement.enabled = (headerLength > _charWrapLimit || contentLength > _charWrapLimit);
        }
    }
}
