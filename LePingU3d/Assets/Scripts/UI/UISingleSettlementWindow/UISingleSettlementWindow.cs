/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UISingleEndWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-19
 *Description:
 *History:
*********************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISingleSettlementWindow : UIBasePanel
{

    public UISingleSettlementWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UISingleSettlementWindow"))
    {
    }

    private UIGameModel mUIGameModel;
    private SingleSettlementResp mSingleSettlementResp;
    private SingleSettlementItem[] mSingleEndItems;
    private HandCardGroupItem mHandCardItem;
    private List<HandCardGroupItem> mHandCradGroupItems;
    private CardItem mCurrCardItem;
    private UISprite mGameResult;
    private UILabel mLabelHuDes;

    protected override void OnAwakeInitUI()
    {
        mUIGameModel = UIModelMgr.Instance.GetModel<UIGameModel>();
		CacheTrans.GetUIEventListener("Root/ContainerBottom/BtnReturn").onClick += OnSingleGameEndClick;
        mSingleEndItems = CacheTrans.FindChindComponents<SingleSettlementItem>("Root/ContainerCenter");
        mCurrCardItem = CacheTrans.FindComponent<CardItem>("Root/ContainerTop/CurrCardItem");
        mHandCardItem = CacheTrans.FindComponent<HandCardGroupItem>("Root/ContainerTop/HandCards");
        if (mHandCradGroupItems == null)
        {
            mHandCradGroupItems = new List<HandCardGroupItem>();
        }
        mHandCradGroupItems.Add(CacheTrans.FindComponent<HandCardGroupItem>("Root/ContainerTop/HandCardGroup/CardGroup0"));
        mHandCradGroupItems.Add(CacheTrans.FindComponent<HandCardGroupItem>("Root/ContainerTop/HandCardGroup/CardGroup1"));
        mHandCradGroupItems.Add(CacheTrans.FindComponent<HandCardGroupItem>("Root/ContainerTop/HandCardGroup/CardGroup2"));
        mHandCradGroupItems.Add(CacheTrans.FindComponent<HandCardGroupItem>("Root/ContainerTop/HandCardGroup/CardGroup3"));
        mGameResult = CacheTrans.FindComponent<UISprite>("Root/ContainerTop/GameResult");
        mLabelHuDes = CacheTrans.FindComponent<UILabel>("Root/ContainerTop/LabelHuDes");
    }

    private void OnSingleGameEndClick(GameObject go)
    {
		if (GameMgr.Instance.isFromRecord) {
			GameMgr.Instance.EnterToMainWindow();
		}
		else {
			OnReturnClick(go);
		}
    }

    public override void OnRefresh()
    {
        Array.ForEach<SingleSettlementItem>(mSingleEndItems, p => { p.SetVisible(false); });
        mSingleSettlementResp = mData as SingleSettlementResp;
        if (mSingleSettlementResp != null)
        {
            mGameResult.spriteName = mSingleSettlementResp.IsWinner.ToString();
            if (!mSingleSettlementResp.SingleSettlementInfos.IsNullOrEmpty())
            {
                for (int i = 0; i < mSingleSettlementResp.SingleSettlementInfos.Count; i++)
                {
                    mSingleEndItems[i].Refresh(mSingleSettlementResp.SingleSettlementInfos[i]);
                }
            }
        }
    }
}
