/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: Action1108.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-09
 *Description:
 *History:
*********************************************************/

using GameRanking.Pack;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1109 : GameAction
{
    private RoomMsgInfo responsePack;

    public Action1109() : base((int)ActionType.RoomChatResp)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = JsonUtil.DeserializeObject<RoomMsgInfo>(reader.readString());
        UIModelMgr.Instance.GetModel<UIGameModel>().RefreshRoomMsgInfo(responsePack);
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
