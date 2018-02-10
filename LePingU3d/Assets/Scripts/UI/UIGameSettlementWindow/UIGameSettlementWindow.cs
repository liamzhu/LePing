/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIWholeEndWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-19
 *Description:
 *History:
*********************************************************/

using cn.sharesdk.unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//结算窗口
public class UIGameSettlementWindow : UIBasePanel
{

    public UIGameSettlementWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIGameSettlementWindow"))
    {
    }

    private GameSettlementResp mGameEndMessage;
    private GameSettlementItem[] mWholeEndItems;

    protected override void OnAwakeInitUI()
    {
        CacheTrans.GetUIEventListener("Root/ContainerBottom/BtnShare").onClick += OnShareClick;
        mWholeEndItems = CacheTrans.FindChindComponents<GameSettlementItem>("Root/ContainerCenter");
        CacheTrans.GetUIEventListener("Root/ContainerBottom/BtnReturn").onClick += OnWholeGameEndClick;
    }

	//图文分享
    private void OnShareClick(GameObject go)
    {
        Application.CaptureScreenshot("CaptureScreenshot.png");
        ShareContent content = new ShareContent();
        content.SetText("");
        content.SetTitle("");
        content.SetShareType(ContentType.Image);
        content.SetImagePath(Application.persistentDataPath + "/CaptureScreenshot.png");
        ApplicationMgr.Instance.ShareSDK.ShowPlatformList(null, content, 150, 150);
    }

    private void OnWholeGameEndClick(GameObject go)
    {
        GameLogicMgr.Instance.LeaveRoom();
    }

    public override void OnRefresh()
    {
        Array.ForEach<GameSettlementItem>(mWholeEndItems, p => p.SetVisible(false));
        mGameEndMessage = mData as GameSettlementResp;
        if (mGameEndMessage != null)
        {
            if (mGameEndMessage.GameSettlementInfo != null && mGameEndMessage.GameSettlementInfo.Count > 0)
            {
                for (int i = 0; i < mGameEndMessage.GameSettlementInfo.Count; i++)
                {
                    mWholeEndItems[i].Refresh(mGameEndMessage.GameSettlementInfo[i]);
                }
            }
        }
    }
}
