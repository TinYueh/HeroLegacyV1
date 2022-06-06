using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCombat
{
    public class ViewSkill : MonoBehaviour
    {
        internal int SkillId { get; private set; } = 0;

        private Image _imgSkill = null;

        internal bool Init()
        {
            _imgSkill = GetComponent<Image>();
            if (_imgSkill == null)
            {
                Debug.LogError("Not found ImageSkill");
                return false;
            }

            return true;
        }

        internal void Set(int skillId)
        {
            SkillId = skillId;

            string path = AssetsPath.SPRITE_SKILL_PATH + SkillId.ToString().PadLeft(5, '0');
            _imgSkill.sprite = Resources.Load<Sprite>(path);
        }

        internal void Show()
        {
            gameObject.SetActive(true);
        }

        internal void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
