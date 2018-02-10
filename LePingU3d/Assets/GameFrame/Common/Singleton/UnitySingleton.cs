using System.Collections;
using System.Threading;
using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour where T : Component
{
    private static volatile T mInstance;
    private static readonly object mLock = new object();
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            return UnitySingleton<T>.GetInstance();
        }
    }

    public static T GetInstance()
    {
        if (applicationIsQuitting)
        {
            DebugHelper.LogInfo("UnitySingleton Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
            return null;
        }
        lock (mLock)
        {
            if ((UnitySingleton<T>.mInstance == null) && !UnitySingleton<T>.applicationIsQuitting)
            {
                UnitySingleton<T>.mInstance = FindObjectOfType<T>();
                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    return UnitySingleton<T>.mInstance;
                }
                if (UnitySingleton<T>.mInstance == null)
                {
                    UnitySingleton<T>.mInstance = new GameObject("_" + typeof(T).Name).AddComponent<T>();
                }
            }
        }
        return UnitySingleton<T>.mInstance;
    }

    public static bool HasInstance()
    {
        return (UnitySingleton<T>.mInstance != null);
    }

    private void Awake()
    {
        DontDestroyOnLoad(base.transform.gameObject);
        this.OnInit();
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnUnInit()
    {
    }

    protected virtual void OnDestroy()
    {
        DebugHelper.LogInfo("OnDestroy UnitySingleton '" + typeof(T).FullName + "'");
    }

    protected virtual void OnApplicationQuit()
    {
        DebugHelper.LogInfo("OnApplicationQuit UnitySingleton '" + typeof(T).FullName + "'");
        this.OnUnInit();
        applicationIsQuitting = true;
        UnitySingleton<T>.mInstance = null;
    }
}
