/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIJoinRoomWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-11-29
 *Description:
 *History:
*********************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UIJoinRoomWindow : UIBasePanel
{

    public UIJoinRoomWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.HideOther, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIJoinRoomWindow"))
    {
    }

    private UILabel mLabelNum;
    private StringBuilder mSBuilder;
    private List<string> mNumList;

    protected override void OnAwakeInitUI()
    {
        mLabelNum = CacheTrans.FindComponent<UILabel>("Root/NumGroup/LabelNum");
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnKey0").onClick += KeyClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnKey1").onClick += KeyClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnKey2").onClick += KeyClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnKey3").onClick += KeyClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnKey4").onClick += KeyClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnKey5").onClick += KeyClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnKey6").onClick += KeyClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnKey7").onClick += KeyClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnKey8").onClick += KeyClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnKey9").onClick += KeyClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnDelete").onClick += DeleteClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/KeyGroup/BtnReInput").onClick += ReInputClick;

        CacheTrans.GetUIEventListener<UIButton>("Root/NumGroup/BG/BtnClose").onClick += OnReturnClick;
    }

    private void ReInputClick(GameObject go)
    {
        if (mNumList != null)
        {
            mNumList.Clear();
            mNumList = null;
        }
        mLabelNum.text = string.Empty;
    }

    private void DeleteClick(GameObject go)
    {
        if (mNumList != null && mNumList.Count > 0)
        {
            mNumList.RemoveAt(mNumList.Count - 1);
            ResetLabel();
        }
    }

    private void KeyClick(GameObject go)
    {
        if (mNumList == null) { mNumList = new List<string>(); }
        if (mNumList.Count >= 6) { return; }
        mNumList.Add(go.name.Substring(go.name.Length - 1, 1));
        ResetLabel();
        OnSendMsg();
    }

    private void ResetLabel()
    {
        if (mNumList != null && mNumList.Count >= 0)
        {
            mLabelNum.text = string.Join("", mNumList.ToArray());
        }
    }

    private void OnSendMsg()
    {
        if (mNumList != null && mNumList.Count >= 6)
        {
            ActionParam mActionParam = new ActionParam();
            mActionParam["RoomNo"] = int.Parse(mLabelNum.text);
            Net.Instance.Send((int)ActionType.JoinRoom, null, mActionParam);
        }
    }

    public override void OnRefresh()
    {
        ReInputClick(null);
    }
}
