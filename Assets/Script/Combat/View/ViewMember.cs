using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class ViewMember : MonoBehaviour
    {
        [SerializeField]
        private float initPosX;
        [SerializeField]
        private float deltaPosX;

        private Dictionary<int, ViewCombatRole> _dicVwCombatRole;  // <memberId, ViewCombatRole>

        internal bool Init()
        {
            _dicVwCombatRole = new Dictionary<int, ViewCombatRole>();

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                int memberId = i + 1;

                float posX = initPosX + (deltaPosX * i);
                GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(AssetsPath.PREFAB_UI_COMBAT_ROLE), new Vector2(posX, 0), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform, false);

                ViewCombatRole vwCombatRole = obj.GetComponent<ViewCombatRole>();
                if (vwCombatRole.Init() == false)
                {
                    Debug.LogError("Init ViewCombatRole failed, MemberId: " + memberId);
                }

                _dicVwCombatRole.Add(memberId, vwCombatRole);

                HideCombatRole(memberId);
            }

            return true;
        }

        internal void SetCombatRole(int memberId, CombatRole combatRole)
        {
            ViewCombatRole vwCombatRole = _dicVwCombatRole[memberId];
            if (vwCombatRole == null)
            {
                Debug.LogError("Not found ViewCombatRole, MemberId: " + memberId);
                return;
            }

            vwCombatRole.SetPortrait(combatRole.Role.Portrait);
            vwCombatRole.SetEmblem(combatRole.Role.Emblem);
        }

        internal void ShowCombatRole(int memberId)
        {
            ViewCombatRole vwCombatRole;
            if (_dicVwCombatRole.TryGetValue(memberId, out vwCombatRole) == false)
            {
                Debug.LogError("Not found ViewCombatRole, MemberId: " + memberId);
                return;
            }

            vwCombatRole.gameObject.SetActive(true);
        }

        internal void HideCombatRole(int memberId)
        {
            ViewCombatRole vwCombatRole;
            if (_dicVwCombatRole.TryGetValue(memberId, out vwCombatRole) == false)
            {
                Debug.LogError("Not found ViewCombatRole, MemberId: " + memberId);
                return;
            }

            vwCombatRole.gameObject.SetActive(false);
        }
    }
}