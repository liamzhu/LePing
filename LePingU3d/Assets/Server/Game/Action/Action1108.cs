/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: Action1107.cs
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

public class Action1108 : GameAction
{
    private ResponseDefaultPacket responsePack;

    public Action1108() : base((int)ActionType.RoomChatReq)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        if (actionParam.HasValue)
        {
            RoomMsgInfo requestPack = actionParam.GetValue<RoomMsgInfo>();
            writer.writeString("RoomMessage", JsonUtil.SerializeObject(requestPack));
        }
    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new ResponseDefaultPacket()
        {
            Result = reader.getInt()
        };
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
