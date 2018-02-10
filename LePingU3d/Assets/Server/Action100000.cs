
/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: Action100000.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2017-02-08
 *Description:   
 *History:  
*********************************************************/


using System;
using System.Collections.Generic;
using GameRanking.Pack;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action100000 : GameAction
{
    private ActionResult result = new ActionResult();

    public Action100000() : base((int)100000)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {

    }

    public override ActionResult GetResponseData()
    {
        return result;
    }
}