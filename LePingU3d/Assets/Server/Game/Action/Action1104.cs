using GameRanking.Pack;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1104 : GameAction
{
    private ResponseDefaultPacket responsePack;

    public Action1104()
        : base((int)ActionType.FireRoom)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeInt32("UserID", actionParam.Get<int>("UserID"));
    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new ResponseDefaultPacket()
        {
            Result = reader.getInt()
        };

        Debug.Log(string.Format("踢人出房间结果：{0}", responsePack.Result));
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
