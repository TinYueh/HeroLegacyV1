using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCore : MonoBehaviour
{
    private void Awake()
    {
        CombatManager.Instance.Init();
    }

    private void Start()
    {
        CreatePlayerTeamSerial(GameConst.MAX_TEAM_MEMBER);

        HeroManager.Instance.ShowAllHero();
    }

    private void Update()
    {
        
    }

    private void CreatePlayerTeamSerial(int num)
    {
        for (int i = 0; i < num; ++i)
        {
            HeroManager.Instance.Create(i + 1);
        }
    }
}
