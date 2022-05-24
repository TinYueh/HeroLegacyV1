using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CircleSocket
    {
        private ViewCircleSocket _vwCircleSocket = null;    // View

        internal int PosId { get; private set; } = 0;
        internal GameEnum.eCircleSocketType Type { get; private set; } = GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_NA;

        private delegate bool DlgExecFunc(ref CombatTeam refTarget);
        private Dictionary<GameEnum.eCircleSocketType, DlgExecFunc> _dicExecFunc = new Dictionary<GameEnum.eCircleSocketType, DlgExecFunc>();

        internal void Init(int posId, ref ViewCircleSocket refVwCircleSocket)
        {
            PosId = posId;
            Type = GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_SPACE;
            _vwCircleSocket = refVwCircleSocket;    // Attach View

            RegistExecFunc();
        }

        private void RegistExecFunc()
        {
            _dicExecFunc.Add(GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_SPACE, ExecSpace);
            _dicExecFunc.Add(GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_COMBAT_ROLE, ExecCombatRole);
        }

        internal void Setup(GameEnum.eCircleSocketType socketType, ref CombatRole refCombatRole)
        {
            Type = socketType;

            _vwCircleSocket.SetSocket(refCombatRole.Role.Attribute);
            _vwCircleSocket.SetEmblem(refCombatRole.Role.Emblem);
            _vwCircleSocket.ShowEmblem();
        }

        internal bool Exec(ref CombatTeam refTarget)
        {
            DlgExecFunc dlgFunc = null;
            if (_dicExecFunc.TryGetValue(Type, out dlgFunc) == false)
            {
                Debug.LogError("Not found ExecFunc for " + Type);
                return false;
            }

            return dlgFunc(ref refTarget);
        }

        private bool ExecSpace(ref CombatTeam refTarget)
        {
            refTarget.ChangeEnergyPoint(1);

            return false;
        }

        private bool ExecCombatRole(ref CombatTeam refTarget)
        {
            return true;
        }
    }
}
