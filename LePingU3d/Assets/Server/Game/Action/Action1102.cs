using GameRanking.Pack;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1102 : GameAction
{
    private ResponseDefaultPacket responsePack;

    public Action1102()
        : base((int)1102)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeInt32("RoomNo", actionParam.Get<int>("RoomNo"));
    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new ResponseDefaultPacket()
        {
            Result = reader.getInt()
        };
        if (responsePack.Result == MyActionResult.Success || responsePack.Result == MyActionResult.AlreadyInRoom)
        {
            GameMgr.Instance.EnterToGameWindow();
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog(responsePack.Result);
        }
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
