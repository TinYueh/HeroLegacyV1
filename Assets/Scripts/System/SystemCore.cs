using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Audio;
using GameSystem.Scene;
using GameSystem.Table;
using GameSystem.UI;
using GameSkill;

namespace GameSystem
{
    public class SystemCore : MonoBehaviour
    {
        [SerializeField]
        private int _bgmId = 0;
        [SerializeField]
        private float _masterVolume = 0;
        [SerializeField]
        private float _bgmVolume = 0;
        [SerializeField]
        private float _sfxVolume = 0;

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
            SkillManager.Instance.Init();
        }

        private void Start()
        {
            // Todo: 音量設定移到 Option
            AudioManager.Instance.SetMasterVolume(_masterVolume);
            AudioManager.Instance.SetBgmVolume(_bgmVolume);
            AudioManager.Instance.SetSfxVolume(_sfxVolume);
        }

        private void Update()
        {
            // Todo: 場景切換轉正
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(SceneManager.Instance.LoadScene(SystemCore.eGameScene.E_GAME_SCENE_COMBAT));
            }
            // Todo: 音樂音效轉正
            else if (Input.GetKeyDown(KeyCode.B))
            {
                AudioManager.Instance.PlayBgm(_bgmId, true);
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                AudioManager.Instance.StopBgm();
            }
        }
    }
}
