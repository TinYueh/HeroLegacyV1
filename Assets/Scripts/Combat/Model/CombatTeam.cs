using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameCombat
{
    public class CombatTeam
    {
        #region Property

        internal ViewCombatTeam ViewCombatTeam { get; private set; }    // View

        internal GameEnum.eCombatTeamType TeamType { get; private set; }
        internal int EnergyPoint { get; private set; }
        internal int EnergyOrb { get; private set; }
        internal int MatchPosId { get; private set; }   // 對戰位
        internal int CastPosId { get; set; }            // 施放位
        internal int CastSkillId { get; set; }          // 施放技能
        internal GameEnum.eRotateDirection RotateDirection { get; private set; }
        internal bool HasFirstToken { get; private set; }   // 判決平先

        private Dictionary<int, CombatRole> _dicCombatRole = new Dictionary<int, CombatRole>();         // <PosId, CombatRole>
        private Dictionary<int, CircleSocket> _dicCircleSocket = new Dictionary<int, CircleSocket>();   // <PosId, CircleSocket>

        #endregion  // Property

        #region Init

        internal bool Init(GameEnum.eCombatTeamType teamType, ViewCombatTeam viewCombatTeam)
        {
            TeamType = teamType;            
            ViewCombatTeam = viewCombatTeam; // Attach View

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                int posId = i + 1;

                ViewCircleSocket viewCircleSocket;
                if (ViewCombatTeam.ViewCombatCircle.GetCircleSocket(posId, out viewCircleSocket) == false)
                {
                    Debug.LogError("Not found ViewCircleSocket, PosId: " + posId);
                    return false;
                }

                CircleSocket circleSocket = new CircleSocket();
                circleSocket.Init(posId, viewCircleSocket);

                _dicCircleSocket.Add(posId, circleSocket);
            }

            return true;
        }

        #endregion  // Init

        #region Energy

        internal void ChangeEnergyOrb(int deltaOrb)
        {
            ChangeEnergyPoint(deltaOrb * GameConst.BAR_ENERGY_POINT);
        }

        internal void ChangeEnergyPoint(int deltaPoint)
        {
            int tmpPoint = EnergyPoint + deltaPoint;

            SetEnergyPoint(tmpPoint);
        }

        internal void SetEnergyPoint(int point)
        {
            if (point < 0)
            {
                EnergyPoint = 0;
            }
            else if (point > GameConst.MAX_ENERGY_POINT)
            {
                EnergyPoint = GameConst.MAX_ENERGY_POINT;
            }
            else
            {
                EnergyPoint = point;
            }

            int viewPoint = EnergyPoint % GameConst.BAR_ENERGY_POINT;
            EnergyOrb = EnergyPoint / GameConst.BAR_ENERGY_POINT;

            ViewCombatTeam.ViewEnergyBar.SetEnergyBar(viewPoint);
            ViewCombatTeam.ViewEnergyBar.SetEnergyOrb(EnergyOrb);

            if (EnergyPoint == GameConst.MAX_ENERGY_POINT)
            {
                // Todo: Lock EnergyBar
            }
        }

        #endregion  // Energy

        #region Get Set

        internal bool Set(int teamId)
        {
            TeamCsvData teamCsvData;
            if (TableManager.Instance.GetTeamCsvData(teamId, out teamCsvData) == false)
            {
                Debug.LogError("Not found TeamCsvData, TeamId: " + teamId);
                return false;
            }

            int memberId = 0;

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                int posId = i + 1;
                int roleId = teamCsvData._arrRoleId[i];

                if (roleId == 0)
                {
                    continue;
                }

                ++memberId;

                if (CreateCombatRole(memberId, posId, roleId) == false)
                {
                    Debug.LogError("CreateCombatRole failed, TeamId: " + teamId + ", PosId: " + posId);
                    return false;
                }

                ViewCombatTeam.ViewMemberList.ShowViewCombatRole(memberId);
            }

            SetEnergyPoint(0);
            SetMatchPosId(1);

            return true;
        }

        internal bool GetCombatRoleByPos(int posId, out CombatRole outCombatRole)
        {
            if (_dicCombatRole.TryGetValue(posId, out outCombatRole) == false)
            {
                Debug.LogError("Not found CombatRole, PosId: " + posId);
                return false;
            }

            return true;
        }

        internal bool GetCombatRoleByMember(int memberId, out CombatRole outCombatRole)
        {
            outCombatRole = null;

            foreach (var combatRole in _dicCombatRole)
            {
                if (combatRole.Value.MemberId == memberId)
                {
                    outCombatRole = combatRole.Value;
                    return true;
                }
            }

            return false;
        }

        internal CombatRole GetMatchCombatRole()
        {
            CombatRole combatRole;
            GetCombatRoleByPos(MatchPosId, out combatRole);

            return combatRole;
        }

        internal CombatRole GetCastCombatRole()
        {
            CombatRole combatRole;
            GetCombatRoleByPos(CastPosId, out combatRole);

            return combatRole;
        }        

        internal bool GetCircleSocket(int posId, out CircleSocket outCircleSocket)
        {
            if (_dicCircleSocket.TryGetValue(posId, out outCircleSocket) == false)
            {
                Debug.LogError("Not found CircleSocket, PosId: " + posId);
                return false;
            }

            return true;
        }

        private bool SetCircleSocket(int posId, CombatRole combatRole)
        {
            CircleSocket circleSocket;
            if (_dicCircleSocket.TryGetValue(posId, out circleSocket) == false)
            {
                Debug.LogError("Not found CircleSocket, PosId: " + posId);
                return false;
            }

            circleSocket.Set(GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_COMBAT_ROLE, combatRole);

            return true;
        }

        internal void SetFirstToken(bool getFirstToken)
        {
            HasFirstToken = getFirstToken;

            if (getFirstToken)
            {
                ViewCombatTeam.ViewCombatStats.ShowFirstToken();
            }
            else
            {
                ViewCombatTeam.ViewCombatStats.HideFirstToken();
            }
        }

        internal void GetCombatRoleList(List<int> listPos, ref List<CombatRole> refListCombatRole)
        {
            CombatRole combatRole;

            foreach (var posId in listPos)
            {
                if (GetCombatRoleByPos(posId, out combatRole))
                {
                    refListCombatRole.Add(combatRole);
                }
            }
        }

        internal void GetCombatRoleList(GameEnum.ePosType posType, ref List<CombatRole> refListCombatRole)
        {
            List<int> listPos = new List<int>();
            if (GetPosList(posType, listPos) == false)
            {
                return;
            }

            GetCombatRoleList(listPos, ref refListCombatRole);
        }

        #endregion  // Get Set

        #region Logic

        internal bool CheckTeamAlive()
        {
            foreach (var combatRole in _dicCombatRole)
            {
                if (combatRole.Value.IsAlive())
                {
                    return true;
                }
            }

            return false;
        }

        internal bool IsCastSkill()
        {
            return (CastPosId > 0 && CastSkillId > 0);
        }

        #endregion  // Logic

        #region Pos

        private void ChangeMatchPosId(GameEnum.eRotateDirection direction)
        {
            int tmpMatchPosId = MatchPosId;

            if (direction == GameEnum.eRotateDirection.E_ROTATE_DIRECTION_RIGHT)
            {
                tmpMatchPosId -= 1;
            }
            else if (direction == GameEnum.eRotateDirection.E_ROTATE_DIRECTION_LEFT)
            {
                tmpMatchPosId += 1;
            }
            else
            {
                return;
            }

            SetMatchPosId(tmpMatchPosId);
        }

        private void SetMatchPosId(int posId)
        {
            MatchPosId = ConvertPosId(posId);
        }

        internal int ConvertPosId(int posId)
        {
            if (posId <= 0)
            {
                posId = GameConst.MAX_TEAM_MEMBER - (posId % GameConst.MAX_TEAM_MEMBER);
            }
            else if (posId > GameConst.MAX_TEAM_MEMBER)
            {
                posId = posId % GameConst.MAX_TEAM_MEMBER;

                if (posId == 0)
                {
                    posId = GameConst.MAX_TEAM_MEMBER;
                }
            }

            return posId;
        }

        internal bool GetPosList(GameEnum.ePosType posType, List<int> listPos)
        {
            if (MatchPosId == 0)
            {
                return false;
            }

            switch (posType)
            {
                case GameEnum.ePosType.E_POS_TYPE_MATCH:
                    {
                        listPos.Add(MatchPosId);
                        break;
                    }
                case GameEnum.ePosType.E_POS_TYPE_WING:
                    {
                        listPos.Add(ConvertPosId(MatchPosId + 1));
                        listPos.Add(ConvertPosId(MatchPosId - 1));
                        break;
                    }
                case GameEnum.ePosType.E_POS_TYPE_FORWARD:
                    {
                        listPos.Add(MatchPosId);
                        listPos.Add(ConvertPosId(MatchPosId + 1));
                        listPos.Add(ConvertPosId(MatchPosId - 1));
                        break;
                    }
                case GameEnum.ePosType.E_POS_TYPE_GUARD:
                    {
                        int posId = MatchPosId + (GameConst.MAX_TEAM_MEMBER / 2);
                        listPos.Add(ConvertPosId(posId));
                        listPos.Add(ConvertPosId(posId + 1));
                        listPos.Add(ConvertPosId(posId - 1));
                        break;
                    }
                case GameEnum.ePosType.E_POS_TYPE_ALL:
                    {
                        for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
                        {
                            listPos.Add(ConvertPosId(MatchPosId + i));
                        }
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            return true;
        }

        internal bool IsMatchPosAlive()
        {
            CombatRole combatRole;
            if (GetCombatRoleByPos(MatchPosId, out combatRole) == false)
            {
                return false;
            }

            return combatRole.IsAlive();
        }

        #endregion  // Pos

        #region Method

        internal bool CreateCombatRole(int memberId, int posId, int roleId)
        {
            RoleCsvData csvData;
            if (TableManager.Instance.GetRoleCsvData(roleId, out csvData) == false)
            {
                Debug.LogError("Not found RoleCsvData, RoleId: " + roleId);
                return false;
            }

            ViewCombatRole viewCombatRole;
            if (ViewCombatTeam.ViewMemberList.GetViewCombatRole(memberId, out viewCombatRole) == false)
            {
                Debug.LogError("Not found RoleCsvData, MemberId: " + memberId);
                return false;
            }

            CombatRole combatRole = new CombatRole();
            if (combatRole.Init(TeamType, memberId, posId, csvData, viewCombatRole) == false)
            {
                Debug.LogError("Init CombatRole failed, RoleId: " + roleId);
                return false;
            }

            _dicCombatRole.Add(posId, combatRole);

            ViewCombatTeam.ViewMemberList.SetViewCombatRole(memberId, combatRole);

            SetCircleSocket(posId, combatRole);

            return true;
        }

        internal void Rotate(GameEnum.eRotateDirection direction)
        {
            RotateDirection = direction;

            ChangeMatchPosId(RotateDirection);
            ViewCombatTeam.Rotate(RotateDirection);
        }

        internal bool ExecCircleSocket(int posId, CombatTeam target)
        {
            CircleSocket circleSocket;
            if (_dicCircleSocket.TryGetValue(posId, out circleSocket) == false)
            {
                Debug.LogError("Not found CircleSocket, PosId: " + posId);
            }

            return circleSocket.Exec(target);
        }

        internal void Prepare()
        {
            var e = _dicCombatRole.GetEnumerator();
            while (e.MoveNext())
            {
                // 降技能 Cd
                e.Current.Value.ChangeAllSkillCd(-1);
            }
        }

        internal void ClearSkill()
        {
            CastPosId = 0;
            CastSkillId = 0;
        }

        #endregion  // Method
    }
}