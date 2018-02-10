using System.Collections;
using UnityEngine;

public struct UIProperty
{
    public UIWindowStyle WindowStyle;//窗口类型
    public UIWindowMode WindowMode;//窗口显示模式
    public UIColliderType ColliderType;//背景点击模式
    public UIAnimationType AnimationType; //UI的显示动画类型
    public string UIPath; //UI路径

    public UIProperty(UIWindowStyle ws, UIWindowMode wm, UIColliderType wc, UIAnimationType ss, string path)
    {
        this.WindowStyle = ws;
        this.WindowMode = wm;
        this.ColliderType = wc;
        this.AnimationType = ss;
        this.UIPath = path;
    }

    public static UIProperty Init()
    {
        UIProperty property = new UIProperty();
        property.WindowStyle = UIWindowStyle.Normal;
        property.WindowMode = UIWindowMode.DoNothing;
        property.ColliderType = UIColliderType.None;
        property.AnimationType = UIAnimationType.Normal;
        property.UIPath = string.Empty;
        return property;
    }
}

/// <summary>
/// UI类型，UI的唯一标识
/// </summary>
public enum EnumUIType
{
    UILogin,    // 登录
}

public enum UIWindowMode
{
    DoNothing,
    HideOther,     // 闭其他界面
    NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
    NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
}

public enum UIColliderType
{
    None,      // 显示该界面不包含碰撞背景
    Normal,    // 碰撞透明背景
    WithBg,    // 碰撞非透明背景
}

/// <summary>
/// UI的显示动画类型
/// </summary>
public enum UIAnimationType
{
    Normal,
    CenterToBig,
    FromTop,
    FromDown,
    FromLeft,
    FromRight,
}

public enum UIWindowStyle
{
    GameUI,
    Fixed,
    Normal,
    PopUp
}

/// <summary>
/// 对象当前状态
/// </summary>
public enum EnumObjectState
{
    /// <summary>
    /// The none.
    /// </summary>
    None,

    /// <summary>
    /// The initial.
    /// </summary>
    Initial,

    /// <summary>
    /// The loading.
    /// </summary>
    Loading,

    /// <summary>
    /// The ready.
    /// </summary>
    Ready,

    /// <summary>
    /// The disabled.
    /// </summary>
    Disabled,

    /// <summary>
    /// The closing.
    /// </summary>
    Closing
}
