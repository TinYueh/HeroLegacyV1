using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewCombatRole : MonoBehaviour
    {
        private Image _imgPortrait = null;
        private Button _btnPortrait = null;

        private Image _imgEmblem = null;
        private Image _imgHealthBar = null;
        private float _barInitLen = 0f;

        private GameEnum.eCombatTeamType _teamType = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;
        private int _memberId = 0;

        internal bool Init(GameEnum.eCombatTeamType teamType, int memberId)
        {
            _teamType = teamType;
            _memberId = memberId;

            _imgPortrait = transform.Find("PortraitButton").GetComponent<Image>();
            if (_imgPortrait == null)
            {
                Debug.LogError("Not found ImagePortrait");
                return false;
            }

            _btnPortrait = transform.Find("PortraitButton").GetComponent<Button>();
            if (_btnPortrait == null)
            {
                Debug.LogError("Not found ButtonPortrait");
                return false;
            }
            _btnPortrait.onClick.AddListener(() => CombatManager.Instance.CombatController.OnClickCombatRolePortrait(_teamType, _memberId));

            _imgEmblem = transform.Find("Emblem").GetComponent<Image>();
            if (_imgEmblem == null)
            {
                Debug.LogError("Not found ImageEmblem");
                return false;
            }

            _imgHealthBar = transform.Find("HealthBar").transform.Find("Bar").GetComponent<Image>();
            if (_imgHealthBar == null)
            {
                Debug.LogError("Not found ImageHealthBar");
                return false;
            }

            _barInitLen = _imgHealthBar.rectTransform.rect.width;

            return true;
        }

        internal void SetPortrait(int portraitId)
        {
            string path = AssetsPath.SPRITE_ROLE_PORTRAIT_PATH + portraitId.ToString().PadLeft(3, '0');
            _imgPortrait.sprite = Resources.Load<Sprite>(path);
        }

        internal void SetEmblem(int emblemId)
        {
            string path = AssetsPath.SPRITE_ROLE_EMBLEM_PATH + emblemId.ToString().PadLeft(3, '0');
            _imgEmblem.sprite = Resources.Load<Sprite>(path);
        }

        internal void SetHealthBar(int value, int max)
        {
            _imgHealthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_barInitLen / max) * value);
        }

        internal void SetStateDying()
        {
            _imgEmblem.color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
            _btnPortrait.interactable = false;
        }

        internal void SetStateAlive()
        {
            _imgEmblem.color = new Color(1f, 1f, 1f, 1f);
            _btnPortrait.interactable = true;
        }
    }
}