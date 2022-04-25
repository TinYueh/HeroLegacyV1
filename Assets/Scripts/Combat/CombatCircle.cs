using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCircle : MonoBehaviour
{
    enum eCombatCircleState
    {
        E_COMBAT_CIRCLE_STATE_NA = 0,
        E_COMBAT_CIRCLE_STATE_STANDBY,  // �ݾ�
        E_COMBAT_CIRCLE_STATE_ROTATE,   // ����
        E_COMBAT_CIRCLE_STATE_LOCK,     // �ᵲ
    }

    [SerializeField]
    private List<GameObject> _listHeroSlot;

    public float RotateAnglePerFrame { get; set; } = 0f;
    public float RotateAngleRemaining { get; set; } = 0f;

    private eCombatCircleState _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_NA;

    public void EnableRotate()
    {
        _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_ROTATE;
    }

    public bool IsStandby()
    {
        return (_combatCircleState == CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY);
    }

    private void Awake()
    {
            
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

            Rotate(rotateAngleDelta);

            if (RotateAngleRemaining <= 0)
            {
                RotateAnglePerFrame = 0;
                _combatCircleState = CombatCircle.eCombatCircleState.E_COMBAT_CIRCLE_STATE_STANDBY;
            }
        }
    }

    public void Rotate(float angle)
    {
        this.transform.Rotate(0, 0, angle);

        foreach (GameObject slot in _listHeroSlot)
        {
            slot.transform.Rotate(0, 0, -angle);
        }
    }
}
