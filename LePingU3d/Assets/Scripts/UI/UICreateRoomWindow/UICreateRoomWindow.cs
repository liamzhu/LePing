/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UICreateRoomWindow.cs
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

public class UICreateRoomWindow : UIBasePanel
{

    public UICreateRoomWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.HideOther, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UICreateRoomWindow"))
    {
    }

    private CToggleGroup mPlayerCountGroup;
    private CToggleGroup mSeqCountGroup;
    private CToggleGroup mEndPointsGroup;
    private CToggleGroup mPlayOptionGroup;

    protected override void OnAwakeInitUI()
    {
        mPlayerCountGroup = CacheTrans.FindComponent<CToggleGroup>("Root/ContainerCenter/ContainerLeft/BtnOption1/Scroll View/OptionGroups/PlayerCountGroup");
        mSeqCountGroup = CacheTrans.FindComponent<CToggleGroup>("Root/ContainerCenter/ContainerLeft/BtnOption1/Scroll View/OptionGroups/SeqCountGroup");
        mEndPointsGroup = CacheTrans.FindComponent<CToggleGroup>("Root/ContainerCenter/ContainerLeft/BtnOption1/Scroll View/OptionGroups/EndPointsGroup");
        mPlayOptionGroup = CacheTrans.FindComponent<CToggleGroup>("Root/ContainerCenter/ContainerLeft/BtnOption1/Scroll View/OptionGroups/PlayOptionGroup");

        CacheTrans.GetUIEventListener("Root/ContainerTop/BtnClose").onClick += OnReturnClick;
        CacheTrans.GetUIEventListener("Root/ContainerBottom/BtnCreateRoom").onClick += OnCreateRoomClick;
    }

    private void OnCreateRoomClick(GameObject go)
    {
        DebugHelper.LogInfo("点击创建房间按钮");

        //值为选择时的序号、0表示默认

        RoomSetting requestPack = new RoomSetting()
        {
            Game = GameType.LPGJ,
            PlayerNo = 4 - mPlayerCountGroup.getIndex(),
            SeqCount = mSeqCountGroup.getIndex(),
        };
        requestPack.Others = new System.Collections.Generic.Dictionary<string, int>();
        requestPack.Others.Add(RoomSettingConst.LPGJ_DianPaoHu, mPlayOptionGroup.getIndex());
        //requestPack.Others.Add(RoomSettingConst.LPGJ_ScorePower, mEndPointsGroup.getIndex());
        requestPack.Others.Add(RoomSettingConst.LPGJ_ScorePower, 0);

        Debug.Log("PlayerNo :" + requestPack.PlayerNo.ToString());
        Net.Instance.Send((int)ActionType.CreatRoom, OnCreatRoomCallback, new ActionParam(requestPack));
    }

    private void OnCreatRoomCallback(ActionResult actionResult)
    {
        Response1101Packet responsePack = actionResult.GetValue<Response1101Packet>();
        //创建成功、跳转到加入房间
        if (responsePack != null && responsePack.Result == MyActionResult.Success)
        {
            UIModelMgr.Instance.GetModel<UIMainModel>().PlayerInfo.RoomNumber = responsePack.RoomNo;
            ActionParam actionParam = new ActionParam();
            actionParam["RoomNo"] = responsePack.RoomNo;
            Net.Instance.Send((int)ActionType.JoinRoom, null, actionParam);
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog(responsePack.Result);
        }
    }

    public override void OnRefresh()
    {

    }
}
