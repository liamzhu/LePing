/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UITteswindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2017-03-24
 *Description:   
 *History:  
*********************************************************/


using UnityEngine;
using System.Collections;

public class UITteswindow : UIBasePanel {

	public UITteswindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UITteswindow"))
    {
    }

    protected override void OnAwakeInitUI()
    {
	
	}

    public override void OnRefresh()
    {

    }
}
