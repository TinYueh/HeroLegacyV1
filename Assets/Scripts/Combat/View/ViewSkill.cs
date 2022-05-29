using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCombat
{
    public class ViewSkill : MonoBehaviour
    {
        internal bool Init()
        {
            return true;
        }

        internal void SetShow()
        {
            this.gameObject.SetActive(true);
        }

        internal void SetHide()
        {
            this.gameObject.SetActive(false);
        }
    }
}
