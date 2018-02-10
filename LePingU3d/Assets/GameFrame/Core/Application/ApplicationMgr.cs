using cn.sharesdk.unity3d;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class ApplicationMgr : UnitySingleton<ApplicationMgr>
{
    [SerializeField, Tooltip("设置开发模式"), Header("系统设置")]
    private AppMode mAppMode = AppMode.Developing;

    [SerializeField]
    private PackMode mPackMode = PackMode.None;

    public PackMode GamePackMode { get { return mPackMode; } }

    public ShareSDK ShareSDK { get; private set; }

    #region 游戏的IP地址和端口号

    public string SeverHost { get { return string.Format("{0}:{1}", IP, Port); } }

    public string IP
    {
        get
        {
            if (mAppMode == AppMode.Developing)
            {
                return AppConst.DevelopingIP;
            }
            else if (mAppMode == AppMode.QA)
            {
                return AppConst.QAIP;
            }
            else if (mAppMode == AppMode.QA1)
            {
                return AppConst.QAIP1;
            }
            else
            {
                return AppConst.ReleaseIP;
            }
        }
    }

    public int Port
    {
        get
        {
            if (mAppMode == AppMode.Developing)
            {
                return AppConst.DevelopingPort;
            }
            else if (mAppMode == AppMode.QA)
            {
                return AppConst.QAPort;
            }
            else if (mAppMode == AppMode.QA1)
            {
                return AppConst.QAPort1;
            }
            else
            {
                return AppConst.ReleasePort;
            }
        }
    }

    #endregion 游戏的IP地址和端口号

    public GameFlag GameFlag { get; private set; }

    #region 构造函数

    private ApplicationMgr()
    {
    }

    #endregion 构造函数

    /// <summary>
    /// 重写初始化方法 在Awake里调用
    /// </summary>
    protected override void OnInit()
    {
        AppLaunch();
        base.OnInit();

        //开始获取安卓定位
        if (Application.platform == RuntimePlatform.Android)
        {
            TencentGPS.Instance.InitGps();
        }
    }

    protected override void OnUnInit()
    {

    }

    private void Start()
    {
        InitializeGame();
    }

    /// <summary>
    /// 程序启动
    /// </summary>
    private void AppLaunch()
    {
        ShareSDK = this.GetComponent<ShareSDK>();

        //DebugMgr.EnableConsoleLog = true;
        //DebugMgr.Level = LogLevel.ALL;
        //DebugMgr.Error(Application.persistentDataPath);
        InitializeUnity();
        InitializeMode();
    }

    private void InitializeGame()
    {
        InitializeMgr();

        if (AppConst.OpenAutoUpdate)
        {
            UIWindowMgr.Instance.PushPanel<UIVersionWindow>();
        }
        GameMgr.Instance.EnterToLoginWindow();
        GameDataManager.Instance.InitGameData();
    }

    private void InitializeMgr()
    {
        GameFlag = new GameFlag();
        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
        gameObject.AddChild<AsyncImageDownload>();
        gameObject.AddChild<AudioManager>();
        gameObject.AddChild<UIWindowMgr>();
        gameObject.AddChild<TimerMgr>();
        gameObject.AddChild<UIModelMgr>();
        gameObject.AddChild<ViSpeak>();
        gameObject.AddChild<ViSpeakRecorder>();
        NetWriter.SetUrl(SeverHost);//绑定服务器端口、只绑定一次
        gameObject.AddChild<Net>();
    }

    #region 程序启动细节

    public void InitializeUnity()
    {
        //设置屏幕自动旋转， 并置支持的方向
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Application.targetFrameRate = AppConst.GameFrameRate;
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void InitializeMode()
    {
        if (mAppMode == AppMode.Developing)
        {
            FPSCounter.Instance.Initialize();
            ResourceManager.Instance.mResLoadType = ResLoadType.Resource;
        }
        else if (mAppMode == AppMode.QA)
        {
            FPSCounter.Instance.Initialize();
            ResourceManager.Instance.mResLoadType = ResLoadType.Resource;
        }
        else
        {
            ResourceManager.Instance.mResLoadType = ResLoadType.Resource;
        }
    }

    #endregion 程序启动细节

    #region 程序生命周期事件派发

    public delegate void LifeCircleCallback();

    public delegate void LifeCircleBoolCallback(bool status);

    public LifeCircleCallback onUpdate = null;
    public LifeCircleCallback onFixedUpdate = null;
    public LifeCircleCallback onLatedUpdate = null;
    public LifeCircleCallback onGUI = null;
    public LifeCircleCallback onDestroy = null;
    public LifeCircleCallback onApplicationQuit = null;
    public LifeCircleBoolCallback onApplicationPause = null;
    public LifeCircleBoolCallback onApplicationFocus = null;

    private void Update()
    {
        if (this.onUpdate != null)
            this.onUpdate();
    }

    private void FixedUpdate()
    {
        if (this.onFixedUpdate != null)
            this.onFixedUpdate();
    }

    private void LatedUpdate()
    {
        if (this.onLatedUpdate != null)
            this.onLatedUpdate();
    }

    private void OnGUI()
    {
        if (this.onGUI != null)
            this.onGUI();
    }

    protected override void OnDestroy()
    {
        if (this.onDestroy != null)
            this.onDestroy();
        base.OnDestroy();
    }

    /*
     * 强制暂停时，先 OnApplicationPause，后 OnApplicationFocus
     * 重新“启动”游戏时，先OnApplicationFocus，后 OnApplicationPause
     */

    private void OnApplicationPause(bool pauseStatus)
    {
        if (onApplicationPause != null)
        {
            onApplicationPause(pauseStatus);
        }
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        if (onApplicationFocus != null)
        {
            onApplicationFocus(focusStatus);
        }
    }

    protected override void OnApplicationQuit()
    {
        if (this.onApplicationQuit != null)
            this.onApplicationQuit();
    }

    #endregion 程序生命周期事件派发

    public enum PackMode
    {
        None = 1,
        Random = 2,
        WeChat = 3
    }

    public enum AppMode
    {
        Developing = 1,
        QA = 2,
        QA1 = 3,
        Release = 4
    }
}
