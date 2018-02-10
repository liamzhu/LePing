/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIMessageCenterWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-01
 *Description:
 *History:
*********************************************************/

using System;
using System.Collections;
using UnityEngine;

public class UIMailBoxWindow : UIBasePanel
{

    public UIMailBoxWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.HideOther, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIMailBoxWindow"))
    {
    }

    private UIMainModel mUIMainModel;
    private UIWrapContent mUIWrapContent;
    private ScrollGrid mScrollGrid;
    private EmailResp mEmailInfo;

    protected override void OnAwakeInitUI()
    {
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
            mUIMainModel.ValueUpdateEvent += OnValueUpdateEvent;
        }
        mUIWrapContent = CacheTrans.FindComponent<UIWrapContent>("Root/MessageGroup/ScrollView/Grid");
        mScrollGrid = CacheTrans.FindComponent<ScrollGrid>("Root/MessageGroup/ScrollView/Grid");
        CacheTrans.GetUIEventListener("Root/TitleGroup/BtnReturn").onClick += OnReturnClick;
        mUIWrapContent.onInitializeItem = onInitializeItem;
    }

    public override void OnRefresh()
    {
        RefreshEmailInfo(mUIMainModel.EmailInfo);
    }

    private void OnValueUpdateEvent(object sender, ValueChangeArgs e)
    {
        switch (e.key)
        {
            case UIMainModelConst.KEY_EmailInfo:
                RefreshEmailInfo((EmailResp)e.newValue);
                break;
            default:
                break;
        }
    }

    private void RefreshEmailInfo(EmailResp emails)
    {
        this.mEmailInfo = emails;
        if (mEmailInfo != null)
        {
            mScrollGrid.SetGrid(mEmailInfo.Emails.Count, setItem);
        }
    }

    private void onInitializeItem(GameObject go, int wrapIndex, int realIndex)
    {
        int index = Mathf.Abs(realIndex);
        Debug.Log(index);
        go.transform.GetComponent<EmailItem>().Refresh(mEmailInfo.Emails[index]);
    }

    private void setItem(Transform[] t, int start, int end)
    {
        for (int i = 0; i < t.Length; i++)
        {
            t[i].GetComponent<EmailItem>().Refresh(mEmailInfo.Emails[i]);
        }
    }
}
