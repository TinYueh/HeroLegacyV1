using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    enum eHeroJob
    {
        E_HERO_JOB_NA = 0,
        E_HERO_JOB_WARRIOR,
        E_HERO_JOB_ASSASSIN,
        E_HERO_JOB_PRIEST,
    }

    [SerializeField]
    private int _life = 0;          // �ͩR
    [SerializeField]
    private int _attack = 0;        // ��
    [SerializeField]
    private int _defence = 0;       // ��
    [SerializeField]
    private int _intelligent = 0;   // ��
    [SerializeField]
    private int _resist = 0;        // ��
    [SerializeField]
    private int _speed = 0;         // �t

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
