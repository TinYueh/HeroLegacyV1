using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class CoverController : MonoBehaviour
    {
        [SerializeField]
        private int _pictureId = 0;

        SpriteRenderer _sprPicture = null;

        private void Awake()
        {
            _sprPicture = transform.Find("Picture").GetComponent<SpriteRenderer>();
            SetPicture(_pictureId);
            HidePicture();
        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        internal void SetPicture(int pictureId)
        {
            _sprPicture.sprite = Resources.Load<Sprite>(AssetsPath.SPRITE_PICTURE_PATH + pictureId.ToString().PadLeft(3, '0'));
        }

        internal void ShowPicture()
        {
            _sprPicture.gameObject.SetActive(true);
        }

        internal void HidePicture()
        {
            _sprPicture.gameObject.SetActive(false);
        }
    }
}
