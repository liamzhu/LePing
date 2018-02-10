using System.Collections;
using UnityEngine;

public class AutoUpdateMgr : Singleton<AutoUpdateMgr>
{
#if UNITY_ANDROID
    private AndroidJavaClass UnityPlayer;
    private AndroidJavaClass Intent;
    private AndroidJavaClass Uri;

    private AndroidJavaObject mCurrentActivity;
#endif

    public override void OnInit()
    {
#if UNITY_ANDROID
        UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        Intent = new AndroidJavaClass("android.content.Intent");
        Uri = new AndroidJavaClass("android.net.Uri");
#elif UNITY_IPHONE

#else
        //DebugHelper.Log("Sdk Connector Start");
#endif
    }

    public void InstallAPP(string path)
    {
        DebugHelper.LogInfo(path);
#if UNITY_ANDROID
        mCurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", Intent.GetStatic<AndroidJavaObject>("ACTION_VIEW"));
        intent.Call<AndroidJavaObject>("setDataAndType", Uri.CallStatic<AndroidJavaObject>("fromFile", new AndroidJavaObject("java.io.File", new AndroidJavaObject("java.lang.String", path))), new AndroidJavaObject("java.lang.String", "application/vnd.android.package-archive"));// "application/vnd.android.package-archive"
        mCurrentActivity.Call("startActivity", intent);
#elif UNITY_IPHONE

#else
        //DebugHelper.Log("Sdk InstallAPP");
#endif
    }

    public void downFile(string filePath)
    {
        //Application.OpenURL(downLoadURL);
#if UNITY_EDITOR
        DebugHelper.LogInfo("do nothing");
#elif UNITY_ANDROID
        AutoUpdateMgr.Instance.mCurrentActivity.Call("downFile", filePath);
#else
		//DebugHelper.Log("do nothing");
#endif
    }

    public void openFile(string filePath)
    {
        //Application.OpenURL(downLoadURL);
#if UNITY_EDITOR
        DebugHelper.LogInfo("do nothing");
#elif UNITY_ANDROID
        AutoUpdateMgr.Instance.mCurrentActivity.Call("openFile", filePath);
#else
		//DebugHelper.Log("do nothing");
#endif
    }
}
