/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIChatWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2017-01-06
 *Description:
 *History:
*********************************************************/

using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class UIChatWindow : UIBasePanel
{

    public UIChatWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIChatWindow"))
    {
    }

    private UIMainModel mUIMainModel;
    private UIInput mChatInput;
    private UIButton[] mSimpleChatItems;

    protected override void OnAwakeInitUI()
    {
        mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();

        mChatInput = CacheTrans.FindComponent<UIInput>("Root/ContainerBottom/ChatInput");
        CacheTrans.GetUIEventListener("Root/BtnClose").onClick += OnCloseClick;
        CacheTrans.GetUIEventListener("Root/ContainerBottom/BtnSend").onClick += OnSendInfoClick;

        mSimpleChatItems = CacheTrans.FindChindComponents<UIButton>("Root/ContainerCenter");
        Array.ForEach<UIButton>(mSimpleChatItems, p => { UIEventListener.Get(p.gameObject).onClick += OnSendCommonInfoClick; });
    }

    private void OnCloseClick(GameObject go)
    {
        UIWindowMgr.Instance.PopPanel<UIChatWindow>();
    }

    private void OnSendCommonInfoClick(GameObject go)
    {
        MsgInfo info = new MsgInfo();
        info.MsgContent = go.GetComponent<UILabel>().text;
        info.MsgIndex = int.Parse(go.name.Replace("ChatInfo", ""));
        RoomMsgInfo msg = new RoomMsgInfo()
        {
            UID = mUIMainModel.PlayerInfo.UserId,
            MsgType = RoomMsgType.SimpleText,
            Content = Encoding.UTF8.GetBytes(JsonUtil.SerializeObject(info))
        };
        ActionParam actionParam = new ActionParam(msg);
        Net.Instance.Send((int)ActionType.RoomChatReq, null, actionParam);
        OnCloseClick(null);
    }

    private void OnSendInfoClick(GameObject go)
    {
        if (string.IsNullOrEmpty(mChatInput.value))
        {
            UIDialogMgr.Instance.ShowDialog("聊天信息不能为空");
            return;
        }
        RoomMsgInfo msg = new RoomMsgInfo()
        {
            UID = mUIMainModel.PlayerInfo.UserId,
            MsgType = RoomMsgType.Text,
            Content = Encoding.UTF8.GetBytes(mChatInput.value)
        };
        ActionParam actionParam = new ActionParam(msg);
        Net.Instance.Send((int)ActionType.RoomChatReq, null, actionParam);
        OnCloseClick(null);
    }

    public override void OnRefresh()
    {
        mChatInput.value = string.Empty;
    }
}
