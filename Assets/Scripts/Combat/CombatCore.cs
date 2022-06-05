using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatCore : MonoBehaviour
    {
        [SerializeField]
        private int _playerTeamId = 0;      // 玩家隊伍 TeamId
        [SerializeField]
        private int _opponentTeamId = 0;    // 敵對隊伍 TeamId

        private delegate void DlgCombatRoundStateFunc();
        private Dictionary<GameEnum.eCombatRoundState, DlgCombatRoundStateFunc> _dicCombatRoundStateFunc = new Dictionary<GameEnum.eCombatRoundState, DlgCombatRoundStateFunc>();

        private void Awake()
        {
            CombatManager.Instance.Init();

            RegistCombatRoundStateFunc();
        }

        private void Start()
        {
            CombatManager.Instance.CreateNewCombat(_playerTeamId, _opponentTeamId);

            // CombatRoundState 保持在 CombatCore 中做切換
            CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY;
        }

        private void Update()
        {
            DlgCombatRoundStateFunc dlgFunc = null;
            if (_dicCombatRoundStateFunc.TryGetValue(CombatManager.Instance.CombatRoundState, out dlgFunc) == false)
            {
                Debug.LogError("Not found CombatRoundStateFunc for " + CombatManager.Instance.CombatRoundState);
                return;
            }

            dlgFunc();
        }

        private void RegistCombatRoundStateFunc()
        {
            _dicCombatRoundStateFunc.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY, CombatRoundStateStandby);
            _dicCombatRoundStateFunc.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE, CombatRoundStateRotate);
            _dicCombatRoundStateFunc.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_MATCH, CombatRoundStateMatch);
            _dicCombatRoundStateFunc.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_FINAL, CombatRoundStateFinal);
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
            // 持續旋轉和執行 CircleSocket 直到 CombatCircle 靜止
            if (CombatManager.Instance.ProcessRoundAction())
            {                   
                CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_MATCH;
            }
        }

        private void CombatRoundStateMatch()
        {
            // 進行對戰
            CombatManager.Instance.ExecRoundAction();
            CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_FINAL;
        }

        private void CombatRoundStateFinal()
        {
            CombatManager.Instance.FinishRoundAction();
            CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY;
        }
    }
}