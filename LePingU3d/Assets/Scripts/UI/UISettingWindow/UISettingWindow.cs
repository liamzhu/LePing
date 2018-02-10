/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UISettingWindow.cs
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
using UnityEngine;

public class UISettingWindow : UIBasePanel
{

    public UISettingWindow() : base(new UIProperty(UIWindowStyle.PopUp, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UISettingWindow"))
    {
    }

    private UISlider mMusicSlider;
    private UISlider mSoundSlider;
    private UIButton mBtnQuit;
    private UIButton mBtnDissolveRoom;
    private bool mIsGaming;
    private UIToggle mToggleNational;
    private UIToggle mToggleLocalism;

    protected override void OnAwakeInitUI()
    {
        mBtnQuit = CacheTrans.FindComponent<UIButton>("Root/BtnQuit");
        mBtnDissolveRoom = CacheTrans.FindComponent<UIButton>("Root/BtnDissolveRoom");
        mToggleNational = CacheTrans.FindComponent<UIToggle>("Root/TypeGroup/Option1");
        mToggleLocalism = CacheTrans.FindComponent<UIToggle>("Root/TypeGroup/Option2");


        UIEventListener.Get(mBtnQuit.gameObject).onClick += OnQuitClick;
        UIEventListener.Get(mBtnDissolveRoom.gameObject).onClick += OnDissolveRoomClick;

        CacheTrans.GetUIEventListener("Root/BtnClose").onClick += OnCloseClick;

        CacheTrans.GetUIEventListener("Root/MusicGroup/BtnCloseMusic").onClick += OnMusicClick;
        CacheTrans.GetUIEventListener("Root/MusicGroup/BtnOpenMusic").onClick += OnMusicClick;

        mMusicSlider = CacheTrans.FindComponent<UISlider>("Root/MusicGroup/Slider");
        mMusicSlider.onChange.Add(new EventDelegate(OnMusicChangeClick));

        CacheTrans.GetUIEventListener("Root/SoundGroup/BtnCloseSound").onClick += OnSoundClick;
        CacheTrans.GetUIEventListener("Root/SoundGroup/BtnOpenSound").onClick += OnSoundClick;
        CacheTrans.GetUIEventListener("Root/TypeGroup/Option1").onClick += OnAudioToggleValueChange;
        CacheTrans.GetUIEventListener("Root/TypeGroup/Option2").onClick += OnAudioToggleValueChange;

        mSoundSlider = CacheTrans.FindComponent<UISlider>("Root/SoundGroup/Slider");
        mSoundSlider.onChange.Add(new EventDelegate(OnSoundChangeClick));
    }

    public override void OnRefresh()
    {
        mIsGaming = (bool)mData;
        mBtnDissolveRoom.gameObject.SetActive(mIsGaming);
        mBtnQuit.gameObject.SetActive(!mIsGaming);
        LoadAllSetting();
    }

    private void OnCloseClick(GameObject go)
    {
        UIWindowMgr.Instance.PopPanel<UISettingWindow>();
        //if (mIsGaming)
        //{
        //    UIWindowMgr.Instance.PopPanel<UISettingWindow>();
        //}
        //else
        //{
        //    OnReturnClick(null);
        //}
    }

    private void OnDissolveRoomClick(GameObject go)
    {
        UIDialogMgr.Instance.ShowDialog(10007, DissolveRoom);
    }

    private void DissolveRoom(GameObject go)
    {
        ActionParam actionParam = new ActionParam();
        actionParam["IsAgree"] = 1;
        Net.Instance.Send((int)ActionType.DissolveRoom, null, actionParam);
    }

    private void OnMusicChangeClick()
    {
        AudioManager.Instance.MusicVolume = mMusicSlider.value;
    }

    private void OnSoundChangeClick()
    {
        AudioManager.Instance.SoundVolume = mSoundSlider.value;
    }

    private void OnSoundClick(GameObject go)
    {
        mSoundSlider.value = go.name.Equals("BtnCloseSound") ? 0 : 1;
        AudioManager.Instance.SoundVolume = go.name.Equals("BtnCloseSound") ? 0 : 1;
    }

    private void OnMusicClick(GameObject go)
    {
        mMusicSlider.value = go.name.Equals("BtnCloseMusic") ? 0 : 1;
        AudioManager.Instance.MusicVolume = go.name.Equals("BtnCloseMusic") ? 0 : 1;
    }

    private void OnQuitClick(GameObject go)
    {
        UIDialogMgr.Instance.ShowDialog(10015, delegate (GameObject go1) { Application.Quit(); });
    }

    private void LoadAllSetting()
    {
        mSoundSlider.value = PlayerPrefsUtil.GetFloat(PrefsConstant.KEY_SOUND_VOLUME);
        mMusicSlider.value = PlayerPrefsUtil.GetFloat(PrefsConstant.KEY_MUSIC_VOLUME);

        int audioType = PlayerPrefsUtil.GetInt(PrefsConstant.AudioType, 0);
        switch (audioType)
        {
            case 0:
                mToggleNational.value = true;
                mToggleLocalism.value = false;
                break;

            case 1:
                mToggleNational.value = false;
                mToggleLocalism.value = true;
                break;
        }
    }


    private void OnAudioToggleValueChange(GameObject go)
    {
        switch (go.name)
        {
            case "Option1":   //¹úÓï
                PlayerPrefsUtil.SetInt(PrefsConstant.AudioType, 0);
                break;

            case "Option2":   //·½ÑÔ
                PlayerPrefsUtil.SetInt(PrefsConstant.AudioType, 1);
                break;
        }
    }
}
