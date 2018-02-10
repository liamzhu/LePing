using System;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1008 : GameAction
{
    private Response1008Packet responsePack;

    public Action1008()
        : base(1008)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {
        UIModelMgr.Instance.GetModel<UIMainModel>().PlayerInfo = JsonUtil.DeserializeObject<UserInfo>(reader.readString());
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
