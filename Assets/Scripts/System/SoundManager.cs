using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource _bgmSource = null;
    private AudioSource _sfxSource = null;

    private Dictionary<int, AudioClip> _dicBgmClip = new Dictionary<int, AudioClip>();

    public override void Init()
    {
        _bgmSource = GameObject.Find("BGMSource").GetComponent<AudioSource>();
        if (_bgmSource == null)
        {
            Debug.Log("Not found BGM AudioSource");
        }

        _sfxSource = GameObject.Find("SFXSource").GetComponent<AudioSource>();
        if (_sfxSource == null)
        {
            Debug.Log("Not found SFX AudioSource");
        }

        // 讀取路徑下的所有 mp3
        string[] arrPath = Directory.GetFiles(Application.dataPath + "/Sounds/BGMs/", "*.mp3", SearchOption.AllDirectories);
        for (int i = 0; i < arrPath.Length; ++i)
        {
            LoadBgm(arrPath[i]);
        }

        Debug.Log("SoundManager Init OK");
    }

    private void LoadBgm(string path)
    {
        int pathPreIndex = path.IndexOf("Assets/");
        int pathSufIndex = 0;
        string assetsPath = path.Substring(pathPreIndex);

        pathPreIndex = path.IndexOf("BGM_");
        pathSufIndex = path.IndexOf(".mp3");
        string fileName = path.Substring(pathPreIndex + 4, pathSufIndex - pathPreIndex - 4);

        int id = int.Parse(fileName);
        //AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetsPath);

        //_dicBgmClip.Add(id, clip);
    }

    public void PlayBgm(AudioClip clip)
    {
        if (_bgmSource)
        {
            _bgmSource.PlayOneShot(clip);
        }
    }

    public void PlayEfx(AudioClip clip)
    {
        if (_sfxSource)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }

    public void SetMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
}
