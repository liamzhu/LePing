/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: Action1108.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-12-09
 *Description:
 *History:
*********************************************************/

using GameRanking.Pack;
using System;
using System.Collections.Generic;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1208 : GameAction
{
	private GameRecordsResp responsePack;

    public Action1208() : base((int)1208)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {
		Debug.Log ("1208*********************************           ");
		
		responsePack = JsonUtil.DeserializeObject<GameRecordsResp>(reader.readString());
        if (responsePack != null)
        {
            //GameLogicMgr.Instance.OperateGameEnd(responsePack);
			Debug.Log ("1208*********************************           "+responsePack.GameRecordDatas.Count);
        }
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
