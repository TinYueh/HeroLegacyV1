using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public override bool Init()
        {
            Debug.Log("UIManager Init OK");

            return true;
        }
    }
}