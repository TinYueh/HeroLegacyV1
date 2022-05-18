using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class ViewCombatRole : MonoBehaviour
    {
        private Image _imgPortrait = null;
        private Image _imgEmblem = null;
        private Image _imgBar = null;
        private float _barInitLen = 0;

        private void Awake()
        {
            _imgPortrait = GetComponent<Image>();
            _imgEmblem = transform.Find("Emblem").GetComponent<Image>();
            _imgBar = transform.Find("HpBar").transform.Find("Bar").GetComponent<Image>();

            _barInitLen = _imgBar.rectTransform.rect.width;
        }

        internal void ChangeViewPortrait(int id)
        {
            string path = AssetsPath.SPRITE_ROLE_PORTRAIT_PATH + id.ToString().PadLeft(3, '0');
            _imgPortrait.sprite = Resources.Load<Sprite>(path);
        }

        internal void ChangeViewEmblem(int id)
        {
            string path = AssetsPath.SPRITE_ROLE_EMBLEM_PATH + id.ToString().PadLeft(3, '0');
            _imgEmblem.sprite = Resources.Load<Sprite>(path);
        }

        internal void ChangeViewBar(int value, int max)
        {
            _imgBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_barInitLen / max) * value);
        }

        internal void ChangeViewStateDying()
        {
            _imgPortrait.color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            _imgEmblem.color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        }

        internal void ChangeViewStateNormal()
        {
            _imgPortrait.color = new Color(1f, 1f, 1f, 1f);
            _imgEmblem.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}