using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PublicGroupMgr : MonoBehaviour
{
    private Transform mCacheTransform;
    private Transform mRunGroup;
    private Transform mPrepareGroup;
    private UIProgressBar mElectricityProgressBar;
    private UILabel mLabelTimeInfo;
    private UILabel mLabelRoomNoInfo;
    private UILabel mLabelGameNoInfo;
    private UILabel mLabelCardNum;
    private UILabel mLabelRoomSetting;
    private TimeEvent mPlatformTimeEvent;

    private void Awake()
    {
        mCacheTransform = this.transform;
        mElectricityProgressBar = mCacheTransform.FindComponent<UIProgressBar>("ContainerLeft/PhoneInfo/ElectricityProgress");
        mLabelTimeInfo = mCacheTransform.FindComponent<UILabel>("ContainerLeft/PhoneInfo/LabelTimeInfo");
        mLabelRoomNoInfo = mCacheTransform.FindComponent<UILabel>("ContainerLeft/LabelRoomNoInfo");
        mLabelCardNum = mCacheTransform.FindComponent<UILabel>("RunGroup/LabelCardNum");
        mLabelGameNoInfo = mCacheTransform.FindComponent<UILabel>("PrepareGroup/ContainerLeft/LabelGameNoInfo");
        mLabelRoomSetting = mCacheTransform.FindComponent<UILabel>("ContainerTop/LabelRoomSetting");
        mRunGroup = mCacheTransform.FindComponent<Transform>("RunGroup");
        mPrepareGroup = mCacheTransform.FindComponent<Transform>("PrepareGroup");
    }

    public void SetGameState(RoomState mRoomState)
    {
        if (mRoomState == RoomState.Gaming)
        {
            mPrepareGroup.SetVisible(false);
            mRunGroup.SetVisible(true);
        }
        else if (mRoomState == RoomState.RoundEnd)
        {
            mPrepareGroup.SetVisible(false);
            mRunGroup.SetVisible(true);
        }
        else
        {
            mPrepareGroup.SetVisible(true);
            mRunGroup.SetVisible(false);
        }
    }

    public void RefreshRoomInfo(RoomInfo mRoomInfo)
    {
        mLabelRoomNoInfo.text = string.Format("房间号 \n{0}", mRoomInfo.RoomsNo);
        //mLabelGameNoInfo.text = string.Format("{0}局 底分{1}分", mRoomInfo.SeqCount, 2);

		if(mRoomInfo.Setting != null)
        	mLabelRoomSetting.text = mRoomInfo.Setting.ToString();
    }

    public void SetPlatformTimeEvent()
    {
        if (mPlatformTimeEvent != null) { mPlatformTimeEvent.Reset(); }
        mPlatformTimeEvent = TimerMgr.instance.Subscribe(100000, false, TimeEventType.IngoreTimeScale).SetIntervalTime(2f).OnUpdate(() =>
        {
            mElectricityProgressBar.value = PlatformPluginMgr.Instance.GetBatteryPct();
            mLabelTimeInfo.text = DateTime.Now.ToString("HH:mm");
        }).OnComplete(() => SetPlatformTimeEvent()).Start();
    }
}
