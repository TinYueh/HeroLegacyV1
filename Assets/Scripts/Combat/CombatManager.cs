using GameSystem.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatManager : Singleton<CombatManager>
    {
        internal CombatAI CombatAI { get; private set; } = new CombatAI();
        internal CombatFormula CombatFormula { get; private set; } = new CombatFormula();
        internal CombatController CombatController { get; private set; } = new CombatController();
        internal GameEnum.eCombatRoundState CombatRoundState { get; set; } = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_NA;

        private GameObject _panelBlock = null;

        public override bool Init()
        {
            if (CombatController.Init() == false)
            {
                Debug.LogError("Init CombatController failed");
                return false;
            }

            _panelBlock = GameObject.Find("BlockPanel");

            Debug.Log("CombatManager Init OK");
            return true;
        }

        internal bool CreateNewCombat(int playerTeamId, int opponentTeamId)
        {
            if (CombatController.CreateNewCombat(playerTeamId, opponentTeamId) == false)
            {
                Debug.LogError("CombatController create new combat failed");
                return false;
            }

            HideBlock();

            return true;
        }

        #region Round Action
        internal void StartRoundAction(GameEnum.eCombatRoundAction playerAction)
        {
            ShowBlock();

            CombatController.StartRoundAction(playerAction);

            CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
        }

        internal void ProcessRoundAction()
        {
            if (CombatController.IsCombatCircleRotate())
            {
                // 正在轉
                return;
            }


            if (CombatController.ProcessRoundAction() == false)
            {
                return;
            }

            CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_MATCH;
        }

        internal void ExecRoundAction()
        {
            CombatController.ExecRoundAction();

            CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_FINAL;
        }

        internal void FinishRoundAction()
        {
            HideBlock();

            if (CombatController.FinishRoundAction())
            {
                // 戰鬥結束
                CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_LEAVE;
            }
            else
            {
                CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY;
            }
        }
        #endregion

        #region Show Hide
        private void ShowBlock()
        {
            _panelBlock.SetActive(true);
        }

        private void HideBlock()
        {
            _panelBlock.SetActive(false);
        }
        #endregion
    }
}
