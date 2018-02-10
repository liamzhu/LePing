using System.Collections;
using UnityEngine;

public class PlatformPluginMgr : Singleton<PlatformPluginMgr>
{
    public override void OnInit()
    {

        base.OnInit();
    }

    public float GetBatteryPct()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (var androidPlugin = new AndroidJavaObject("com.henry.AndroidPlugin.BatteryHelper", currentActivity))
                    {
                        return androidPlugin.Call<float>("GetBatteryPct");
                    }
                }
            }
        }
        return 1f;
    }

    public string GetNetType()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (var androidPlugin = new AndroidJavaObject("com.henry.AndroidPlugin.NetWorkHelper", currentActivity))
                    {
                        return androidPlugin.Call<string>("getWifiInfo");
                    }
                }
            }
        }
        return "7";
    }

    public void showDialog()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (var androidPlugin = new AndroidJavaObject("com.henry.AndroidPlugin.NetWorkHelper", currentActivity))
                    {
                        androidPlugin.Call("checkNetState");
                    }
                }
            }
        }
    }
}
