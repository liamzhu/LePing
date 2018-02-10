// ******************************************
// 文件名(File Name):             ResMgr.cs
// 创建时间(CreateTime):        20160320
// ******************************************
using System;
using System.Collections;
using UnityEngine;

public class ResMgr : Singleton<ResMgr>
{
    private Hashtable cacheTable;

    /// <summary>
    /// 释放未使用的Asset占用的内存
    /// </summary>
    public void ReleaseUnusedAsset()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public string LoadConfig(string assetPath, bool cache = false)
    {
        TextAsset asset = LoadRes<TextAsset>(ResUtility.CONFIG_PATH + assetPath + ResUtility.XML, cache);
        return asset == null ? "" : asset.text;
    }

    public string LoadJson(string assetPath, bool cache = false)
    {
        TextAsset asset = LoadRes<TextAsset>(ResUtility.JSON_PATH + assetPath, cache);
        return asset == null ? "" : asset.text;
    }

    public TextAsset LoadTextAsset(string assetPath, bool cache = false)
    {
        return LoadRes<TextAsset>(ResUtility.JSON_PATH + assetPath, cache);
    }

    public Texture LoadTexture(string assetPath, bool cache = false)
    {
        return LoadRes<Texture>(ResUtility.TEXTURE_PATH + assetPath + ResUtility.PNG, cache);
    }

    public GameObject LoadUIPrefab(string assetPath, bool cache = false)
    {
        return GameObject.Instantiate(LoadRes<GameObject>(assetPath, cache));
        //return GameObject.Instantiate(LoadRes<GameObject>(ResUtility.UI_PATH + assetPath + ResUtility.PREFAB, cache));
    }

    public bool LoadUIPrefabAsync(string assetPath, Action<float, bool, GameObject> onProcess, bool cache = false)
    {
        return LoadResAsync<GameObject>(assetPath, cache, onProcess);
    }

    public bool LoadResAsync<T>(string assetPath, bool cache, Action<float, bool, T> onProcess) where T : UnityEngine.Object
    {
        if (cacheTable == null) { cacheTable = new Hashtable(); }
        if (string.IsNullOrEmpty(assetPath))
            return false;
        if (cacheTable.Contains(assetPath))
        {
            if (onProcess != null)
                onProcess(1f, true, cacheTable[assetPath] as T);
            return true;
        }
        ResourceRequest request = Resources.LoadAsync(assetPath, typeof(T));
        if (request == null)
            return false;
        if (request.isDone)
        {
            T orgObj = request.asset as T;
            if (orgObj == null)
            {
                return false;
            }
            if (cache && orgObj != null)
            {
                cacheTable.Add(assetPath, orgObj);
            }
            if (onProcess != null)
                onProcess(request.progress, request.isDone, orgObj);
            return true;
        }
        return false;
    }

    public UIAtlas LoadUIAtlas(string assetPath, bool cache = false)
    {
        return LoadRes<UIAtlas>(assetPath, cache);
        //return LoadRes<UIAtlas>(ResUtility.UIATLAS_PATH + assetPath + ResUtility.PREFAB, cache);
    }

    /// <summary>
    /// 根据资源路径和类型加载资源
    /// </summary>
    public T LoadRes<T>(string assetPath, bool cache) where T : UnityEngine.Object
    {
        if (cacheTable == null) { cacheTable = new Hashtable(); }

        if (cacheTable.Contains(assetPath))
        {
            return cacheTable[assetPath] as T;
        }
        T ret = Resources.Load<T>(assetPath);
        //#if UNITY_EDITOR
        //        ret = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
        //#else
        //		//ret = m_AssetBundleMemMgr.GetAssetObjectAtPath<T>(assetPath);
        //#endif
        if (ret == null)
        {
            Debug.LogError("Load Failed " + assetPath);
        }
        if (cache && ret != null)
        {
            cacheTable.Add(assetPath, ret);
        }
        return ret;
    }
}
