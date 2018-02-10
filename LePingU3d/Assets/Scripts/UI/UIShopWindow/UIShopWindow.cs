/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIShopWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-01
 *Description:
 *History:
*********************************************************/

using System.Collections;
using UnityEngine;

public class UIShopWindow : UIBasePanel
{

    public UIShopWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.HideOther, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIShopWindow"))
    {
    }

    private UILabel mDiamondLabel;
    private UILabel mIntegralLabel;
    private UIMainModel mUIMainModel;

    protected override void OnAwakeInitUI()
    {
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
            mUIMainModel.ValueUpdateEvent += OnValueUpdateEvent;
        }
        mDiamondLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerBottom/DiamondGroup/Label");
        mIntegralLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerBottom/IntegralGroup/Label");
        CacheTrans.GetUIEventListener("Root/ContainerTop/BtnReturn").onClick += OnReturnClick;
    }

    public override void OnRefresh()
    {
        SetUserData();
    }

    private void SetUserData()
    {
        if (mUIMainModel.PlayerInfo != null)
        {
            OnUserInfo(mUIMainModel.PlayerInfo);
        }
    }

    private void OnValueUpdateEvent(object sender, ValueChangeArgs e)
    {
        switch (e.key)
        {
            case UIMainModelConst.KEY_UserInfo:
                OnUserInfo((UserInfo)e.newValue);
                break;
            default:
                break;
        }
    }

    private void OnUserInfo(UserInfo mUserInfo)
    {
        mDiamondLabel.text = mUserInfo.Ingot.ToString();
        mIntegralLabel.text = mUserInfo.Gold.ToString();
    }
}
