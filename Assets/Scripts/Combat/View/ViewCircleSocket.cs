using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewCircleSocket : MonoBehaviour
    {
        private Image _imgSocket = null;
        private Image _imgEmblem = null;

        internal bool Init()
        {
            _imgSocket = GetComponent<Image>();
            if (_imgSocket == null)
            {
                Debug.LogError("Not found ImageSocket");
                return false;
            }

            _imgEmblem = transform.Find("Emblem").GetComponent<Image>();
            if (_imgEmblem == null)
            {
                Debug.LogError("Not found ImageEmblem");
                return false;
            }

            return true;
        }

        internal void SetSocket(GameEnum.eRoleAttribute attribute)
        {
            string path = AssetsPath.SPRITE_ROLE_ATTRIBUTE_PATH + (int)attribute;
            _imgSocket.sprite = Resources.Load<Sprite>(path);
        }

        internal void SetEmblem(int emblemId)
        {
            string path = AssetsPath.SPRITE_ROLE_EMBLEM_PATH + emblemId.ToString().PadLeft(3, '0');
            _imgEmblem.sprite = Resources.Load<Sprite>(path);
        }

        internal void ShowEmblem()
        {
            _imgEmblem.gameObject.SetActive(true);
        }

        internal void HideEmblem()
        {
            _imgEmblem.gameObject.SetActive(false);
        }
    }
}