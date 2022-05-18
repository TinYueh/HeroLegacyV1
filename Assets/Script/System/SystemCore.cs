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
            E_GAME_SCENE_START,     // �}�l
            E_GAME_SCENE_ORIGIN,    // �_��
            E_GAME_SCENE_MAP,       // �a��
            E_GAME_SCENE_COMBAT,    // �԰�
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
