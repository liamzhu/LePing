// ******************************************
// 文件名(File Name):             Singleton.cs
// 创建时间(CreateTime):        20160320
// ******************************************
using System;
using System.Reflection;

public class Singleton<T> where T : class, new()
{
    private static volatile T mInstance;
    private static readonly object mLock = new object();

    protected Singleton()
    {
    }

    //~Singleton()
    //{
    //    DestoryInstance();
    //}

    public virtual void OnInit()
    {
        //ApplicationMgr.Instance.onDestroy += OnDestory;
    }

    public virtual void OnUnInit()
    {
    }

    private void OnDestory()
    {
        //ApplicationMgr.Instance.onDestroy -= OnDestory;
        DestoryInstance();
    }

    public static T Instance
    {
        get
        {
            return Singleton<T>.CreateInstance();
        }
    }

    public static bool HasInstance()
    {
        return (Singleton<T>.mInstance != null);
    }

    private static T CreateInstance()
    {
        if (Singleton<T>.mInstance == null)
        {
            lock (mLock)
            {
                if (Singleton<T>.mInstance == null)
                {
                    Singleton<T>.mInstance = Activator.CreateInstance<T>();
                    (Singleton<T>.mInstance as Singleton<T>).OnInit();
                }
            }
        }
        return Singleton<T>.mInstance;
    }

    private static void DestoryInstance()
    {
        if (Singleton<T>.mInstance != null)
        {
            DebugHelper.LogInfo("OnDestroy Singleton '" + typeof(T).FullName + ",");
            (Singleton<T>.mInstance as Singleton<T>).OnUnInit();
            Singleton<T>.mInstance = null;
        }
    }

}
