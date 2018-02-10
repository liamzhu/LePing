using System.Collections;
using UnityEngine;

public class SignatureVerify
{

    /// <summary>
    /// Verify the signature is correct
    /// </summary>
    /// <returns></returns>
    public static bool IsCorrect()
    {
#if UNITY_EDITOR
        return true;
#endif

        // 获取Android的PackageManager
        AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject Activity = Player.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject PackageManager = Activity.Call<AndroidJavaObject>("getPackageManager");

        // 获取当前Android应用的包名
        string packageName = Activity.Call<string>("getPackageName");

        // 调用PackageManager的getPackageInfo方法来获取签名信息数组
        int GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
        AndroidJavaObject PackageInfo = PackageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
        AndroidJavaObject[] Signatures = PackageInfo.Get<AndroidJavaObject[]>("signatures");

        // 获取当前的签名的哈希值，判断其与我们签名的哈希值是否一致
        if (Signatures != null && Signatures.Length > 0)
        {
            int hashCode = Signatures[0].Call<int>("hashCode");
            return hashCode == 8888888888;//我们签名的哈希值
        }
        return false;
    }
}
