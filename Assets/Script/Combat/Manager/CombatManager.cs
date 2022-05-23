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
        }

        private void StartActionRotateLeft()
        {
            _combatPlayer.SetRotation(GameEnum.eRotateDirection.E_ROTATE_DIRECTION_LEFT);

            GameEnum.eRotateDirection direction = GameEnum.eRotateDirection.E_ROTATE_DIRECTION_NA;
            _combatAI.GetNextAction(out direction);
            _combatOpponent.SetRotation(direction);

            AudioManager.Instance.PlaySfx(_rotateSfxId);
        }

        private void StartActionCast()
        {

        }

        internal bool ProcessRoundAction()
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

            // ÄÝ©Ê¹ï¾Ô
            GameEnum.eCombatAttributeMatchResult result = GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_NA;
            if (HandleAttributeMatch(ref combatPlayer, ref combatOpponent, ref result) == false)
            {
                Debug.LogError("HandleAttributeMatch failed");
            }

            // ´¶§ð
            if (HandleNormalAttack(ref combatPlayer, ref combatOpponent, result) == false)
            {
                Debug.LogError("HandleNormalAttack failed");
            }

        }

        private  bool HandleAttributeMatch(ref CombatRole refPlayer, ref CombatRole refOpponent, ref GameEnum.eCombatAttributeMatchResult refResult)
        {
            refResult = _combatFormula.CheckAttributeMatch(refPlayer.Role.Attribute, refOpponent.Role.Attribute);
            
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

        private bool HandleNormalAttack(ref CombatRole refPlayer, ref CombatRole refOpponent, GameEnum.eCombatAttributeMatchResult result)
        {
            int damageValue = 0;

            _combatFormula.GetNormalDamage(refPlayer, refOpponent, (result == GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_WIN), out damageValue);
            refPlayer.NormalDamage = damageValue;
            _combatOpponent.ChangeHealth(refOpponent.MemberId, -damageValue);

            _combatFormula.GetNormalDamage(refOpponent, refPlayer, (result == GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_DRAW), out damageValue);
            refOpponent.NormalDamage = damageValue;
            _combatPlayer.ChangeHealth(refPlayer.MemberId, -damageValue);

            return true;
        }
    }
}
