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
        private float _barInitLen = 0;
        private float _barLenPerUnit = 0;

        private void Awake()
        {
            _imgBar =  transform.Find("Bar").GetComponent<Image>();
            _imgCube = transform.Find("Cube").GetComponent<Image>();

            _barInitLen = _imgBar.rectTransform.rect.width;

            SetBarLenPerUnit(GameConst.BAR_ENERGY_POINT);
        }

        internal void SetBarLenPerUnit(int max)
        {
            _barLenPerUnit = _barInitLen / max;
        }

        internal void ChangeViewBar(int value)
        {
            _imgBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _barLenPerUnit * value);
        }

        internal void ChangeViewCube(int value)
        {
            string path = AssetsPath.SPRITE_ENERGY_CUBE_NUM_PATH + value;
            _imgCube.sprite = Resources.Load<Sprite>(path);
        }
    }
}
