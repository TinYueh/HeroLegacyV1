using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class BackgroundController : MonoBehaviour
    {
        SpriteRenderer _sprPicture = null;

        private void Awake()
        {
            _sprPicture = transform.Find("Picture").GetComponent<SpriteRenderer>();
            _sprPicture.sprite = Resources.Load<Sprite>(AssetsPath.SPRITE_PICTURE_PATH + "001");
            _sprPicture.gameObject.SetActive(true);
        }

        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}
