using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatManager : Singleton<CombatManager>
    {
        private CombatAI _combatAI = null;
        private CombatTeam _playerCombatTeam = null;
        private CombatTeam _opponentCombatTeam = null;

        internal int RotateSfxId { get; set; } = 0;

        private delegate void DlgStartActionFunc();
        private Dictionary<CombatCore.eCombatTeamAction, DlgStartActionFunc> _dicStartActionFunc = null;

        internal CombatCore.eCombatRoundState CombatRoundState { get; set; } = CombatCore.eCombatRoundState.E_COMBAT_ROUND_STATE_NA;

        public override void Init()
        {
            _combatAI = new CombatAI();
            _dicStartActionFunc = new Dictionary<CombatCore.eCombatTeamAction, DlgStartActionFunc>();

            RegistStartActionFunc();

            Debug.Log("CombatManager Init OK");
        }

        private void RegistStartActionFunc()
        {
            _dicStartActionFunc.Add(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_ROTATE_RIGHT, StartActionRotateRight);
            _dicStartActionFunc.Add(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_ROTATE_LEFT, StartActionRotateLeft);
            _dicStartActionFunc.Add(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_CAST, StartActionCast);
        }

        internal void InitCombatTeam(ref CombatTeam refTeam, int teamId)
        {
            if (refTeam._team == CombatCore.eCombatTeam.E_COMBAT_TEAM_PLAYER)
            {
                _playerCombatTeam = refTeam;
            }
            else if (refTeam._team == CombatCore.eCombatTeam.E_COMBAT_TEAM_OPPONENT)
            {
                _opponentCombatTeam = refTeam;
            }
            else
            {
                Debug.LogError("Unknown CombatTeam: " + refTeam._team);
                return;
            }

            refTeam._uiCombatCircle.Init();

            refTeam._uiEnergyBar.SetWidthPerPoint(GameConst.BAR_ENERGY_POINT);

            refTeam.SetEnergyPoint(0);
            refTeam.SetMatchMemberId(1);

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
            CombatRole playerCombatRole = new CombatRole();
            _playerCombatTeam.GetMatchCombatRole(out playerCombatRole);

            CombatRole opponentCombatRole = new CombatRole();
            _opponentCombatTeam.GetMatchCombatRole(out opponentCombatRole);

            CombatCore.eCombatMatchResult result = CheckCombatRoleMatch(playerCombatRole, opponentCombatRole);

            switch (result)
            {
                case CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN:
                {
                    _playerCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_WIN_ENERGY_POINT);
                    _opponentCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_LOSE_ENERGY_POINT);
                    break;
                }
                case CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE:
                {
                    _playerCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_LOSE_ENERGY_POINT);
                    _opponentCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_WIN_ENERGY_POINT);
                    break;
                }
                case CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_DRAW:
                {
                    _playerCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_DRAW_ENERGY_POINT);
                    _opponentCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_DRAW_ENERGY_POINT);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        private void StartActionRotateRight()
        {
            RotateCombatCircle(ref _playerCombatTeam._uiCombatCircle, true);
            _playerCombatTeam.ChangeMatchMemberId(true);

            bool isDirectionRight = _combatAI.GetNextAction();
            RotateCombatCircle(ref _opponentCombatTeam._uiCombatCircle, isDirectionRight);
            _opponentCombatTeam.ChangeMatchMemberId(isDirectionRight);

            AudioManager.Instance.PlaySfx(RotateSfxId);

            CombatRoundState = CombatCore.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
        }

        private void StartActionRotateLeft()
        {
            RotateCombatCircle(ref _playerCombatTeam._uiCombatCircle, false);
            _playerCombatTeam.ChangeMatchMemberId(false);

            bool isDirectionRight = _combatAI.GetNextAction();
            RotateCombatCircle(ref _opponentCombatTeam._uiCombatCircle, isDirectionRight);
            _opponentCombatTeam.ChangeMatchMemberId(isDirectionRight);

            AudioManager.Instance.PlaySfx(RotateSfxId);

            CombatRoundState = CombatCore.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
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
        }

        internal bool IsCombatCircleStandby()
        {
            if (_playerCombatTeam._uiCombatCircle.IsStandby() && _opponentCombatTeam._uiCombatCircle.IsStandby())
            {
                return true;
            }

            return false;
        }

        private CombatCore.eCombatMatchResult CheckCombatRoleMatch(CombatRole playerCombatRole, CombatRole opponentCombatRole)
        {
            if (playerCombatRole.Role.Attribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
            {
                if (opponentCombatRole.Role.Attribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN;
                }
                else if (opponentCombatRole.Role.Attribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE;
                }
            }
            else if (playerCombatRole.Role.Attribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
            {
                if (opponentCombatRole.Role.Attribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE;
                }
                else if (opponentCombatRole.Role.Attribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN;
                }
            }
            else  if (playerCombatRole.Role.Attribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
            {
                if (opponentCombatRole.Role.Attribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN;
                }
                else if (opponentCombatRole.Role.Attribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE;
                }
            }

            return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_DRAW;
        }
    }
}
