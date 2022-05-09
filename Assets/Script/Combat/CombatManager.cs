using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatManager : Singleton<CombatManager>
    {
        private CombatAI _combatAI = new CombatAI();
        private CombatTeam _combatPlayer = null;
        private CombatTeam _combatOpponent = null;

        internal int RotateSfxId { get; set; } = 0;

        private delegate void DlgStartActionFunc();
        private Dictionary<CombatCore.eCombatTeamAction, DlgStartActionFunc> _dicStartActionFunc = new Dictionary<CombatCore.eCombatTeamAction, DlgStartActionFunc>();

        internal CombatCore.eCombatState CombatState { get; set; } = CombatCore.eCombatState.E_COMBAT_STATE_NA;

        public override void Init()
        {
            RegistStartActionFunc();

            Debug.Log("CombatManager Init OK");
        }

        private void RegistStartActionFunc()
        {
            _dicStartActionFunc.Add(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_ROTATE_RIGHT, StartActionRotateRight);
            _dicStartActionFunc.Add(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_ROTATE_LEFT, StartActionRotateLeft);
            _dicStartActionFunc.Add(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_CAST, StartActionCast);
        }

        internal void InitCombatTeam(ref CombatTeam refTeam, float initAngle, float anglePerFrame, float anglePerTime, int teamId)
        {
            if (refTeam._team == CombatCore.eCombatTeam.E_COMBAT_TEAM_PLAYER)
            {
                _combatPlayer = refTeam;
            }
            else if (refTeam._team == CombatCore.eCombatTeam.E_COMBAT_TEAM_OPPONENT)
            {
                _combatOpponent = refTeam;
            }
            else
            {
                Debug.LogError("Unknown CombatTeam: " + refTeam._team);
                return;
            }

            refTeam._uiCombatCircle.RotateAnglePerFrame = anglePerFrame;
            refTeam._uiCombatCircle.RotateAnglePerTime = anglePerTime;
            refTeam._uiCombatCircle.Rotate(initAngle);

            refTeam._uiEnergyBar.SetWidthPerPoint(GameConst.BAR_ENERGY_POINT);

            refTeam.SetEnergyPoint(0);
            refTeam.SetFocusTeamId(1);

            TeamCsvData teamCsvData = new TeamCsvData();
            if (TableManager.Instance.GetTeamCsvData(teamId, out teamCsvData) == false)
            {
                Debug.LogError("Not found TeamCsvData, Id: " + teamId);
            }

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                if (teamCsvData._arrRoleId[i] == 0)
                {
                    break;
                }

                refTeam.CreateCombatRole(i + 1, teamCsvData._arrRoleId[i]);
            }
        }

        internal void StartCombatTeamAction(CombatCore.eCombatTeamAction action)
        {
            DlgStartActionFunc dlgFunc = null;
            _dicStartActionFunc.TryGetValue(action, out dlgFunc);
            if (dlgFunc == null)
            {
                Debug.LogError("Not found StartActionFunc for " + action);
                return;
            }

            dlgFunc();
        }

        internal void ExecCombatTeamAction()
        {
            _combatPlayer.ChangeEnergyPoint(1);
            _combatOpponent.ChangeEnergyPoint(1);
        }

        private void StartActionRotateRight()
        {
            RotateCombatCircle(ref _combatPlayer._uiCombatCircle, true);
            _combatPlayer.ChangeFocusTeamId(true);

            bool isDirectionRight = _combatAI.GetNextAction();
            RotateCombatCircle(ref _combatOpponent._uiCombatCircle, isDirectionRight);
            _combatOpponent.ChangeFocusTeamId(isDirectionRight);

            AudioManager.Instance.PlaySfx(RotateSfxId);
        }

        private void StartActionRotateLeft()
        {
            RotateCombatCircle(ref _combatPlayer._uiCombatCircle, false);
            _combatPlayer.ChangeFocusTeamId(false);

            bool isDirectionRight = _combatAI.GetNextAction();
            RotateCombatCircle(ref _combatOpponent._uiCombatCircle, isDirectionRight);
            _combatOpponent.ChangeFocusTeamId(isDirectionRight);

            AudioManager.Instance.PlaySfx(RotateSfxId);
        }

        private void StartActionCast()
        {

        }

        private void RotateCombatCircle(ref UICombatCircle refUICombatCircle, bool isDirectionRight)
        {
            if (isDirectionRight)
            {
                refUICombatCircle.RotateAnglePerFrameActual = -refUICombatCircle.RotateAnglePerFrame;
            }
            else
            {
                refUICombatCircle.RotateAnglePerFrameActual = refUICombatCircle.RotateAnglePerFrame;
            }

            refUICombatCircle.EnableRotate();

            CombatState = CombatCore.eCombatState.E_COMBAT_STATE_ROTATE;
        }

        internal bool IsCombatCircleStandby()
        {
            if (_combatPlayer._uiCombatCircle.IsStandby() && _combatOpponent._uiCombatCircle.IsStandby())
            {
                return true;
            }

            return false;
        }
    }
}
