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
    private float _rotateAnglePerFrame = 0f;        // �C�� frame �����ਤ��
    [SerializeField]
    private float _combatCirclePlayerInitialAngle = 0f;
    [SerializeField]
    private float _combatCircleOpponentInitialAngle = 0f;

    private const float _rotateAnglePerTime = 60f;  // �C�����O�����ਤ��

    private EnemyAI _enemyAI = null;

    private void Awake()
    {
        _enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        _combatCirclePlayer.Rotate(_combatCirclePlayerInitialAngle);
        _combatCircleOpponent.Rotate(_combatCircleOpponentInitialAngle);
    }

    private void Update()
    {
        if (_combatCirclePlayer.IsStandby() && _combatCircleOpponent.IsStandby())
        {
            if (Input.GetKey("up"))
            {
                // ������
                RotateCombatCircle(_combatCirclePlayer, false);

                // Opponent
                RotateCombatCircle(_combatCircleOpponent, _enemyAI.GetNextRotateDirection());
            }
            else if (Input.GetKey("down"))
            {
                // �f����
                RotateCombatCircle(_combatCirclePlayer, true);

                // Opponent
                RotateCombatCircle(_combatCircleOpponent, _enemyAI.GetNextRotateDirection());
            }
        }
    }

    private void RotateCombatCircle(CombatCircle combatCircle, bool isClockWiseDirection)
    {
        if (isClockWiseDirection)
        {
            combatCircle.RotateAnglePerFrame = -_rotateAnglePerFrame;
        }
        else
        {
            combatCircle.RotateAnglePerFrame = _rotateAnglePerFrame;
        }

        combatCircle.RotateAngleRemaining = _rotateAnglePerTime;
        combatCircle.EnableRotate();
    }
}
