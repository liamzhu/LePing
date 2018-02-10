using GameRanking.Pack;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1401 : GameAction
{
    private string _location;

    public Action1401()
        : base((int)1401)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeInt32("UserId", actionParam.Get<int>("UserId"));
    }

    protected override void DecodePackage(NetReader reader)
    {
        _location = reader.readString();
    }

    public override ActionResult GetResponseData()
    {
        ActionResult ar = new ActionResult();
        ar["Location"] = _location;
        return ar;
    }
}
