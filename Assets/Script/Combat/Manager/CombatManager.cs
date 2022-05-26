using GameSystem.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CombatManager : Singleton<CombatManager>
    {
        private CombatAI _combatAI = new CombatAI();
        private CombatFormula _combatFormula = new CombatFormula();

        // Todo: 統一管理音效
        private int _rotateSfxId = 201;

        private CombatTeam _combatPlayer = new CombatTeam();
        private CombatTeam _combatOpponent = new CombatTeam();

        private delegate void DlgStartActionFunc();
        private Dictionary<GameEnum.eCombatRoundAction, DlgStartActionFunc> _dicStartActionFunc = new Dictionary<GameEnum.eCombatRoundAction, DlgStartActionFunc>();

        internal GameEnum.eCombatRoundState CombatRoundState { get; set; } = GameEnum.eCombatRoundState.E_COMBAT_ROUND_STATE_NA;

        public override bool Init()
        {
            ViewCombatTeam vwCombatPlayer = GameObject.Find("UIPlayer").GetComponent<ViewCombatTeam>();
            if (vwCombatPlayer.Init() == false)
            {
                Debug.LogError("Init ViewCombatPlayer failed");
                return false;
            }

            ViewCombatTeam vwCombatOpponent = GameObject.Find("UIOpponent").GetComponent<ViewCombatTeam>();
            if (vwCombatOpponent.Init() == false)
            {
                Debug.LogError("Init ViewCombatOpponent failed");
                return false;
            }

            if (_combatPlayer.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER, vwCombatPlayer) == false)
            {
                Debug.LogError("Init CombatPlayer failed");
                return false;
            }

            if (_combatOpponent.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_OPPONENT, vwCombatOpponent) == false)
            {
                Debug.LogError("Init CombatOpponent failed");
                return false;
            }

            RegistStartActionFunc();

            Debug.Log("CombatManager Init OK");
            return true;
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

            HandleFirstToken();

            return true;
        }

        internal void StartRoundAction(GameEnum.eCombatRoundAction action)
        {
            DlgStartActionFunc dlgFunc = null;
            if (_dicStartActionFunc.TryGetValue(action, out dlgFunc) == false)
            {
                Debug.LogError("Not found StartActionFunc for " + action);
                return;
            }

            dlgFunc();
        }

        private void StartActionRotateRight()
        {
            _combatPlayer.HandleRotation(GameEnum.eRotateDirection.E_ROTATE_DIRECTION_RIGHT);

            GameEnum.eRotateDirection direction = GameEnum.eRotateDirection.E_ROTATE_DIRECTION_NA;
            _combatAI.GetNextAction(out direction);
            _combatOpponent.HandleRotation(direction);

            AudioManager.Instance.PlaySfx(_rotateSfxId);
        }

        private void StartActionRotateLeft()
        {
            _combatPlayer.HandleRotation(GameEnum.eRotateDirection.E_ROTATE_DIRECTION_LEFT);

            GameEnum.eRotateDirection direction = GameEnum.eRotateDirection.E_ROTATE_DIRECTION_NA;
            _combatAI.GetNextAction(out direction);
            _combatOpponent.HandleRotation(direction);

            AudioManager.Instance.PlaySfx(_rotateSfxId);
        }

        private void StartActionCast()
        {

        }

        internal bool ProcessRoundAction()
        {
            if (_combatPlayer.VwCombatTeam.IsStandby() == false || _combatOpponent.VwCombatTeam.IsStandby() == false)
            {
                return false;
            }

            bool isReady = true;

            if (_combatPlayer.ExecCircleSocket(_combatPlayer.MatchPosId, _combatOpponent) == false)
            {
                _combatPlayer.HandleRotation(_combatPlayer.RotateDirection);
                isReady = false;
            }

            if (_combatOpponent.ExecCircleSocket(_combatOpponent.MatchPosId, _combatPlayer) == false)
            {
                _combatOpponent.HandleRotation(_combatOpponent.RotateDirection);
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
            CombatRole combatPlayer = null;
            if (_combatPlayer.GetCombatRoleByPos(_combatPlayer.MatchPosId, out combatPlayer) == false)
            {
                Debug.LogError("Get CombatPlayer failed");
                return;
            }

            CombatRole combatOpponent = null;
            if (_combatOpponent.GetCombatRoleByPos(_combatOpponent.MatchPosId, out combatOpponent) == false)
            {
                Debug.LogError("Get CombatOpponent failed");
                return;
            }

            // 屬性對戰
            GameEnum.eCombatAttributeMatchResult result = GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_NA;
            if (HandleAttributeMatch(combatPlayer, combatOpponent, ref result) == false)
            {
                Debug.LogError("HandleAttributeMatch failed");
            }

            // 普攻
            if (HandleNormalAttack(combatPlayer, combatOpponent, result) == false)
            {
                Debug.LogError("HandleNormalAttack failed");
            }
        }

        private bool HandleAttributeMatch(CombatRole player, CombatRole opponent, ref GameEnum.eCombatAttributeMatchResult refResult)
        {
            refResult = _combatFormula.CheckAttributeMatch(player.Role.Attribute, opponent.Role.Attribute);

            switch (refResult)
            {
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_WIN:
                    {
                        _combatPlayer.ChangeEnergyPoint(GameConst.COMBAT_MATCH_WIN_ENERGY_POINT);
                        _combatOpponent.ChangeEnergyPoint(GameConst.COMBAT_MATCH_LOSE_ENERGY_POINT);
                        break;
                    }
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_LOSE:
                    {
                        _combatPlayer.ChangeEnergyPoint(GameConst.COMBAT_MATCH_LOSE_ENERGY_POINT);
                        _combatOpponent.ChangeEnergyPoint(GameConst.COMBAT_MATCH_WIN_ENERGY_POINT);
                        break;
                    }
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_DRAW:
                    {
                        _combatPlayer.ChangeEnergyPoint(GameConst.COMBAT_MATCH_DRAW_ENERGY_POINT);
                        _combatOpponent.ChangeEnergyPoint(GameConst.COMBAT_MATCH_DRAW_ENERGY_POINT);
                        break;
                    }
                default:
                    {
                        Debug.LogError("Unknown CombatAttributeMatchResult: " + refResult);
                        return false;
                    }
            }

            return true;
        }

        private bool HandleNormalAttack(CombatRole player, CombatRole opponent, GameEnum.eCombatAttributeMatchResult result)
        {
            int damageValue = 0;

            CombatRole first = null;
            CombatRole second = null;

            switch (result)
            {
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_WIN:
                    {
                        first = player;
                        second = opponent;
                        break;
                    }
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_LOSE:
                    {
                        first = opponent;
                        second = player;
                        break;
                    }
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_DRAW:
                    {
                        first = (_combatPlayer.HasFirstToken == true) ? player : opponent;
                        second = (_combatPlayer.HasFirstToken == false) ? player : opponent;
                        HandleFirstToken();
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            // Todo: 重複 code 拆 func
            if (first.State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING
                || second.State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING)
            {
                return true;
            }

            _combatFormula.GetNormalDamage(first, second, out damageValue);
            first.NormalDamage = damageValue;
            second.ChangeHealth(-damageValue);

            if (first.State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING
                || second.State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING)
            {
                return true;
            }

            _combatFormula.GetNormalDamage(second, first, out damageValue);
            second.NormalDamage = damageValue;
            first.ChangeHealth(-damageValue);

            return true;
        }

        private void HandleFirstToken()
        {
            if (_combatPlayer.HasFirstToken != _combatOpponent.HasFirstToken)
            {
                bool getFirstToken = _combatOpponent.HasFirstToken;

                _combatPlayer.SetFirstToken(getFirstToken);
                _combatOpponent.SetFirstToken(!getFirstToken);
            }
            else
            {
                bool getFirstToken = (Random.Range(0, 2) == 0);

                _combatPlayer.SetFirstToken(getFirstToken);
                _combatOpponent.SetFirstToken(!getFirstToken);
            }
        }
    }
}
