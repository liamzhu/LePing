// ******************************************
// 文件名(File Name):             MonoSingleton.cs
// 创建时间(CreateTime):        20160314
// ******************************************

using System;
using System.Collections;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static volatile T mInstance;
    private static readonly object mLock = new object();
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            return MonoSingleton<T>.GetInstance();
        }
    }

    public static T GetInstance()
    {
        if (applicationIsQuitting)
        {
            DebugHelper.LogInfo("MonoSingleton Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
            return null;
        }
        lock (mLock)
        {
            if ((MonoSingleton<T>.mInstance == null) && !MonoSingleton<T>.applicationIsQuitting)
            {
                MonoSingleton<T>.mInstance = FindObjectOfType<T>();
                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    return MonoSingleton<T>.mInstance;
                }
                if (MonoSingleton<T>.mInstance == null)
                {
                    MonoSingleton<T>.mInstance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return MonoSingleton<T>.mInstance;
        }
    }

    public static bool HasInstance()
    {
        return (MonoSingleton<T>.mInstance != null);
    }

    private void Awake()
    {
        this.OnInit();
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnUnInit()
    {
    }

    private void OnDestroy()
    {
        DebugHelper.LogInfo("OnDestroy MonoSingleton '" + typeof(T).FullName + "'");
        this.OnUnInit();
        applicationIsQuitting = true;
        MonoSingleton<T>.mInstance = null;
    }
}
