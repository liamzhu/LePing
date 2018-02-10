using System;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1009 : GameAction
{
    private ResponseDefaultPacket responsePack;

    public Action1009()
        : base(1009)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeString("AgentID", actionParam.Get<string>("AgentID"));
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
