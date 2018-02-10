using System.Collections;
using UnityEngine;

public class WIFIState
{

    private const int WIFI_STATE_DISABLED = 1;

    private const int WIFI_STATE_ENABLED = 3;

    private const int WIFI_STATE_DISABLING = 0;

    private const int WIFI_STATE_ENABLING = 2;

    public static string getWIFIState()
    {
        string stateString = "WIFI_STATE_UNKNOWN";
#if UNITY_ANDROID
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject wifiManager = currentActivity.Call<AndroidJavaObject>("getSystemService", new AndroidJavaObject("java.lang.String", "wifi"));
        if (wifiManager != null)
        {
            int wifiState = wifiManager.Call<int>("getWifiState");
            switch (wifiState)
            {
                case WIFI_STATE_DISABLED:
                    Debug.Log("WIFI:DISABLED");
                    stateString = "WIFI_STATE_DISABLED";
                    break;
                case WIFI_STATE_DISABLING:
                    Debug.Log("WIFI:DISABLING");
                    stateString = "WIFI_STATE_DISABLING";
                    break;
                case WIFI_STATE_ENABLED:
                    Debug.Log("WIFI:ENABLED");
                    stateString = "WIFI_STATE_ENABLED";
                    break;
                case WIFI_STATE_ENABLING:
                    Debug.Log("WIFI:ENABLING");
                    stateString = "WIFI_STATE_ENABLING";
                    break;
            }
        }
#endif
        return stateString;
    }
}
