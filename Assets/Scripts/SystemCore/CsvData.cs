using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCsvData
{
    public int id;
    public int name;
    public int attack;
    public int defence;
}

public class TeamCsvData
{
    public int id;
    public int[] mobId = new int[GameConst.MAX_TEAM_MEMBER];
}

// �s�W Table: �w�q���c