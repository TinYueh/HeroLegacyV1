using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSystem.Tooltip;
using GameSkill;

namespace GameCombat
{
    public class ViewSkill : MonoBehaviour
    {
        // 基本資料
        private int _skillId;
        private GameEnum.eCombatTeamType _teamType;
        private int _cd;
        // 技能
        private Image _imgSkill;
        private Button _btnSkill;
        private TooltipTrigger _tooltipTrigger;

        internal bool Init(GameEnum.eCombatTeamType teamType)
        {
            _teamType = teamType;

            _imgSkill = GetComponent<Image>();
            if (_imgSkill == null)
            {
                Debug.LogError("Not found ImageSkill");
                return false;
            }

            _btnSkill = GetComponent<Button>();
            if (_btnSkill == null)
            {
                Debug.LogError("Not found ButtonSkill");
                return false;
            }
            _btnSkill.onClick.AddListener(() => CombatManager.Instance.Controller.OnClickSkill(_teamType, _skillId));

            _tooltipTrigger = GetComponent<TooltipTrigger>();
            if (_tooltipTrigger == null)
            {
                Debug.LogError("Not found TooltipTrigger");
                return false;
            }
            _tooltipTrigger._dlgHandleTipText += HandleTipText;

            return true;
        }

        internal void Set(int skillId, int cd)
        {
            _skillId = skillId;
            _cd = cd;

            string path = AssetsPath.SPRITE_SKILL_PATH + _skillId.ToString().PadLeft(5, '0');
            _imgSkill.sprite = Resources.Load<Sprite>(path);
        }

        internal void Show()
        {
            gameObject.SetActive(true);
        }
        internal void Hide()
        {
            gameObject.SetActive(false);
        }

        internal void Enable()
        {
            _btnSkill.interactable = true;
        }
        internal void Disable()
        {
            _btnSkill.interactable = false;
        }

        internal void HandleTipText(out string outContent, out string outHeader)
        {
            outContent = "";
            outHeader = "";

            Skill skill;
            if (SkillManager.Instance.GetSkill(_skillId, out skill) == false)
            {
                return;
            }

            outHeader = skill.Name;

            outContent = "ID: " + skill.Id + "\n"
                + "消耗: " + skill.Cost + "\n"
                + "冷卻: " + _cd + " / " + skill.Cd + "\n"
                + "施放位: " + skill.PosType + "\n"
                + "範圍: " + skill.Range;
        }
    }
}
