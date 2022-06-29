using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewCircleSocket : MonoBehaviour
    {
        #region Property

        private Image _imgSocket;

        private Image _imgEmblem;
        private Button _btnEmblem;

        private GameEnum.eCombatTeamType _teamType;
        private int _posId;

        #endregion  // Property

        #region Init

        internal bool Init(GameEnum.eCombatTeamType teamType, int posId)
        {
            _teamType = teamType;
            _posId = posId;

            _imgSocket = GetComponent<Image>();
            if (_imgSocket == null)
            {
                Debug.LogError("Not found ImageSocket");
                return false;
            }

            _imgEmblem = transform.Find("EmblemButton").GetComponent<Image>();
            if (_imgEmblem == null)
            {
                Debug.LogError("Not found ImageEmblem");
                return false;
            }

            _btnEmblem = transform.Find("EmblemButton").GetComponent<Button>();
            if (_btnEmblem == null)
            {
                Debug.LogError("Not found ButtonEmblem");
                return false;
            }
            _btnEmblem.onClick.AddListener(() => CombatManager.Instance.Controller.OnClickCircleSocketEmblem(_teamType, _posId));

            HideEmblem();

            return true;
        }

        #endregion  // Init

        #region Get Set

        internal void SetSocket(GameEnum.eRoleAttribute attribute)
        {
            string path = AssetsPath.SPRITE_ROLE_ATTRIBUTE_PATH + (int)attribute;
            _imgSocket.sprite = Resources.Load<Sprite>(path);
        }

        internal void SetEmblem(int emblemId)
        {
            string path = AssetsPath.SPRITE_ROLE_EMBLEM_PATH + emblemId.ToString().PadLeft(3, '0');
            _imgEmblem.sprite = Resources.Load<Sprite>(path);
        }

        #endregion  // Get Set

        #region Show Hide

        internal void ShowEmblem()
        {
            _imgEmblem.gameObject.SetActive(true);
        }

        internal void HideEmblem()
        {
            _imgEmblem.gameObject.SetActive(false);
        }

        #endregion  // Show Hide
    }
}