/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIShareWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-01
 *Description:
 *History:
*********************************************************/

using cn.sharesdk.unity3d;
using System;
using System.Collections;
using UnityEngine;

public class UIShareWindow : UIBasePanel
{

    public UIShareWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIShareWindow"))
    {
    }

    private UIMainModel mUIMainModel;

    protected override void OnAwakeInitUI()
    {
        mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        CacheTrans.GetUIEventListener("Root/BtnClose").onClick += OnReturnClick;
        CacheTrans.GetUIEventListener("Root/BtnFriend").onClick += OnFriendClick;
        CacheTrans.GetUIEventListener("Root/BtnCircle").onClick += OnCircleClick;
        ApplicationMgr.Instance.ShareSDK.shareHandler = OnShareResultHandler;

    }

    private void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        //		if (state == ResponseState.Success)
        //		{
        //			text.text= ("share result :");
        //			text.text= (MiniJSON.jsonEncode(result));
        //		}
        //		else if (state == ResponseState.Fail)
        //		{
        //			text.text =("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
        //		}
        //		else if (state == ResponseState.Cancel)
        //		{
        //			text.text= ("cancel !");
        //		}
    }

    private void OnCircleClick(GameObject go)
    {
        ShareInfo mShareInfo = mUIMainModel.GetShareInfo(ShareType.Circles);
        if (mShareInfo != null)
        {
            ShareContent content = new ShareContent();
            content.SetImagePath(AppConst.ImagePath);
            content.SetTitle(mShareInfo.Title);
            content.SetText(mShareInfo.Content);
            content.SetUrl(mShareInfo.Url);
            content.SetShareType(ContentType.Webpage);
            ApplicationMgr.Instance.ShareSDK.ShowShareContentEditor(PlatformType.WeChat, content);
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog("没有找到分享的相关配置");
        }
    }

    private void OnFriendClick(GameObject go)
    {
        ShareInfo mShareInfo = mUIMainModel.GetShareInfo(ShareType.Friend);
        if (mShareInfo != null)
        {
            ShareContent content = new ShareContent();
            content.SetImagePath(AppConst.ImagePath);
            content.SetTitle(mShareInfo.Title);
            content.SetText(mShareInfo.Content);
            content.SetUrl(mShareInfo.Url);
            content.SetShareType(ContentType.Webpage);
            ApplicationMgr.Instance.ShareSDK.ShareContent(PlatformType.WeChatMoments, content);
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog("没有找到分享的相关配置");
        }
    }

    public override void OnRefresh()
    {

    }
}
