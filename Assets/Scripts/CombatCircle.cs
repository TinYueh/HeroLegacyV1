using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCircle : MonoBehaviour
{
    enum eCombatCircleState
    {
        E_COMBAT_CIRCLE_STATE_NA = 0,
        E_COMBAT_CIRCLE_STATE_STANDBY,  // «Ý¾÷
        E_COMBAT_CIRCLE_STATE_ROTATE,   // ±ÛÂà
        E_COMBAT_CIRCLE_STATE_LOCK,     // ­áµ²
    }

    float _rotateDeltaAngle = 0f;
    public float RotateDeltaAngle
    {
        set
        {
            _rotateDeltaAngle = value;
        }
    }

    float _rotateRemainingAngle = 0f;
    public float RotateRemainingAngle
    {
        set
        {
            _rotateRemainingAngle = value;
        }
    }

    eCombatCircleState _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_NA;
    public void Rotate()
    {
        _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE;
    }
    public bool IsStandby()
    {
        return (_combatCircleState == CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY);
    }

    void Start()
    {
        _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_combatCircleState == CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE)
        {
            float rotateAngleThisFrame = 0f;

            if (_rotateDeltaAngle > 0)
            {
                rotateAngleThisFrame = (_rotateRemainingAngle < _rotateDeltaAngle) ? _rotateRemainingAngle : _rotateDeltaAngle;
                _rotateRemainingAngle -= _rotateDeltaAngle;
            }
            else if (_rotateDeltaAngle < 0)
            {
                rotateAngleThisFrame = (_rotateRemainingAngle < -_rotateDeltaAngle) ? -_rotateRemainingAngle : _rotateDeltaAngle;
                _rotateRemainingAngle += _rotateDeltaAngle;
            }

            this.transform.Rotate(0, 0, rotateAngleThisFrame);

            if (_rotateRemainingAngle <= 0)
            {
                _rotateDeltaAngle = 0;
                _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY;
            }
        }
    }
}
