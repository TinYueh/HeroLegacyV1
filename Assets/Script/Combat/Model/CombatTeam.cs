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

        private Dictionary<int, CombatRole> _dicCombatRole;         // ¶¤¥î¦¨­û <memberId, CombatRole>
        private List<CombatCircleSocket> _listCombatCircleSocket;   // CombatCircleSocket

        internal void Init(GameEnum.eCombatTeamType teamType, ViewCombatTeam vwCombatTeam)
        {
            TeamType = teamType;
            VwCombatTeam = vwCombatTeam;

            _dicCombatRole = new Dictionary<int, CombatRole>();
            _listCombatCircleSocket = new List<CombatCircleSocket>();

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                int socketId = i + 1;
                CombatCircleSocket socket = new CombatCircleSocket();
                socket.Init(socketId);

                _listCombatCircleSocket.Add(socket);
            }
        }

        internal void Setup(int teamId)
        {
            TeamCsvData teamCsvData = new TeamCsvData();
            if (TableManager.Instance.GetTeamCsvData(teamId, out teamCsvData) == false)
            {
                Debug.LogError("Not found TeamCsvData, Id: " + teamId);
            }

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                if (teamCsvData._arrRoleId[i] == 0)
                {
                    break;
                }
            }
        }
    }
}
