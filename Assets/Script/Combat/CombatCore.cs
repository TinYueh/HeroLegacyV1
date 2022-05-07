using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatCore : MonoBehaviour
    {
        [Header("CombatRole")]
        [SerializeField]
        private CombatRole _combatPlayer = null;
        [SerializeField]
        private CombatRole _combatOpponent = null;
        [Header("CombatCircle")]
        [SerializeField]
        private float _combatPlayerInitAngle = 0f;
        [SerializeField]
        private float _combatOpponentInitAngle = 0f;
        [SerializeField]
        private float _rotateAnglePerFrame = 0f;        // 每個 frame 的旋轉角度
        [SerializeField]
        private int _rotateSfxId = 0;                   // 旋轉音效

        private const float _rotateAnglePerTime = 60f;  // 每次指令的旋轉角度

        private delegate void DlgCombatStateFunc();
        private Dictionary<CombatCore.eCombatState, DlgCombatStateFunc> _dicCombatStateFunc = new Dictionary<CombatCore.eCombatState, DlgCombatStateFunc>();

        public enum eCombatRole : byte
        {
            E_COMBAT_ROLE_NA = 0,
            E_COMBAT_ROLE_PLAYER,   // 玩家
            E_COMBAT_ROLE_OPPONENT, // 對手
            E_COMBAT_ROLE_LIMIT,
        }

        public enum eCombatRoleAction : byte
        {
            E_COMBAT_ROLE_ACTION_NA = 0,
            E_COMBAT_ROLE_ACTION_ROTATE_RIGHT,  // 順時鐘移動
            E_COMBAT_ROLE_ACTION_ROTATE_LEFT,   // 逆時鐘移動
            E_COMBAT_ROLE_ACTION_CAST,          // 使用技能
            E_COMBAT_ROLE_ACTION_ROTATE_LIMIT,
        }

        public enum eCombatState : byte
        {
            E_COMBAT_STATE_NA = 0,
            E_COMBAT_STATE_STANDBY,
            E_COMBAT_STATE_ROTATE,
            E_COMBAT_STATE_EXEC,
            E_COMBAT_STATE_LIMIT,
        }

        private void Awake()
        {
            CombatManager.Instance.Init();

            RegistCombatStateFunc();
        }

        private void Start()
        {
            // 旋轉音效
            CombatManager.Instance.RotateSfxId = _rotateSfxId;

            CreateNewCombat();

            CombatManager.Instance.CombatState = eCombatState.E_COMBAT_STATE_STANDBY;
        }

        private void Update()
        {
            DlgCombatStateFunc dlgFunc = null;
            _dicCombatStateFunc.TryGetValue(CombatManager.Instance.CombatState, out dlgFunc);
            if (dlgFunc == null)
            {
                Debug.LogError("Not found CombatStateFunc for " + CombatManager.Instance.CombatState);
                return;
            }

            dlgFunc();
        }

        private void RegistCombatStateFunc()
        {
            _dicCombatStateFunc.Add(CombatCore.eCombatState.E_COMBAT_STATE_STANDBY, CombatStateStandby);
            _dicCombatStateFunc.Add(CombatCore.eCombatState.E_COMBAT_STATE_ROTATE, CombatStateRotate);
            _dicCombatStateFunc.Add(CombatCore.eCombatState.E_COMBAT_STATE_EXEC, CombatStateExec);
        }

        private void CreateNewCombat()
        {
            CombatManager.Instance.InitCombatRole(ref _combatPlayer, _combatPlayerInitAngle, _rotateAnglePerFrame, _rotateAnglePerTime);
            CombatManager.Instance.InitCombatRole(ref _combatOpponent, _combatOpponentInitAngle, _rotateAnglePerFrame, _rotateAnglePerTime);
        }

        private void CombatStateStandby()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow))
            {
                CombatManager.Instance.StartCombatRoleAction(CombatCore.eCombatRoleAction.E_COMBAT_ROLE_ACTION_ROTATE_LEFT);
                CombatManager.Instance.CombatState = CombatCore.eCombatState.E_COMBAT_STATE_ROTATE;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                CombatManager.Instance.StartCombatRoleAction(CombatCore.eCombatRoleAction.E_COMBAT_ROLE_ACTION_ROTATE_RIGHT);
                CombatManager.Instance.CombatState = CombatCore.eCombatState.E_COMBAT_STATE_ROTATE;
            }
        }

        private void CombatStateRotate()
        {
            if (CombatManager.Instance.IsCombatCircleStandby())
            {
                CombatManager.Instance.CombatState = eCombatState.E_COMBAT_STATE_EXEC;
            }
        }

        private void CombatStateExec()
        {
            CombatManager.Instance.ExecCombatRoleAction();
            CombatManager.Instance.CombatState = eCombatState.E_COMBAT_STATE_STANDBY;
        }
    }
}
