/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UITopBarWindow.cs
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

public class UITopBarWindow : UIBasePanel
{

    public UITopBarWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UITopBarWindow"))
    {
    }

    private UILabel mNameLabel;
    private UILabel mUserIdLabel;
    private UILabel mDiamondLabel;
    private UITexture mHeadIcon;
    private UIMainModel mUIMainModel;

    protected override void OnAwakeInitUI()
    {
		GameMgr.Instance.isFromRecord = false;
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
            mUIMainModel.ValueUpdateEvent += OnValueUpdateEvent;
        }
        mHeadIcon = CacheTrans.FindComponent<UITexture>("Root/ContainerTop/ContainerLeft/HeadGroup/HeadIcon");
        mNameLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerTop/ContainerLeft/HeadGroup/Name");
        mUserIdLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerTop/ContainerLeft/HeadGroup/ID");
        mDiamondLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerTop/ContainerLeft/DiamondsGroup/LabelDiamond");

        CacheTrans.GetUIEventListener("Root/ContainerTop/ContainerLeft/DiamondsGroup/BtnAdd").onClick += OnDiamondClick;
        CacheTrans.GetUIEventListener("Root/ContainerTop/ContainerLeft/HeadGroup/HeadIcon").onClick += OnUserClick;

        CacheTrans.GetUIEventListener("Root/ContainerTop/ContainerRight/BtnSetting").onClick += OnSettingClick;
        CacheTrans.GetUIEventListener("Root/ContainerTop/ContainerRight/BtnPlay").onClick += OnPlayClick;
        CacheTrans.GetUIEventListener("Root/ContainerTop/ContainerRight/BtnNews").onClick += OnNewsClick;
        CacheTrans.GetUIEventListener("Root/ContainerTop/ContainerRight/BtnTutorial").onClick += OnTutorialClick;

        CacheTrans.GetUIEventListener("Root/ContainerBottom/BtnReward").onClick += OnRewardClick;
        CacheTrans.GetUIEventListener("Root/ContainerBottom/BtnPlayback").onClick += OnPlaybackClick;
        CacheTrans.GetUIEventListener("Root/ContainerBottom/BtnShare").onClick += OnShareClick;
        CacheTrans.GetUIEventListener("Root/ContainerBottom/BtnActivity").onClick += OnActivityClick;
        CacheTrans.GetUIEventListener("Root/ContainerBottom/BtnShop").onClick += OnShopClick;

        Net.Instance.Send(1303, null, null);
    }

    public override void OnRefresh()
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
        AsyncImageDownload.Instance.SetAsyncImage(mUserInfo.HeadUrl, mHeadIcon);
        mNameLabel.text = mUserInfo.NickName;
        mUserIdLabel.text = string.Format("ID: {0}", mUserInfo.UserId);
        mDiamondLabel.text = mUserInfo.Ingot.ToString();
    }

    private void OnTutorialClick(GameObject go)
    {
        UIWindowMgr.Instance.PushPanel<UITutorialWindow>();
    }

    private void OnNewsClick(GameObject go)
    {
        UIWindowMgr.Instance.PushPanel<UIMailBoxWindow>();
    }

    private void OnShopClick(GameObject go)
    {
        UIDialogMgr.Instance.ShowDialog(10004);
        //UIWindowMgr.Instance.PushPanel<UIShopWindow>();
    }

    private void OnActivityClick(GameObject go)
    {
        UIDialogMgr.Instance.ShowDialog(10004);
        //UIWindowMgr.Instance.PushPanel<UIActivityWindow>();
    }

    private void OnShareClick(GameObject go)
    {
        UIWindowMgr.Instance.PushPanel<UIShareWindow>();
    }

    private void OnPlaybackClick(GameObject go)
    {
        //UIDialogMgr.Instance.ShowDialog(10022);
		Net.Instance.Send((int)ActionType.GameRecord, OnGameRecordCallBack, null);
    }

	void OnGameRecordCallBack(ActionResult actionResult){
		GameRecordsResp responsePack = actionResult.GetValue<GameRecordsResp>();
		//Debug.Log ("FFFFFFFFFFFFFFFFFFFF  "+responsePack.GameRecordDatas.Count);

		if (responsePack.GameRecordDatas.Count == 0) {
			UIDialogMgr.Instance.ShowDialog(10005);
		}
		else {
			UIModelMgr.Instance.GetModel<UIMainModel>().GameRecordRespInfo = responsePack;
			UIWindowMgr.Instance.PushPanel<UIGameRecordWindow>();
		}
	}

    private void OnRewardClick(GameObject go)
    {
        UIDialogMgr.Instance.ShowDialog(10004);
    }

    private void OnPlayClick(GameObject go)
    {
        UIWindowMgr.Instance.PushPanel<UIGamePlayWindow>();
    }

    private void OnSettingClick(GameObject go)
    {
        UIWindowMgr.Instance.PushPanel<UISettingWindow>(false);
    }

    private void OnUserClick(GameObject go)
    {
        UIWindowMgr.Instance.PushPanel<UIUserInfoWindow>();
    }

    private void OnDiamondClick(GameObject go)
    {
        UIWindowMgr.Instance.PushPanel<UIReferralCodeWindow>();
    }

}
