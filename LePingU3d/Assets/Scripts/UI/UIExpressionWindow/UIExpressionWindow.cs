/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIExpressionWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.5.2
 *Date: 2017-04-01
 *Description:
 *History:
*********************************************************/

using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class UIExpressionWindow : UIBasePanel
{

    public UIExpressionWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIExpressionWindow"))
    {
    }

    private UIMainModel mUIMainModel;

    protected override void OnAwakeInitUI()
    {
        mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        CacheTrans.GetUIEventListener("Root/BtnClose").onClick += OnCloseClick;
        ExpressionItem[] expressionItems = CacheTrans.GetComponentsInChildren<ExpressionItem>();
        foreach(var item in expressionItems)
        {
            item.OnCloseClick += OnCloseClick;
        }
    }

    private void OnCloseClick(GameObject go)
    {
        UIWindowMgr.Instance.PopPanel<UIExpressionWindow>();
    }

    private void OnCloseClick()
    {
        UIWindowMgr.Instance.PopPanel<UIExpressionWindow>();
    }

    public override void OnRefresh()
    {

    }
}
