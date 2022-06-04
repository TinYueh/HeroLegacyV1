using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class ViewMemberList : MonoBehaviour
    {
        [SerializeField]
        private float initPosX = 0;     // 0, 750
        [SerializeField]
        private float deltaPosX = 0;    // 150, -150

        private GameEnum.eCombatTeamType _teamType = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;
        private Dictionary<int, ViewCombatRole> _dicViewCombatRole = new Dictionary<int, ViewCombatRole>();   // <memberId, ViewCombatRole>

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

                HideCombatRole(memberId);
            }

            return true;
        }

        internal bool GetCombatRole(int memberId, out ViewCombatRole outViewCombatRole)
        {
            if (_dicViewCombatRole.TryGetValue(memberId, out outViewCombatRole) == false)
            {
                Debug.LogError("Not found ViewCombatRole, MemberId: " + memberId);
                return false;
            }

            return true;
        }

        internal void SetCombatRole(int memberId, CombatRole combatRole)
        {
            ViewCombatRole viewCombatRole = null;
            if (GetCombatRole(memberId, out viewCombatRole) == false)
            {
                return;
            }

            viewCombatRole.SetPortrait(combatRole.Role.Portrait);
            viewCombatRole.SetEmblem(combatRole.Role.Emblem);
        }

        internal void ShowCombatRole(int memberId)
        {
            ViewCombatRole viewCombatRole = null;
            if (GetCombatRole(memberId, out viewCombatRole) == false)
            {
                return;
            }

            viewCombatRole.gameObject.SetActive(true);
        }

        internal void HideCombatRole(int memberId)
        {
            ViewCombatRole viewCombatRole = null;
            if (GetCombatRole(memberId, out viewCombatRole) == false)
            {
                return;   
            }

            viewCombatRole.gameObject.SetActive(false);
        }
    }
}