using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CircleSocket
    {
        private ViewCircleSocket _viewCircleSocket = null;    // View

        internal int PosId { get; private set; } = 0;
        internal GameEnum.eCircleSocketType Type { get; private set; } = GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_NA;

        private delegate bool DlgExec(CombatTeam target);
        private Dictionary<GameEnum.eCircleSocketType, DlgExec> _dicDlgExec = new Dictionary<GameEnum.eCircleSocketType, DlgExec>();

        internal void Init(int posId, ViewCircleSocket viewCircleSocket)
        {
            PosId = posId;
            Type = GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_SPACE;
            _viewCircleSocket = viewCircleSocket;   // Attach View

            RegistExecFunc();
        }

        private void RegistExecFunc()
        {
            _dicDlgExec.Add(GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_SPACE, ExecSpace);
            _dicDlgExec.Add(GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_COMBAT_ROLE, ExecCombatRole);
        }

        internal void Set(GameEnum.eCircleSocketType socketType, CombatRole combatRole)
        {
            Type = socketType;

            _viewCircleSocket.SetSocket(combatRole.Role.Attribute);
            _viewCircleSocket.SetEmblem(combatRole.Role.Emblem);
            _viewCircleSocket.ShowEmblem();
        }

        internal void Clear()
        {
            Type = GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_SPACE;

            _viewCircleSocket.SetSocket(GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_NA);
            _viewCircleSocket.HideEmblem();
        }

        internal bool Exec(CombatTeam target)
        {
            DlgExec dlgFunc = null;
            if (_dicDlgExec.TryGetValue(Type, out dlgFunc) == false)
            {
                Debug.LogError("Not found ExecFunc for " + Type);
                return false;
            }

            return dlgFunc(target);
        }

        private bool ExecSpace(CombatTeam target)
        {
            target.ChangeEnergyPoint(1);

            return false;
        }

        private bool ExecCombatRole(CombatTeam target)
        {
            return true;
        }
    }
}
