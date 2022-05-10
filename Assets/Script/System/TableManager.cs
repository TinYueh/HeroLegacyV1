using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TableManager : Singleton<TableManager>
{
    // CsvData dictionary
    private Dictionary<int, TeamCsvData> _dicTeamCsvData = new Dictionary<int, TeamCsvData>();
    private Dictionary<int, RoleCsvData> _dicRoleCsvData = new Dictionary<int, RoleCsvData>();
    // �s�W Table: �w�q dictionary

    // LoadCsvData delegate
    private delegate bool DlgLoadCsvData(string[] rowData, out int outIndex);

    // LoadCsvData delegate dictionary
    private Dictionary<string, DlgLoadCsvData> _dicLoadCsvFunc = new Dictionary<string, DlgLoadCsvData>();

    public override void Init()
    {
        // ���U Load function
        RegistLoadFunc();

        // Ū���Ҧ��� csv
        LoadTable();

        Debug.Log("TableManager Init OK");
    }

    private void LoadTable()
    {
        // �Ҧ��� Table
        TextAsset[] arrTextAsset = Resources.LoadAll<TextAsset>(AssetsPath.TABLE_PATH);

        foreach (var t in arrTextAsset)
        {
            // �ɮצW��
            int preIndex = t.name.IndexOf(" - ");
            string fileName = t.name.Substring(preIndex + 3);

            // Load function
            DlgLoadCsvData dlgFunc = null;
            _dicLoadCsvFunc.TryGetValue(fileName, out dlgFunc);
            if (dlgFunc == null)
            {
                Debug.LogError("Not found LoadCsvFunc for " + fileName);
                continue;
            }

            string[] fileData = t.text.Split("\r\n");
            string[] key = fileData[0].Split(',');

            // ��Ʊq�� 2 ��}�l
            for (int i = 1; i < fileData.Length; ++i)
            {
                int index = 0;
                string[] rowData = fileData[i].Split(',');

                if (dlgFunc(rowData, out index) == false)
                {
                    Debug.LogError("Fail to exec load function, FileName: " + fileName + ", Row: " + i);
                }

                if (key.Length != index + 1)
                {
                    Debug.LogError("Column length mismatch, Keys: " + key.Length + ", Index(+): " + (index + 1) + ", FileName: " + fileName + ", Row: " + i);
                }
            }
        }
    }

    private void RegistLoadFunc()
    {
        _dicLoadCsvFunc.Add("Team", LoadTeamCsvData);
        _dicLoadCsvFunc.Add("Role", LoadRoleCsvData);
        // �s�W Table: ���U
    }

    private bool LoadTeamCsvData(string[] rowData, out int outIndex)
    {
        TeamCsvData data = new TeamCsvData();
        outIndex = 0;

        data._id = int.Parse(rowData[outIndex]);
        for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
        {
            data._arrRoleId[i] = int.Parse(rowData[++outIndex]);
        }

        _dicTeamCsvData.Add(data._id, data);

        return true;
    }

    private bool LoadRoleCsvData(string[] rowData, out int outIndex)
    {
        RoleCsvData data = new RoleCsvData();
        outIndex = 0;

        data._id = int.Parse(rowData[outIndex]);
        data._team = int.Parse(rowData[++outIndex]);
        data._portrait = int.Parse(rowData[++outIndex]);
        data._emblem = int.Parse(rowData[++outIndex]);
        data._name = int.Parse(rowData[++outIndex]);
        data._attribute = int.Parse(rowData[++outIndex]);
        data._talent = int.Parse(rowData[++outIndex]);
        data._life = int.Parse(rowData[++outIndex]);
        data._attack = int.Parse(rowData[++outIndex]);
        data._defence = int.Parse(rowData[++outIndex]);
        data._ai = int.Parse(rowData[++outIndex]);

        _dicRoleCsvData.Add(data._id, data);

        return true;
    }

    // �s�W Table: �w�q Load function

    //public bool GetHeroCsvData(int id, out HeroCsvData outCsvData)
    //{
    //    if (_dicHeroCsvData.TryGetValue(id, out outCsvData))
    //    {
    //        return true;
    //    }

    //    outCsvData = null;

    //    return false;
    //}

    //public bool GetMobCsvData(int id, out MobCsvData outCsvData)
    //{
    //    if (_dicMobCsvData.TryGetValue(id, out outCsvData))
    //    {
    //        return true;
    //    }

    //    outCsvData = null;

    //    return false;
    //}

    public bool GetTeamCsvData(int id, out TeamCsvData outCsvData)
    {
        if (_dicTeamCsvData.TryGetValue(id, out outCsvData))
        {
            return true;
        }

        outCsvData = null;

        return false;
    }

    public bool GetRoleCsvData(int id, out RoleCsvData outCsvData)
    {
        if (_dicRoleCsvData.TryGetValue(id, out outCsvData))
        {
            return true;
        }

        outCsvData = null;

        return false;
    }

    // �s�W Table: �w�q Get function
}
