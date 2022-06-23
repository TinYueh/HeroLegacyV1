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
        private TextMeshProUGUI _header;
        [SerializeField]
        private TextMeshProUGUI _content;
        [SerializeField]
        private LayoutElement _layoutElement;
        [SerializeField]
        private int _charWrapLimit;

        private bool _isTextUpdate;

        private void Update()
        {
            if (_isTextUpdate)
            {
                int headerLength = _header.text.Length;
                int contentLength = _content.text.Length;

                _layoutElement.enabled = (headerLength > _charWrapLimit || contentLength > _charWrapLimit);

                _isTextUpdate = false;
            }
        }

        internal void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                _header.gameObject.SetActive(false);
            }
            else
            {
                _header.gameObject.SetActive(true);
                _header.text = header;
            }

            _content.text = content;

            _isTextUpdate = true;
        }
    }
}
