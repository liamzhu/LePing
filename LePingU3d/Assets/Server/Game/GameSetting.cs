using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Setting : MonoBehaviour
{
    private static Setting setting;

    static Setting()
    {
        setting = FindObjectOfType(typeof(Setting)) as Setting;
        if (setting == null)
        {
            GameObject obj2 = new GameObject("GameSetting");
            setting = obj2.AddComponent(typeof(Setting)) as Setting;
        }
        if (setting == null) return;

        setting.MobileType = 1;
        setting.DeviceID = "";
        setting.ScreenX = 0;
        setting.ScreenY = 0;
        setting.RetailID = "0000";
        setting.GameType = 1;
        setting.ServerID = 1;
        setting.ClientAppVersion = "1.0";
    }

    public static Setting Instance { get { return setting; } }

    public string Pid { get; set; }

    public const string Password = "W123J8a";

    public int MobileType { get; set; }
    public string DeviceID { get; set; }
    public string OpenID { get; set; }
    public int ScreenX { get; set; }
    public int ScreenY { get; set; }
    public string RetailID { get; set; }
    public int GameType { get; set; }
    public int ServerID { get; set; }
    public string ClientAppVersion { get; set; }

}
