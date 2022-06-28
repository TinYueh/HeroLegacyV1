using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Tooltip
{
    public class Tooltip : MonoBehaviour
    {
        #region Property

        [SerializeField]
        private TextMeshProUGUI _header;
        [SerializeField]
        private TextMeshProUGUI _content;
        [SerializeField]
        private LayoutElement _layoutElement;
        [SerializeField]
        private int _charWrapLimit;

        private bool _isTextUpdate;
        private RectTransform _rectTransform;

        #endregion  // Property

        #region Mono

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_isTextUpdate)
            {
                int headerLength = _header.text.Length;
                int contentLength = _content.text.Length;

                _layoutElement.enabled = (headerLength > _charWrapLimit || contentLength > _charWrapLimit);

                _isTextUpdate = false;
            }

            UpdatePosition();
        }

        #endregion  // Mono

        #region Method

        internal void UpdatePosition()
        {
            if (transform.position != Input.mousePosition)
            {
                // Todo: 改成相對 UI 的位置, 而不跟隨滑鼠

                transform.position = Input.mousePosition;

                float pivotX = transform.position.x / Screen.width;
                float pivotY = transform.position.y / Screen.height;
                
                pivotY += (pivotY > 0.5) ? 0.1f : -0.1f;

                _rectTransform.pivot = new Vector2(pivotX, pivotY);
            }
        }

        internal void SetText(string content, string header)
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

        #endregion  // Method
    }
}
