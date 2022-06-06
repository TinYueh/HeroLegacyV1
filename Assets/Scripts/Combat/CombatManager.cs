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

        private void ShowBlock()
        {
            _panelBlock.SetActive(true);
        }

        private void HideBlock()
        {
            _panelBlock.SetActive(false);
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

        internal void StartRoundAction(GameEnum.eCombatRoundAction playerAction)
        {
            ShowBlock();

            CombatController.StartRoundAction(playerAction);
        }

        internal bool ProcessRoundAction()
        {
            if (CombatController.IsCombatCircleRotate())
            {
                // 正在轉
                return false;
            }

            return CombatController.ProcessRoundAction();
        }

        internal void ExecRoundAction()
        {
            CombatController.ExecRoundAction();
        }

        internal bool FinishRoundAction()
        {
            HideBlock();

            return CombatController.FinishRoundAction();
        }
    }
}
