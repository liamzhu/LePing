using System.Collections;
using UnityEngine;

public class UnityPluginHelper : AndroidBehaviour<UnityPluginHelper>
{

    protected override string javaClassName
    {
        get { return "unityplugin.UnityPluginHelper"; }
    }

    public static void checkUpdateInfo()
    {
        instance.CallStatic("checkUpdateInfo");
    }

    public static string getPhoneNumber()
    {
        return instance.CallStatic<string>("getPhoneNumber");
    }

    /// <summary>
    /// 电量
    /// </summary>
    /// <returns></returns>
    public static int getBatteryLevel()
    {
        return instance.CallStatic<int>("getBatteryLevel");
    }

    /// <summary>
    /// 电池电压
    /// </summary>
    /// <returns></returns>
    public static int getBatteryVoltage()
    {
        return instance.CallStatic<int>("getBatteryVoltage");
    }

    /// <summary>
    /// 电池温度
    /// </summary>
    /// <returns></returns>
    public static double getBatteryTemperature()
    {
        return instance.CallStatic<double>("getBatteryTemperature");
    }

    /// <summary>
    /// 电池状态
    /// </summary>
    /// <returns></returns>
    public static string getBatteryStatus()
    {
        return instance.CallStatic<string>("getBatteryStatus");
    }

    /// <summary>
    /// 电池使用情况
    /// </summary>
    /// <returns></returns>
    public static string getBatteryHealth()
    {
        return instance.CallStatic<string>("getBatteryHealth");
    }
}
