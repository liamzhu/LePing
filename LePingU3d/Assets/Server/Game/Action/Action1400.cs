using GameRanking.Pack;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1400 : GameAction
{
    private ResponseDefaultPacket responsePack;

    public Action1400()
        : base((int)1400)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeInt32("UserId", actionParam.Get<int>("UserId"));
        writer.writeString("Location", actionParam.Get<string>("Location"));
    }

    protected override void DecodePackage(NetReader reader)
    {
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
