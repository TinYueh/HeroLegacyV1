using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewEnergyBar : MonoBehaviour
    {
        private Image _imgBar = null;
        private Image _imgOrb = null;
        private float _barInitLen = 0f;
        private float _barLenPerUnit = 0f;
        GameEnum.eCombatTeamType _teamType = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;

        internal bool Init(GameEnum.eCombatTeamType teamType)
        {
            _teamType = teamType;

            _imgBar =  transform.Find("Bar").GetComponent<Image>();
            if (_imgBar == null)
            {
                Debug.LogError("Not found ImageBar");
                return false;
            }

            _imgOrb = transform.Find("Orb").GetComponent<Image>();
            if (_imgOrb == null)
            {
                Debug.LogError("Not found ImageOrb");
                return false;
            }

            _barInitLen = _imgBar.rectTransform.rect.width;

            _barLenPerUnit = _barInitLen / GameConst.BAR_ENERGY_POINT;

            return true;
        }

        internal void SetEnergyBar(int value)
        {
            _imgBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _barLenPerUnit * value);
        }

        internal void SetEnergyOrb(int value)
        {
            string path = AssetsPath.SPRITE_ENERGY_ORB_NUM_PATH + value;
            _imgOrb.sprite = Resources.Load<Sprite>(path);
        }
    }
}
