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

public class Action1107 : GameAction
{
    private Response1107Packet responsePack;
    private UIGameModel mUIGameModel;

    public Action1107() : base((int)1107)
    {
        if (mUIGameModel == null)
        {
            mUIGameModel = UIModelMgr.Instance.GetModel<UIGameModel>();
        }
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeInt32("UserID", actionParam.Get<int>("UserID"));
    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new Response1107Packet()
        {
            Result = reader.getInt()
        };
        if (responsePack.Success)
        {
            responsePack.UserInfos = new List<UserInfo>();
            responsePack.UserInfos = JsonUtil.Deserialize2List<UserInfo>(reader.readString());
            if (responsePack.UserInfos != null && responsePack.UserInfos.Count > 0)
            {
                mUIGameModel.AddUsers(responsePack.UserInfos);
            }

        }
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
