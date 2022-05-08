using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatManager : Singleton<CombatManager>
    {
        private CombatAI _combatAI = new CombatAI();
        private CombatTeam _combatPlayer = null;
        private CombatTeam _combatOpponent = null;
        private GameObject _prefabCombatRole = null;

        internal int RotateSfxId { get; set; } = 0;

        private delegate void DlgStartActionFunc();
        private Dictionary<CombatCore.eCombatTeamAction, DlgStartActionFunc> _dicStartActionFunc = new Dictionary<CombatCore.eCombatTeamAction, DlgStartActionFunc>();

        internal CombatCore.eCombatState CombatState { get; set; } = CombatCore.eCombatState.E_COMBAT_STATE_NA;

        public override void Init()
        {
            RegistStartActionFunc();

            _prefabCombatRole = Resources.Load<GameObject>(AssetsPath.PREFAB_UI_COMBAT_ROLE);
            if (_prefabCombatRole == null)
            {
                Debug.LogError("Not found Prefab: " + AssetsPath.PREFAB_UI_COMBAT_ROLE);
            }

            Debug.Log("CombatManager Init OK");
        }

        private void RegistStartActionFunc()
        {
            _dicStartActionFunc.Add(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_ROTATE_RIGHT, StartActionRotateRight);
            _dicStartActionFunc.Add(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_ROTATE_LEFT, StartActionRotateLeft);
            _dicStartActionFunc.Add(CombatCore.eCombatTeamAction.E_COMBAT_TEAM_ACTION_CAST, StartActionCast);
        }

        internal void InitCombatTeam(ref CombatTeam refTeam, float initAngle, float anglePerFrame, float anglePerTime, int teamId)
        {
            if (refTeam._team == CombatCore.eCombatTeam.E_COMBAT_TEAM_PLAYER)
            {
                _combatPlayer = refTeam;
            }
            else if (refTeam._team == CombatCore.eCombatTeam.E_COMBAT_TEAM_OPPONENT)
            {
                _combatOpponent = refTeam;
            }
            else
            {
                Debug.LogError("Unknown CombatTeam: " + refTeam._team);
                return;
            }

            refTeam._uiCombatCircle.RotateAnglePerFrame = anglePerFrame;
            refTeam._uiCombatCircle.RotateAnglePerTime = anglePerTime;
            refTeam._uiCombatCircle.Rotate(initAngle);

            refTeam._uiEnergyBar.SetWidthPerPoint(GameConst.BAR_ENERGY_POINT);

            refTeam.SetEnergyPoint(0);

            // «Ø¥ß¶¤¥î
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

                int roleId = teamCsvData._arrRoleId[i];

                HeroCsvData heroCsvData = new HeroCsvData();
                if (TableManager.Instance.GetHeroCsvData(roleId, out heroCsvData) == false)
                {
                    Debug.LogError("Not found HeroCsvData, Id: " + roleId);
                    break;
                }

                float posX = refTeam._uiRoleList.initialPosX + (refTeam._uiRoleList.deltaPosX * i);
                
                GameObject combatRole = GameObject.Instantiate(_prefabCombatRole, new Vector2(posX, 0), Quaternion.identity);
                combatRole.transform.SetParent(refTeam._uiRoleList.gameObject.transform, false);

                combatRole.GetComponent<UICombatRole>().ShowPortrait(heroCsvData._portrait);
                combatRole.GetComponent<UICombatRole>().ShowEmblem(heroCsvData._emblem);

                refTeam._uiRoleList._dicUICombatRole.Add(i + 1, combatRole);
            }
        }

        internal void StartCombatTeamAction(CombatCore.eCombatTeamAction action)
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

        internal void ExecCombatTeamAction()
        {
            _combatPlayer.ChangeEnergyPoint(1);
            _combatOpponent.ChangeEnergyPoint(1);
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
