using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameCombat
{
    public class CombatTeam
    {
        internal GameEnum.eCombatTeamType TeamType { get; private set; }

        private ViewCombatTeam _vwCombatTeam;
        private int _energyPoint;
        private int _matchPosId;
        internal GameEnum.eRotateDirection RotateDirection { get; private set; } = GameEnum.eRotateDirection.E_ROTATE_DIRECTION_NA;

        private Dictionary<int, CombatRole> _dicCombatRole;     // ¶¤¥î¦¨­û <memberId, CombatRole>
        private Dictionary<int, CircleSocket> _dicCircleSocket; // <posId, CircleSocket>

        internal void Init(GameEnum.eCombatTeamType teamType, ref ViewCombatTeam ref_vwCombatTeam)
        {
            TeamType = teamType;
            _vwCombatTeam = ref_vwCombatTeam;

            _dicCombatRole = new Dictionary<int, CombatRole>();
            _dicCircleSocket = new Dictionary<int, CircleSocket>();

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                int posId = i + 1;
                CircleSocket socket = new CircleSocket();
                socket.Init(posId);

                _dicCircleSocket.Add(posId, socket);
            }
        }

        internal bool Setup(int teamId)
        {
            TeamCsvData teamCsvData;
            if (TableManager.Instance.GetTeamCsvData(teamId, out teamCsvData) == false)
            {
                Debug.LogError("Not found TeamCsvData, Id: " + teamId);
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

                if (CreateCombatRole(posId, memberId, roleId) == false)
                {
                    Debug.LogError("CreateCombatRole failed, TeamId: " + teamId + ", PosId: " + posId);
                    return false;
                }

                _vwCombatTeam.ShowCombatRole(memberId);
            }

            SetEnergyPoint(0);
            SetMatchPosId(1);

            return true;
        }

        internal bool CreateCombatRole(int posId, int memberId, int roleId)
        {
            RoleCsvData csvData;
            if (TableManager.Instance.GetRoleCsvData(roleId, out csvData) == false)
            {
                Debug.LogError("Not found RoleCsvData, Id: " + roleId);
                return false;
            }

            CombatRole combatRole = new CombatRole();
            combatRole.Init(memberId, ref csvData);

            _dicCombatRole.Add(posId, combatRole);

            _vwCombatTeam.SetCombatRole(posId, memberId, ref combatRole);

            SetCircleSocket(posId, ref combatRole);

            return true;
        }

        internal bool ExecMatchCombatCircle()
        {
            CircleSocket socket;
            if (_dicCircleSocket.TryGetValue(_matchPosId, out socket) == false)
            {
                Debug.Log("Not found CircleSocket, PosId: " + _matchPosId);
            }

            return socket.Exec();
        }

        internal bool IsStandby()
        {
            return _vwCombatTeam.IsStandby();
        }

        private void SetCircleSocket(int posId, ref CombatRole refCombatRole)
        {
            CircleSocket socket;
            if (_dicCircleSocket.TryGetValue(posId, out socket) == false)
            {
                Debug.Log("Not found CircleSocket, PosId: " + posId);
            }

            socket.Setup(GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_COMBAT_ROLE, ref refCombatRole);
        }

        internal void SetRotation(GameEnum.eRotateDirection direction)
        {
            RotateDirection = direction;

            ChangeMatchPosId(RotateDirection);
            _vwCombatTeam.SetRotation(RotateDirection);
        }

        internal void ChangeEnergyPoint(int deltaPoint)
        {
            int tmpPoint = _energyPoint + deltaPoint;

            SetEnergyPoint(tmpPoint);
        }

        internal void SetEnergyPoint(int point)
        {
            if (point < 0)
            {
                _energyPoint = 0;
            }
            else if (point > GameConst.MAX_ENERGY_POINT)
            {
                _energyPoint = GameConst.MAX_ENERGY_POINT;
            }
            else
            {
                _energyPoint = point;
            }

            int vwPoint = _energyPoint % GameConst.BAR_ENERGY_POINT;
            int vwOrb = _energyPoint / GameConst.BAR_ENERGY_POINT;

            _vwCombatTeam.SetEnergyBar(vwPoint);
            _vwCombatTeam.SetEnergyOrb(vwOrb);

            if (_energyPoint == GameConst.MAX_ENERGY_POINT)
            {
                // Todo: Lock EnergyBar
            }
        }

        private void ChangeMatchPosId(GameEnum.eRotateDirection direction)
        {
            int tmpMatchPosId = _matchPosId;

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

            _matchPosId = posId;
        }
    }
}
