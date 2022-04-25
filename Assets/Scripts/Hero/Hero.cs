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
    private int _life = 0;          // ¥Í©R
    [SerializeField]
    private int _attack = 0;        // §ð
    [SerializeField]
    private int _defence = 0;       // ¨¾
    [SerializeField]
    private int _intelligent = 0;   // ´¼
    [SerializeField]
    private int _resist = 0;        // §Ü
    [SerializeField]
    private int _speed = 0;         // ³t

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
