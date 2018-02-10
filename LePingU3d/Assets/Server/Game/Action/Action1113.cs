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

public class Action1113 : GameAction
{
    private DissolveRoomResp responsePack;

    public Action1113() : base((int)1113)
    {

    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = JsonUtil.DeserializeObject<DissolveRoomResp>(reader.readString());
        if (responsePack != null)
        {
            UIWindowMgr.Instance.PopPanel<UISettingWindow>();
            UIWindowMgr.Instance.PushPanel<UIDissolveRoomWindow>(responsePack);
        }
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
