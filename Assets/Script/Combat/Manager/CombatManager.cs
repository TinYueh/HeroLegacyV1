using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem;

namespace GameCombat
{
    public class CombatManager : Singleton<CombatManager>
    {
        private CombatAI _combatAI;
        private CombatFormula _combatFormula;

        // View
        private ViewCombatTeam _playerVwCombatTeam;
        private ViewCombatTeam _opponentVwCombatTeam;

        // Model
        private CombatTeam _playerCombatTeam;
        private CombatTeam _opponentCombatTeam;

        private delegate void DlgStartActionFunc();
        private Dictionary<GameEnum.eCombatRoundAction, DlgStartActionFunc> _dicStartActionFunc;

        internal GameEnum.eCombatRoundState CombatRoundState { get; set; }

        public override void Init()
        {
            _combatAI = new CombatAI();
            _combatFormula = new CombatFormula();

            _playerVwCombatTeam = GameObject.Find("UIPlayer").GetComponent<ViewCombatTeam>();
            _opponentVwCombatTeam = GameObject.Find("UIOpponent").GetComponent<ViewCombatTeam>();

            _playerCombatTeam = new CombatTeam();
            _playerCombatTeam.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER, _playerVwCombatTeam);

            _opponentCombatTeam = new CombatTeam();
            _playerCombatTeam.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER, _opponentVwCombatTeam);

            _dicStartActionFunc = new Dictionary<GameEnum.eCombatRoundAction, DlgStartActionFunc>();

            RegistStartActionFunc();

            Debug.Log("CombatManager Init OK");
        }

        private void RegistStartActionFunc()
        {           
            _dicStartActionFunc.Add(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_RIGHT, StartActionRotateRight);
            _dicStartActionFunc.Add(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_LEFT, StartActionRotateLeft);
            _dicStartActionFunc.Add(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_CAST, StartActionCast);
        }

        internal bool CreateNewCombat(int playerTeamId, int opponentTeamId)
        {
            //InitCombatTeam(_playerCombatTeam, _playerViewCombatTeam, playerTeamId);

            return true;
        }

        internal void InitCombatTeam(CombatTeam team, int teamId)
        {
            //team.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER, view);
            




        }

        internal void StartRoundAction(GameEnum.eCombatRoundAction action)
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
            //CombatRole playerCombatRole = new CombatRole();
            //_playerCombatTeam.GetMatchCombatRole(out playerCombatRole);
            //if (playerCombatRole == null)
            //{
            //    Debug.LogError("Not found Player MatchCombatRole");
            //    return;
            //}

            //CombatRole opponentCombatRole = new CombatRole();
            //_opponentCombatTeam.GetMatchCombatRole(out opponentCombatRole);
            //if (opponentCombatRole == null)
            //{
            //    Debug.LogError("Not found Opponent MatchCombatRole");
            //    return;
            //}

            //GameEnum.eCombatMatchResult result = GetMatchResult(playerCombatRole.Role.Attribute, opponentCombatRole.Role.Attribute);
            //switch (result)
            //{
            //    case GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN:
            //    {
            //        _playerCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_WIN_ENERGY_POINT);
            //        _opponentCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_LOSE_ENERGY_POINT);
            //        break;
            //    }
            //    case GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE:
            //    {
            //        _playerCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_LOSE_ENERGY_POINT);
            //        _opponentCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_WIN_ENERGY_POINT);
            //        break;
            //    }
            //    case GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_DRAW:
            //    {
            //        _playerCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_DRAW_ENERGY_POINT);
            //        _opponentCombatTeam.ChangeEnergyPoint(GameConst.COMBAT_MATCH_DRAW_ENERGY_POINT);
            //        break;
            //    }
            //    default:
            //    {
            //        Debug.LogError("Unknown CombatMatchResult: " + result);
            //        break;
            //    }
            //}

            //int damageValue = 0;
            
            //_combatFormula.GetNormalDamage(playerCombatRole, opponentCombatRole, (result == GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN), out damageValue);
            //playerCombatRole.NormalDamage = damageValue;
            //opponentCombatRole.ChangeLife(-damageValue);
            
            //_combatFormula.GetNormalDamage(opponentCombatRole, playerCombatRole, (result == GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_DRAW), out damageValue);
            //opponentCombatRole.NormalDamage = damageValue;
            //playerCombatRole.ChangeLife(-damageValue);
        }

        private void StartActionRotateRight()
        {
            //RotateCombatCircle(ref _playerCombatTeam._viewCombatCircle, true, 1);
            //_playerCombatTeam.ChangeMatchSlotId(true);

            //bool isDirectionRight = _combatAI.GetNextAction();
            //RotateCombatCircle(ref _opponentCombatTeam._viewCombatCircle, isDirectionRight, 1);
            //_opponentCombatTeam.ChangeMatchSlotId(isDirectionRight);

            //AudioManager.Instance.PlaySfx(201);

            //CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
        }

        private void StartActionRotateLeft()
        {
            //RotateCombatCircle(ref _playerCombatTeam._viewCombatCircle, false, 1);
            //_playerCombatTeam.ChangeMatchSlotId(false);

            //bool isDirectionRight = _combatAI.GetNextAction();
            //RotateCombatCircle(ref _opponentCombatTeam._viewCombatCircle, isDirectionRight, 1);
            //_opponentCombatTeam.ChangeMatchSlotId(isDirectionRight);

            //AudioManager.Instance.PlaySfx(201);

            //CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
        }

        private void StartActionCast()
        {

        }

        private void RotateCombatCircle(ref ViewCombatCircle refUICombatCircle, bool isDirectionRight, int deltaSlot)
        {
            if (isDirectionRight)
            {
                refUICombatCircle.RotateAnglePerFrameActual = -refUICombatCircle.RotateAnglePerFrame;
            }
            else
            {
                refUICombatCircle.RotateAnglePerFrameActual = refUICombatCircle.RotateAnglePerFrame;
            }

            refUICombatCircle.RotateAnglePerTime = GameConst.COMBAT_CIRCLE_SLOT_ANGLE * deltaSlot;

            refUICombatCircle.EnableRotate();
        }

        internal bool IsCombatCircleStandby()
        {
            //if (_playerCombatTeam._viewCombatCircle.IsStandby() && _opponentCombatTeam._viewCombatCircle.IsStandby())
            //{
            //    return true;
            //}

            return false;
        }

        private GameEnum.eCombatMatchResult GetMatchResult(GameEnum.eRoleAttribute playerAttribute, GameEnum.eRoleAttribute opponentAttribute)
        {
            if (playerAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
            {
                if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
                {
                    return GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN;
                }
                else if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
                {
                    return GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE;
                }
            }
            else if (playerAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
            {
                if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
                {
                    return GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE;
                }
                else if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
                {
                    return GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN;
                }
            }
            else  if (playerAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_TECHNIQUE)
            {
                if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_POWER)
                {       
                    return GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_WIN;
                }
                else if (opponentAttribute == GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_SPEED)
                {
                    return GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE;
                }
            }

            return GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_DRAW;
        }
    }
}
