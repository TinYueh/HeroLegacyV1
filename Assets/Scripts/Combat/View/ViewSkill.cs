using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewSkill : MonoBehaviour
    {
        private Image _imgSkill = null;
        private Button _btnSkill = null;

        private GameEnum.eCombatTeamType _teamType = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;
        private int _skillId = 0;

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
            _btnSkill.onClick.AddListener(() => CombatManager.Instance.CombatController.OnClickSkill(_teamType, _skillId));

            return true;
        }

        internal void Set(int skillId)
        {
            _skillId = skillId;

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
    }
}
