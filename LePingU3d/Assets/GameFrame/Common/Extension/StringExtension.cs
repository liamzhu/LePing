using UnityEngine;
using System.Collections;
using System;

public static class StringExtension
{
    public static string getMd5(this string value)
    {
        return MD5Util.getStringMd5(value);
    }

    public static int ToInt(this string value)
    {
        return Int32.Parse(value);
    }

    public static int ToInt(this string str, int defaultValue)
    {
        int result = defaultValue;
        return int.TryParse(str, out result) ? result : defaultValue;
    }

    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static bool IsNullOrWhiteSpace(this string value)
    {
        if (value != null)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
