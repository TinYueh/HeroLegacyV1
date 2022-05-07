using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class UIEnergyBar : MonoBehaviour
    {
        private Image _bar = null;
        private Image _cube = null;
        private int _energyPoint = 0;
        //private int _energyCube = 0;
        private float _widthPerUnit = 0;

        private void Awake()
        {
            _bar =  transform.Find("Bar").GetComponent<Image>();
            _cube = transform.Find("Cube").GetComponent<Image>();
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

            _widthPerUnit = _bar.rectTransform.rect.width / barPoint;
        }

        internal void ChangeEnergyPoint(int deltaPoint)
        {
            int tmpPoint = _energyPoint + deltaPoint;

            SetEnergyPoint(tmpPoint);
        }

        internal void SetEnergyPoint(int point)
        {
            if (point < 0)
            {
                _energyPoint = 0;
            }
            else if (point > GameConst.MAX_ENERGY_POINT)
            {
                _energyPoint = GameConst.MAX_ENERGY_POINT;
            }
            else
            {
                _energyPoint = point;
            }

            int showPoint = _energyPoint % GameConst.BAR_ENERGY_POINT;
            int showCube = _energyPoint / GameConst.BAR_ENERGY_POINT;

            _bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _widthPerUnit * showPoint);

            string path = AssetsPath.ENERGY_CUBE_NUM_PATH + showCube;
            _cube.sprite = Resources.Load<Sprite>(path);

            if (_energyPoint == GameConst.MAX_ENERGY_POINT)
            {
                // Todo: Lock EnergyBar
            }
        }
    }
}
