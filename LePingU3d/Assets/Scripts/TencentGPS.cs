using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TencentGPS 
{
    private static TencentGPS _instance;
    public static TencentGPS Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TencentGPS();

            return _instance;
        }
    }

    AndroidJavaClass jc;
    AndroidJavaObject tcMap;


    public void InitGps()
    {
         jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
         AndroidJavaObject android_context = jc.GetStatic<AndroidJavaObject>("currentActivity");

         tcMap = new AndroidJavaObject("com.tencentmap.wulining.TCMap");
         tcMap.Call<bool>("init", android_context);
    }

    public string GetAddress()
    {
        string currentAddress = "获取定位中...";
        try
        {
            currentAddress = tcMap.Get<String>("currentAddress");
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return currentAddress;
    }
}
