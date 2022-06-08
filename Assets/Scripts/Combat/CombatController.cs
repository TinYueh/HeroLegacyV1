using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Audio;
using GameSkill;

namespace GameCombat
{
    public class CombatController
    {
        private ViewCombatTeam _viewCombatPlayer = null;
        private ViewCombatTeam _viewCombatOpponent = null;

        private CombatTeam _combatPlayer = new CombatTeam();
        private CombatTeam _combatOpponent = new CombatTeam();

        private int _sfxRotate = 201;

        private delegate void DlgStartRoundActionFunc(CombatTeam combatTeam);
        private Dictionary<GameEnum.eCombatRoundAction, DlgStartRoundActionFunc> _dicStartRoundActionFunc = new Dictionary<GameEnum.eCombatRoundAction, DlgStartRoundActionFunc>();

        internal bool Init()
        {
            _viewCombatPlayer = GameObject.Find("UIPlayer").GetComponent<ViewCombatTeam>();
            if (_viewCombatPlayer.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER) == false)
            {
                Debug.LogError("Init ViewCombatPlayer failed");
                return false;
            }

            _viewCombatOpponent = GameObject.Find("UIOpponent").GetComponent<ViewCombatTeam>();
            if (_viewCombatOpponent.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_OPPONENT) == false)
            {
                Debug.LogError("Init ViewCombatOpponent failed");
                return false;
            }

