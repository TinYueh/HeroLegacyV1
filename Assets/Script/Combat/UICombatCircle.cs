using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class UICombatCircle : MonoBehaviour
    {
        enum eCombatCircleState : byte
        {
            E_COMBAT_CIRCLE_STATE_NA = 0,
            E_COMBAT_CIRCLE_STATE_STANDBY,  // 待機
            E_COMBAT_CIRCLE_STATE_ROTATE,   // 旋轉
            E_COMBAT_CIRCLE_STATE_LIMIT,
        }

        [SerializeField]
        private List<GameObject> _listRoleSlot = new List<GameObject>();

        internal float RotateAnglePerFrame { get; set; } = 0f;
        internal float RotateAnglePerTime { get; set; } = 0f;
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

            foreach (GameObject slot in _listRoleSlot)
            {
                slot.transform.Rotate(0, 0, -angle);
            }
        }

        internal void ShowRoleSlot(int teamId, ref RoleCsvData refCsvData)
        {
            if (teamId == 0 || teamId > GameConst.MAX_TEAM_MEMBER)
            {
                return;
            }

            GameObject roleSlot = _listRoleSlot[teamId - 1];
            if (roleSlot == null)
            {
                return;
            }

            roleSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>(AssetsPath.SPRITE_ROLE_CLASS_GEM_PATH + refCsvData._class);

            string path = AssetsPath.SPRITE_ROLE_EMBLEM_PATH + refCsvData._emblem.ToString().PadLeft(3, '0');
            roleSlot.transform.Find("Emblem").GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        }
    }
}
