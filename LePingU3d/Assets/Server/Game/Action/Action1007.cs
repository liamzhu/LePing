using System;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1007 : GameAction
{
    private Response1007Packet responsePack;

    public Action1007()
        : base((int)ActionType.Notice)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {
        UIModelMgr.Instance.GetModel<UIMainModel>().NoticeMsg = reader.readString();
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
