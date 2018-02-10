/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: GameDataManager.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-01
 *Description:
 *History:
*********************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    public DataType mDataType = DataType.Json;
    private Dictionary<string, string> cfgDic = new Dictionary<string, string>();

    public void InitGameData()
    {
        DialogCfgHelper.Instance.AnalysisConfig(ConfigRule.DialogCfg);
    }

    public List<T> GetData<T>(string key) where T : class
    {
        if (!Instance.cfgDic.ContainsKey(key))
        {
            Instance.cfgDic.Add(key, Instance.LoadData(key));
        }
        if (cfgDic.ContainsKey(key))
        {
            return JsonUtil.Deserialize2List<T>(cfgDic[key]);
        }
        else
        {
            return null;
        }
    }

    private string LoadData(string cfgName)
    {
        return ResMgr.Instance.LoadJson(cfgName);
    }

    public enum DataType
    {
        Json,
        Csv,
        Lua,
        Xml
    }
}
