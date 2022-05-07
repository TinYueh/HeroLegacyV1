using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatManager : Singleton<CombatManager>
    {
        private CombatAI _combatAI = new CombatAI();
        private CombatRole _combatPlayer = null;
        private CombatRole _combatOpponent = null;
        internal int RotateSfxId { get; set; } = 0;

        private delegate void DlgStartActionFunc();
        private Dictionary<CombatCore.eCombatRoleAction, DlgStartActionFunc> _dicStartActionFunc = new Dictionary<CombatCore.eCombatRoleAction, DlgStartActionFunc>();

        internal CombatCore.eCombatState CombatState { get; set; } = CombatCore.eCombatState.E_COMBAT_STATE_NA;

        public override void Init()
        {
            RegistStartActionFunc();

            Debug.Log("CombatManager Init OK");
        }

        private void RegistStartActionFunc()
        {
            _dicStartActionFunc.Add(CombatCore.eCombatRoleAction.E_COMBAT_ROLE_ACTION_ROTATE_RIGHT, StartActionRotateRight);
            _dicStartActionFunc.Add(CombatCore.eCombatRoleAction.E_COMBAT_ROLE_ACTION_ROTATE_LEFT, StartActionRotateLeft);
            _dicStartActionFunc.Add(CombatCore.eCombatRoleAction.E_COMBAT_ROLE_ACTION_CAST, StartActionCast);
        }

        internal void InitCombatRole(ref CombatRole refRole, float initAngle, float anglePerF, float anglePerT)
        {
            if (refRole._role == CombatCore.eCombatRole.E_COMBAT_ROLE_PLAYER)
            {
                _combatPlayer = refRole;
            }
            else if (refRole._role == CombatCore.eCombatRole.E_COMBAT_ROLE_OPPONENT)
            {
                _combatOpponent = refRole;
            }
            else
            {
                Debug.LogError("Unknown CombatRole: " + refRole._role);
                return;
            }

            refRole._uiCombatCircle.RotateAnglePerFrame = anglePerF;
            refRole._uiCombatCircle.RotateAnglePerTime = anglePerT;
            refRole._uiCombatCircle.Rotate(initAngle);

            refRole._uiEnergyBar.SetWidthPerPoint(GameConst.BAR_ENERGY_POINT);
            refRole._uiEnergyBar.SetEnergyPoint(0);
        }

        internal void StartCombatRoleAction(CombatCore.eCombatRoleAction action)
        {
            DlgStartActionFunc dlgFunc = null;
            _dicStartActionFunc.TryGetValue(action, out dlgFunc);
            if (dlgFunc == null)
            {
                Debug.LogError("Not found StartActionFunc for " + action);
                return;
            }

            dlgFunc();
        }

        internal void ExecCombatRoleAction()
        {
            _combatPlayer._uiEnergyBar.ChangeEnergyPoint(1);
            _combatOpponent._uiEnergyBar.ChangeEnergyPoint(1);
        }

        private void StartActionRotateRight()
        {
            RotateCombatCircle(ref _combatPlayer._uiCombatCircle, true);
            RotateCombatCircle(ref _combatOpponent._uiCombatCircle, _combatAI.GetNextAction());

            AudioManager.Instance.PlaySfx(RotateSfxId);
        }

        private void StartActionRotateLeft()
        {
            RotateCombatCircle(ref _combatPlayer._uiCombatCircle, false);
            RotateCombatCircle(ref _combatOpponent._uiCombatCircle, _combatAI.GetNextAction());

            AudioManager.Instance.PlaySfx(RotateSfxId);
        }

        private void StartActionCast()
        {

        }

        private void RotateCombatCircle(ref UICombatCircle refUICombatCircle, bool isDirectionRight)
        {
            if (isDirectionRight)
            {
                refUICombatCircle.RotateAnglePerFrameActual = -refUICombatCircle.RotateAnglePerFrame;
            }
            else
            {
                refUICombatCircle.RotateAnglePerFrameActual = refUICombatCircle.RotateAnglePerFrame;
            }

            refUICombatCircle.EnableRotate();

            CombatState = CombatCore.eCombatState.E_COMBAT_STATE_ROTATE;
        }

        internal bool IsCombatCircleStandby()
        {
            if (_combatPlayer._uiCombatCircle.IsStandby() && _combatOpponent._uiCombatCircle.IsStandby())
            {
                return true;
            }

            return false;
        }
    }
}
