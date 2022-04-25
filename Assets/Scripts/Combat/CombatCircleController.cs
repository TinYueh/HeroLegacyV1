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
    private float _rotateAnglePerFrame = 0f;        // 每個 frame 的旋轉角度
    [SerializeField]
    private float _combatCirclePlayerInitialAngle = 0f;
    [SerializeField]
    private float _combatCircleOpponentInitialAngle = 0f;

    private const float _rotateAnglePerTime = 60f;  // 每次指令的旋轉角度

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
                // 順時鐘
                RotateCombatCircle(_combatCirclePlayer, false);

                // Opponent
                RotateCombatCircle(_combatCircleOpponent, _enemyAI.GetNextRotateDirection());
            }
            else if (Input.GetKey("down"))
            {
                // 逆時鐘
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
