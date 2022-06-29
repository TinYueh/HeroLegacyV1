using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewCombatStats : MonoBehaviour
    {
        #region Property

        private Image _imgFirstToken;
        private GameEnum.eCombatTeamType _teamType;

        #endregion  // Property

        #region Init

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

        #endregion  // Init

        #region Show Hide

        internal void ShowFirstToken()
        {
            _imgFirstToken.gameObject.SetActive(true);
        }

        internal void HideFirstToken()
        {
            _imgFirstToken.gameObject.SetActive(false);
        }

        #endregion  // Show Hide
    }
}