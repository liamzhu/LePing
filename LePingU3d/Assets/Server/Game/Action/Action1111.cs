﻿/********************************************************
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

public class Action1111 : GameAction
{

    public Action1111() : base((int)ActionType.StartGame)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {
        GameLogicMgr.Instance.StartGame();
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult();
    }
}
