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

        public override bool Init()
        {
            if (CombatController.Init() == false)
            {
                Debug.LogError("Init CombatController failed");
                return false;
            }

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

            return true;
        }

        internal void StartRoundAction(GameEnum.eCombatRoundAction playerAction)
        {
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
    }
}
