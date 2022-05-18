using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public class SystemCore : MonoBehaviour
    {
        internal enum eGameScene
        {
            E_GAME_SCENE_NA = 0,
            E_GAME_SCENE_START,     // 開始
            E_GAME_SCENE_ORIGIN,    // 起源
            E_GAME_SCENE_MAP,       // 地圖
            E_GAME_SCENE_COMBAT,    // 戰鬥
            E_GAME_SCENE_LIMIT,
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);

            TableManager.Instance.Init();
            UIManager.Instance.Init();
            AudioManager.Instance.Init();
            SceneManager.Instance.Init();
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(SceneManager.Instance.LoadScene(SystemCore.eGameScene.E_GAME_SCENE_COMBAT));
            }
        }
    }
}
