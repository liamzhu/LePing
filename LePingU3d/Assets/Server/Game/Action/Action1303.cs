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

public class Action1303 : GameAction
{
    private EmailResp responsePack;

    public Action1303() : base((int)1303)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = JsonUtil.DeserializeObject<EmailResp>(reader.readString());
        if (responsePack != null)
        {
            UIModelMgr.Instance.GetModel<UIMainModel>().EmailInfo = responsePack;
        }
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
