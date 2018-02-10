using System.Collections;
using UnityEngine;
using UnityEngine.Internal;

public sealed class PlayerPrefsUtil
{
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public static int GetInt(string key, [DefaultValue("0")] int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    public static float GetFloat(string key, [DefaultValue("0.0F")] float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static string GetString(string key, [DefaultValue("\"\"")] string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    public static void SetBool(string key, bool value)
    {
        if (value)
        {
            PlayerPrefs.SetInt(key, 1);
        }
        else
        {
            PlayerPrefs.SetInt(key, 0);
        }
    }

    public static bool GetBool(string key)
    {
        int val = PlayerPrefs.GetInt(key);
        if (val == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
