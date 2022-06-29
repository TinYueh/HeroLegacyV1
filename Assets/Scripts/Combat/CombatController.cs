using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Audio;
using GameSkill;

namespace GameCombat
{
    public class CombatController
    {
        #region Property

        private ViewCombatTeam _viewCombatPlayer;
        private ViewCombatTeam _viewCombatOpponent;

        private CombatTeam _combatPlayer = new CombatTeam();
        private CombatTeam _combatOpponent = new CombatTeam();       

        private delegate void DlgStartRoundAction(CombatTeam sourceTeam, CombatTeam targetTeam);
        private Dictionary<GameEnum.eCombatRoundAction, DlgStartRoundAction> _dicDlgStartRoundAction = new Dictionary<GameEnum.eCombatRoundAction, DlgStartRoundAction>();

        // Todo: 音效轉正
        private int _sfxRotate = 201;

        #endregion  // Property

        #region Init

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

            RegistDlgStartRoundAction();

            return true;
        }

        #endregion  // Init

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

        internal bool GetCombatRoleByMember(GameEnum.eCombatTeamType teamType, int memberId, out CombatRole outCombatRole)
        {
            outCombatRole = null;

            CombatTeam combatTeam;
            if (GetCombatTeam(teamType, out combatTeam) == false)
            {
                return false;
            }

            if (combatTeam.GetCombatRoleByMember(memberId, out outCombatRole) == false)
            {
                return false;
            }

            return true;
        }

        internal bool GetCombatRoleByPos(GameEnum.eCombatTeamType teamType, int posId, out CombatRole outCombatRole)
        {
            outCombatRole = null;

            CombatTeam combatTeam;
            if (GetCombatTeam(teamType, out combatTeam) == false)
            {
                return false;
            }

            if (combatTeam.GetCombatRoleByPos(posId, out outCombatRole) == false)
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
                    Skill skill;
                    if (SkillManager.Instance.GetSkill(combatRole.Role.ListSkill[i], out skill) == false)
                    {
                        continue;
                    }

                    dicEnable[GameEnum.eSkillEnableCondition.E_SKILL_ENABLE_CONDITION_TEAM] = (combatTeam.TeamType == GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_PLAYER);
                    dicEnable[GameEnum.eSkillEnableCondition.E_SKILL_ENABLE_CONDITION_POS] = CheckPos(skill.PosType, combatTeam, combatRole.PosId);
                    dicEnable[GameEnum.eSkillEnableCondition.E_SKILL_ENABLE_CONDITION_ENERGY] = (combatTeam.EnergyOrb >= skill.Cost);
                    dicEnable[GameEnum.eSkillEnableCondition.E_SKILL_ENABLE_CONDITION_MATCH] = combatTeam.IsMatchPosAlive();
                    dicEnable[GameEnum.eSkillEnableCondition.E_SKILL_ENABLE_CONDITION_CD] = (combatRole.IsSkillCd(skill.Id) == false);

                    combatTeam.ViewCombatTeam.ViewSkillList.ShowSkill(i, skill.Id, combatRole.GetSkillCd(skill.Id), CheckSkillEnableCondition(dicEnable));
                }
                else
                {
                    combatTeam.ViewCombatTeam.ViewSkillList.HideSkill(i);
                }

