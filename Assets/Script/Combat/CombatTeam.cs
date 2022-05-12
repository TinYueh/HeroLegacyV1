using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatTeam : MonoBehaviour
    {
        [SerializeField]
        internal GameEnum.eCombatTeam _team = GameEnum.eCombatTeam.E_COMBAT_TEAM_NA;
        [SerializeField]
        internal UICombatCircle _uiCombatCircle = null;
        [SerializeField]
        internal UIEnergyBar _uiEnergyBar = null;
        [SerializeField]
        internal UIRoleList _uiRoleList = null;

        // 能量點
        internal int EnergyPoint { get; private set; } = 0;
        // 對戰中的成員
        internal int MatchSlotId { get; private set; } = 0;

        private Dictionary<int, CombatRole> _dicCombatRole = new Dictionary<int, CombatRole>(); // <memberId, CombatRole>
        private Dictionary<int, int> _dicSlotMember = new Dictionary<int, int>();               // <slotId, memberId>

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

            int viewPoint = EnergyPoint % GameConst.BAR_ENERGY_POINT;
            int viewOrb = EnergyPoint / GameConst.BAR_ENERGY_POINT;

            _uiEnergyBar.ChangeViewBar(viewPoint);
            _uiEnergyBar.ChangeViewOrb(viewOrb);

            if (EnergyPoint == GameConst.MAX_ENERGY_POINT)
            {
                // Todo: Lock EnergyBar
            }
        }

        internal int ConvertFormalSlotId(int informalId)
        {
            int slotId = 0;

            if (informalId > GameConst.MAX_TEAM_MEMBER)
            {
                slotId = informalId % GameConst.MAX_TEAM_MEMBER;
            }
            else if (informalId < 0)
            {
                slotId = GameConst.MAX_TEAM_MEMBER - (informalId % GameConst.MAX_TEAM_MEMBER);
            }
            else
            {
                slotId = informalId;
            }

            if (slotId == 0)
            {
                slotId = GameConst.MAX_TEAM_MEMBER;
            }

            return slotId;
        }

        internal void ChangeMatchSlotId(int deltaSlotId)
        {
            int tmpId = MatchSlotId + deltaSlotId;

            SetMatchSlotId(tmpId);
        }

        internal void ChangeMatchSlotId(bool isDirectionRight)
        {
            int tmpId = MatchSlotId;

            if (isDirectionRight)
            {
                tmpId -= 1;
            }
            else
            {
                tmpId += 1;
            }

            SetMatchSlotId(tmpId);
        }

        internal void SetMatchSlotId(int slotId)
        {
            MatchSlotId = ConvertFormalSlotId(slotId);
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

            float posX = _uiRoleList.initPosX + (_uiRoleList.deltaPosX * (memberId - 1));
            GameObject objUICombatRole = GameObject.Instantiate(Resources.Load<GameObject>(AssetsPath.PREFAB_UI_COMBAT_ROLE), new Vector2(posX, 0), Quaternion.identity);
            objUICombatRole.transform.SetParent(_uiRoleList.gameObject.transform, false);

            combatRole._uiCombatRole = objUICombatRole.GetComponent<UICombatRole>();
            combatRole._uiCombatRole.ChangeViewPortrait(csvData._portrait);
            combatRole._uiCombatRole.ChangeViewEmblem(csvData._emblem);

            _uiCombatCircle.ChangeViewRoleSlot(memberId, ref csvData);

            // UICombatRole 加入 RoleList
            _uiRoleList._dicUICombatRole.Add(memberId, objUICombatRole);

            // CombatRole 加入 CombatTeam
            _dicCombatRole.Add(memberId, combatRole);

            return true;
        }

        internal void GetCombatRoleByMember(int memberId, out CombatRole outCombatRole)
        {
            _dicCombatRole.TryGetValue(memberId, out outCombatRole);
        }

        internal void GetCombatRoleBySlot(int slotId, out CombatRole outCombatRole)
        {
            int memberId = 0;
            
            _dicSlotMember.TryGetValue(slotId, out memberId);
            _dicCombatRole.TryGetValue(memberId, out outCombatRole);
        }

        internal void GetMatchCombatRole(out CombatRole outCombatRole)
        {
            //GetCombatRoleBySlot(MatchSlotId, out outCombatRole);
            GetCombatRoleByMember(MatchSlotId, out outCombatRole);
        }
    }
}