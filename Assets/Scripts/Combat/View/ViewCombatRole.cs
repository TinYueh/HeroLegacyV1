using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSystem.Tooltip;

namespace GameCombat
{
    public class ViewCombatRole : MonoBehaviour
    {
        #region Property

        // 基本資料
        private GameEnum.eCombatTeamType _teamType;
        private int _memberId;
        // 頭像
        private Image _imgPortrait;
        private Button _btnPortrait;
        private TooltipTrigger _tooltipTrigger;
        // 徽章
        private Image _imgEmblem;
        // 生命條
        private Image _imgHealthBar;
        private float _barInitLen;

        #endregion  // Property

        #region Init

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
            _btnPortrait.onClick.AddListener(() => CombatManager.Instance.Controller.OnClickCombatRolePortrait(_teamType, _memberId));

            _tooltipTrigger = transform.Find("PortraitButton").GetComponent<TooltipTrigger>();
            if (_tooltipTrigger == null)
            {
                Debug.LogError("Not found TooltipTrigger");
                return false;
            }
            _tooltipTrigger._dlgHandleTipText += HandleTipText;

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

        #endregion  // Init

        #region Get Set

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

        #endregion  // Get Set

        #region Method

        internal void HandleTipText(out string outContent, out string outHeader)
        {
            outContent = string.Empty;
            outHeader = string.Empty;

            CombatRole combatRole;
            if (CombatManager.Instance.Controller.GetCombatRoleByMember(_teamType, _memberId, out combatRole) == false)
            {
                return;
            }

            outHeader = combatRole.Role.Name;

            string markPtk = string.Empty;
            string markMtk = string.Empty;
            if (combatRole.Role.AttackType == GameEnum.eRoleAttackType.E_ROLE_ATTACK_TYPE_PHYSICAL)
            {
                markPtk = " *";
            }
            else
            {
                markMtk = " *";
            }

            outContent = "ID: " + combatRole.Role.Id + "\n" 
                + "生命: " + combatRole.Health + " / " + combatRole.Role.Health + "\n"
                + "物攻: " + combatRole.Role.Ptk + markPtk + "\n"
                + "物防: " + combatRole.Role.Pef + "\n"
                + "魔攻: " + combatRole.Role.Mtk + markMtk + "\n"
                + "魔防: " + combatRole.Role.Mef;
        }

        #endregion  // Method
    }
}