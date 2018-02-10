/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIRechargeTipWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-01
 *Description:
 *History:
*********************************************************/

using System.Collections;
using UnityEngine;

public class UIRechargeTipWindow : UIBasePanel
{

    public UIRechargeTipWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIRechargeTipWindow"))
    {
    }

    private UILabel mDes;

    protected override void OnAwakeInitUI()
    {
        mDes = CacheTrans.FindComponent<UILabel>("Root/Des");
        CacheTrans.GetUIEventListener("Root/BtnConfirm").onClick += OnReturnClick;
    }

    public override void OnRefresh()
    {
        mDes.text = "钻石购买:联系代理或群主\n\n申请代理加微信: lepingmj";
    }
}
