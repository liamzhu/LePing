/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIUserInfoWindow.cs
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

public class UIUserInfoWindow : UIBasePanel
{

    public UIUserInfoWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.HideOther, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIUserInfoWindow"))
    {
    }

    private UILabel mNameLabel;
    private UILabel mUserIdLabel;
    private UILabel mDiamondLabel;
    private UILabel mIntegralLabel;
    private UITexture mHeadIcon;
    private UISprite mSexIcon;
    private UIMainModel mUIMainModel;

    protected override void OnAwakeInitUI()
    {
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        }
        mHeadIcon = CacheTrans.FindComponent<UITexture>("Root/UserGroup/HeadIcon");
        mSexIcon = CacheTrans.FindComponent<UISprite>("Root/UserGroup/HeadIcon/SexIcon");
        mNameLabel = CacheTrans.FindComponent<UILabel>("Root/UserGroup/Name");
        mUserIdLabel = CacheTrans.FindComponent<UILabel>("Root/UserGroup/ID");
        mDiamondLabel = CacheTrans.FindComponent<UILabel>("Root/UserGroup/Diamond");
        mIntegralLabel = CacheTrans.FindComponent<UILabel>("Root/UserGroup/Integral");
        CacheTrans.GetUIEventListener("Root/TitleGroup/BtnReturn").onClick += OnReturnClick;
        CacheTrans.GetUIEventListener("Root/UserGroup/Diamond/BtnRecharge").onClick += OnRechargeClick;
        mIntegralLabel.gameObject.SetVisible(false);
    }

    private void OnRechargeClick(GameObject go)
    {
        UIWindowMgr.Instance.PushPanel<UIRechargeTipWindow>();
    }

    public override void OnRefresh()
    {
        SetUserData();
    }

    private void SetUserData()
    {
        if (mUIMainModel.PlayerInfo != null)
        {
            AsyncImageDownload.Instance.SetAsyncImage(mUIMainModel.PlayerInfo.HeadUrl, mHeadIcon);
            mSexIcon.spriteName = string.Format("sex{0}", mUIMainModel.PlayerInfo.Sex);
            mNameLabel.text = string.Format("昵称: {0}", mUIMainModel.PlayerInfo.NickName);
            mUserIdLabel.text = string.Format("ID: {0}", mUIMainModel.PlayerInfo.UserId);
            mDiamondLabel.text = string.Format("钻石: {0}", mUIMainModel.PlayerInfo.Ingot);
            //mIntegralLabel.text = string.Format("积分: {0}", mUIMainModel.PlayerInfo.Gold);
        }
    }
}
