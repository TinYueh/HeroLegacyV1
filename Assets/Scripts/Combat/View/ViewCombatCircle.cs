using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewCombatCircle : MonoBehaviour
    {
        private enum eCombatCircleState
        {
            E_COMBAT_CIRCLE_STATE_NA = 0,
            E_COMBAT_CIRCLE_STATE_STATIONARY,   // 靜止
            E_COMBAT_CIRCLE_STATE_ROTATE,       // 旋轉
            E_COMBAT_CIRCLE_STATE_LIMIT,
        }

        [SerializeField]
        private float _adjustRadiusForSocket = 0f;  // -70f 微調 CircleSocket 與圓心的距離
        [SerializeField]
        private float _initAngle = 0f;              // 45f 和 225f 戰鬥開始時的起始角度

        internal float RotateAnglePerFrame { get; set; } = 2f;  // 每個 frame 的旋轉角度
        internal float RotateAnglePerFrameActual { get; set; } = 0f;
        internal float RotateAngleRemaining { get; set; } = 0f;

        private GameEnum.eCombatTeamType _teamType = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;

        private eCombatCircleState _combatCircleState = eCombatCircleState.E_COMBAT_CIRCLE_STATE_NA;
        private Dictionary<int, ViewCircleSocket> _dicViewCircleSocket = new Dictionary<int, ViewCircleSocket>();

        internal bool Init(GameEnum.eCombatTeamType teamType)
        {
            _teamType = teamType;

            float radius = (transform.GetComponent<RectTransform>().sizeDelta.x + _adjustRadiusForSocket) / 2;

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                int posId = i + 1;

                float posX = radius * Mathf.Cos(-60 * i * Mathf.Deg2Rad);
                float posY = radius * Mathf.Sin(-60 * i * Mathf.Deg2Rad);

                GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(AssetsPath.PREFAB_UI_CIRCLE_SOCKET), new Vector2(posX, posY), Quaternion.identity);
                obj.transform.SetParent(transform, false);

                ViewCircleSocket viewCircleSocket = obj.GetComponent<ViewCircleSocket>();
                if (viewCircleSocket.Init(teamType, posId) == false)
                {
                    Debug.LogError("Init ViewCircleSocket failed, PosId: " + posId);
                }

                _dicViewCircleSocket.Add(posId, viewCircleSocket);
            }

            Rotate(_initAngle);

            _combatCircleState = ViewCombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STATIONARY;

            return true;
        }

        private void FixedUpdate()
        {
            if (_combatCircleState == ViewCombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE)
            {
                float rotateAngleDelta = 0f;

                if (RotateAnglePerFrameActual > 0)
                {
                    rotateAngleDelta = (RotateAngleRemaining < RotateAnglePerFrameActual) ? RotateAngleRemaining : RotateAnglePerFrameActual;
                    RotateAngleRemaining -= rotateAngleDelta;
                }
                else if (RotateAnglePerFrameActual < 0)
                {
                    rotateAngleDelta = (RotateAngleRemaining < -RotateAnglePerFrameActual) ? -RotateAngleRemaining : RotateAnglePerFrameActual;
                    RotateAngleRemaining += rotateAngleDelta;
                }

                Rotate(rotateAngleDelta);

                if (RotateAngleRemaining <= 0)
                {
                    RotateAnglePerFrameActual = 0;
                    RotateAngleRemaining = 0;
                    _combatCircleState = ViewCombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STATIONARY;
                }
            }
        }

        internal bool IsStationary()
        {
            return _combatCircleState == ViewCombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STATIONARY;
        }

        internal bool IsRotate()
        {
            return _combatCircleState == ViewCombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE;
        }

        internal void Rotate()
        {
            _combatCircleState = ViewCombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE;
        }

        internal void Rotate(float angle)
        {
            transform.Rotate(0, 0, angle);

            foreach (var viewSocket in _dicViewCircleSocket)
            {
                viewSocket.Value.transform.Rotate(0, 0, -angle);
            }
        }

        internal bool GetCircleSocket(int posId, out ViewCircleSocket outViewCircleSocket)
        {
            if (_dicViewCircleSocket.TryGetValue(posId, out outViewCircleSocket) == false)
            {
                Debug.LogError("Not found ViewCircleSocket, PosId: " + posId);
                return false;
            }

            return true;
        }
    }
}
