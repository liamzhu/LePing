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

public class Action1201 : GameAction
{
    private ResponseDefaultPacket responsePack;

    public Action1201() : base((int)1201)
    {

    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        if (actionParam.HasValue)
        {
            MahJongGameAction requestPack = actionParam.GetValue<MahJongGameAction>();
            writer.writeString("GameAction", JsonUtil.SerializeObject(requestPack));
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
