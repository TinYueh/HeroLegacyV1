using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameProperty;

namespace GameCombat
{
    public class CombatRole
    {
        private ViewCombatRole _viewCombatRole;    // View

        internal GameEnum.eCombatTeamType TeamType { get; private set; }
        internal int MemberId { get; private set; }
        internal int PosId { get; private set; }
        internal Role Role { get; private set; } = new Role();
        internal GameEnum.eCombatRoleState State { get; private set; }
        internal int Health { get; private set; }
        internal int NormalDamage { get; set; }

        private Dictionary<int, int> _dicSkillCd = new Dictionary<int, int>();  // <SkillId, Cd>

        internal bool Init(GameEnum.eCombatTeamType teamType, int memberId, int posId, RoleCsvData csvData, ViewCombatRole viewCombatRole)
        {
            if (Role.Init(csvData) == false)
            {
                Debug.LogError("Init Role failed, RoleId: " + csvData._id);
                return false;
            }

            TeamType = teamType;
            MemberId = memberId;
            PosId = posId;
            Health = Role.Health;
            State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_ALIVE;
            _viewCombatRole = viewCombatRole;    // Attach View

            return true;
        }

        #region Get Set
        internal void ChangeHealth(int deltaHealth)
        {
            int tmpHealth = Health + deltaHealth;

            SetHealth(tmpHealth);
        }
        internal void SetHealth(int health)
        {
            if (health < 0)
            {
                Health = 0;
            }
            else if (health > Role.Health)
            {
                Health = Role.Health;
            }
            else
            {
                Health = health;
            }

            _viewCombatRole.SetHealthBar(Health, Role.Health);

            if (Health == 0)
            {
                // Todo: 整個區塊移到 CombatController
                State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING;
                _viewCombatRole.SetStateDying();

                CircleSocket circleSocket = null;
                if (CombatManager.Instance.Controller.GetCircleSocket(TeamType, PosId, out circleSocket))
                {
                    circleSocket.Clear();
                }
            }
            //else
            //{
            //    // 實作復活時需要
            //    State = GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_ALIVE;
            //    _viewCombatRole.SetStateAlive();
            //}
        }
        internal void ChangeAllSkillCd(int deltaCd)
        {
            for (int i = 0; i < _dicSkillCd.Count; ++i)
            {
                ChangeSkillCd(_dicSkillCd.ElementAt(i).Key, deltaCd);
            }
        }
        internal void ChangeSkillCd(int skillId, int deltaCd)
        {
            if (_dicSkillCd.ContainsKey(skillId))
            {
                int value = _dicSkillCd[skillId];
                if (value == 0)
                {
                    return;
                }

                value += deltaCd;
                if (value < 0)
                {
                    value = 0;
                }

                _dicSkillCd[skillId] = value;
            }
        }
        internal void SetSkillCd(int skillId, int cd)
        {
            _dicSkillCd[skillId] = cd;
        }
        internal int GetSkillCd(int skillId)
        {
            int cd;
            _dicSkillCd.TryGetValue(skillId, out cd);

            return cd;
        }
        #endregion

        #region Logic
        internal bool IsAlive()
        {
            return (State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_ALIVE);
        }
        internal bool IsDying()
        {
            return (State == GameEnum.eCombatRoleState.E_COMBAT_ROLE_STATE_DYING);
        }
        internal bool IsSkillCd(int skillId)
        {
            int cd = GetSkillCd(skillId);

            return (cd > 0);
        }
        #endregion
    }
}