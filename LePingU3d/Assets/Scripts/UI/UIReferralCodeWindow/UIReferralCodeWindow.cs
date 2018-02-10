/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIReferralCodeWindow.cs
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

public class UIReferralCodeWindow : UIBasePanel
{

    public UIReferralCodeWindow() : base(new UIProperty(UIWindowStyle.Fixed, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIReferralCodeWindow"))
    {
    }

    private UIInput mInputReferralCode;
    private Transform mNoReferralCodeState;
    private Transform mHasReferralCodeState;
    private UIMainModel mUIMainModel;

    protected override void OnAwakeInitUI()
    {
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        }
        mNoReferralCodeState = CacheTrans.FindComponent<Transform>("Root/ContainerCenter");
        mHasReferralCodeState = CacheTrans.FindComponent<Transform>("Root/Label");
        mInputReferralCode = CacheTrans.FindComponent<UIInput>("Root/ContainerCenter/InputReferralCode");

        CacheTrans.GetUIEventListener("Root/TitleGroup/BtnClose").onClick += OnReturnClick;
        CacheTrans.GetUIEventListener("Root/ContainerCenter/BtnConfirm").onClick += OnBindingClick;
    }

    public override void OnRefresh()
    {
        mInputReferralCode.value = string.Empty;
        if (mUIMainModel.PlayerInfo != null)
        {
            if (mUIMainModel.PlayerInfo.AgentID.IsNullOrEmpty())
            {
                mNoReferralCodeState.gameObject.SetActive(true);
                mHasReferralCodeState.gameObject.SetActive(false);
            }
            else
            {
                mNoReferralCodeState.gameObject.SetActive(false);
                mHasReferralCodeState.gameObject.SetActive(true);
            }
        }
    }

    private void OnBindingClick(GameObject go)
    {
        if (string.IsNullOrEmpty(mInputReferralCode.value))
        {
            UIDialogMgr.Instance.ShowDialog(10011);
            return;
        }

        ActionParam action = new ActionParam();
        action["AgentID"] = mInputReferralCode.value;
        Net.Instance.Send((int)ActionType.ReferralCode, onCallback, action);
    }

    private void onCallback(ActionResult actionResult)
    {
        ResponseDefaultPacket responsePack = actionResult.GetValue<ResponseDefaultPacket>();
        if (responsePack.Success)
        {
            UIDialogMgr.Instance.ShowDialog(10012, delegate (GameObject go) { UIWindowMgr.Instance.PopPanel(); });
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog(responsePack.Result);
        }
    }

}
