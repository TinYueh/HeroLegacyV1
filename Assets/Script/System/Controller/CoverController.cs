using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class CoverController : MonoBehaviour
    {
        SpriteRenderer _sprPicture = null;

        private void Awake()
        {
            _sprPicture = transform.Find("Picture").GetComponent<SpriteRenderer>();
        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        internal void ShowPicture()
        {
            
        }
    }
}
