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

public class Action1205 : GameAction
{
    private Response1205Packet responsePack;

    public Action1205() : base((int)1205)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new Response1205Packet()
        {
            Result = reader.getInt()
        };
        if (responsePack.Success)
        {
            responsePack.GameData = JsonUtil.DeserializeObject<ReConnectDataResp>(reader.readString());
            GameLogicMgr.Instance.OperateReconnectGameData(responsePack.GameData);
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog(responsePack.Result);
        }
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
