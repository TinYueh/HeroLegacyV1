using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class UICombatRole : MonoBehaviour
    {
        private Image _imgPortrait = null;
        private Image _imgEmblem = null;

        private void Awake()
        {
            _imgPortrait = GetComponent<Image>();
            _imgEmblem = transform.Find("Emblem").GetComponent<Image>();
        }

        private void Start()
        {

        }

        private void Update()
        {

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
    }
}