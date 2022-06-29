using GameSystem.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatManager : Singleton<CombatManager>
    {
        #region Property

        internal CombatAI AI { get; private set; } = new CombatAI();
        internal CombatFormula Formula { get; private set; } = new CombatFormula();
        internal CombatController Controller { get; private set; } = new CombatController();
        internal GameEnum.eCombatRoundState CombatRoundState { get; set; }

        private GameObject _pnlBlock;

        #endregion  // Property

        #region Init

        public override bool Init()
        {
            if (Controller.Init() == false)
            {
                Debug.LogError("Init CombatController failed");
                return false;
            }

            _pnlBlock = GameObject.Find("BlockPanel");

            Debug.Log("CombatManager Init OK");

            return true;
        }

        #endregion  // Init

        #region Round Action

        internal void StartRoundAction(GameEnum.eCombatRoundAction playerAction)
        {
            ShowBlock();

            Controller.StartRoundAction(playerAction);

            CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
        }

        internal void ProcessRoundAction()
        {
            if (Controller.IsCombatCircleRotate())
            {
                // 正在轉
                return;
            }


            if (Controller.ProcessRoundAction() == false)
            {
                return;
            }

            CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_MATCH;
        }

        internal void ExecRoundAction()
        {
            Controller.ExecRoundAction();

            CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_FINAL;
        }

        internal void FinishRoundAction()
        {           
            if (Controller.FinishRoundAction())
            {
                // 戰鬥結束
                CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_LEAVE;
            }
            else
            {
                HideBlock();

                Controller.PrepareRoundAction();

                CombatManager.Instance.CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_STANDBY;
            }
        }

        #endregion  // Round Action

        #region Show Hide

        internal void ShowBlock()
        {
            _pnlBlock.SetActive(true);
        }

        internal void HideBlock()
        {
            _pnlBlock.SetActive(false);
        }

        #endregion  // Show Hide

        #region Method

        internal bool CreateNewCombat(int playerTeamId, int opponentTeamId)
        {
            if (Controller.CreateNewCombat(playerTeamId, opponentTeamId) == false)
            {
                Debug.LogError("CombatController create new combat failed");
                return false;
            }

            HideBlock();

            return true;
        }

        #endregion  // Method
    }
}