                dicEnable.Clear();
            }
        }

        #endregion  // Get Set

        #region Logic

        private bool CheckAbortNormalAttack(CombatRole source, CombatRole target)
        {
            // 立即中止普攻的條件

            if (source.IsDying() || target.IsDying())
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
            List<int> listPos = new List<int>();
            if (combatTeam.GetPosList(posType, listPos) == false)
            {
                return false;
            }

            if (listPos.Exists(x => x == posId) == false)
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

        internal bool IsCombatCircleRotate()
        {
            return (_viewCombatPlayer.ViewCombatCircle.IsRotate() || _viewCombatOpponent.ViewCombatCircle.IsRotate());
        }

        #endregion  // Logic

        #region On Click

        internal void OnClickCombatRolePortrait(GameEnum.eCombatTeamType teamType, int memberId)
        {
            CombatTeam combatTeam;
            if (GetCombatTeam(teamType, out combatTeam) == false)
            {
                Debug.LogError("Not found CombatTeam, TeamType: " + teamType);
                return;
            }

            CombatRole combatRole;
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
            CombatTeam combatTeam;
            if (GetCombatTeam(teamType, out combatTeam) == false)
            {
                Debug.LogError("Not found CombatTeam, TeamType: " + teamType);
                return;
            }

            CombatRole combatRole;
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
            CombatTeam combatTeam;
            if (GetCombatTeam(teamType, out combatTeam) == false)
            {
                Debug.LogError("Not found CombatTeam, TeamType: " + teamType);
                return;
            }

            combatTeam.CastSkillId = skillId;

            CombatManager.Instance.StartRoundAction(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_CAST);
        }

        #endregion

        #region Round Action

        private void RegistDlgStartRoundAction()
        {
            _dicDlgStartRoundAction.Add(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_RIGHT, StartRoundActionRotateRight);
            _dicDlgStartRoundAction.Add(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_ROTATE_LEFT, StartRoundActionRotateLeft);
            _dicDlgStartRoundAction.Add(GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_CAST, StartRoundActionCast);
        }

        internal void StartRoundAction(GameEnum.eCombatRoundAction playerAction)
        {
            // 進行中關閉 UI
            _combatOpponent.ViewCombatTeam.ViewSkillList.Hide();
            _combatPlayer.ViewCombatTeam.ViewSkillList.Hide();

            // Player
            _dicDlgStartRoundAction[playerAction](_combatPlayer, _combatOpponent);

            // Opponent
            GameEnum.eCombatRoundAction opponentAction = GameEnum.eCombatRoundAction.E_COMBAT_ROUND_ACTION_NA;
            CombatManager.Instance.AI.GetRoundAction(out opponentAction);

            _dicDlgStartRoundAction[opponentAction](_combatOpponent, _combatPlayer);

            if (IsCombatCircleRotate())
            {
                // Todo: 音效統一管理
                AudioManager.Instance.PlaySfx(_sfxRotate);
            }
        }

        private void StartRoundActionRotateRight(CombatTeam sourceTeam, CombatTeam targetTeam)
        {
            sourceTeam.Rotate(GameEnum.eRotateDirection.E_ROTATE_DIRECTION_RIGHT);
        }

        private void StartRoundActionRotateLeft(CombatTeam sourceTeam, CombatTeam targetTeam)
        {
            sourceTeam.Rotate(GameEnum.eRotateDirection.E_ROTATE_DIRECTION_LEFT);
        }

        private void StartRoundActionCast(CombatTeam sourceTeam, CombatTeam targetTeam)
        {
        }

        internal bool ProcessRoundAction()
        {
            if (_combatPlayer.ExecCircleSocket(_combatPlayer.MatchPosId, _combatOpponent) == false)
            {
                // 繼續旋轉
                _combatPlayer.Rotate(_combatPlayer.RotateDirection);
            }

            if (_combatOpponent.ExecCircleSocket(_combatOpponent.MatchPosId, _combatPlayer) == false)
            {
                // 繼續旋轉
                _combatOpponent.Rotate(_combatOpponent.RotateDirection);
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
            // 屬性對戰
            GameEnum.eCombatAttributeMatchResult result = GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_NA;
            if (HandleAttributeMatch(_combatPlayer, _combatOpponent, out result) == false)
            {
                Debug.LogError("HandleAttributeMatch failed");
                return;
            }

            CombatTeam first = null;
            CombatTeam second = null;

            // 決定先後
            if (DecideFirst(_combatPlayer, _combatOpponent, result, ref first, ref second) == false)
            {
                Debug.LogError("HandleDecideFirst failed");
                return;
            }

            // 施放技能
            if (HandleCastSkill(first, second) == false)
            {
                Debug.LogError("HandleCastSkill failed");
            }

            // 普攻
            if (HandleNormalAttack(first.GetMatchCombatRole(), second.GetMatchCombatRole(), result) == false)
            {
                Debug.LogError("HandleNormalAttack failed");
            }
        }

        internal bool FinishRoundAction()
        {
            if (_combatPlayer.CheckTeamAlive() == false)
            {
                // Game Over
                Debug.Log("Game Over");

                return true;
            }
            else if (_combatOpponent.CheckTeamAlive() == false)
            {
                // Accomplish
                Debug.Log("Accomplish");

                return true;
            }

            return false;
        }

        internal void PrepareRoundAction()
        {
            _combatPlayer.Prepare();
            _combatOpponent.Prepare();
        }

        #endregion  // Round Action

        #region Method

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

        private bool HandleAttributeMatch(CombatTeam player, CombatTeam opponent, out GameEnum.eCombatAttributeMatchResult outResult)
        {
            outResult = CombatManager.Instance.Formula.CheckAttributeMatch(player.GetMatchCombatRole().Role.Attribute, opponent.GetMatchCombatRole().Role.Attribute);

            switch (outResult)
            {
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_WIN:
                    {
                        player.ChangeEnergyPoint(GameConst.COMBAT_MATCH_WIN_ENERGY_POINT);
                        opponent.ChangeEnergyPoint(GameConst.COMBAT_MATCH_LOSE_ENERGY_POINT);
                        break;
                    }
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_LOSE:
                    {
                        player.ChangeEnergyPoint(GameConst.COMBAT_MATCH_LOSE_ENERGY_POINT);
                        opponent.ChangeEnergyPoint(GameConst.COMBAT_MATCH_WIN_ENERGY_POINT);
                        break;
                    }
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_DRAW:
                    {
                        player.ChangeEnergyPoint(GameConst.COMBAT_MATCH_DRAW_ENERGY_POINT);
                        opponent.ChangeEnergyPoint(GameConst.COMBAT_MATCH_DRAW_ENERGY_POINT);
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

        private bool DecideFirst(CombatTeam player, CombatTeam opponoent, GameEnum.eCombatAttributeMatchResult result, ref CombatTeam refFirst, ref CombatTeam refSecond)
        {
            switch (result)
            {
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_WIN:
                    {
                        refFirst = player;
                        refSecond = opponoent;
                        break;
                    }
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_LOSE:
                    {
                        refFirst = opponoent;
                        refSecond = player;
                        break;
                    }
                case GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_DRAW:
                    {
                        refFirst = (player.HasFirstToken == true) ? player : opponoent;
                        refSecond = (refFirst == player) ? opponoent : player;
                        FlipFirstToken();
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            return true;
        }

        private bool HandleCastSkill(CombatTeam first, CombatTeam second)
        {
            // 先攻
            if (first.IsCastSkill())
            {
                SkillManager.Instance.ExecSkill(first.CastSkillId, first.GetCastCombatRole(), first, second);
                first.ClearSkill();
            }

            // 後攻
            if (second.IsCastSkill())
            {
                SkillManager.Instance.ExecSkill(second.CastSkillId, second.GetCastCombatRole(), second, first);
                second.ClearSkill();
            }

            return true;
        }

        private bool HandleNormalAttack(CombatRole first, CombatRole second, GameEnum.eCombatAttributeMatchResult result)
        {
            int damage = 0;

            // 先攻
            if (CheckAbortNormalAttack(first, second))
            {
                return true;
            }

            if (CheckExecNormalAttack(first, second))
            {
                // 先攻是因為屬性才會觸發爆擊
                bool isCriticalHit = (result == GameEnum.eCombatAttributeMatchResult.E_COMBAT_ATTRIBUTE_MATCH_WIN);

                damage = CombatManager.Instance.Formula.GetNormalAttackDamage(first, second, isCriticalHit);
                first.NormalDamage = damage;
                second.ChangeHealth(-damage);
            }

            // 後攻
            if (CheckAbortNormalAttack(second, first))
            {
                return true;
            }

            if (CheckExecNormalAttack(second, first))
            {
                damage = CombatManager.Instance.Formula.GetNormalAttackDamage(second, first, false);
                second.NormalDamage = damage;
                first.ChangeHealth(-damage);
            }

            return true;
        }

        #endregion  // Method
    }
}