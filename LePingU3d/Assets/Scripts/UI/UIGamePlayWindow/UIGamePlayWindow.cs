/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIGamePlayWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-01
 *Description:
 *History:
*********************************************************/

using System.Collections;
using UnityEngine;

public class UIGamePlayWindow : UIBasePanel
{

    public UIGamePlayWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.HideOther, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIGamePlayWindow"))
    {
    }

    protected override void OnAwakeInitUI()
    {
        CacheTrans.GetUIEventListener("Root/TitleGroup/BtnReturn").onClick += OnReturnClick;
    }

    public override void OnRefresh()
    {

    }
}
