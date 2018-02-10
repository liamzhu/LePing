using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICfg
{
    string GetKey();
}

public abstract class BaseCfgHelper<T, U>
    where T : new()
    where U : class, ICfg, new()
{

    private static T m_Instance = default(T);

    protected Dictionary<string, U> cfgDictionary = new Dictionary<string, U>();

    protected List<U> cfgList = new List<U>();

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new T();
            }
            return m_Instance;
        }
    }

    public void AnalysisConfig(string key)
    {
        DebugHelper.LogInfo("正在加载配置表: " + key);
        if (cfgDictionary.Count > 0)
        {
            return;
        }
        cfgList = GameDataManager.Instance.GetData<U>(key);
        if (cfgList != null && cfgList.Count > 0)
        {
            foreach (U cfg in cfgList)
            {
                if (cfgDictionary.ContainsKey(cfg.GetKey()))
                {
                    cfgDictionary[cfg.GetKey()] = cfg;
                }
                else {
                    cfgDictionary.Add(cfg.GetKey(), cfg);
                }
            }
            Init();
        }
        DebugHelper.LogInfo("配置表加载完成: " + key);
    }

    protected virtual void Init()
    {
        //特殊初始化时，子类可复写这里
    }

    public U GetCfg(int key)
    {
        return GetCfg(key.ToString());
    }

    public U GetCfg(string key)
    {
        if (string.IsNullOrEmpty(key) || !cfgDictionary.ContainsKey(key))
        {
            return null;
        }
        else {
            return cfgDictionary[key];
        }
    }

    public List<U> GetAllCfg()
    {
        return cfgList;
    }

    public bool Contains(int key)
    {
        return Contains(key.ToString());
    }

    public bool Contains(string key)
    {
        return cfgDictionary.ContainsKey(key);
    }
}
