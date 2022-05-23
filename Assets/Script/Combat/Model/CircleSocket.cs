using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class CircleSocket
    {
        internal int PosId { get; private set; } = 0;

        internal GameEnum.eCircleSocketType Type { get; private set; } = GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_NA;

        private CombatRole _combatRole = null;

        private delegate bool DlgExecFunc(ref CombatTeam refTarget);
        private Dictionary<GameEnum.eCircleSocketType, DlgExecFunc> _dicExecFunc = new Dictionary<GameEnum.eCircleSocketType, DlgExecFunc>();

        internal void Init(int posId)
        {
            PosId = posId;
            Type = GameEnum.eCircleSocketType.E_CIRCLE_SOCKET_TYPE_SPACE;

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
            _combatRole = refCombatRole;
        }

        internal bool Exec(ref CombatTeam refTarget)
        {
            DlgExecFunc dlgFunc;
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
