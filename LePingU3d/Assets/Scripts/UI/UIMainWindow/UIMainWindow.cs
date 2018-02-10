/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIMainWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-11-28
 *Description:
 *History:
*********************************************************/

using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class UIMainWindow : UIBasePanel
{
    private UIMainModel mUIMainModel;
    private UIButton mBtnReturnRoom;
    private UIButton mBtnCreateRoom;
    private UILabel mNoticeMsg;
    private Tweener mTweener;

    public UIMainWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIMainWindow"))
    {
    }

    protected override void OnAwakeInitUI()
    {
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
            mUIMainModel.ValueUpdateEvent += OnValueUpdateEvent;
        }
        mBtnReturnRoom = CacheTrans.FindComponent<UIButton>("Root/ContainerCenter/BtnReturnRoom");
        mBtnCreateRoom = CacheTrans.FindComponent<UIButton>("Root/ContainerCenter/BtnCreateRoom");
        UIEventListener.Get(mBtnReturnRoom.gameObject).onClick += ReturnRoomClick;
        UIEventListener.Get(mBtnCreateRoom.gameObject).onClick += CreateRoomClick;

        CacheTrans.GetUIEventListener("Root/ContainerCenter/BtnJoinRoom").onClick += JoinRoomClick;

        mNoticeMsg = CacheTrans.FindComponent<UILabel>("Root/ContainerCenter/Notice/ScrollView/NoticeInfo");
        mTweener = mNoticeMsg.transform.DOLocalMoveX(-1200, 15f);
        mTweener.SetAutoKill(false)
            .SetDelay(2f)
            .OnComplete(() => mTweener.Restart())
            .Pause();
    }

    public override void OnRefresh()
    {
        if (mUIMainModel.PlayerInfo != null && mUIMainModel.PlayerInfo.RoomNumber > 0)
        {
            mBtnCreateRoom.gameObject.SetVisible(false);
            mBtnReturnRoom.gameObject.SetVisible(true);
        }
        else
        {
            mBtnCreateRoom.gameObject.SetVisible(true);
            mBtnReturnRoom.gameObject.SetVisible(false);
        }
        if (!mUIMainModel.NoticeMsg.IsNullOrEmpty())
        {
            RefreshNoticeMsg(mUIMainModel.NoticeMsg);
        }
    }

    private void OnValueUpdateEvent(object sender, ValueChangeArgs e)
    {
        switch (e.key)
        {
            case UIMainModelConst.KEY_NoticeMsg:
                RefreshNoticeMsg((string)e.newValue);
                break;
            default:
                break;
        }
    }

    private void RefreshNoticeMsg(string msg)
    {
        mNoticeMsg.text = msg;
        mTweener.Restart();
    }

    private void OnCheckState(TimeEvent mTimeEvent)
    {
        if (mUIMainModel.PlayerInfo != null && mUIMainModel.PlayerInfo.RoomNumber > 0)
        {
            ActionParam actionParam = new ActionParam();
            actionParam["RoomNo"] = mUIMainModel.PlayerInfo.RoomNumber;
            Net.Instance.Send((int)ActionType.JoinRoom, null, actionParam);
        }
    }

    private void ReturnRoomClick(GameObject go)
    {
        OnCheckState(null);
    }

    private void JoinRoomClick(GameObject go)
    {
        if (mUIMainModel.PlayerInfo != null && mUIMainModel.PlayerInfo.RoomNumber > 0)
        {
            UIDialogMgr.Instance.ShowDialog(1010);
        }
        else
        {
            UIWindowMgr.Instance.PushPanel<UIJoinRoomWindow>();
        }
    }

    private void CreateRoomClick(GameObject go)
    {
        UIWindowMgr.Instance.PushPanel<UICreateRoomWindow>();
    }

}
