using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Role
{
    public int Life { get; private set; } = 0;
    public int Attack { get; private set; } = 0;
    public int Defence { get; private set; } = 0;
    public int Ai { get; private set; } = 0;

    public override bool Init(int id)
    {
        MobCsvData csvData = new MobCsvData();
        if (TableManager.Instance.GetMobCsvData(id, out csvData) == false)
        {
            Debug.LogError("Not found MobCsvData, Id: " + id);
            return false;
        }

        Id = csvData.id;
        Portrait = csvData.portrait;
        Emblem = csvData.emblem;
        Name = csvData.name;
        Life = csvData.life;
        Attack = csvData.attack;
        Defence = csvData.defence;
        Ai = csvData.ai;

        return true;
    }
}
