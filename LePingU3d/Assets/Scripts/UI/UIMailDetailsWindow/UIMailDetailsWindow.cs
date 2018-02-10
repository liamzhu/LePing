/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIMsgDetailsWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-01
 *Description:
 *History:
*********************************************************/

using System.Collections;
using UnityEngine;

public class UIMailDetailsWindow : UIBasePanel
{

    public UIMailDetailsWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIMailDetailsWindow"))
    {
    }

    private UILabel mLabelDes;

    protected override void OnAwakeInitUI()
    {
        mLabelDes = CacheTrans.FindComponent<UILabel>("Root/ContainerCenter/Des");
        CacheTrans.GetUIEventListener("Root/TitleGroup/BtnClose").onClick += OnReturnClick;
    }

    public override void OnRefresh()
    {
        EmailInfo mEmail = mData as EmailInfo;
        mLabelDes.text = mEmail.Content;
    }
}
