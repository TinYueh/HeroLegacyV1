using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace GameSystem.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        private float _bgmDefaultVolume = 0.5f;
        private float _sfxDefaultVolume = 1f;

        private AudioSource _bgmSource = null;
        private AudioSource _sfxSource = null;

        public override bool Init()
        {
            _bgmSource = GameObject.Find("BgmSource").GetComponent<AudioSource>();
            if (_bgmSource == null)
            {
                Debug.LogError("Not found Bgm AudioSource");
                return false;
            }
            SetBgmVolume(_bgmDefaultVolume);

            _sfxSource = GameObject.Find("SfxSource").GetComponent<AudioSource>();
            if (_sfxSource == null)
            {
                Debug.LogError("Not found Sfx AudioSource");
                return false;
            }
            SetSfxVolume(_sfxDefaultVolume);

            Debug.Log("AudioManager Init OK");
            return true;
        }

        public void PlayBgm(int id, bool isLoop)
        {
            if (_bgmSource == null)
            {
                Debug.LogError("Not found Bgm AudioSource");
                return;
            }

            string path = AssetsPath.AUDIO_BGM_PATH + id;
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip == null)
            {
                Debug.LogError("Not found " + path);
                return;
            }

            _bgmSource.clip = clip;
            _bgmSource.loop = isLoop;
            _bgmSource.Play();
        }

        public void StopBgm()
        {
            if (_bgmSource == null)
            {
                Debug.LogError("Not found Bgm AudioSource");
                return;
            }

            _bgmSource.clip = null;
            _bgmSource.loop = false;
            _bgmSource.Stop();
        }

        public void PlaySfx(int id)
        {
            if (_sfxSource == null)
            {
                Debug.LogError("Not found Sfx AudioSource");
                return;
            }

            string path = AssetsPath.AUDIO_SFX_PATH + id;
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip == null)
            {
                Debug.LogError("Not found " + path);
                return;
            }

            _sfxSource.PlayOneShot(clip);
        }

        public void SetMasterVolume(float value)
        {
            AudioListener.volume = value;
        }

        public void SetBgmVolume(float value)
        {
            if (_bgmSource == null)
            {
                Debug.LogError("Not found Bgm AudioSource");
                return;
            }

            _bgmSource.volume = value;
        }

        public void SetSfxVolume(float value)
        {
            if (_bgmSource == null)
            {
                Debug.LogError("Not found Sfx AudioSource");
                return;
            }

            _sfxSource.volume = value;
        }
    }
}