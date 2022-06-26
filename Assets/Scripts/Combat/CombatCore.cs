using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Audio;

namespace GameCombat
{
    public class CombatCore : MonoBehaviour
    {
        [SerializeField]
        private int _playerTeamId = 0;      // 玩家隊伍 TeamId
        [SerializeField]
        private int _opponentTeamId = 0;    // 敵對隊伍 TeamId

        private delegate void DlgCombatRoundState();
        private Dictionary<GameEnum.eCombatRoundState, DlgCombatRoundState> _dicDlgCombatRoundState = new Dictionary<GameEnum.eCombatRoundState, DlgCombatRoundState>();

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
            DlgCombatRoundState dlgFunc = null;
            if (_dicDlgCombatRoundState.TryGetValue(CombatManager.Instance.CombatRoundState, out dlgFunc) == false)
            {
                Debug.LogError("Not found CombatRoundStateFunc for " + CombatManager.Instance.CombatRoundState);
                return;
            }

            dlgFunc();
        }

        private void RegistCombatRoundStateFunc()
        {
            _dicDlgCombatRoundState.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY, CombatRoundStateStandby);
            _dicDlgCombatRoundState.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE, CombatRoundStateRotate);
            _dicDlgCombatRoundState.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_MATCH, CombatRoundStateMatch);
            _dicDlgCombatRoundState.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_FINAL, CombatRoundStateFinal);
            _dicDlgCombatRoundState.Add(GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_LEAVE, CombatRoundStateLeave);
        }

        private void CombatRoundStateStandby()
        {
            // Todo: 改為讀數 1 秒後確定, 因此先用 GetKey
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z))
            {
                CombatManager.Instance.StartRoundAction(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_LEFT);
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.X))
            {
                CombatManager.Instance.StartRoundAction(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_RIGHT);
            }
        }

        private void CombatRoundStateRotate()
        {
            // 持續旋轉和執行 CircleSocket 直到 CombatCircle 靜止
            CombatManager.Instance.ProcessRoundAction();
        }

        private void CombatRoundStateMatch()
        {
            // 進行對戰
            CombatManager.Instance.ExecRoundAction();
        }

        private void CombatRoundStateFinal()
        {
            // 結算
            CombatManager.Instance.FinishRoundAction();
        }

        private void CombatRoundStateLeave()
        {
            // 戰鬥結束
        }
    }
}