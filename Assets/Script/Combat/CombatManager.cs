using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatManager : Singleton<CombatManager>
    {
        private CombatAI _combatAI = new CombatAI();
        private CombatFormula _combatFormula = new CombatFormula();
        private CombatTeam _playerCombatTeam = null;
        private CombatTeam _opponentCombatTeam = null;

        internal int RotateSfxId { get; set; } = 0;     // 戰圓旋轉音效

        private delegate void DlgStartActionFunc();
        private Dictionary<CombatCore.eCombatRoundAction, DlgStartActionFunc> _dicStartActionFunc = new Dictionary<CombatCore.eCombatRoundAction, DlgStartActionFunc>();

        internal CombatCore.eCombatRoundState CombatRoundState { get; set; } = CombatCore.eCombatRoundState.E_COMBAT_ROUND_STATE_NA;

        public override void Init()
        {
            RegistStartActionFunc();

            Debug.Log("CombatManager Init OK");
        }

        private void RegistStartActionFunc()
        {
            _dicStartActionFunc.Add(CombatCore.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_RIGHT, StartActionRotateRight);
            _dicStartActionFunc.Add(CombatCore.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_LEFT, StartActionRotateLeft);
            _dicStartActionFunc.Add(CombatCore.eCombatRoundAction.E_COMBAT_ROUND_ACTION_CAST, StartActionCast);
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

        internal void StartRoundAction(CombatCore.eCombatRoundAction action)
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

        internal void ExecRoundAction()
        {
            CombatRole playerCombatRole = new CombatRole();
            _playerCombatTeam.GetMatchCombatRole(out playerCombatRole);
            if (playerCombatRole == null)
            {
                Debug.LogError("Not found Player MatchCombatRole");
                return;
            }

            CombatRole opponentCombatRole = new CombatRole();
            _opponentCombatTeam.GetMatchCombatRole(out opponentCombatRole);
            if (opponentCombatRole == null)
            {
                Debug.LogError("Not found Opponent MatchCombatRole");
                return;
            }

            CombatCore.eCombatMatchResult result = GetMatchResult(playerCombatRole.Role.Attribute, opponentCombatRole.Role.Attribute);
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
                        Debug.LogError("Unknown CombatMatchResult: " + result);
                        break;
                    }
            }

            int damageValue = 0;
            
            _combatFormula.GetNormalDamage(playerCombatRole, opponentCombatRole, (result == CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN), out damageValue);
            playerCombatRole.NormalDamage = damageValue;
            opponentCombatRole.ChangeLife(-damageValue);
            
            _combatFormula.GetNormalDamage(opponentCombatRole, playerCombatRole, (result == CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_DRAW), out damageValue);
            opponentCombatRole.NormalDamage = damageValue;
            playerCombatRole.ChangeLife(-damageValue);
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

        private CombatCore.eCombatMatchResult GetMatchResult(GameEnum.eRoleAttribute playerAttribute, GameEnum.eRoleAttribute opponentAttribute)
        {
            if (playerAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
            {
                if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN;
                }
                else if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE;
                }
            }
            else if (playerAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
            {
                if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE;
                }
                else if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN;
                }
            }
            else  if (playerAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
            {
                if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
                {       
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN;
                }
                else if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
                {
                    return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE;
                }
            }

            return CombatCore.eCombatMatchResult.E_COMBAT_MATCH_RESULT_DRAW;
        }
    }
}
