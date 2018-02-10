using GameRanking.Pack;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1103 : GameAction
{
    private ResponseDefaultPacket responsePack;

    public Action1103()
        : base((int)1103)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new ResponseDefaultPacket()
        {
            Result = reader.getInt()
        };
        if (responsePack.Result == MyActionResult.Success || responsePack.Result == MyActionResult.NotInRoom)
        {
            GameLogicMgr.Instance.LeaveRoom();
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog(responsePack.Result);
        }
        Debug.Log(string.Format("离开房间结果：{0}", responsePack.Result));
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
