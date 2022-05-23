using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameSystem.Table
{
    public class TableManager : Singleton<TableManager>
    {
        // CsvData dictionary
        private Dictionary<int, TeamCsvData> _dicTeamCsvData;
        private Dictionary<int, RoleCsvData> _dicRoleCsvData;
        // 新增 Table: 定義 dictionary

        // LoadCsvData delegate
        private delegate bool DlgLoadCsvData(string[] rowData, out int outIndex);

        // LoadCsvData delegate dictionary
        private Dictionary<string, DlgLoadCsvData> _dicLoadCsvFunc;

        public override void Init()
        {
            _dicTeamCsvData = new Dictionary<int, TeamCsvData>();
            _dicRoleCsvData = new Dictionary<int, RoleCsvData>();
            // 新增 Table: new Dictionary

            _dicLoadCsvFunc = new Dictionary<string, DlgLoadCsvData>();

            // 註冊 Load function
            RegistLoadFunc();

            // 讀取所有的 csv
            LoadTable();

            Debug.Log("TableManager Init OK");
        }

        private void LoadTable()
        {
            // 所有的 Table
            TextAsset[] arrTextAsset = Resources.LoadAll<TextAsset>(AssetsPath.TABLE_PATH);

            foreach (var t in arrTextAsset)
            {
                // 檔案名稱
                int preIndex = t.name.IndexOf(" - ");
                string fileName = t.name.Substring(preIndex + 3);

                // Load function
                DlgLoadCsvData dlgFunc;
                if (_dicLoadCsvFunc.TryGetValue(fileName, out dlgFunc) == false)
                {
                    Debug.LogError("Not found LoadCsvFunc for " + fileName);
                    continue;
                }

                string[] fileData = t.text.Split("\r\n");
                string[] key = fileData[0].Split(',');

                // 資料從第 2 行開始
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
            // 新增 Table: 註冊
        }

        private bool LoadTeamCsvData(string[] rowData, out int outIndex)
        {
            TeamCsvData data = new TeamCsvData();
            outIndex = 0;

            int.TryParse(rowData[outIndex], out data._id);

            for (int i = 0; i < GameConst.MAX_TEAM_MEMBER; ++i)
            {
                int.TryParse(rowData[++outIndex], out data._arrRoleId[i]);
            }

            _dicTeamCsvData.Add(data._id, data);

            return true;
        }

        private bool LoadRoleCsvData(string[] rowData, out int outIndex)
        {
            RoleCsvData data = new RoleCsvData();
            outIndex = 0;

            int.TryParse(rowData[outIndex], out data._id);
            int.TryParse(rowData[++outIndex], out data._teamType);
            int.TryParse(rowData[++outIndex], out data._portrait);
            int.TryParse(rowData[++outIndex], out data._emblem);
            int.TryParse(rowData[++outIndex], out data._name);
            int.TryParse(rowData[++outIndex], out data._attribute);
            int.TryParse(rowData[++outIndex], out data._talent);
            int.TryParse(rowData[++outIndex], out data._health);
            int.TryParse(rowData[++outIndex], out data._attack);
            int.TryParse(rowData[++outIndex], out data._defence);
            int.TryParse(rowData[++outIndex], out data._ai);

            _dicRoleCsvData.Add(data._id, data);

            return true;
        }

        // 新增 Table: 定義 Load function

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

        // 新增 Table: 定義 Get function
    }
}
