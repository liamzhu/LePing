using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public ResLoadType mResLoadType = ResLoadType.Resource;
    private Dictionary<string, object> dicAssetInfo = null;

    public override void OnInit()
    {
        dicAssetInfo = new Dictionary<string, object>();
        base.OnInit();
    }

    public AudioClip LoadAudioClip(string assetPath, bool cache = false)
    {
        return LoadRes<AudioClip>(ResConst.SOUND_PATH + assetPath, cache);
    }

    public string LoadConfig(string assetPath, bool cache = false)
    {
        TextAsset asset = LoadRes<TextAsset>(ResConst.CONFIG_PATH + assetPath + ResConst.XML, cache);
        return asset == null ? "" : asset.text;
    }

    public string LoadJson(string assetPath, bool cache = false)
    {
        TextAsset asset = LoadRes<TextAsset>(ResConst.JSON_PATH + assetPath, cache);
        return asset == null ? "" : asset.text;
    }

    public TextAsset LoadTextAsset(string assetPath, bool cache = false)
    {
        return LoadRes<TextAsset>(ResConst.JSON_PATH + assetPath, cache);
    }

    public Texture LoadTexture(string assetPath, bool cache = false)
    {
        return LoadRes<Texture>(ResConst.TEXTURE_PATH + assetPath + ResConst.PNG, cache);
    }

    public GameObject LoadUIPrefab(string assetPath, bool cache = false)
    {
        return GameObject.Instantiate(LoadRes<GameObject>(assetPath, cache));
        //return GameObject.Instantiate(LoadRes<GameObject>(ResUtility.UI_PATH + assetPath + ResUtility.PREFAB, cache));
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
        if (dicAssetInfo == null) { dicAssetInfo = new Dictionary<string, object>(); }

        if (string.IsNullOrEmpty(assetPath))
        {
            Debug.LogError("Error: null _path name: " + assetPath);
        }
        object obj;
        if (!dicAssetInfo.TryGetValue(assetPath, out obj))
        {
            return obj as T;
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
            dicAssetInfo.Add(assetPath, ret);
        }
        return ret;
    }

    public void ReleaseUnusedAsset()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
}

public enum ResLoadType
{
    Default,

    Resource,
    Streaming,
    Persistent,
    Catch,

    HotUpdate
}
