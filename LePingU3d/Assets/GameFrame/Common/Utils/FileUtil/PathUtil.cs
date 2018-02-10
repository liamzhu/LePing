using UnityEngine;
using System.Collections;

public class PathUtil
{
    public static string StreamingAssetsPath
    {
        get
        {
#if UNITY_ANDROID
            return "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
            return Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
            return "file://" + Application.dataPath + "/StreamingAssets/";
#else
            string.Empty;
#endif
        }
    }
}
