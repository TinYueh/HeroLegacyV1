using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Audio;

namespace GameCombat
{
    public class CombatManager : Singleton<CombatManager>
    {
        private CombatAI _combatAI = new CombatAI();
        private CombatFormula _combatFormula = new CombatFormula();

        private int _rotateSfxId = 201;

        // Model
        private CombatTeam _combatPlayer = new CombatTeam();
        private CombatTeam _combatOpponent = new CombatTeam();

        private delegate void DlgStartActionFunc();
        private Dictionary<GameEnum.eCombatRoundAction, DlgStartActionFunc> _dicStartActionFunc = new Dictionary<GameEnum.eCombatRoundAction, DlgStartActionFunc>();

        internal GameEnum.eCombatRoundState CombatRoundState { get; set; }

        public override void Init()
        {
            ViewCombatTeam vwCombatPlayer = GameObject.Find("UIPlayer").GetComponent<ViewCombatTeam>();
            vwCombatPlayer.Init();

            ViewCombatTeam vwCombatOpponent = GameObject.Find("UIOpponent").GetComponent<ViewCombatTeam>();
            vwCombatOpponent.Init();

            _combatPlayer.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER, ref vwCombatPlayer);

            _combatOpponent.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_OPPONENT, ref vwCombatOpponent);

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
            if (_combatPlayer.Setup(playerTeamId) == false)
            {
                Debug.LogError("Setup CombatPlayer failed, TeamId: " + playerTeamId);
                return false;
            }
            
            if (_combatOpponent.Setup(opponentTeamId) == false)
            {
                Debug.LogError("Setup CombatOpponent failed, TeamId: " + opponentTeamId);
                return false;
            }

            return true;
        }

        internal void StartRoundAction(GameEnum.eCombatRoundAction action)
        {
            DlgStartActionFunc dlgFunc;
            if (_dicStartActionFunc.TryGetValue(action, out dlgFunc) == false)
            {
                Debug.LogError("Not found StartActionFunc for " + action);
                return;
            }

            dlgFunc();
        }

        private void StartActionRotateRight()
        {
            _combatPlayer.SetRotation(GameEnum.eRotateDirection.E_ROTATE_DIRECTION_RIGHT);

            GameEnum.eRotateDirection direction = GameEnum.eRotateDirection.E_ROTATE_DIRECTION_NA;
            _combatAI.GetNextAction(out direction);
            _combatOpponent.SetRotation(direction);

            AudioManager.Instance.PlaySfx(_rotateSfxId);

            CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
        }

        private void StartActionRotateLeft()
        {
            _combatPlayer.SetRotation(GameEnum.eRotateDirection.E_ROTATE_DIRECTION_LEFT);

            GameEnum.eRotateDirection direction = GameEnum.eRotateDirection.E_ROTATE_DIRECTION_NA;
            _combatAI.GetNextAction(out direction);
            _combatOpponent.SetRotation(direction);

            AudioManager.Instance.PlaySfx(_rotateSfxId);

            CombatRoundState = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_ROTATE;
        }

        private void StartActionCast()
        {

        }

        internal bool CheckRoundAction()
        {
            if (_combatPlayer.IsStandby() == false || _combatOpponent.IsStandby() == false)
            {
                return false;
            }

            bool isReady = true;

            if (_combatPlayer.ExecMatchCombatCircle(ref _combatOpponent) == false)
            {
                _combatPlayer.SetRotation(_combatPlayer.RotateDirection);
                isReady = false;
            }

            if (_combatOpponent.ExecMatchCombatCircle(ref _combatPlayer) == false)
            {
                _combatOpponent.SetRotation(_combatOpponent.RotateDirection);
                isReady = false;
            }

            if (isReady == false)
            {
                AudioManager.Instance.PlaySfx(_rotateSfxId);
            }

            return isReady;
        }

        internal void ExecRoundAction()
        {
            //CombatRole playerCombatRole = new CombatRole();
            //_combatPlayer.GetMatchCombatRole(out playerCombatRole);
            //if (playerCombatRole == null)
            //{
            //    Debug.LogError("Not found Player MatchCombatRole");
            //    return;
            //}

            //CombatRole opponentCombatRole = new CombatRole();
            //_combatOpponent.GetMatchCombatRole(out opponentCombatRole);
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
            //        _combatPlayer.ChangeEnergyPoint(GameConst.COMBAT_MATCH_WIN_ENERGY_POINT);
            //        _combatOpponent.ChangeEnergyPoint(GameConst.COMBAT_MATCH_LOSE_ENERGY_POINT);
            //        break;
            //    }
            //    case GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_LOSE:
            //    {
            //        _combatPlayer.ChangeEnergyPoint(GameConst.COMBAT_MATCH_LOSE_ENERGY_POINT);
            //        _combatOpponent.ChangeEnergyPoint(GameConst.COMBAT_MATCH_WIN_ENERGY_POINT);
            //        break;
            //    }
            //    case GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_DRAW:
            //    {
            //        _combatPlayer.ChangeEnergyPoint(GameConst.COMBAT_MATCH_DRAW_ENERGY_POINT);
            //        _combatOpponent.ChangeEnergyPoint(GameConst.COMBAT_MATCH_DRAW_ENERGY_POINT);
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
            //opponentCombatRole.ChangeHealth(-damageValue);
            
            //_combatFormula.GetNormalDamage(opponentCombatRole, playerCombatRole, (result == GameEnum.eCombatMatchResult.E_COMBAT_MATCH_RESULT_DRAW), out damageValue);
            //opponentCombatRole.NormalDamage = damageValue;
            //playerCombatRole.ChangeHealth(-damageValue);
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
