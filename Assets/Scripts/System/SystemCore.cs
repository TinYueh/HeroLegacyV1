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
        //SoundManager.Instance.PlayBgm(_clip);
    }

    private void Update()
    {
        
    }
}
