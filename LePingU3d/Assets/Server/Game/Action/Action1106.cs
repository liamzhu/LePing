using GameRanking.Pack;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1106 : GameAction
{
    private Response1106Packet responsePack;

    public Action1106() : base((int)1106)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new Response1106Packet()
        {
            Result = reader.getInt()
        };
        if (responsePack.Success)
        {
            responsePack.RoomInfo = UIModelMgr.Instance.GetModel<UIGameModel>().RoomInfo = JsonUtil.DeserializeObject<RoomInfo>(reader.readString());
        }
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