            if (_combatPlayer.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER, _viewCombatPlayer) == false)
            {
                Debug.LogError("Init CombatPlayer failed");
                return false;
            }

            if (_combatOpponent.Init(GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_OPPONENT, _viewCombatOpponent) == false)
            {
                Debug.LogError("Init CombatOpponent failed");
                return false;
            }

            RegistStartRoundActionFunc();

            return true;
        }

        private void RegistStartRoundActionFunc()
        {
            _dicStartRoundActionFunc.Add(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_RIGHT, StartRoundActionRotateRight);
            _dicStartRoundActionFunc.Add(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_LEFT, StartRoundActionRotateLeft);
            _dicStartRoundActionFunc.Add(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_CAST, StartRoundActionCast);
        }

        internal bool CreateNewCombat(int playerTeamId, int opponentTeamId)
        {
            if (_combatPlayer.Set(playerTeamId) == false)
            {
                Debug.LogError("Set CombatPlayer failed, TeamId: " + playerTeamId);
                return false;
            }

            if (_combatOpponent.Set(opponentTeamId) == false)
            {
                Debug.LogError("Set CombatOpponent failed, TeamId: " + opponentTeamId);
                return false;
            }

            FlipFirstToken();

            return true;
        }

        private void FlipFirstToken()
        {
            if (_combatPlayer.HasFirstToken != _combatOpponent.HasFirstToken)
            {
                bool hasFirstToken = _combatOpponent.HasFirstToken;

                _combatPlayer.SetFirstToken(hasFirstToken);
                _combatOpponent.SetFirstToken(!hasFirstToken);
            }
            else
            {
                bool hasFirstToken = (Random.Range(0, 2) == 0);

                _combatPlayer.SetFirstToken(hasFirstToken);
                _combatOpponent.SetFirstToken(!hasFirstToken);
            }
        }

        internal bool IsCombatCircleRotate()
        {
            return (_viewCombatPlayer.ViewCombatCircle.IsRotate() || _viewCombatOpponent.ViewCombatCircle.IsRotate());
        }

        private bool HandleAttributeMatch(CombatRole player, CombatRole opponent, out GameEnum.eCombatAttributeMatchResult outResult)
        {
            outResult = CombatManager.Instance.CombatFormula.CheckAttributeMatch(player.Role.Attribute, opponent.Role.Attribute);

            switch (outResult)
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
                        Debug.LogError("Unknown CombatAttributeMatchResult: " + outResult);
                        return false;
                    }
            }

            return true;
        }

        private bool HandleNormalAttack(CombatRole player, CombatRole opponent, GameEnum.eCombatAttributeMatchResult result)
        {
            int damageValue = 0;

            CombatRole first = null;    // 先攻
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
                        second = (first == player) ? opponent : player;
                        FlipFirstToken();
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            // 先攻
            if (CheckAbortNormalAttack(first, second))
            {
                return true;
            }

            if (CheckExecNormalAttack(first, second))
            {
                CombatManager.Instance.CombatFormula.GetNormalAttackDamage(first, second, true, out damageValue);
                first.NormalDamage = damageValue;
                second.ChangeHealth(-damageValue);
            }

            // 後攻
            if (CheckAbortNormalAttack(second, first))
            {
                return true;
            }

            if (CheckExecNormalAttack(second, first))
            {
                CombatManager.Instance.CombatFormula.GetNormalAttackDamage(second, first, false, out damageValue);
                second.NormalDamage = damageValue;
                first.ChangeHealth(-damageValue);
            }

            return true;
        }

        #region Round Action

        internal void StartRoundAction(GameEnum.eCombatRoundAction playerAction)
        {
            // 進行中關閉 UI
            _combatOpponent.ViewCombatTeam.ViewSkillList.Hide();
            _combatPlayer.ViewCombatTeam.ViewSkillList.Hide();

            // Player
            _dicStartRoundActionFunc[playerAction](_combatPlayer);

            // Opponent
            GameEnum.eCombatRoundAction opponentAction = GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_NA;
            CombatManager.Instance.CombatAI.GetRoundAction(out opponentAction);

            _dicStartRoundActionFunc[opponentAction](_combatOpponent);

            if (IsCombatCircleRotate())
            {
                // Todo: 音效統一管理
                AudioManager.Instance.PlaySfx(_sfxRotate);
            }
        }

        private void StartRoundActionRotateRight(CombatTeam combatTeam)
        {
            combatTeam.HandleRotation(GameEnum.eRotateDirection.E_ROTATE_DIRECTION_RIGHT);
        }

        private void StartRoundActionRotateLeft(CombatTeam combatTeam)
        {
            combatTeam.HandleRotation(GameEnum.eRotateDirection.E_ROTATE_DIRECTION_LEFT);
        }

        private void StartRoundActionCast(CombatTeam combatTeam)
        {

        }

        internal bool ProcessRoundAction()
        {
            if (_combatPlayer.ExecCircleSocket(_combatPlayer.MatchPosId, _combatOpponent) == false)
            {
                // 繼續旋轉
                _combatPlayer.HandleRotation(_combatPlayer.RotateDirection);
            }

            if (_combatOpponent.ExecCircleSocket(_combatOpponent.MatchPosId, _combatPlayer) == false)
            {
                // 繼續旋轉
                _combatOpponent.HandleRotation(_combatOpponent.RotateDirection);
            }

            if (IsCombatCircleRotate())
            {
                AudioManager.Instance.PlaySfx(_sfxRotate);

                return false;
            }

            // 真正靜止
            return true;
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
            if (HandleAttributeMatch(combatPlayer, combatOpponent, out result) == false)
            {
                Debug.LogError("HandleAttributeMatch failed");
            }

            // 普攻
            if (HandleNormalAttack(combatPlayer, combatOpponent, result) == false)
            {
                Debug.LogError("HandleNormalAttack failed");
            }
        }

        internal bool FinishRoundAction()
        {
            if (_combatPlayer.CheckTeamLiving() == false)
            {
                // Game Over
                Debug.Log("Game Over");

                return true;
            }
            else if (_combatOpponent.CheckTeamLiving() == false)
            {
                // Accomplish
                Debug.Log("Accomplish");

                return true;
            }

            return false;
        }

        #endregion

        #region On Click

        internal void OnClickCombatRolePortrait(GameEnum.eCombatTeamType teamType, int memberId)
        {
            CombatTeam combatTeam = null;
            if (GetCombatTeam(teamType, out combatTeam) == false)
            {
                Debug.LogError("Not found CombatTeam, TeamType: " + teamType);
                return;
            }

            CombatRole combatRole = null;
            if (combatTeam.GetCombatRoleByMember(memberId, out combatRole) == false)
            {
                Debug.LogError("Not found CombatRole, MemberId: " + memberId);
                return;
            }

            if (combatTeam.ViewCombatTeam.ViewSkillList.IsShow() && combatTeam.CastPosId == combatRole.PosId)
            {
                // 重複點選相同技能列則關閉
                combatTeam.CastPosId = 0;
                combatTeam.ViewCombatTeam.ViewSkillList.Hide();
            }
            else
            {
                // 開啟技能列
                combatTeam.CastPosId = combatRole.PosId;
                SetViewSkillList(combatTeam, combatRole);   // 設定技能列的內容
                combatTeam.ViewCombatTeam.ViewSkillList.Show();
            }
        }

        internal void OnClickCircleSocketEmblem(GameEnum.eCombatTeamType teamType, int posId)
        {
            CombatTeam combatTeam = null;
            if (GetCombatTeam(teamType, out combatTeam) == false)
            {
                Debug.LogError("Not found CombatTeam, TeamType: " + teamType);
                return;
            }

            CombatRole combatRole = null;
            if (combatTeam.GetCombatRoleByPos(posId, out combatRole) == false)
            {
                Debug.LogError("Not found CombatRole, PosId: " + posId);
                return;
            }

            if (combatTeam.ViewCombatTeam.ViewSkillList.IsShow() && combatTeam.CastPosId == combatRole.PosId)
            {
                // 重複點選相同技能列則關閉
                combatTeam.CastPosId = 0;
                combatTeam.ViewCombatTeam.ViewSkillList.Hide();
            }
            else
            {
                // 開啟技能列
                combatTeam.CastPosId = combatRole.PosId;
                SetViewSkillList(combatTeam, combatRole);
                combatTeam.ViewCombatTeam.ViewSkillList.Show();
            }
        }

        internal void OnClickSkill(GameEnum.eCombatTeamType teamType, int skillId)
        {
            CombatTeam combatTeam = null;
            if (GetCombatTeam(teamType, out combatTeam) == false)
            {
                Debug.LogError("Not found CombatTeam, TeamType: " + teamType);
                return;
            }

            combatTeam.CastSkillId = skillId;

            CombatManager.Instance.StartRoundAction(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_CAST);
        }

        #endregion

        #region Check

        private bool CheckAbortNormalAttack(CombatRole source, CombatRole target)
        {
            // 立即中止普攻的條件

            if (source.State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING
                || target.State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING)
            {
                return true;
            }

            return false;
        }

        private bool CheckExecNormalAttack(CombatRole source, CombatRole target)
        {
            // 排除不能普攻的條件

            return true;
        }

        private bool CheckPos(GameEnum.ePosType posType, CombatTeam combatTeam, int posId)
        {
            List<int> listSkill = new List<int>();
            if (combatTeam.GetPosList(posType, listSkill) == false)
            {
                return false;
            }

            if (listSkill.Exists(x => x == posId) == false)
            {
                return false;
            }

            return true;
        }

        private bool CheckSkillEnableCondition(Dictionary<GameEnum.eSkillEnableCondition, bool> dicEnable)
        {
            foreach (var enable in dicEnable)
            {
                if (enable.Value == false)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Get Set

        internal bool GetCombatTeam(GameEnum.eCombatTeamType teamType, out CombatTeam outCombatTeam)
        {
            outCombatTeam = null;

            if (teamType == GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER)
            {
                outCombatTeam = _combatPlayer;
            }
            else if (teamType == GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_OPPONENT)
            {
                outCombatTeam = _combatOpponent;
            }
            else
            {
                return false;
            }

            return true;
        }

        internal bool GetCircleSocket(GameEnum.eCombatTeamType teamType, int posId, out CircleSocket outCircleSocket)
        {
            outCircleSocket = null;

            if (teamType == GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER)
            {
                _combatPlayer.GetCircleSocket(posId, out outCircleSocket);
            }
            else if (teamType == GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_OPPONENT)
            {
                _combatOpponent.GetCircleSocket(posId, out outCircleSocket);
            }
            else
            {
                return false;
            }

            return true;
        }

        private void SetViewSkillList(CombatTeam combatTeam, CombatRole combatRole)
        {
            // Todo: UI 顯示不可施放的原因
            Dictionary<GameEnum.eSkillEnableCondition, bool> dicEnable = new Dictionary<GameEnum.eSkillEnableCondition, bool>();

            for (int i = 0; i < GameConst.MAX_ROLE_SKILL; ++i)
            {
                if (i < combatRole.Role.ListSkill.Count)
                {
                    Skill skill = null;
                    if (SkillManager.Instance.GetSkill(combatRole.Role.ListSkill[i], out skill) == false)
                    {
                        continue;
                    }

                    dicEnable[GameEnum.eSkillEnableCondition.E_SKILL_ENABLE_CONDITION_TEAM] = (combatTeam.TeamType == GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER);
                    dicEnable[GameEnum.eSkillEnableCondition.E_SKILL_ENABLE_CONDITION_POS] = CheckPos(skill.PosType, combatTeam, combatRole.PosId);
                    dicEnable[GameEnum.eSkillEnableCondition.E_SKILL_ENABLE_CONDITION_ENERGY] = (combatTeam.EnergyOrb >= skill.Cost);

                    combatTeam.ViewCombatTeam.ViewSkillList.ShowSkill(i, skill.Id, CheckSkillEnableCondition(dicEnable));
                }
                else
                {
                    combatTeam.ViewCombatTeam.ViewSkillList.HideSkill(i);
                }

                dicEnable.Clear();
            }
        }

        #endregion
    }
}