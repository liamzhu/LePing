using GameRanking.Pack;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1105 : GameAction
{
    private ResponseDefaultPacket responsePack;

    public Action1105()
        : base((int)ActionType.RoomReady)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        //1为准备 -1取消准备
        writer.writeInt32("IsGetReady", actionParam.Get<int>("IsGetReady"));
    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new ResponseDefaultPacket()
        {
            Result = reader.getInt()
        };

        Debug.Log(string.Format("玩家准备/取消准备结果：{0}", responsePack.Result));
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
