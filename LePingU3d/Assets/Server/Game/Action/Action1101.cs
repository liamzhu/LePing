using GameRanking.Pack;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1101 : GameAction
{
    private Response1101Packet responsePack;

    public Action1101()
        : base(1101)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        if (actionParam.HasValue)
        {
            RoomSetting requestPack = actionParam.GetValue<RoomSetting>();
            writer.writeString("RoomSetting", JsonUtil.SerializeObject(requestPack));
        }
    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new Response1101Packet()
        {
            Result = reader.getInt(),
            RoomNo = reader.getInt()
        };
        Debug.Log(string.Format("创建房间{0}", responsePack.RoomNo));
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
