using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewCombatRole : MonoBehaviour
    {
        private Image _imgPortrait = null;
        private Image _imgEmblem = null;
        private Image _imgHealthBar = null;
        private Button _btnPortrait = null;
        private float _barInitLen = 0f;

        internal bool Init()
        {
            _imgPortrait = transform.Find("PortraitButton").GetComponent<Image>();
            if (_imgPortrait == null)
            {
                Debug.LogError("Not found ImgPortrait");
                return false;
            }

            _btnPortrait = transform.Find("PortraitButton").GetComponent<Button>();
            if (_btnPortrait == null)
            {
                Debug.LogError("Not found BtnPortrait");
                return false;
            }

            _imgEmblem = transform.Find("Emblem").GetComponent<Image>();
            if (_imgEmblem == null)
            {
                Debug.LogError("Not found ImgEmblem");
                return false;
            }

            _imgHealthBar = transform.Find("HealthBar").transform.Find("Bar").GetComponent<Image>();
            if (_imgHealthBar == null)
            {
                Debug.LogError("Not found ImgHealthBar");
                return false;
            }

            _barInitLen = _imgHealthBar.rectTransform.rect.width;

            return true;
        }

        internal void SetPortrait(int portraitId)
        {
            string path = AssetsPath.SPRITE_ROLE_PORTRAIT_PATH + portraitId.ToString().PadLeft(3, '0');
            _imgPortrait.sprite = Resources.Load<Sprite>(path);
        }

        internal void SetEmblem(int emblemId)
        {
            string path = AssetsPath.SPRITE_ROLE_EMBLEM_PATH + emblemId.ToString().PadLeft(3, '0');
            _imgEmblem.sprite = Resources.Load<Sprite>(path);
        }

        internal void SetHealthBar(int value, int max)
        {
            _imgHealthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_barInitLen / max) * value);
        }

        internal void SetStateDying()
        {
            _imgPortrait.color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
            _imgEmblem.color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        }

        internal void SetStateNormal()
        {
            _imgPortrait.color = new Color(1f, 1f, 1f, 1f);
            _imgEmblem.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}