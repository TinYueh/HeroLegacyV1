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

        internal bool Init()
        {
            _sprPicture = transform.Find("Picture").GetComponent<SpriteRenderer>();
            SetPicture(_pictureId);
            FullScreenSprite();
            HidePicture();

            return true;
        }

        internal void FullScreenSprite()
        {
            float cameraHeight = Camera.main.orthographicSize * 2;
            Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
            Vector2 spriteSize = _sprPicture.sprite.bounds.size;

            Vector2 scale = transform.localScale;
            if (cameraSize.x >= cameraSize.y)
            {
                // Landscape
                scale *= cameraSize.x / spriteSize.x;
            }
            else
            {
                // Portrait
                scale *= cameraSize.y / spriteSize.y;
            }

            transform.position = Vector2.zero;
            transform.localScale = scale;
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
