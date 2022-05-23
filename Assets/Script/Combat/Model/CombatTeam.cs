using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Table;

namespace GameCombat
{
    public class CombatTeam
    {
        internal GameEnum.eCombatTeamType TeamType { get; private set; }
        internal ViewCombatTeam VwCombatTeam { get; private set; }
        internal int EnergyPoint { get; private set; }
        internal int MatchSocketId { get; private set; }

        private Dictionary<int, CombatRole> _dicCombatRole;     // ¶¤¥î¦¨­û <memberId, CombatRole>
        private Dictionary<int, CircleSocket> _dicCircleSocket; // <posId, CircleSocket>

        internal void Init(GameEnum.eCombatTeamType teamType, ViewCombatTeam vwCombatTeam)
        {
            TeamType = teamType;
            VwCombatTeam = vwCombatTeam;

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

                VwCombatTeam.ShowCombatRole(memberId);
            }

            SetEnergyPoint(0);

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
            combatRole.Init(memberId, csvData);

            _dicCombatRole.Add(posId, combatRole);

            VwCombatTeam.SetCombatRole(posId, memberId, combatRole);

            return true;
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

        //internal int ConvertFormalSlotId(int informalId)
        //{
        //    int slotId = 0;

        //    if (informalId > GameConst.MAX_TEAM_MEMBER)
        //    {
        //        slotId = informalId % GameConst.MAX_TEAM_MEMBER;
        //    }
        //    else if (informalId < 0)
        //    {
        //        slotId = GameConst.MAX_TEAM_MEMBER - (informalId % GameConst.MAX_TEAM_MEMBER);
        //    }
        //    else
        //    {
        //        slotId = informalId;
        //    }

        //    if (slotId == 0)
        //    {
        //        slotId = GameConst.MAX_TEAM_MEMBER;
        //    }

        //    return slotId;
        //}
    }
}
