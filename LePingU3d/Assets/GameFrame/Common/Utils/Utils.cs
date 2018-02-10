using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Text;

public class Utils
{
    /// <summary>
    /// 0-60分钟内，显示xx分钟之前；其中0-2分钟显示1分钟之前，2-3分钟显示2分钟之前，59-60分钟显示59分钟之前。以此类推。
    /// 1-24小时内，显示xx小时之前；
    /// 1-2天内，显示1天之前；
    /// 2天以前，显示具体日期。
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    //public static string ShowTime(int serlverTimestamp, int time)
    //{
    //    int deta = serlverTimestamp - time;
    //    int minute = (deta / 60) % 60 + 1;
    //    int hour = deta / (60 * 60);
    //    int day = deta / (60 * 60 * 24);
    //    string result = "";
    //    if (day == 0)
    //        result = hour == 0 ? string.Format("{0}分钟前", minute) : string.Format("{0}小时前", hour);
    //    else if (day <= 2)
    //        result = string.Format("{0}天前", day);
    //    else
    //        result = GetTime(time.ToString()).ToString("yyyy-MM-dd HH:mm:ss");

    //    return result;
    //}

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string MD5File(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string MD5String(string str)
    {
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        string ret = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str)), 4, 8);
        return ret.Replace("-", "");
    }

    /// <summary>
    /// 是否为数字
    /// </summary>
    public static bool IsNumber(string strNumber)
    {
        Regex regex = new Regex("[^0-9]");
        return !regex.IsMatch(strNumber);
    }

    public static bool IsNetReachable
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    public static bool IsWIFIMode
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }
}
