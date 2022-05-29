using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewCombatStats : MonoBehaviour
    {
        private Image _imgFirstToken = null;
        internal bool Init()
        {
            _imgFirstToken = transform.Find("FirstToken").GetComponent<Image>();
            if (_imgFirstToken == null)
            {
                Debug.LogError("Not found ImgFirstToken");
                return false;
            }

            return true;
        }

        internal void ShowFirstToken()
        {
            _imgFirstToken.gameObject.SetActive(true);
        }

        internal void HideFirstToken()
        {
            _imgFirstToken.gameObject.SetActive(false);
        }
    }
}
