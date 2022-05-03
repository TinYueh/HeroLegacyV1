using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TableManager : Singleton<TableManager>
{
    // CsvData dictionary
    private Dictionary<int, HeroCsvData> _dicHeroCsvData = new Dictionary<int, HeroCsvData>();
    private Dictionary<int, MobCsvData> _dicMobCsvData = new Dictionary<int, MobCsvData>();
    private Dictionary<int, TeamCsvData> _dicTeamCsvData = new Dictionary<int, TeamCsvData>();
    // 新增 Table: 定義 dictionary

    // LoadCsvData delegate
    private delegate bool DlgLoadCsvData(string[] rowData, ref int refIndex);

    // LoadCsvData delegate dictionary
    private Dictionary<string, DlgLoadCsvData> _dicLoadCsvFunc = new Dictionary<string, DlgLoadCsvData>();

    public override void Init()
    {
        // 註冊 Load function
        RegistLoadFunc();

        // 讀取路徑下的所有 csv
        string[] arrPath = Directory.GetFiles(Application.dataPath + "/Tables/", "*.csv", SearchOption.AllDirectories);
        for (int i = 0; i < arrPath.Length; ++i)
        {
            LoadTable(arrPath[i]);
        }

        Debug.Log("TableManager Init OK");
    }

    private void LoadTable(string path)
    {
        // 檔案名稱
        int pathPreIndex = path.IndexOf(" - ");
        int pathSufIndex = path.IndexOf(".csv");
        string fileName = path.Substring(pathPreIndex + 3, pathSufIndex - pathPreIndex - 3);

        // Load function
        DlgLoadCsvData dlgLoadCsvFunc;
        _dicLoadCsvFunc.TryGetValue(fileName, out dlgLoadCsvFunc);
        if (dlgLoadCsvFunc == null)
        {
            Debug.LogError("Not found LoadCsvFunc for " + fileName);
            return;
        }

        string[] fileData = File.ReadAllLines(path);
        string[] keys = fileData[0].Split(',');

        // 資料從第 2 行開始
        for (int i = 1; i < fileData.Length; ++i)
        {
            int index = 0;
            string[] rowData = fileData[i].Split(',');

            if (dlgLoadCsvFunc(rowData, ref index) == false)
            {
                Debug.LogError("Fail to exec load function, FileName: " + fileName + ", Row: " + i);
            }

            if (keys.Length != index + 1)
            {
                Debug.LogError("Column length mismatch, Keys: " + keys.Length + ", Index(+): " + (index + 1) + ", FileName: " + fileName + ", Row: " + i);
            }
        }
    }

    private void RegistLoadFunc()
    {
        _dicLoadCsvFunc.Add("Hero", LoadHeroCsvData);
        _dicLoadCsvFunc.Add("Mob", LoadMobCsvData);
        _dicLoadCsvFunc.Add("Team", LoadTeamCsvData);
        // 新增 Table: 註冊
    }

    private bool LoadHeroCsvData(string[] rowData, ref int refIndex)
    {
        HeroCsvData data = new HeroCsvData();
        refIndex = 0;

        data.id = int.Parse(rowData[refIndex]);
        data.portrait = int.Parse(rowData[++refIndex]);
        data.emblem = int.Parse(rowData[++refIndex]);
        data.name = int.Parse(rowData[++refIndex]);
        data.talent = int.Parse(rowData[++refIndex]);
        data.life = int.Parse(rowData[++refIndex]);
        data.attack = int.Parse(rowData[++refIndex]);
        data.defence = int.Parse(rowData[++refIndex]);

        _dicHeroCsvData.Add(data.id, data);

        return true;
    }

    private bool LoadMobCsvData(string[] rowData, ref int refIndex)
    {
        MobCsvData data = new MobCsvData();
        refIndex = 0;

        data.id = int.Parse(rowData[refIndex]);
        data.portrait = int.Parse(rowData[++refIndex]);
        data.emblem = int.Parse(rowData[++refIndex]);
        data.name = int.Parse(rowData[++refIndex]);
        data.life = int.Parse(rowData[++refIndex]);
        data.attack = int.Parse(rowData[++refIndex]);
        data.defence = int.Parse(rowData[++refIndex]);
        data.ai = int.Parse(rowData[++refIndex]);

        _dicMobCsvData.Add(data.id, data);

        return true;
    }

    private bool LoadTeamCsvData(string[] rowData, ref int refIndex)
    {
        TeamCsvData data = new TeamCsvData();
        refIndex = 0;

        data.id = int.Parse(rowData[refIndex]);
        for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
        {
            data.mobId[i] = int.Parse(rowData[++refIndex]);
        }

        _dicTeamCsvData.Add(data.id, data);

        return true;
    }

    // 新增 Table: 定義 Load function

    public HeroCsvData GetHeroCsvData(int id)
    {
        HeroCsvData csvData = new HeroCsvData();

        if (_dicHeroCsvData.TryGetValue(id, out csvData))
        {
            return csvData;
        }

        return null;
    }

    public MobCsvData GetMobCsvData(int id)
    {
        MobCsvData csvData = new MobCsvData();

        if (_dicMobCsvData.TryGetValue(id, out csvData))
        {
            return csvData;
        }

        return null;
    }

    public TeamCsvData GetTeamCsvData(int id)
    {
        TeamCsvData csvData = new TeamCsvData();

        if (_dicTeamCsvData.TryGetValue(id, out csvData))
        {
            return csvData;
        }

        return null;
    }

    // 新增 Table: 定義 Get function
}
