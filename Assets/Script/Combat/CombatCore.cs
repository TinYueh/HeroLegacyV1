using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatCore : MonoBehaviour
    {
        [Header("CombatTeam")]
        [SerializeField]
        private CombatTeam _playerCombatTeam = null;
        [SerializeField]
        private CombatTeam _opponentCombatTeam = null;
        [SerializeField]
        private int _rotateSfxId = 0;                   // 旋轉音效
        [SerializeField]
        private int _playerTeamId = 0;
        [SerializeField]
        private int _opponentTeamId = 0;

        private delegate void DlgCombatRoundStateFunc();
        private Dictionary<CombatCore.eCombatRoundState, DlgCombatRoundStateFunc> _dicCombatRoundStateFunc = new Dictionary<CombatCore.eCombatRoundState, DlgCombatRoundStateFunc>();

        public enum eCombatTeam
        {
            E_COMBAT_TEAM_NA = 0,
            E_COMBAT_TEAM_PLAYER,   // 玩家
            E_COMBAT_TEAM_OPPONENT, // 對手
            E_COMBAT_TEAM_LIMIT,
        }

        public enum eCombatTeamAction
        {
            E_COMBAT_TEAM_ACTION_NA = 0,
            E_COMBAT_TEAM_ACTION_ROTATE_RIGHT,  // 順時鐘移動
            E_COMBAT_TEAM_ACTION_ROTATE_LEFT,   // 逆時鐘移動
            E_COMBAT_TEAM_ACTION_CAST,          // 使用技能
            E_COMBAT_TEAM_ACTION_ROTATE_LIMIT,
        }

        public enum eCombatRoundState
        {
            E_COMBAT_ROUND_STATE_NA = 0,
            E_COMBAT_ROUND_STATE_STANDBY,
            E_COMBAT_ROUND_STATE_ROTATE,
            E_COMBAT_ROUND_STATE_MATCH,
            E_COMBAT_ROUND_STATE_LIMIT,
        }

        public enum eCombatMatchResult
        {
            E_COMBAT_MATCH_RESULT_NA = 0,
            E_COMBAT_MATCH_RESULT_WIN,      // 勝
            E_COMBAT_MATCH_RESULT_LOSE,     // 負
            E_COMBAT_MATCH_RESULT_DRAW,     // 平
            E_COMBAT_MATCH_RESULT_LIMIT,
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

            CombatManager.Instance.CombatRoundState = eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY;
        }

        private void Update()
        {
            DlgCombatRoundStateFunc dlgFunc = null;
            _dicCombatRoundStateFunc.TryGetValue(CombatManager.Instance.CombatRoundState, out dlgFunc);
            if (dlgFunc == null)
            {
                Debug.LogError("Not found CombatRoundStateFunc for " + CombatManager.Instance.CombatRoundState);
                return;
            }

            dlgFunc();
        }

        private void RegistCombatStateFunc()
        {
            _dicCombatRoundStateFunc.Add(CombatCore.eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY, CombatRoundStateStandby);
            _dicCombatRoundStateFunc.Add(CombatCore.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE, CombatRoundStateRotate);
            _dicCombatRoundStateFunc.Add(CombatCore.eCombatRoundState.E_COMBAT_ROUND_STATE_MATCH, CombatRoundStateMatch);
        }

        private void CreateNewCombat()
        {
            CombatManager.Instance.InitCombatTeam(ref _playerCombatTeam, _playerTeamId);
            CombatManager.Instance.InitCombatTeam(ref _opponentCombatTeam, _opponentTeamId);
        }

        private void CombatRoundStateStandby()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow))
            {
                CombatManager.Instance.StartCombatTeamAction(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_ROTATE_LEFT);
                CombatManager.Instance.CombatRoundState = CombatCore.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                CombatManager.Instance.StartCombatTeamAction(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_ROTATE_RIGHT);
                CombatManager.Instance.CombatRoundState = CombatCore.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
            }
        }

        private void CombatRoundStateRotate()
        {
            if (CombatManager.Instance.IsCombatCircleStandby())
            {
                CombatManager.Instance.CombatRoundState = eCombatRoundState.E_COMBAT_ROUND_STATE_MATCH;
            }
        }

        private void CombatRoundStateMatch()
        {
            CombatManager.Instance.ExecCombatTeamAction();
            CombatManager.Instance.CombatRoundState = eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY;
        }
    }
}
