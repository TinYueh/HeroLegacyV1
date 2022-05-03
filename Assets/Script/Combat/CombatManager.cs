using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    //[SerializeField]
    //private int _opponentTeamId = 0;

    //private CombatCircleController _combatCircleController = null;

    public override void Init()
    {
        //_combatCircleController = GetComponent<CombatCircleController>();

        //SetupOpponent(_opponentTeamId);

        Debug.Log("CombatManager Init OK");
    }

    //private void SetupOpponent(int teamId)
    //{
    //    TeamCsvData csvData = TableManager.Instance.GetTeamCsvData(teamId);
    //    if (csvData == null)
    //    {
    //        Debug.LogWarning("Not found TeamCsvData, Id: " + teamId);
    //    }
    //}
}
