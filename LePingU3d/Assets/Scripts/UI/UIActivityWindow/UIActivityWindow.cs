/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIActivityWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-01
 *Description:
 *History:
*********************************************************/

using System.Collections;
using UnityEngine;

public class UIActivityWindow : UIBasePanel
{

    public UIActivityWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.HideOther, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIActivityWindow"))
    {
    }

    protected override void OnAwakeInitUI()
    {

    }

    public override void OnRefresh()
    {

    }
}
