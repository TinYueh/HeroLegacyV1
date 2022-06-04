using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewCombatStats : MonoBehaviour
    {
        private Image _imgFirstToken = null;
        private GameEnum.eCombatTeamType _teamType = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;

        internal bool Init(GameEnum.eCombatTeamType teamType)
        {
            _teamType = teamType;

            _imgFirstToken = transform.Find("FirstToken").GetComponent<Image>();
            if (_imgFirstToken == null)
            {
                Debug.LogError("Not found ImageFirstToken");
                return false;
            }

            return true;
        }

        internal void ShowFirstToken()
        {
            _imgFirstToken.gameObject.SetActive(true);
        }

        internal void HideFirstToken()
        {
            _imgFirstToken.gameObject.SetActive(false);
        }
    }
}
