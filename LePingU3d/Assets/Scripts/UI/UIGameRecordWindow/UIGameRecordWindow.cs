/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIMessageCenterWindow.cs
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

public class UIGameRecordWindow : UIBasePanel
{

	public UIGameRecordWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.HideOther, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIGameRecordWindow"))
    {
    }

    private UIMainModel mUIMainModel;
    private UIWrapContent mUIWrapContent;
    private ScrollGrid mScrollGrid;
    //private EmailResp mEmailInfo;
	private GameRecordsResp mGameRecodInfo;

    protected override void OnAwakeInitUI()
    {
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
            mUIMainModel.ValueUpdateEvent += OnValueUpdateEvent;
        }
        mUIWrapContent = CacheTrans.FindComponent<UIWrapContent>("Root/MessageGroup/ScrollView/Grid");
        mScrollGrid = CacheTrans.FindComponent<ScrollGrid>("Root/MessageGroup/ScrollView/Grid");
        CacheTrans.GetUIEventListener("Root/TitleGroup/BtnReturn").onClick += OnReturnClick;
        mUIWrapContent.onInitializeItem = onInitializeItem;

		//Net.Instance.Send((int)ActionType.GameRecord, null, null);
    }

    public override void OnRefresh()
    {
		RefreshEmailInfo(mUIMainModel.GameRecordRespInfo);
    }

    private void OnValueUpdateEvent(object sender, ValueChangeArgs e)
    {
        switch (e.key)
        {
		case UIMainModelConst.KEY_GameRecord:
				RefreshEmailInfo((GameRecordsResp)e.newValue);
                break;
            default:
                break;
        }
    }

	private void RefreshEmailInfo(GameRecordsResp records)
    {
		this.mGameRecodInfo = records;
		if (mGameRecodInfo != null)
        {
			mScrollGrid.SetGrid(mGameRecodInfo.GameRecordDatas.Count, setItem);
        }
    }

    private void onInitializeItem(GameObject go, int wrapIndex, int realIndex)
    {
        int index = Mathf.Abs(realIndex);
        Debug.Log(index);
		go.transform.GetComponent<RecordItem>().Refresh(mGameRecodInfo.GameRecordDatas[index]);
    }

    private void setItem(Transform[] t, int start, int end)
    {
        for (int i = 0; i < t.Length; i++)
        {
			t[i].GetComponent<RecordItem>().Refresh(mGameRecodInfo.GameRecordDatas[i]);
        }
    }
}
