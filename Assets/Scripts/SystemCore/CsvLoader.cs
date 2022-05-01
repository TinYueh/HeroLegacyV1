using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CsvLoader : MonoBehaviour
{
    // CsvData dictionary
    private Dictionary<int, HeroCsvData> _dicHeroCsvData = new Dictionary<int, HeroCsvData>();
    private Dictionary<int, TeamCsvData> _dicTeamCsvData = new Dictionary<int, TeamCsvData>();
    // 新增 Table: 定義 dictionary

    // LoadCsvData delegate
    private delegate bool DlgLoadCsvData(string[] lineData);

    // LoadCsvData delegate dictionary
    private Dictionary<string, DlgLoadCsvData> _dicLoadCsvFunc = new Dictionary<string, DlgLoadCsvData>();

    private void Awake()
    {
        // 註冊 Load function
        RegistLoadFunc();

        // 讀取 Tables 路徑下的所有 csv
        string[] arrTablePath = Directory.GetFiles(Application.dataPath + "/Tables/", "*.csv", SearchOption.AllDirectories);
        for (int i = 0; i < arrTablePath.Length; ++i)
        {
            LoadTable(arrTablePath[i]);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        
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
            Debug.Log("Not found LoadCsvFunc for " + fileName);
            return;
        }    

        string[] fileData = File.ReadAllLines(path);
        string[] keys = fileData[0].Split(',');
        
        // 第 2 行開始是資料
        for (int i = 1; i < fileData.Length; ++i)
        {
            string[] lineData = fileData[i].Split(',');

            if (dlgLoadCsvFunc(lineData) == false)
            {
                Debug.Log("Exec load function failed, FileName: " + fileName + " Row: " + i);
            }
        }
    }

    private void RegistLoadFunc()
    {
        _dicLoadCsvFunc.Add("Hero", LoadHeroCsvData);
        _dicLoadCsvFunc.Add("Team", LoadTeamCsvData);
        // 新增 Table: 註冊
    }

    private bool LoadHeroCsvData(string[] lineData)
    {
        HeroCsvData data = new HeroCsvData();
        int index = 0;

        data.id = int.Parse(lineData[index]);
        data.name = int.Parse(lineData[++index]);
        data.attack = int.Parse(lineData[++index]);
        data.defence = int.Parse(lineData[++index]);

        _dicHeroCsvData.Add(data.id, data);

        return true;
    }

    private bool LoadTeamCsvData(string[] lineData)
    {
        TeamCsvData data = new TeamCsvData();
        int index = 0;

        data.id = int.Parse(lineData[index]);
        for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
        {
            data.mobId[i] = int.Parse(lineData[++index]);
        }

        _dicTeamCsvData.Add(data.id, data);

        return true;
    }

    // 新增 Table: 定義 Load function
}
