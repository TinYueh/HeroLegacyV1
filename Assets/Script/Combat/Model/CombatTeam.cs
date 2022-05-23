using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameCombat
{
    public class CombatTeam
    {
        internal GameEnum.eCombatTeamType TeamType { get; private set; } = GameEnum.eCombatTeamType.E_COMBAT_TEAM_TYPE_NA;

        private ViewCombatTeam _vwCombatTeam = null;
        private int _energyPoint = 0;
        internal int MatchPosId { get; private set; } = 0;
        internal GameEnum.eRotateDirection RotateDirection { get; private set; } = GameEnum.eRotateDirection.E_ROTATE_DIRECTION_NA;

        private Dictionary<int, CombatRole> _dicCombatRole = new Dictionary<int, CombatRole>();         // 隊伍成員 <memberId, CombatRole>
        private Dictionary<int, CircleSocket> _dicCircleSocket = new Dictionary<int, CircleSocket>();   // <posId, CircleSocket>

        internal void Init(GameEnum.eCombatTeamType teamType, ref ViewCombatTeam refVwCombatTeam)
        {
            TeamType = teamType;
            _vwCombatTeam = refVwCombatTeam;

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

            _dicCombatRole.Add(memberId, combatRole);

            SetCircleSocket(posId, ref combatRole);

            _vwCombatTeam.SetCombatRole(posId, memberId, ref combatRole);

            return true;
        }

        internal bool IsStandby()
        {
            return _vwCombatTeam.IsStandby();
        }

        private void SetCircleSocket(int posId, ref CombatRole refCombatRole)
        {
            CircleSocket socket = null;
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

        internal bool ExecMatchCombatCircle(ref CombatTeam refTarget)
        {
            CircleSocket socket;
            if (_dicCircleSocket.TryGetValue(MatchPosId, out socket) == false)
            {
                Debug.Log("Not found CircleSocket, PosId: " + MatchPosId);
            }

            return socket.Exec(ref refTarget);
        }

        internal bool GetCombatRoleByMember(int memberId, out CombatRole outCombatRole)
        {
            return _dicCombatRole.TryGetValue(memberId, out outCombatRole);
        }

        internal bool GetCombatRoleByPos(int posId, out CombatRole outCombatRole)
        {
            outCombatRole = null;

            CircleSocket socket = null;
            if (_dicCircleSocket.TryGetValue(posId, out socket) == false)
            {
                Debug.LogError("Not found CircleSocket, PosId: " + posId);
                return false;
            }

            if (socket.GetCombatRole(out outCombatRole) == false)
            {
                Debug.LogError("GetCombatRole from CircleSocket failed, PosId: " + posId);
                return false;
            }

            return true;
        }

        //internal void ChangeHealth(int deltaHealth)
        //{
        //    int tmpHealth = Health + deltaHealth;

        //    SetHealth(tmpHealth);
        //}

        //internal void SetHealth(int health)
        //{
        //    if (health < 0)
        //    {
        //        Health = 0;
        //    }
        //    else if (health > Role.Health)
        //    {
        //        Health = Role.Health;
        //    }
        //    else
        //    {
        //        Health = health;
        //    }

        //    _viewCombatRole.SetHealthBar(Health, Role.Health);

        //    if (State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NORMAL
        //        && Health == 0)
        //    {
        //        State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING;

        //        _viewCombatRole.SetStateDying();
        //    }
        //    else if (State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING
        //        && Health > 0)
        //    {
        //        State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_NORMAL;

        //        _viewCombatRole.SetStateNormal();
        //    }
        //}
    }
}
