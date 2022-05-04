using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Role
{
    public int Talent { get; private set; } = 0;
    public int Life { get; private set; } = 0;
    public int Attack { get; private set; } = 0;
    public int Defence { get; private set; } = 0;

    public override bool Init(int id)
    {
        HeroCsvData csvData = new HeroCsvData();
        if (TableManager.Instance.GetHeroCsvData(id, out csvData) == false)
        {
            Debug.LogError("Not found HeroCsvData, Id: " + id);
            return false;
        }

        Id = csvData.id;
        Portrait = csvData.portrait;
        Emblem = csvData.emblem;
        Name = csvData.name;
        Talent = csvData.talent;
        Life = csvData.life;
        Attack = csvData.attack;
        Defence = csvData.defence;

        return true;
    }
}
