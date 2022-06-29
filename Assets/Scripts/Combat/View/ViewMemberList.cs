using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class ViewMemberList : MonoBehaviour
    {
        #region Property

        [SerializeField]
        private float initPosX;     // 0, 750
        [SerializeField]
        private float deltaPosX;    // 150, -150

        private GameEnum.eCombatTeamType _teamType;

        private Dictionary<int, ViewCombatRole> _dicViewCombatRole = new Dictionary<int, ViewCombatRole>();   // <memberId, ViewCombatRole>

        #endregion  // Property

        #region Init

        internal bool Init(GameEnum.eCombatTeamType teamType)
        {
            _teamType = teamType;

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                int memberId = i + 1;

                float posX = initPosX + (deltaPosX * i);
                GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(AssetsPath.PREFAB_UI_COMBAT_ROLE), new Vector2(posX, 0), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform, false);

                ViewCombatRole viewCombatRole = obj.GetComponent<ViewCombatRole>();
                if (viewCombatRole.Init(_teamType, memberId) == false)
                {
                    Debug.LogError("Init ViewCombatRole failed, MemberId: " + memberId);
                }

                _dicViewCombatRole.Add(memberId, viewCombatRole);

                HideViewCombatRole(memberId);
            }

            return true;
        }

        #endregion  // Init

        #region Get Set

        internal bool GetViewCombatRole(int memberId, out ViewCombatRole outViewCombatRole)
        {
            if (_dicViewCombatRole.TryGetValue(memberId, out outViewCombatRole) == false)
            {
                Debug.LogError("Not found ViewCombatRole, MemberId: " + memberId);
                return false;
            }

            return true;
        }

        internal void SetViewCombatRole(int memberId, CombatRole combatRole)
        {
            ViewCombatRole viewCombatRole;
            if (GetViewCombatRole(memberId, out viewCombatRole) == false)
            {
                return;
            }

            viewCombatRole.SetPortrait(combatRole.Role.Portrait);
            viewCombatRole.SetEmblem(combatRole.Role.Emblem);
        }

        #endregion  // Get Set

        #region Show Hide

        internal void ShowViewCombatRole(int memberId)
        {
            ViewCombatRole viewCombatRole;
            if (GetViewCombatRole(memberId, out viewCombatRole) == false)
            {
                return;
            }

            viewCombatRole.gameObject.SetActive(true);
        }

        internal void HideViewCombatRole(int memberId)
        {
            ViewCombatRole viewCombatRole;
            if (GetViewCombatRole(memberId, out viewCombatRole) == false)
            {
                return;   
            }

            viewCombatRole.gameObject.SetActive(false);
        }

        #endregion  // Show Hide
    }
}