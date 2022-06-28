using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CircleSocket
    {
        #region Property

        private ViewCircleSocket _viewCircleSocket;    // View

        internal int PosId { get; private set; }
        internal GameEnum.eCircleSocketType Type { get; private set; }

        private delegate bool DlgExec(CombatTeam target);
        private Dictionary<GameEnum.eCircleSocketType, DlgExec> _dicDlgExec = new Dictionary<GameEnum.eCircleSocketType, DlgExec>();

        #endregion  // Property

        #region Init

        internal void Init(int posId, ViewCircleSocket viewCircleSocket)
        {
            PosId = posId;
            Type = GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_SPACE;
            _viewCircleSocket = viewCircleSocket;   // Attach View

            RegistDlgExec();
        }

        #endregion  // Init

        #region Exec

        private void RegistDlgExec()
        {
            _dicDlgExec.Add(GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_SPACE, ExecSpace);
            _dicDlgExec.Add(GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_COMBAT_ROLE, ExecCombatRole);
        }

        internal bool Exec(CombatTeam target)
        {
            DlgExec dlg;
            if (_dicDlgExec.TryGetValue(Type, out dlg) == false)
            {
                Debug.LogError("Not found ExecFunc for " + Type);
                return false;
            }

            return dlg(target);
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

        #endregion  // Exec

        #region Get Set

        internal void Set(GameEnum.eCircleSocketType socketType, CombatRole combatRole)
        {
            Type = socketType;

            _viewCircleSocket.SetSocket(combatRole.Role.Attribute);
            _viewCircleSocket.SetEmblem(combatRole.Role.Emblem);
            _viewCircleSocket.ShowEmblem();
        }

        #endregion  // Get Set

        #region Method

        internal void Clear()
        {
            Type = GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_SPACE;

            _viewCircleSocket.SetSocket(GameEnum.eRoleAttribute.E_ROLE_ATTRIBUTE_NA);
            _viewCircleSocket.HideEmblem();
        }

        #endregion  // Method
    }
}