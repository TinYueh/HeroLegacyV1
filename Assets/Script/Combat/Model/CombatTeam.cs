using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameCombat
{
    public class CombatTeam
    {        
        internal ViewCombatTeam VwCombatTeam { get; private set; } = null;  // View

        internal GameEnum.eCombatTeamType TeamType { get; private set; } = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;
        internal int EnergyPoint { get; private set; } = 0;
        internal int MatchPosId { get; private set; } = 0;
        internal GameEnum.eRotateDirection RotateDirection { get; private set; } = GameEnum.eRotateDirection.E_ROTATE_DIRECTION_NA;

        private Dictionary<int, CombatRole> _dicCombatRole = new Dictionary<int, CombatRole>();         // 隊伍成員 <PosId, CombatRole>
        private Dictionary<int, CircleSocket> _dicCircleSocket = new Dictionary<int, CircleSocket>();   // <PosId, CircleSocket>

        internal bool Init(GameEnum.eCombatTeamType teamType, ref ViewCombatTeam refVwCombatTeam)
        {
            TeamType = teamType;            
            VwCombatTeam = refVwCombatTeam; // Attach View

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                int posId = i + 1;

                ViewCircleSocket vwCircleSocket = null;
                if (VwCombatTeam.GetCircleSocket(posId, out vwCircleSocket) == false)
                {
                    Debug.LogError("Not found ViewCircleSocket, PosId: " + posId);
                    return false;
                }

                CircleSocket circleSocket = new CircleSocket();
                circleSocket.Init(posId, ref vwCircleSocket);

                _dicCircleSocket.Add(posId, circleSocket);
            }

            return true;
        }

        internal bool Setup(int teamId)
        {
            TeamCsvData teamCsvData = null;
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

                VwCombatTeam.ShowCombatRole(memberId);
            }

            SetEnergyPoint(0);
            SetMatchPosId(1);

            return true;
        }

        internal bool CreateCombatRole(int memberId, int posId, int roleId)
        {
            RoleCsvData csvData = null;
            if (TableManager.Instance.GetRoleCsvData(roleId, out csvData) == false)
            {
                Debug.LogError("Not found RoleCsvData, RoleId: " + roleId);
                return false;
            }

            ViewCombatRole vwCombatRole = null;
            if (VwCombatTeam.GetCombatRole(memberId, out vwCombatRole) == false)
            {
                Debug.LogError("Not found RoleCsvData, MemberId: " + memberId);
                return false;
            }

            CombatRole combatRole = new CombatRole();
            if (combatRole.Init(memberId, posId, ref csvData, ref vwCombatRole) == false)
            {
                Debug.LogError("Init CombatRole failed, RoleId: " + roleId);
                return false;
            }

            _dicCombatRole.Add(posId, combatRole);            

            VwCombatTeam.SetCombatRole(memberId, ref combatRole);

            SetupCircleSocket(posId, ref combatRole);

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

        internal void HandleRotation(GameEnum.eRotateDirection direction)
        {
            RotateDirection = direction;

            ChangeMatchPosId(RotateDirection);
            VwCombatTeam.HandleRotation(RotateDirection);
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

            int vwPoint = EnergyPoint % GameConst.BAR_ENERGY_POINT;
            int vwOrb = EnergyPoint / GameConst.BAR_ENERGY_POINT;

            VwCombatTeam.SetEnergyBar(vwPoint);
            VwCombatTeam.SetEnergyOrb(vwOrb);

            if (EnergyPoint == GameConst.MAX_ENERGY_POINT)
            {
                // Todo: Lock EnergyBar
            }
        }

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

            MatchPosId = posId;
        }

        private bool SetupCircleSocket(int posId, ref CombatRole refCombatRole)
        {
            CircleSocket circleSocket = null;
            if (_dicCircleSocket.TryGetValue(posId, out circleSocket) == false)
            {
                Debug.Log("Not found CircleSocket, PosId: " + posId);
                return false;
            }

            circleSocket.Setup(GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_COMBAT_ROLE, ref refCombatRole);

            return true;
        }

        internal bool ExecCircleSocket(int posId, ref CombatTeam refTarget)
        {
            CircleSocket circleSocket = null;
            if (_dicCircleSocket.TryGetValue(posId, out circleSocket) == false)
            {
                Debug.Log("Not found CircleSocket, PosId: " + posId);
            }

            return circleSocket.Exec(ref refTarget);
        }
    }
}
