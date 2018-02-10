/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UILoadingWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-11-28
 *Description:
 *History:
*********************************************************/

using System;
using System.Collections;
using UnityEngine;

public class UILoadingWindow : UIBasePanel
{

    public UILoadingWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UILoadingWindow"))
    {
    }

    protected override void OnAwakeInitUI()
    {

    }

    protected override void RegisterEvent()
    {
        EventMgr.Instance.LoginMgr.AddObserver(EventConst.EventPreloading, onPreloadingCallBack);
        base.RegisterEvent();
    }

    private void onPreloadingCallBack()
    {
        GameMgr.Instance.EnterToMainWindow();
    }

    protected override void RemoveEvent()
    {
        EventMgr.Instance.LoginMgr.RemoveObserver(EventConst.EventPreloading, onPreloadingCallBack);
        base.RemoveEvent();
    }

    public override void OnRefresh()
    {
        // TimerMgr.instance.Subscribe(1, false, TimeEventType.IngoreTimeScale).OnComplete(GameMgr.Instance.EnterToMainWindow);
    }
}
