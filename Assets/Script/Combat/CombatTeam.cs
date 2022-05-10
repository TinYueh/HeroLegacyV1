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
        // 對戰中的成員
        internal int MatchMemberId { get; private set; } = 0;

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

            int newPoint = EnergyPoint % GameConst.BAR_ENERGY_POINT;
            int newCube = EnergyPoint / GameConst.BAR_ENERGY_POINT;

            _uiEnergyBar.ChangeViewBar(newPoint);
            _uiEnergyBar.ChangeViewCube(newCube);

            if (EnergyPoint == GameConst.MAX_ENERGY_POINT)
            {
                // Todo: Lock EnergyBar
            }
        }

        internal void ChangeMatchMemberId(int deltaMemberId)
        {
            int tmpId = MatchMemberId + deltaMemberId;

            SetMatchMemberId(tmpId);
        }

        internal void ChangeMatchMemberId(bool isDirectionRight)
        {
            int tmpId = MatchMemberId;

            if (isDirectionRight)
            {
                tmpId -= 1;
            }
            else
            {
                tmpId += 1;
            }

            SetMatchMemberId(tmpId);
        }

        internal void SetMatchMemberId(int memberId)
        {
            if (memberId > GameConst.MAX_TEAM_MEMBER)
            {
                MatchMemberId = memberId % GameConst.MAX_TEAM_MEMBER;
            }
            else if (memberId <= 0)
            {
                MatchMemberId = GameConst.MAX_TEAM_MEMBER - (memberId % GameConst.MAX_TEAM_MEMBER);
            }
            else
            {
                MatchMemberId = memberId;
            }
        }

        internal bool CreateCombatRole(int memberId, int roleId)
        {
            CombatRole combatRole = new CombatRole();
            combatRole.Init(memberId, roleId);

            RoleCsvData csvData = new RoleCsvData();
            if (TableManager.Instance.GetRoleCsvData(roleId, out csvData) == false)
            {
                Debug.LogError("Not found RoleCsvData, Id: " + roleId);
                return false;
            }

            float posX = _uiRoleList.initialPosX + (_uiRoleList.deltaPosX * (memberId - 1));
            combatRole.UICombatRole = GameObject.Instantiate(Resources.Load<GameObject>(AssetsPath.PREFAB_UI_COMBAT_ROLE), new Vector2(posX, 0), Quaternion.identity);
            combatRole.UICombatRole.transform.SetParent(_uiRoleList.gameObject.transform, false);
            combatRole.UICombatRole.GetComponent<UICombatRole>().ChangeViewPortrait(csvData._portrait);
            combatRole.UICombatRole.GetComponent<UICombatRole>().ChangeViewEmblem(csvData._emblem);

            _uiCombatCircle.ChangeViewRoleSlot(memberId, ref csvData);

            // 加入隊伍
            _dicCombatRole.Add(memberId, combatRole);

            return true;
        }

        internal void GetCombatRole(int memberId, out CombatRole outCombatRole)
        {
            _dicCombatRole.TryGetValue(memberId, out outCombatRole);
        }

        internal void GetMatchCombatRole(out CombatRole outCombatRole)
        {
            _dicCombatRole.TryGetValue(MatchMemberId, out outCombatRole);
        }
    }
}