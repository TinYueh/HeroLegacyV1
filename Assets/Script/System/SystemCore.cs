using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCore : MonoBehaviour
{
    [SerializeField]
    private int _bgmNext = 0;

    //[SerializeField]
    //private int _sfxNext = 0;

    private void Awake()
    {
        TableManager.Instance.Init();
        UIManager.Instance.Init();
        AudioManager.Instance.Init();
        HeroManager.Instance.Init();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            AudioManager.Instance.StopBgm();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.Instance.PlayBgm(_bgmNext, true);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            AudioManager.Instance.PlaySfx(101);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            AudioManager.Instance.PlaySfx(102);
        }
    }
}
