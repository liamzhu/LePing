/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIPlayerInfoWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-19
 *Description:
 *History:
*********************************************************/

using System;
using System.Collections;
using UnityEngine;

public class UIPlayerInfoWindow : UIBasePanel
{

    public UIPlayerInfoWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIPlayerInfoWindow"))
    {
    }

    private UILabel mLabelName;
    private UILabel mLabelId;
    private UILabel mLabelIp;
    private UserInfo mUserInfo;
    private UITexture mHeadIcon;
    private UISprite mSexIcon;
    private UILabel mLabelAddress;
    private UIMainModel mUIMainModel;
    private GameObject mPropObject;

    protected override void OnAwakeInitUI()
    {
        mHeadIcon = CacheTrans.FindComponent<UITexture>("Root/HeadIcon");
        mSexIcon = CacheTrans.FindComponent<UISprite>("Root/HeadIcon/SexIcon");
        mLabelName = CacheTrans.FindComponent<UILabel>("Root/LabelName");
        mLabelId = CacheTrans.FindComponent<UILabel>("Root/LabelId");
        mLabelIp = CacheTrans.FindComponent<UILabel>("Root/LabelIp");
        mLabelAddress = CacheTrans.FindComponent<UILabel>("Root/LabelAddress");
        CacheTrans.GetUIEventListener("MaskCollider").onClick += OnClosedClick;

        mPropObject = CacheTrans.Find("RootRight").gameObject;

        GPS._instence.OnLocate += OnLocate;
        GPS._instence.OnError += OnError;

        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        }
    }

    private void OnClosedClick(GameObject go)
    {
        UIWindowMgr.Instance.PopPanel<UIPlayerInfoWindow>();
    }

    public override void OnRefresh()
    {
        mUserInfo = mData as UserInfo;
        RefreshUserInfo();

        mLabelAddress.text = "正在尝试更新地理位置";

        //非本人才显示道具栏
        if (mUserInfo.UserId != mUIMainModel.PlayerInfo.UserId)
            mPropObject.SetActive(true);
        else
            mPropObject.SetActive(false);

        //向道具栏传递参数
        Array.ForEach<PropItem>(mPropObject.transform.GetComponentsInChildren<PropItem>(), x => { x.SetSendParam(mUIMainModel.PlayerInfo.UserId, mUserInfo.UserId); });

        if (mUserInfo.UserId == mUIMainModel.PlayerInfo.UserId)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                mLabelAddress.text = "正在获取地理位置";
                //android定位
                try
                {
                    mLabelAddress.text = TencentGPS.Instance.GetAddress();
                }
                catch (Exception e)
                {
                    mLabelAddress.text = e.Message;
                }
            }
            else
            {
                mLabelAddress.text = "正在获取地理位置";
                //ios定位
                IOSTencentMap.Instance.OnGetAddress += address => 
                {
                    IOSTencentMap.Instance.OnGetAddress = null;
                    mLabelAddress.text = address;
                };
                IOSTencentMap.Instance.StartGetAddr();
            }
        }
        else
        {
            //获取非本人定位
            ActionParam ap = new ActionParam();
            ap["UserId"] = mUserInfo.UserId;
            Net.Instance.Send((int)ActionType.GetLocation, result => 
            {
                mLabelAddress.text = (string)result["Location"];
            }, ap);
        }
    }

    private void RefreshUserInfo()
    {
        if (mUserInfo != null)
        {
            AsyncImageDownload.Instance.SetAsyncImage(mUserInfo.HeadUrl, mHeadIcon);
            mSexIcon.spriteName = string.Format("sex{0}", mUserInfo.Sex);
            mLabelName.text = mUserInfo.NickName;
            mLabelId.text = string.Format("ID: {0}", mUserInfo.UserId);
            mLabelIp.text = string.Format("IP: {0}", mUserInfo.LoginIP);
        }
    }

    private void OnError() //报错
    {
        mLabelAddress.text = "无法获取当前地理位置";
    }

    private void OnLocate(string location) //获取当前位置
    {
        mLabelAddress.text = location;
    }
}
