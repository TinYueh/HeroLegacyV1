using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameSystem.Table
{
    public class TableManager : Singleton<TableManager>
    {
        #region Property

        // CsvData dictionary
        internal readonly Dictionary<int, TeamCsvData> _dicTeamCsvData = new Dictionary<int, TeamCsvData>();
        internal readonly Dictionary<int, RoleCsvData> _dicRoleCsvData = new Dictionary<int, RoleCsvData>();
        internal readonly Dictionary<int, SkillCsvData> _dicSkillCsvData = new Dictionary<int, SkillCsvData>();
        // 新增 Table: 定義 dictionary

        // LoadCsvData delegate
        private delegate bool DlgLoadCsvData(string[] rowData, out int outIndex);
        // LoadCsvData delegate dictionary
        private Dictionary<string, DlgLoadCsvData> _dicDlgLoadCsv = new Dictionary<string, DlgLoadCsvData>();

        #endregion  // Property

        #region Init

        public override bool Init()
        {
            // 註冊 Load function
            RegistDlgLoadCsv();

            // 讀取所有的 csv
            LoadTable();

            Debug.Log("TableManager Init OK");

            return true;
        }
        private void RegistDlgLoadCsv()
        {
            _dicDlgLoadCsv.Add("Team", LoadTeamCsvData);
            _dicDlgLoadCsv.Add("Role", LoadRoleCsvData);
            _dicDlgLoadCsv.Add("Skill", LoadSkillCsvData);
            // 新增 Table: 註冊
        }

        #endregion  // Init

        #region Load

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
                if (_dicDlgLoadCsv.TryGetValue(fileName, out dlgFunc) == false)
                {
                    Debug.LogError("Not found LoadCsvFunc for " + fileName);
                    continue;
                }

                string[] fileData = t.text.Split("\r\n");
                string[] key = fileData[0].Split(',');

                // 資料從第 2 行開始
                for (int i = 1; i < fileData.Length; ++i)
                {
                    int index;
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
            data._name = rowData[++outIndex];
            int.TryParse(rowData[++outIndex], out data._uiName);
            int.TryParse(rowData[++outIndex], out data._portrait);
            int.TryParse(rowData[++outIndex], out data._emblem);
            int.TryParse(rowData[++outIndex], out data._attribute);
            int.TryParse(rowData[++outIndex], out data._talent);
            int.TryParse(rowData[++outIndex], out data._health);
            int.TryParse(rowData[++outIndex], out data._attackType);
            int.TryParse(rowData[++outIndex], out data._ptk);
            int.TryParse(rowData[++outIndex], out data._mtk);
            int.TryParse(rowData[++outIndex], out data._pef);
            int.TryParse(rowData[++outIndex], out data._mef);
            int.TryParse(rowData[++outIndex], out data._ai);

            for (int i = 0; i < GameConst.MAX_ROLE_SKILL; ++i)
            {
                int.TryParse(rowData[++outIndex], out data._skillId[i]);
            }

            _dicRoleCsvData.Add(data._id, data);

            return true;
        }

        private bool LoadSkillCsvData(string[] rowData, out int outIndex)
        {
            SkillCsvData data = new SkillCsvData();
            outIndex = 0;

            int.TryParse(rowData[outIndex], out data._id);
            data._name = rowData[++outIndex];
            int.TryParse(rowData[++outIndex], out data._uiName);
            int.TryParse(rowData[++outIndex], out data._posType);
            int.TryParse(rowData[++outIndex], out data._cost);
            int.TryParse(rowData[++outIndex], out data._cd);
            int.TryParse(rowData[++outIndex], out data._range);
            
            for (int i = 0; i < GameConst.MAX_SKILL_EFFECT; ++i)
            {
                data._effect[i] = new SkillEffectCsvData();
                int.TryParse(rowData[++outIndex], out data._effect[i]._type);
                int.TryParse(rowData[++outIndex], out data._effect[i]._effectValueType);
                int.TryParse(rowData[++outIndex], out data._effect[i]._effectValue);
            }

            _dicSkillCsvData.Add(data._id, data);

            return true;
        }
        // 新增 Table: 定義 Load Method

        #endregion  // Load

        #region Get Set

        public bool GetTeamCsvData(int id, out TeamCsvData outCsvData)
        {
            if (_dicTeamCsvData.TryGetValue(id, out outCsvData))
            {
                return true;
            }

            return false;
        }

        public bool GetRoleCsvData(int id, out RoleCsvData outCsvData)
        {
            if (_dicRoleCsvData.TryGetValue(id, out outCsvData))
            {
                return true;
            }

            return false;
        }

        public bool GetSkillCsvData(int id, out SkillCsvData outCsvData)
        {
            if (_dicSkillCsvData.TryGetValue(id, out outCsvData))
            {
                return true;
            }

            return false;
        }
        // 新增 Table: 定義 Get Method

        #endregion  // Get Set
    }
}