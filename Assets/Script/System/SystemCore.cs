using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemCore : MonoBehaviour
{
    private void Awake()
    {
        TableManager.Instance.Init();
        UIManager.Instance.Init();
        SoundManager.Instance.Init();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SoundManager.Instance.StopBgm();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            SoundManager.Instance.PlayBgm(101, true);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SoundManager.Instance.PlaySfx(101);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SoundManager.Instance.PlaySfx(102);
        }
    }
}
