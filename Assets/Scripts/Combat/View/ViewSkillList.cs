using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class ViewSkillList : MonoBehaviour
    {
        [SerializeField]
        private float initPosX = 0;     // 0, 750
        [SerializeField]
        private float deltaPosX = 0;    // 150, -150

        private GameEnum.eCombatTeamType _teamType = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;
        private List<ViewSkill> _listViewSkill = new List<ViewSkill>();

        internal bool Init(GameEnum.eCombatTeamType teamType)
        {
            _teamType = teamType;

            for (int i = 0; i < GameConst.MAX_ROLE_SKILL; ++i)
            {
                float posX = initPosX + (deltaPosX * i);
                GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(AssetsPath.PREFAB_UI_SKILL), new Vector2(posX, 0), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform, false);

                ViewSkill viewSkill = obj.GetComponent<ViewSkill>();
                if (viewSkill.Init(_teamType) == false)
                {
                    Debug.LogError("Init ViewSkill failed, Index: " + i);
                }

                _listViewSkill.Add(viewSkill);
            }

            Hide();

            return true;
        }

        internal void ShowSkill(int index, int skillId, bool isEnable)
        {
            ViewSkill viewSkill = _listViewSkill[index];
            
            viewSkill.Set(skillId);
            
            if (isEnable)
            {
                viewSkill.Enable();
            }
            else
            {
                viewSkill.Disable();
            }
            
            viewSkill.Show();
        }

        internal void HideSkill(int index)
        {
            ViewSkill viewSkill = _listViewSkill[index];
            
            viewSkill.Hide();
        }

        internal bool IsShow()
        {
            return gameObject.activeInHierarchy;
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
