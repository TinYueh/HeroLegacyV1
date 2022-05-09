using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatTeam : MonoBehaviour
    {
        [SerializeField]
        internal CombatCore.eCombatTeam _team = CombatCore.eCombatTeam.E_COMBAT_TEAM_NA;
        [SerializeField]
        internal UICombatCircle _uiCombatCircle = null;
        [SerializeField]
        internal UIEnergyBar _uiEnergyBar = null;
        [SerializeField]
        internal UIRoleList _uiRoleList = null;

        // 能量點
        internal int EnergyPoint { get; private set; } = 0;
        // 戰鬥隊員
        internal int FocusTeamId { get; private set; } = 0;

        private Dictionary<int, CombatRole> _dicCombatRole = new Dictionary<int, CombatRole>();       

        private void Start()
        {

        }

        private void Update()
        {

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

            int showPoint = EnergyPoint % GameConst.BAR_ENERGY_POINT;
            int showCube = EnergyPoint / GameConst.BAR_ENERGY_POINT;

            _uiEnergyBar.ShowBar(showPoint);
            _uiEnergyBar.ShowCube(showCube);

            if (EnergyPoint == GameConst.MAX_ENERGY_POINT)
            {
                // Todo: Lock EnergyBar
            }
        }

        internal void ChangeFocusTeamId(int deltaTeamId)
        {
            int tmpId = FocusTeamId + deltaTeamId;

            SetFocusTeamId(tmpId);
        }

        internal void ChangeFocusTeamId(bool isDirectionRight)
        {
            int tmpId = FocusTeamId;

            if (isDirectionRight)
            {
                tmpId -= 1;
            }
            else
            {
                tmpId += 1;
            }

            SetFocusTeamId(tmpId);
        }

        internal void SetFocusTeamId(int teamId)
        {
            if (teamId > GameConst.MAX_TEAM_MEMBER)
            {
                FocusTeamId = teamId % GameConst.MAX_TEAM_MEMBER;
            }
            else if (teamId <= 0)
            {
                FocusTeamId = GameConst.MAX_TEAM_MEMBER - (teamId % GameConst.MAX_TEAM_MEMBER);
            }
            else
            {
                FocusTeamId = teamId;
            }
        }

        internal bool CreateCombatRole(int teamId, int roleId)
        {
            CombatRole combatRole = new CombatRole();

            RoleCsvData csvData = new RoleCsvData();
            if (TableManager.Instance.GetRoleCsvData(roleId, out csvData) == false)
            {
                Debug.LogError("Not found RoleCsvData, Id: " + roleId);
                return false;
            }

            float posX = _uiRoleList.initialPosX + (_uiRoleList.deltaPosX * (teamId - 1));
            combatRole.UICombatRole = GameObject.Instantiate(Resources.Load<GameObject>(AssetsPath.PREFAB_UI_COMBAT_ROLE), new Vector2(posX, 0), Quaternion.identity);
            combatRole.UICombatRole.transform.SetParent(_uiRoleList.gameObject.transform, false);
            combatRole.UICombatRole.GetComponent<UICombatRole>().ShowPortrait(csvData._portrait);
            combatRole.UICombatRole.GetComponent<UICombatRole>().ShowEmblem(csvData._emblem);

            _uiCombatCircle.ShowRoleSlot(teamId, ref csvData);

            // 加入隊伍
            _dicCombatRole.Add(teamId, combatRole);

            return true;
        }
    }
}