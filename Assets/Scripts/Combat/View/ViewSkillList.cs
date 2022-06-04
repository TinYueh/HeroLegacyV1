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
                if (viewSkill.Init() == false)
                {
                    Debug.LogError("Init ViewSkill failed, Index: " + i);
                }

                _listViewSkill.Add(viewSkill);
            }

            SetHide();

            return true;
        }

        internal void SetSkill(List<int> listSkill)
        {
            for (int i = 0; i < _listViewSkill.Count; ++i)
            {
                ViewSkill viewSkill = _listViewSkill[i];

                if (i < listSkill.Count)
                {
                    viewSkill.Set(listSkill[i]);
                    viewSkill.SetShow();
                }
                else
                {
                    viewSkill.SetHide();
                }
            }
        }

        internal bool IsShow()
        {
            return gameObject.activeInHierarchy;
        }

        internal void SetShow()
        {
            gameObject.SetActive(true);
        }

        internal void SetHide()
        {
            gameObject.SetActive(false);
        }
    }
}
