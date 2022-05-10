using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class UIEnergyBar : MonoBehaviour
    {
        private Image _imgBar = null;
        private Image _imgCube = null;
        private float _widthPerUnit = 0;

        private void Awake()
        {
            _imgBar =  transform.Find("Bar").GetComponent<Image>();
            _imgCube = transform.Find("Cube").GetComponent<Image>();
        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        internal void SetWidthPerPoint(int barPoint)
        {
            if (barPoint == 0)
            {
                return;   
            }

            _widthPerUnit = _imgBar.rectTransform.rect.width / barPoint;
        }

        internal void ChangeViewBar(int point)
        {
            _imgBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _widthPerUnit * point);
        }

        internal void ChangeViewCube(int cube)
        {
            string path = AssetsPath.SPRITE_ENERGY_CUBE_NUM_PATH + cube;
            _imgCube.sprite = Resources.Load<Sprite>(path);
        }
    }
}
