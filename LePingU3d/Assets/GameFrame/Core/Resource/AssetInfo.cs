using System.Collections;
using UnityEngine;

public class AssetInfo
{
    public int ReferenceCount;
    public string AssetPath;
    public UnityEngine.Object AssetObject;

    public AssetInfo(string path, UnityEngine.Object obj)
    {
        this.ReferenceCount = 0;
        this.AssetPath = path;
        this.AssetObject = obj;
    }

    public void IncreaseRC()
    {
        ReferenceCount++;
    }

    public void ReduceRC()
    {
        ReferenceCount--;
    }

    public T GetObject<T>() where T : UnityEngine.Object
    {
        return AssetObject as T;
    }
}
