/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UIGameWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-11-28
 *Description:
 *History:
*********************************************************/

using cn.sharesdk.unity3d;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UIGameWindow : UIBasePanel
{
    private static UIGameWindow _instance = null;
    private UIGameModel mUIGameModel;
    private UIMainModel mUIMainModel;
    private PublicGroupMgr mPublicGroupMgr;
    private UILabel mLabelSeqCount;
    private UILabel mLabelCardNum;
    private UIButton mBtnDissolveRoom;
    private UIButton mBtnLeaveRoom;
    private UIButton mBtnSetting;
    private UIAtlas mCardsAtlas;

    private List<PlayerInfoItem> mPlayerItems;
    private List<PlayerCardGroupItem> mPlayerCardGroupList;
    private GameTableItem mGameTableItem;

    private CardItem[] mJingCardItems;
    private CardItem[] mOpenCardItems;
	private RoomState mRoomState;
    private UISpriteAnimation mVoiceAnimation;

	private RecordControl mRecordControl;

    public static UIGameWindow Instance { get { return _instance; } }

    public UIGameWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIGameWindow"))
    {
    }

    protected override void OnAwakeInitUI()
    {
        _instance = this;

        if (mUIGameModel == null)
        {
            mUIGameModel = UIModelMgr.Instance.GetModel<UIGameModel>();
            mUIGameModel.ValueUpdateEvent += OnValueUpdateEvent;
        }

        if (mUIMainModel == null) { mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>(); }
        mPublicGroupMgr = CacheTrans.FindComponent<PublicGroupMgr>("Root/PublicGroup");
        mLabelSeqCount = CacheTrans.FindComponent<UILabel>("Root/PublicGroup/RunGroup/LabelSeqCount");
        mLabelCardNum = CacheTrans.FindComponent<UILabel>("Root/PublicGroup/RunGroup/LabelCardNum");
        mGameTableItem = CacheTrans.FindComponent<GameTableItem>("Root/PublicGroup/RunGroup/GameTable");

		mRecordControl = CacheTrans.FindComponent<RecordControl>("Root/PublicGroup/RunGroup/RecordControl");

        mJingCardItems = CacheTrans.FindChindComponents<CardItem>("Root/PublicGroup/RunGroup/ContainerTop/JingCards");
        mOpenCardItems = CacheTrans.FindChindComponents<CardItem>("Root/PublicGroup/RunGroup/OpenCardItems");

        if (mPlayerItems == null) { mPlayerItems = new List<PlayerInfoItem>(); }

        mPlayerItems.Add(CacheTrans.FindComponent<PlayerInfoItem>("Root/ContainerBottom/PlayerInfo"));
        mPlayerItems.Add(CacheTrans.FindComponent<PlayerInfoItem>("Root/ContainerRight/PlayerInfo"));
        mPlayerItems.Add(CacheTrans.FindComponent<PlayerInfoItem>("Root/ContainerTop/PlayerInfo"));
        mPlayerItems.Add(CacheTrans.FindComponent<PlayerInfoItem>("Root/ContainerLeft/PlayerInfo"));

        if (mPlayerCardGroupList == null) { mPlayerCardGroupList = new List<PlayerCardGroupItem>(); }
        mPlayerCardGroupList.Add(CacheTrans.FindComponent<PlayerCardGroupItem>("Root/ContainerBottom/PlayerCardGroup"));
        mPlayerCardGroupList.Add(CacheTrans.FindComponent<PlayerCardGroupItem>("Root/ContainerRight/PlayerCardGroup"));
        mPlayerCardGroupList.Add(CacheTrans.FindComponent<PlayerCardGroupItem>("Root/ContainerTop/PlayerCardGroup"));
        mPlayerCardGroupList.Add(CacheTrans.FindComponent<PlayerCardGroupItem>("Root/ContainerLeft/PlayerCardGroup"));
        mVoiceAnimation = CacheTrans.FindComponent<UISpriteAnimation>("Root/PublicGroup/VoiceAnimation");

        #region 注册按钮的点击方法

        mBtnDissolveRoom = CacheTrans.FindComponent<UIButton>("Root/PublicGroup/PrepareGroup/ContainerRight/BtnDissolveRoom");
        mBtnLeaveRoom = CacheTrans.FindComponent<UIButton>("Root/PublicGroup/PrepareGroup/ContainerRight/BtnLeaveRoom");
        UIEventListener.Get(mBtnDissolveRoom.gameObject).onClick += OnDissolveRoomClick;
        UIEventListener.Get(mBtnLeaveRoom.gameObject).onClick += OnDissolveRoomClick;
        CacheTrans.GetUIEventListener("Root/PublicGroup/PrepareGroup/ContainerRight/BtnReturnHall").onClick += OnReturnHallClick;
        CacheTrans.GetUIEventListener("Root/PublicGroup/PrepareGroup/BtnWeChat").onClick += OnWeChatClick;
        CacheTrans.GetUIEventListener("Root/PublicGroup/RunGroup/ContainerRight/BtnSetting").onClick += OnSettingClick;
        CacheTrans.GetUIEventListener("Root/PublicGroup/RunGroup/ContainerRight/BtnTextChat").onClick += OnChatClick;
        CacheTrans.GetUIEventListener("Root/PublicGroup/RunGroup/ContainerRight/BtnExpression").onClick += OnExpressionClick;
        CacheTrans.GetUIEventListener("Root/PublicGroup/ContainerRight/BtnVoiceChat").onPress += OnRecording;

        #endregion 注册按钮的点击方法

        SetVoiceAnimation(false);
    }

    private void ReplaceCardsSkin()
    {
        if (mCardsAtlas == null)
        {
            mCardsAtlas = Resources.Load<GameObject>("CardPrefab/8_UICardsAtlasNew").GetComponent<UIAtlas>();
        }

        UISprite[] cards = CacheTrans.GetComponentsInChildren<UISprite>(true);
        foreach(UISprite sprite in cards)
        {
            if (sprite.atlas.name == "8_UICardsAtlas") 
            {
                /*
                Vector2 tempSize = sprite.localSize;
                string tempSpriteName = sprite.spriteName;
                GameObject spriteObject = sprite.gameObject;
                GameObject.Destroy(sprite);
                
                UISprite newSprite = spriteObject.AddComponent<UISprite>();
                newSprite.atlas = mCardsAtlas;
                newSprite.spriteName = tempSpriteName;
                newSprite.SetRect(0, 0, tempSize.x, tempSize.y);
                Debug.Log(tempSize.x.ToString() + "  " + tempSize.y.ToString());
                */
                sprite.atlas = mCardsAtlas;
            }
        }
    }

	//发送录音
    private void OnRecording(GameObject goSender, bool flag)
    {
		if (GameMgr.Instance.isFromRecord) {
			return;
		}

        if (flag)
        {
            MahjongAudioMgr.Instance.PauseBGM(true);
            SetVoiceAnimation(true);
            ViSpeak.Instance.StartRecording();
        }
        else
        {
            //按钮弹起结束录音
            SetVoiceAnimation(false);
            ViSpeak.Instance.StopReconding();
            MahjongAudioMgr.Instance.PauseBGM(false);
        }
    }

    public void SetVoiceAnimation(bool isVisible)
    {
        if (isVisible)
        {
            mVoiceAnimation.Play();
        }
        else
        {
            mVoiceAnimation.Pause();
        }
        mVoiceAnimation.gameObject.SetVisible(isVisible);
    }

    public override void OnRefresh()
    {
		if (GameMgr.Instance.isFromRecord) {
			//mPublicGroupMgr.SetPlatformTimeEvent ();
			SetGameState(RoomState.None);
			mPublicGroupMgr.SetPlatformTimeEvent();

			mRecordControl.gameObject.SetActive (true);

			//设置牌为显示状态
			SetHandCardState(true);
		} 
		else {
			Initialize();
			//ReplaceCardsSkin();
			SendLocationToServer();

			mRecordControl.gameObject.SetActive (false);
			SetHandCardState(false);
		}
    }

	void SetHandCardState(bool isShow){
		for (int i = 0; i < mPlayerCardGroupList.Count; i++) {
			PlayerCardGroupItem item = mPlayerCardGroupList[i];
			item.SetHandCardShow (isShow);
		}
	}

    /// <summary>
    /// 发送位置给服务器
    /// </summary>
    private void SendLocationToServer()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            //发送Android定位
            string location = TencentGPS.Instance.GetAddress();
            //Send
            ActionParam ap = new ActionParam();
            ap["UserId"] = mUIMainModel.PlayerInfo.UserId;
            ap["Location"] = location;
            Net.Instance.Send((int)ActionType.SendLocation, null, ap);
        }
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //发送Ios定位
            IOSTencentMap.Instance.OnGetAddress += address => 
            {
                //Clear Delegate
                IOSTencentMap.Instance.OnGetAddress = null;
                //Send
                ActionParam ap = new ActionParam();
                ap["UserId"] = mUIMainModel.PlayerInfo.UserId;
                ap["Location"] = address;
                Net.Instance.Send((int)ActionType.SendLocation, null, ap);
            };
            IOSTencentMap.Instance.StartGetAddr();
        }
    }

    private void Initialize()
    {
        SetGameState(RoomState.None);
        Net.Instance.Send((int)ActionType.RoomInfo, null, null);
        //RefreshRoomInfo(mUIGameModel.RoomInfo);
        mPublicGroupMgr.SetPlatformTimeEvent();
    }

	void RefreshRoomInfoFromRecord(RoomInfo mRoomInfo){
		if (mRoomInfo != null)
		{
			Debug.Log(mUIGameModel.IsOwner + "  " + mRoomInfo.OwnerId + "  " + mRoomInfo.State);

			SetGameState(RoomState.Gaming);
			//ActionParam actionParam = new ActionParam();
			//actionParam["UserID"] = 0;
			//Net.Instance.Send((int)ActionType.RoomUsers, null, actionParam);
			mPublicGroupMgr.RefreshRoomInfo(mRoomInfo);
			//if (mRoomInfo.State == RoomState.Gaming)
			//{
			//	Net.Instance.Send((int)ActionType.ReConnect, null, null);
			//}
			mBtnDissolveRoom.gameObject.SetActive(mUIGameModel.IsOwner);
			mBtnLeaveRoom.gameObject.SetActive(!mUIGameModel.IsOwner);
		}
	}

    private void RefreshRoomInfo(RoomInfo mRoomInfo)
    {
        if (mRoomInfo != null)
        {
            Debug.Log(mUIGameModel.IsOwner + "  " + mRoomInfo.OwnerId + "  " + mRoomInfo.State);
            SetGameState(mRoomInfo.State);

			//获取房间玩家信息
            ActionParam actionParam = new ActionParam();
            actionParam["UserID"] = 0;
            Net.Instance.Send((int)ActionType.RoomUsers, null, actionParam);
            mPublicGroupMgr.RefreshRoomInfo(mRoomInfo);
            if (mRoomInfo.State == RoomState.Gaming)
            {
                Net.Instance.Send((int)ActionType.ReConnect, null, null);
            }
            mBtnDissolveRoom.gameObject.SetActive(mUIGameModel.IsOwner);
            mBtnLeaveRoom.gameObject.SetActive(!mUIGameModel.IsOwner);
        }

    }

    private void OnValueUpdateEvent(object sender, ValueChangeArgs e)
    {
        switch (e.key)
        {
		case UIGameModelConst.KEY_RefreshRoomInfo:
				if (GameMgr.Instance.isFromRecord) {
					RefreshRoomInfoFromRecord ((RoomInfo)e.newValue);
					break;
				}
                RefreshRoomInfo((RoomInfo)e.newValue);
                break;
            case UIGameModelConst.KEY_RefreshCardInfo:
                RefreshCardsInfo();
                break;
            case UIGameModelConst.KEY_GameAction:
                SetMahJongGameAction((MahJongGameAction)e.newValue);
                break;
            case UIGameModelConst.KEY_RoomMsgInfo:
                RefreshRoomMsgInfo((RoomMsgInfo)e.newValue);
                break;
            case UIGameModelConst.KEY_ReconnectGameAction:
                ExecuteReconnectAction((MahJongGameAction)e.newValue);
                break;
            case UIGameModelConst.LeaveRoom:
                LeaveRoom();
                break;
            case UIGameModelConst.RoomActionCull:
                RoomActionCull();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 显示聊天信息
    /// </summary>
    /// <param name="mRoomMsgInfo"></param>
    private void RefreshRoomMsgInfo(RoomMsgInfo mRoomMsgInfo)
    {
        if (mUIGameModel.GetUser(mRoomMsgInfo.UID) != null)
        {
            int pos = GameLogicMgr.Instance.getPlayerIndex(mUIGameModel.GetUser(mRoomMsgInfo.UID).RoomOrder);
            mPlayerItems[pos].ShowRoomMsgInfo(mRoomMsgInfo);
        }
    }

    public void RoomActionCull()
    {
        UIDialogMgr.Instance.ShowDialog(10010, (GameObject go) => LeaveRoom());
    }

    private void SetGameState(RoomState state)
    {
        this.mRoomState = state;
        if (mRoomState == RoomState.Gaming)
        {
            GamePlaying();
        }
        else if (mRoomState == RoomState.RoundEnd)
        {
            GameRoundEnd();
        }
        else
        {
            GameNone();
        }
        mPublicGroupMgr.SetGameState(mRoomState);
    }

    private void SetRoomInfo(bool isShow)
    {
        mLabelSeqCount.gameObject.SetVisible(isShow);
        mLabelCardNum.gameObject.SetVisible(isShow);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void GamePlaying()
    {
        SetRoomInfo(true);
        mLabelSeqCount.text = string.Format("第{0}/{1}局", mUIGameModel.RoomInfo.SeqCurrent, mUIGameModel.RoomInfo.SeqCount);
        mRoomState = mUIGameModel.RoomInfo.State = RoomState.Gaming;
        Array.ForEach<CardItem>(mJingCardItems, p => { p.SetVisible(false); });
        Array.ForEach<CardItem>(mOpenCardItems, p => { p.SetVisible(false); });
        mPublicGroupMgr.SetGameState(mRoomState);
        mPlayerItems.ForEach(p => p.Initialize(mRoomState));
        mPlayerCardGroupList.ForEach(p => p.StartGame());
    }

    private void GameNone()
    {
        mLabelCardNum.text = string.Format("剩余{0}张", 0);
        mGameTableItem.CleanUp();
        SetRoomInfo(false);
        Array.ForEach<CardItem>(mJingCardItems, p => { p.SetVisible(false); });
        Array.ForEach<CardItem>(mOpenCardItems, p => { p.SetVisible(false); });
        mUIGameModel.ClearUsersData();
        mUIGameModel.ClearUserCardData();
        mPlayerItems.ForEach(p => p.Initialize(mRoomState));
        mPlayerItems.ForEach(p => p.Refresh(null));
        mPlayerCardGroupList.ForEach(p => p.Initialize());
    }

    private void GameRoundEnd()
    {
        mLabelCardNum.text = string.Format("剩余{0}张", 0);
        mGameTableItem.CleanUp();
        SetRoomInfo(false);
        Array.ForEach<CardItem>(mJingCardItems, p => { p.SetVisible(false); });
        Array.ForEach<CardItem>(mOpenCardItems, p => { p.SetVisible(false); });
        mUIGameModel.ClearUserCardData();
        mPlayerItems.ForEach(p => p.Initialize(mRoomState));
        mPlayerCardGroupList.ForEach(p => p.Initialize());
    }

    private void SetMahJongGameAction(MahJongGameAction action)
    {
        switch (action.Action)
        {
            case MahJongActionType.StartOpen: //起手翻牌
                StartOpen(action);
                break;
            case MahJongActionType.EndOpen:
                break;
            default:
                break;
        }
        if (action.UserID != 0)
        {
            mLabelCardNum.text = string.Format("剩余{0}张", action.CardsNo);
            //获取执行action的玩家pos
			int pos = 0;
			if (GameMgr.Instance.isFromRecord) {
				for (int i = 0; i < mUIGameModel.liamUserList.Count; i++) {
					UserInfo user = mUIGameModel.liamUserList[i];
					if (user.UserId == action.UserID) {
						pos = i;
						break;
					}
				}
			}
			else {
				pos = GameLogicMgr.Instance.getPlayerIndex(action.Order);  
			}
            mPlayerCardGroupList[pos].OperateGameAction(action);
            mGameTableItem.OperateAction(pos, action.Timer);
        }
    }

	//起手翻牌 特殊牌
    private void StartOpen(MahJongGameAction action)
    {
        if (action != null && !action.OtherCards.IsNullOrEmpty())
        {
            for (int i = 0; i < action.OtherCards.Count; i++)
            {
                mOpenCardItems[i].Refresh(action.OtherCards[i], DirectionType.bottom);
                mOpenCardItems[i].transform.DOMove(mJingCardItems[i].transform.position, 1)
                    .OnComplete(() =>
                    {
                        mJingCardItems[i].Refresh(action.OtherCards[i], DirectionType.bottom);
                        mJingCardItems[i].SetVisible(true);
                        mOpenCardItems[i].transform.position = Vector3.zero;
                        mOpenCardItems[i].SetVisible(false);
                    });
            }
        }
    }

    private void ExecuteReconnectAction(MahJongGameAction action)
    {
        if (action != null)
        {
            int pos = GameLogicMgr.Instance.getPlayerIndex(action.Order);
            //Debug.Log(action.Action + "  " + action.Order + "  " + mUIMainModel.PlayerInfo.RoomOrder + "  " + pos);
            mPlayerCardGroupList[pos].ReconnectGameAction(action);
            mGameTableItem.OperateAction(pos, action.Timer);
        }
    }

    private void RefreshCardsInfo()
    {
        mPlayerCardGroupList.ForEach(p => p.RefreshCardsInfo());
    }

    #region 注册和移除事件

    protected override void RegisterEvent()
    {
        EventMgr.Instance.MainMgr.AddObserver(EventConst.EventStartGame, GamePlaying);
        EventMgr.Instance.MainMgr.AddObserver(EventConst.EventLeaveRoom, LeaveRoom);
        EventMgr.Instance.MainMgr.AddObserver<OperationBehaviorResp>(EventConst.EventOperationBehavior, SetOperationBehavior);
        EventMgr.Instance.MainMgr.AddObserver<PlayerAction>(EventConst.EventPlayerAction, SetPlayerAction);
        EventMgr.Instance.MainMgr.AddObserver<GameEndActionResp>(EventConst.EventGameEnd, OperateGameEnd);
    }

    protected override void RemoveEvent()
    {
        EventMgr.Instance.MainMgr.RemoveObserver(EventConst.EventStartGame, GamePlaying);
        EventMgr.Instance.MainMgr.RemoveObserver(EventConst.EventLeaveRoom, LeaveRoom);
        EventMgr.Instance.MainMgr.RemoveObserver<OperationBehaviorResp>(EventConst.EventOperationBehavior, SetOperationBehavior);
        EventMgr.Instance.MainMgr.RemoveObserver<PlayerAction>(EventConst.EventPlayerAction, SetPlayerAction);
        EventMgr.Instance.MainMgr.RemoveObserver<GameEndActionResp>(EventConst.EventGameEnd, OperateGameEnd);
    }

    private void OperateGameEnd(GameEndActionResp resp)
    {
        if (resp != null)
        {
            if (resp.Type == RoundEndType.RoundEnd) //单局结束
            {
                SingleGameEnd(resp.SingleSettlement);
            }
            else if (resp.Type == RoundEndType.Disband)
            {
                WholeGameEnd(resp.GameSettlement);
            }
            else 									
            {
                WholeGameEnd(resp.GameSettlement);
                SingleGameEnd(resp.SingleSettlement);
            }
        }
    }

	//游戏结束 弹结算框
    private void WholeGameEnd(GameSettlementResp msg)
    {
        MahjongAudioMgr.Instance.PauseBGM(true);
        mUIGameModel.RoomInfo.State = RoomState.None;
        mUIMainModel.PlayerInfo.RoomNumber = 0;
        mUIMainModel.PlayerInfo.IsReady = false;
        UIWindowMgr.Instance.PushPanel<UIGameSettlementWindow>(msg);
    }

    private void SingleGameEnd(SingleSettlementResp msg)
    {
        mUIGameModel.RoomInfo.State = RoomState.RoundEnd;
        mUIMainModel.PlayerInfo.RoomNumber = 0;
        mUIMainModel.PlayerInfo.IsReady = false;

        UIWindowMgr.Instance.PushPanel<UISingleSettlementWindow>(msg);
    }

    private void SetOperationBehavior(OperationBehaviorResp action)
    {
        mPlayerCardGroupList[0].SetOperationBehavior(action);
    }

    private void SetPlayerAction(PlayerAction action)
    {
        int pos = GameLogicMgr.Instance.getPlayerIndex(action.RoomOrder);
        //Debug.Log("SetPlayerAction  " + action.RoomOrder + "  " + pos);
        mPlayerItems[pos].Refresh(action.User);
        mPlayerCardGroupList[pos].SetUserInfo(action.User);
    }

    private void LeaveRoom()
    {
        GameNone();
        mUIGameModel.ClearGameEndDatas();
        mUIMainModel.PlayerInfo.RoomNumber = 0;
        mUIMainModel.PlayerInfo.IsReady = false;
        GameMgr.Instance.EnterToMainWindow();
    }

    #endregion 注册和移除事件

    #region 按钮方法以及回调

    private void OnSettingClick(GameObject go)
    {
		if (GameMgr.Instance.isFromRecord) {
			return;
		}
        UIWindowMgr.Instance.PushPanel<UISettingWindow>(true);
    }

    private void OnDissolveRoomClick(GameObject go)
    {
		if (GameMgr.Instance.isFromRecord) {
			return;
		}
        UIDialogMgr.Instance.ShowDialog(mUIGameModel.IsOwner ? 10003 : 10006, DissolveRoom);
    }

    private void DissolveRoom(GameObject go)
    {
		if (GameMgr.Instance.isFromRecord) {
			return;
		}
        Net.Instance.Send((int)ActionType.LeaveRoom, null, null);
    }

    private void OnReturnHallClick(GameObject go)
    {
		if (GameMgr.Instance.isFromRecord) {
			return;
		}
        DebugHelper.LogInfo("点击返回大厅按钮");
        UIDialogMgr.Instance.ShowDialog(10002, ReturnHall);
    }

    private void ReturnHall(GameObject go)
    {
        GameMgr.Instance.EnterToMainWindow();
		if (GameMgr.Instance.isFromRecord) {
			return;
		}
        ActionParam actionParam = new ActionParam();
        actionParam["IsGetReady"] = 0;
        Net.Instance.Send((int)ActionType.RoomReady, null, actionParam);
    }

    private void OnChatClick(GameObject go)
    {
		if (GameMgr.Instance.isFromRecord) {
			return;
		}
        UIWindowMgr.Instance.PushPanel<UIChatWindow>();
    }

    private void OnExpressionClick(GameObject go)
    {
		if (GameMgr.Instance.isFromRecord) {
			return;
		}
        UIWindowMgr.Instance.PushPanel<UIExpressionWindow>();
    }

    private void OnWeChatClick(GameObject go)
    {
		if (GameMgr.Instance.isFromRecord) {
			return;
		}

        ShareInfo mShareInfo = mUIMainModel.GetShareInfo(ShareType.Room);
        if (mShareInfo != null)
        {
            ShareContent content = new ShareContent();
            content.SetImagePath(AppConst.ImagePath);
            content.SetTitle(mShareInfo.Title);
            content.SetText(string.Format(mShareInfo.Content, mUIGameModel.RoomInfo.RoomsNo, mUIGameModel.RoomInfo.Setting.ToString()));
            content.SetUrl(mShareInfo.Url);
            content.SetShareType(ContentType.Webpage);
            ApplicationMgr.Instance.ShareSDK.ShowShareContentEditor(PlatformType.WeChat, content);
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog("没有找到分享的相关配置");
        }
    }

    #endregion 按钮方法以及回调
}
