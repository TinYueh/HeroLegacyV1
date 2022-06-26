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

        private RectTransform _rectTransform;

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
        internal void UpdatePosition()
        {
            if (transform.position != Input.mousePosition)
            {
                transform.position = Input.mousePosition;

                float pivotX = transform.position.x / Screen.width;
                float pivotY = transform.position.y / Screen.height;

                if (pivotY > 0.5f)
                {
                    pivotY += 0.1f;
                }
                else
                {
                    pivotY -= 0.1f;
                }

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
    }
}
