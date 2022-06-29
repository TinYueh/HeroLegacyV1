using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class ViewSkillList : MonoBehaviour
    {
        #region Property

        [SerializeField]
        private float initPosX;     // 0, 750
        [SerializeField]
        private float deltaPosX;    // 150, -150

        private GameEnum.eCombatTeamType _teamType;

        private List<ViewSkill> _listViewSkill = new List<ViewSkill>();

        #endregion  // Property

        #region Init

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

        #endregion  // Init

        #region Logic

        internal bool IsShow()
        {
            return gameObject.activeInHierarchy;
        }

        #endregion  // Logic

        #region Show Hide

        internal void Show()
        {
            gameObject.SetActive(true);
        }

        internal void Hide()
        {
            gameObject.SetActive(false);
        }
        
        internal void ShowSkill(int index, int skillId, int cd, bool isEnable)
        {
            ViewSkill viewSkill = _listViewSkill[index];

            viewSkill.Set(skillId, cd);

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

        #endregion  // Show Hide
    }
}