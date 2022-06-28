using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSkill;
using GameSystem.Audio;
using GameSystem.Scene;
using GameSystem.Table;
using GameSystem.Tooltip;

namespace GameSystem
{
    public class SystemCore : MonoBehaviour
    {
        #region Enum

        internal enum eGameScene
        {
            E_GAME_SCENE_NA = 0,
            E_GAME_SCENE_START,     // 開始
            E_GAME_SCENE_ORIGIN,    // 起源
            E_GAME_SCENE_MAP,       // 地圖
            E_GAME_SCENE_COMBAT,    // 戰鬥
            E_GAME_SCENE_LIMIT,
        }

        #endregion  // Enum

        #region Property

        [SerializeField]
        private int _bgmId;
        [SerializeField]
        private float _masterVolume;
        [SerializeField]
        private float _bgmVolume;
        [SerializeField]
        private float _sfxVolume;

        #endregion  // Property

        #region Mono

        private void Awake()
        {
            DontDestroyOnLoad(this);

            TableManager.Instance.Init();
            AudioManager.Instance.Init();
            SceneManager.Instance.Init();
            SkillManager.Instance.Init();
            TooltipManager.Instance.Init();
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

        #endregion  // Mono
    }
}