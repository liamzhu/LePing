/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UITutorialWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-01
 *Description:
 *History:
*********************************************************/

using System.Collections;
using UnityEngine;

public class UITutorialWindow : UIBasePanel
{

    public UITutorialWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.HideOther, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UITutorialWindow"))
    {
    }

    protected override void OnAwakeInitUI()
    {
        CacheTrans.GetUIEventListener<UIButton>("Root/ScrollView/Grid/Item4/BtnStartGame").onClick += OnReturnClick;
    }

    public override void OnRefresh()
    {

    }
}
