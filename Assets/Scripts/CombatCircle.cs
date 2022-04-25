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

    public float RotateAnglePerFrame { private get; set; } = 0f;
    public float RotateAngleRemaining { private get; set; } = 0f;

    private eCombatCircleState _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_NA;

    public void Rotate()
    {
        _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE;
    }

    public bool IsStandby()
    {
        return (_combatCircleState == CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY);
    }

    private void Start()
    {
        _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_combatCircleState == CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE)
        {
            float rotateAngleDelta = 0f;

            if (RotateAnglePerFrame > 0)
            {
                rotateAngleDelta = (RotateAngleRemaining < RotateAnglePerFrame) ? RotateAngleRemaining : RotateAnglePerFrame;
                RotateAngleRemaining -= rotateAngleDelta;
            }
            else if (RotateAnglePerFrame < 0)
            {
                rotateAngleDelta = (RotateAngleRemaining < -RotateAnglePerFrame) ? -RotateAngleRemaining : RotateAnglePerFrame;
                RotateAngleRemaining += rotateAngleDelta;
            }

            this.transform.Rotate(0, 0, rotateAngleDelta);

            if (RotateAngleRemaining <= 0)
            {
                RotateAnglePerFrame = 0;
                _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY;
            }
        }
    }
}
