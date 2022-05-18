using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatCore : MonoBehaviour
    {
        [SerializeField]
        private ViewCombatTeam _playerCombatTeam = null;
        [SerializeField]
        private ViewCombatTeam _opponentCombatTeam = null;
        [SerializeField]
        private int _playerTeamId = 0;                  // 玩家隊伍 TeamId
        [SerializeField]
        private int _opponentTeamId = 0;                // 敵對隊伍 TeamId
        [SerializeField]
        private int _rotateSfxId = 0;                   // 戰圓旋轉音效

        private delegate void DlgCombatRoundStateFunc();
        private Dictionary<GameEnum.eCombatRoundState, DlgCombatRoundStateFunc> _dicCombatRoundStateFunc = new Dictionary<GameEnum.eCombatRoundState, DlgCombatRoundStateFunc>();

        private void Awake()
        {
            CombatManager.Instance.Init();

            RegistCombatStateFunc();
        }

        private void Start()
        {
            // 戰圓旋轉音效
            CombatManager.Instance.RotateSfxId = _rotateSfxId;

            CreateNewCombat();

            CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY;
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
            _dicCombatRoundStateFunc.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY, CombatRoundStateStandby);
            _dicCombatRoundStateFunc.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE, CombatRoundStateRotate);
            _dicCombatRoundStateFunc.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_MATCH, CombatRoundStateMatch);
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
                CombatManager.Instance.StartRoundAction(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_LEFT);
                CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                CombatManager.Instance.StartRoundAction(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_RIGHT);
                CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
            }
        }

        private void CombatRoundStateRotate()
        {
            if (CombatManager.Instance.IsCombatCircleStandby())
            {
                CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_MATCH;
            }
        }

        private void CombatRoundStateMatch()
        {
            CombatManager.Instance.ExecRoundAction();
            CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY;
        }
    }
}
