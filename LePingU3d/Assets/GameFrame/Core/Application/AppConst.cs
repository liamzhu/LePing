using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AppConst
{
    public static readonly string ImagePath = Application.persistentDataPath + "/icon.png";
    public const int GameFrameRate = 60; //游戏帧频
    public const bool IsNetConnect = true; //是否连接网络
    public static bool IsAsyncUI = false; //是否异步加载UI

    public static UILayerConst.UIDisplayMode UIDisplayMode = UILayerConst.UIDisplayMode.UIActive;

    public static bool OpenAutoUpdate = false; //安卓整包更新开关
    public const string ApkUpdateWebsite = "http://112.74.168.1:8088/share/lp/index.html";

    public const string AppName = "屋里宁麻将";
    public const string AppVersion = "1.0.0";
    public const int BundleVersionCode = 1;

    public const string ClientPasswordKey = "j6=9=1ac";

    public const string DevelopingIP = "192.168.55.108";
    public const int DevelopingPort = 9001;
    public const string QAIP = "192.168.55.108";
    public const int QAPort = 9001;
    public const string QAIP1 = "127.0.0.1";
    public const int QAPort1 = 9001;
	public const string ReleaseIP = "123.206.186.238";
    public const int ReleasePort = 9001;

    #region 游戏分辨率

    public const int AppContentWidth = 1280;
    public const int AppContentHeight = 720;

    #endregion 游戏分辨率

    public static bool IsNetAvailable
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    public static bool IsWifi
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }

    public static void CheckNetworkState()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UIDialogMgr.Instance.ShowDialog(10020);
            return;
        }
    }

    public static bool Is64Bit
    {
        get
        {
            if (IntPtr.Size == 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static bool IsAndroid
    {
        get
        {
#if UNITY_ANDROID
            return true;
#else
            return false;
#endif
        }
    }

    public static bool IsIOS
    {
        get
        {
#if UNITY_IPHONE || UNITY_IOS
            return true;
#else
            return false;
#endif
        }
    }
}
