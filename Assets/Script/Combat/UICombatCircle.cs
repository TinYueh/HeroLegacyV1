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
        private List<GameObject> _listRoleSlot = new List<GameObject>();
        [SerializeField]
        private float _initAngle = 0f; 

        internal float RotateAnglePerFrame { get; set; } = 2f;      // 每個 frame 的旋轉角度
        internal float RotateAnglePerTime { get; set; } = 0f;       // 每次指令的旋轉角度
        internal float RotateAnglePerFrameActual { get; set; } = 0f;
        private float RotateAngleRemaining { get; set; } = 0f;

        private eCombatCircleState _combatCircleState = UICombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_NA;

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

            foreach (GameObject objSlot in _listRoleSlot)
            {
                objSlot.transform.Rotate(0, 0, -angle);
            }
        }

        internal void ChangeViewRoleSlot(int slotId, ref RoleCsvData refCsvData)
        {
            if (slotId == 0 || slotId > GameConst.MAX_TEAM_MEMBER)
            {
                return;
            }

            GameObject objSlot = _listRoleSlot[slotId - 1];
            if (objSlot == null)
            {
                return;
            }

            objSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>(AssetsPath.SPRITE_ROLE_ATTRIBUTE_GEM_PATH + refCsvData._attribute);

            string path = AssetsPath.SPRITE_ROLE_EMBLEM_PATH + refCsvData._emblem.ToString().PadLeft(3, '0');
            GameObject objEmblem = objSlot.transform.Find("Emblem").gameObject;
            objEmblem.SetActive(true);
            objEmblem.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        }
    }
}
