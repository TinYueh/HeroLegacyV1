using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class UICombatCircle : MonoBehaviour
    {
        private enum eCombatCircleState
        {
            E_COMBAT_CIRCLE_STATE_NA = 0,
            E_COMBAT_CIRCLE_STATE_STANDBY,  // 待機
            E_COMBAT_CIRCLE_STATE_ROTATE,   // 旋轉
            E_COMBAT_CIRCLE_STATE_LIMIT,
        }

        [SerializeField]
        private float _adjustRadiusForSocket = -70f; // 微調 CircleSocket 與圓心的距離
        [SerializeField]
        private float _initAngle = 0f;              // 45f 和 225f 戰鬥開始時的起始角度

        internal float RotateAnglePerFrame { get; set; } = 2f;      // 每個 frame 的旋轉角度
        internal float RotateAnglePerTime { get; set; } = 0f;       // 每次指令的旋轉角度
        internal float RotateAnglePerFrameActual { get; set; } = 0f;
        private float RotateAngleRemaining { get; set; } = 0f;

        private eCombatCircleState _combatCircleState = UICombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_NA;
        private Dictionary<int, GameObject> _dicCircleSocket = new Dictionary<int, GameObject>();

        private void Awake()
        {
            float radius = (this.transform.GetComponent<RectTransform>().sizeDelta.x + _adjustRadiusForSocket) / 2;

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                int socketId = i + 1;

                float posX = radius * Mathf.Cos(-60 * i * Mathf.Deg2Rad);
                float posY = radius * Mathf.Sin(-60 * i * Mathf.Deg2Rad);

                GameObject objSocket = GameObject.Instantiate(Resources.Load<GameObject>(AssetsPath.PREFAB_COMBAT_CIRCLE_SOCKET), new Vector2(posX, posY), Quaternion.identity);
                objSocket.transform.SetParent(this.transform, false);

                _dicCircleSocket.Add(socketId, objSocket);
            }
        }

        private void Start()
        {
            _combatCircleState = UICombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY;
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            if (_combatCircleState == UICombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE)
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
                    _combatCircleState = UICombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY;
                }
            }
        }

        internal void Init()
        {
            Rotate(_initAngle);
        }

        internal bool IsStandby()
        {
            return _combatCircleState == UICombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY;
        }

        internal void EnableRotate()
        {
            RotateAngleRemaining = RotateAnglePerTime;
            _combatCircleState = UICombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE;
        }

        internal void Rotate(float angle)
        {
            this.transform.Rotate(0, 0, angle);

            foreach (var socket in _dicCircleSocket)
            {
                socket.Value.transform.Rotate(0, 0, -angle);
            }
        }

        internal void ChangeViewSocket(int socketId, ref RoleCsvData refCsvData)
        {
            GameObject objSocket = new GameObject();

            if (_dicCircleSocket.TryGetValue(socketId, out objSocket) == false)
            {
                return;
            }

            // 屬性
            objSocket.GetComponent<Image>().sprite = Resources.Load<Sprite>(AssetsPath.SPRITE_ROLE_ATTRIBUTE_GEM_PATH + refCsvData._attribute);

            // 徽章
            string path = AssetsPath.SPRITE_ROLE_EMBLEM_PATH + refCsvData._emblem.ToString().PadLeft(3, '0');
            GameObject objEmblem = objSocket.transform.Find("Emblem").gameObject;
            objEmblem.SetActive(true);
            objEmblem.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        }
    }
}
