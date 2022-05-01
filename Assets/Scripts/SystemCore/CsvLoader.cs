using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CsvLoader : MonoBehaviour
{
    // CsvData dictionary
    private Dictionary<int, HeroCsvData> _dicHeroCsvData = new Dictionary<int, HeroCsvData>();
    private Dictionary<int, TeamCsvData> _dicTeamCsvData = new Dictionary<int, TeamCsvData>();
    // �s�W Table: �w�q dictionary

    // LoadCsvData delegate
    private delegate bool DlgLoadCsvData(string[] lineData);

    // LoadCsvData delegate dictionary
    private Dictionary<string, DlgLoadCsvData> _dicLoadCsvFunc = new Dictionary<string, DlgLoadCsvData>();

    private void Awake()
    {
        // ���U Load function
        RegistLoadFunc();

        // Ū�� Tables ���|�U���Ҧ� csv
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
        // �ɮצW��
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
        
        // �� 2 ��}�l�O���
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
        // �s�W Table: ���U
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

    // �s�W Table: �w�q Load function
}
