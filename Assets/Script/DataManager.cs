using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
public class DataManager : MonoBehaviour
{

    public static DataManager Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<DataManager>();
            if (_instance == null)
            {
                GameObject container = new GameObject("DataManager");
                _instance = container.AddComponent<DataManager>();
            }
        }
        return _instance;
    }
    private static DataManager _instance;
    private string soulDataPath = "Database/SoulData";
    private Dictionary<string, SoulData> soulDataDic = new Dictionary<string, SoulData>();
    public Dictionary<string, SoulData> SoulDataDic { get { return soulDataDic; } }
    private List<string> soulList = new List<string>();
    public List<string> SoulList { get { return soulList; } }

    private void Awake()
    {
        _instance = this;
        LoadBaseData();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadBaseData()
    {
        List<Dictionary<string, object>> soulData = CSVReader.Read(soulDataPath);
        for(int i = 0; i <soulData.Count; i++)
        {
            soulDataDic.Add(soulData[i]["Name"].ToString(), 
                new SoulData(soulData[i]["ID"].ToString(),soulData[i]["Name"].ToString(), soulData[i]["Speed"].ToString(), soulData[i]["Damage"].ToString(),
                soulData[i]["Range"].ToString(), soulData[i]["IsUseDash"].ToString(), soulData[i]["AvailableJumpCount"].ToString()));
            soulList.Add(soulData[i]["Name"].ToString());
        }
        /*if (!File.Exists(soulDataPath))
            return;
        string text = File.ReadAllText(soulDataPath);
        string[] lines = text.Substring(0,text.Length - 1).Split("\n");
        for (int i = 1; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(',');
            soulDataDic.Add(row[0], new SoulData(row[0], row[1], row[2], row[3], row[4], row[5]));
        }*/

    }
}
