using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCircleController : MonoBehaviour
{
    [SerializeField]
    CombatCircle _combatCirclePlayer = null;
    [SerializeField]
    CombatCircle _combatCircleOpponent = null;
    [SerializeField]
    float _rotateDeltaAngle = 0f;   // 每 frame 旋轉角度

    const float _rotateTotalAngle = 60f; // 每次指令的旋轉角度

    void Start()
    {
    }

    void Update()
    {
        if (_combatCirclePlayer.IsStandby() && _combatCircleOpponent.IsStandby())
        {
            if (Input.GetKey("up"))
            {
                RotateCombatCircle(false);
            }
            else if (Input.GetKey("down"))
            {
                RotateCombatCircle(true);
            }
        }
    }

    private void FixedUpdate()
    {
        //if (_combatCircleState == CombatCircleController.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE)
        //{
        //    _combatCirclePlayer.transform.Rotate(0, 0, _rotateDeltaValue);

        //    _rotateTotalValue -= _rotateDeltaValue;

        //    if (_rotateTotalValue == 0)
        //    {
        //        _combatCircleState = CombatCircleController.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY;
        //    }
        //}
    }

    private void RotateCombatCircle(bool isClockWise)
    {
        if (isClockWise)
        {
            _combatCirclePlayer.RotateDeltaAngle = -_rotateDeltaAngle;
        }
        else
        {
            _combatCirclePlayer.RotateDeltaAngle = _rotateDeltaAngle;
        }

        _combatCirclePlayer.RotateRemainingAngle = _rotateTotalAngle;
        _combatCirclePlayer.Rotate();
    }
}
