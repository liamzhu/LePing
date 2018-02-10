/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIDissolveRoomWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-29
 *Description:
 *History:
*********************************************************/

using System;
using System.Collections;
using UnityEngine;

public class UIDissolveRoomWindow : UIBasePanel
{

    private UILabel mDesLabel;
    private UILabel mPlayerLabel;
    private DissolveRoomResp mDissolveRoomResp;
    private UIButton mConfirmButton;
    private UIButton mCanelButton;

    public UIDissolveRoomWindow() : base(new UIProperty(UIWindowStyle.PopUp, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIDissolveRoomWindow"))
    {
    }

    protected override void OnAwakeInitUI()
    {
        mConfirmButton = CacheTrans.FindComponent<UIButton>("Root/ContainerCenter/DialogGroup/ButtonGroup/ConfirmButton");
        mCanelButton = CacheTrans.FindComponent<UIButton>("Root/ContainerCenter/DialogGroup/ButtonGroup/CanelButton");
        UIEventListener.Get(mConfirmButton.gameObject).onClick += OnConfirmClick;
        UIEventListener.Get(mCanelButton.gameObject).onClick += OnCanelClick;

        mDesLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerCenter/DialogGroup/ButtonGroup/DesLabel");
        mPlayerLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerCenter/DialogGroup/ButtonGroup/PlayerLabel");
    }

    private void OnCanelClick(GameObject go)
    {
        ActionParam actionParam = new ActionParam();
        actionParam["IsAgree"] = 2;
        Net.Instance.Send((int)ActionType.DissolveRoom, null, actionParam);
    }

    private void OnConfirmClick(GameObject go)
    {
        ActionParam actionParam = new ActionParam();
        actionParam["IsAgree"] = 1;
        Net.Instance.Send((int)ActionType.DissolveRoom, null, actionParam);
    }

    public override void OnRefresh()
    {
        mDissolveRoomResp = mData as DissolveRoomResp;
        Debug.Log(mDissolveRoomResp.IsEnd + "  " + mDissolveRoomResp.IsLeave);
        if (mDissolveRoomResp.IsEnd)
        {
            if (mDissolveRoomResp.IsLeave)
            {
                UIDialogMgr.Instance.ShowDialog(10008, null, null, mDissolveRoomResp.AgreeUser);
            }
            else
            {
                UIDialogMgr.Instance.ShowDialog(10009, null, null, mDissolveRoomResp.DisagreeUser);
            }
            UIWindowMgr.Instance.PopPanel<UIDissolveRoomWindow>();
        }
        else
        {
            NGUITools.SetActive(mConfirmButton.gameObject, !mDissolveRoomResp.IsOpe);
            NGUITools.SetActive(mCanelButton.gameObject, !mDissolveRoomResp.IsOpe);
            mDesLabel.text = string.Format("玩家[{0}]申请解散房间,是否同意?", mDissolveRoomResp.ReqName);
            mPlayerLabel.text = string.Format("玩家[{0}]已经同意", mDissolveRoomResp.AgreeUser);
        }
    }
}
