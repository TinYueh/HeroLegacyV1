using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatCore : MonoBehaviour
    {
        [Header("CombatTeam")]
        [SerializeField]
        private CombatTeam _combatPlayer = null;
        [SerializeField]
        private CombatTeam _combatOpponent = null;
        [Header("CombatCircle")]
        [SerializeField]
        private float _combatPlayerInitAngle = 0f;
        [SerializeField]
        private float _combatOpponentInitAngle = 0f;
        [SerializeField]
        private float _rotateAnglePerFrame = 0f;        // 每個 frame 的旋轉角度
        [SerializeField]
        private int _rotateSfxId = 0;                   // 旋轉音效
        [Header("Team")]
        [SerializeField]
        private int _teamPlayer = 0;
        [SerializeField]
        private int _teamOpponent = 0;

        private const float _rotateAnglePerTime = 60f;  // 每次指令的旋轉角度

        private delegate void DlgCombatStateFunc();
        private Dictionary<CombatCore.eCombatState, DlgCombatStateFunc> _dicCombatStateFunc = new Dictionary<CombatCore.eCombatState, DlgCombatStateFunc>();

        public enum eCombatTeam : byte
        {
            E_COMBAT_TEAM_NA = 0,
            E_COMBAT_TEAM_PLAYER,   // 玩家
            E_COMBAT_TEAM_OPPONENT, // 對手
            E_COMBAT_TEAM_LIMIT,
        }

        public enum eCombatTeamAction : byte
        {
            E_COMBAT_TEAM_ACTION_NA = 0,
            E_COMBAT_TEAM_ACTION_ROTATE_RIGHT,  // 順時鐘移動
            E_COMBAT_TEAM_ACTION_ROTATE_LEFT,   // 逆時鐘移動
            E_COMBAT_TEAM_ACTION_CAST,          // 使用技能
            E_COMBAT_TEAM_ACTION_ROTATE_LIMIT,
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
            CombatManager.Instance.InitCombatTeam(ref _combatPlayer, _combatPlayerInitAngle, _rotateAnglePerFrame, _rotateAnglePerTime, _teamPlayer);
            CombatManager.Instance.InitCombatTeam(ref _combatOpponent, _combatOpponentInitAngle, _rotateAnglePerFrame, _rotateAnglePerTime, _teamOpponent);
        }

        private void CombatStateStandby()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow))
            {
                CombatManager.Instance.StartCombatTeamAction(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_ROTATE_LEFT);
                CombatManager.Instance.CombatState = CombatCore.eCombatState.E_COMBAT_STATE_ROTATE;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                CombatManager.Instance.StartCombatTeamAction(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_ROTATE_RIGHT);
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
            CombatManager.Instance.ExecCombatTeamAction();
            CombatManager.Instance.CombatState = eCombatState.E_COMBAT_STATE_STANDBY;
        }
    }
}
