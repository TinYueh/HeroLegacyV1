using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class ViewMember : MonoBehaviour
    {
        [SerializeField]
        private float initPosX = 0;     // 0, 750
        [SerializeField]
        private float deltaPosX = 0;    // 150, -150

        private Dictionary<int, ViewCombatRole> _dicVwCombatRole = new Dictionary<int, ViewCombatRole>();   // <memberId, ViewCombatRole>

        internal bool Init()
        {
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

        internal bool GetCombatRole(int memberId, out ViewCombatRole outVwCombatRole)
        {
            if (_dicVwCombatRole.TryGetValue(memberId, out outVwCombatRole) == false)
            {
                Debug.LogError("Not found ViewCombatRole, MemberId: " + memberId);
                return false;
            }

            return true;
        }

        internal void SetCombatRole(int memberId, ref CombatRole refCombatRole)
        {
            ViewCombatRole vwCombatRole = null;
            if (GetCombatRole(memberId, out vwCombatRole) == false)
            {
                return;
            }

            vwCombatRole.SetPortrait(refCombatRole.Role.Portrait);
            vwCombatRole.SetEmblem(refCombatRole.Role.Emblem);
        }

        internal void ShowCombatRole(int memberId)
        {
            ViewCombatRole vwCombatRole = null;
            if (GetCombatRole(memberId, out vwCombatRole) == false)
            {
                return;
            }

            vwCombatRole.gameObject.SetActive(true);
        }

        internal void HideCombatRole(int memberId)
        {
            ViewCombatRole vwCombatRole = null;
            if (GetCombatRole(memberId, out vwCombatRole) == false)
            {
                return;   
            }

            vwCombatRole.gameObject.SetActive(false);
        }
    }
}