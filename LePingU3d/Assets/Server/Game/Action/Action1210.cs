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

public class Action1210 : GameAction
{
	private GameActionRecorData responsePack = new GameActionRecorData();

	List<MahJongGameAction> actionList = null;
	public static int curIndex = 0;
	public static int nodeLen = 0;

	UIMainModel mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();

    public Action1210() : base((int)1210)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
		writer.writeInt32("RoundID", actionParam.Get<int>("RoundID"));
    }

    protected override void DecodePackage(NetReader reader)
    {
		string tempStr = reader.readString ();
		Debug.Log (tempStr);
		responsePack = LitJson.JsonMapper.ToObject<GameActionRecorData>(tempStr);
        if (responsePack != null)
        {
            //GameLogicMgr.Instance.OperateGameEnd(responsePack);
			Debug.Log ("1210*********************************           "+responsePack.GameActionList.Count);
			Debug.Log ("1210*********************************           "+responsePack.roomInfo.roomNum);
			Debug.Log ("1210*********************************           "+responsePack.endList.Count);

			List<UserInfo> tempUserList = new List<UserInfo> ();
			int myIndex = 0;
			for (int i = 0; i < responsePack.userList.Count; i++) {
				LiamUserInfo uInfo = responsePack.userList[i];

				UserInfo user = new UserInfo ();
				user.NickName = uInfo.userName;
				user.UserId = uInfo.userId;
				//Debug.Log ("@@@@@@@@@@@@@@@@@  user.NickName = "+user.NickName);
				//Debug.Log ("@@@@@@@@@@@@@@@@@  user.userID = "+user.UserId);

				user.Sex = uInfo.sex;
				user.HeadUrl = uInfo.headUrl;
				user.IsOwner = uInfo.isCreater;

				tempUserList.Add (user);
				if (mUIMainModel.PlayerInfo.NickName == user.NickName) {
					myIndex = i;
				}
			}
			UIGameModel uIGameModel = UIModelMgr.Instance.GetModel<UIGameModel> ();
			uIGameModel.liamUserList.Clear ();

			List<UserInfo> userList = GetPosUserList (tempUserList, myIndex);
			for (int i = 0; i < userList.Count; i++) {
				GameLogicMgr.Instance.NotifyUserInfo(new PlayerAction(i, userList[i]));
			}
			uIGameModel.liamUserList = userList;

//			{//获取room userInfo
//				UIGameModel mUIGameModel = UIModelMgr.Instance.GetModel<UIGameModel>();
//				mUIGameModel.AddUsers(userList);
//			}

			{//设置room信息
				RoomInfo room = new RoomInfo ();
				room.State = RoomState.Gaming;
				room.Capacity = responsePack.userList.Count;
				room.RoomsNo = responsePack.roomInfo.roomNum;
				room.Setting = LitJson.JsonMapper.ToObject<RoomSetting>(responsePack.roomInfo.roomSetting);
				UIGameModel mUIGameModel = UIModelMgr.Instance.GetModel<UIGameModel>();
				mUIGameModel.RoomInfo = room;
			}
				
			actionList = GetActionList ();
			nodeLen = actionList.Count;
			curIndex = 0;
			TimerManager.Instance.recordBackDurTime = 2.0f;
			//UIModelMgr.Instance.GetModel<UIGameModel>().RefreshCardInfo();
			Call();
			TimerManager.Instance.LiamTimeStart (Call);
		}
    }

	void  Call(){
		if (curIndex < actionList.Count) {
			UIModelMgr.Instance.GetModel<UIGameModel> ().HandleGameAction(actionList[curIndex]);
		}
		curIndex++;
		if (curIndex >= actionList.Count) {
			//curIndex = 0;
			TimerManager.Instance.LiamTimeCancel ();
		
			//GameEndActionResp endResp = GetEndResp();
			//结束了
			//GameLogicMgr.Instance.OperateGameEnd(endResp);
		}
	}

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }

	//转换成客户端的数据结构
	List<MahJongGameAction> GetActionList(){
		List<MahJongGameAction> mJGAList = new List<MahJongGameAction> ();

		for (int i = 0; i < responsePack.GameActionList.Count; i++) {
			GameActionRecord record = responsePack.GameActionList[i];

			MahJongGameAction action = new MahJongGameAction ();
			action.Action = (MahJongActionType)record.Action;
			action.Parame = record.Parame;
			action.Cards = LitJson.JsonMapper.ToObject<List<Card>> (record.Cards);
			action.UserID = record.UserID;
			action.Timer = record.Timer;
			action.CardsNo = record.CardsNo;
			action.OtherCards = LitJson.JsonMapper.ToObject<List<Card>> (record.OtherCards);
			action.RoundID = record.RoundID;
			action.Order = record.Order;

			mJGAList.Add (action);
		}
		return mJGAList;
	}

	List<UserInfo> GetPosUserList(List<UserInfo> source, int myPos){
		List<UserInfo> myList = new List<UserInfo> ();
		if (myPos == 0) {
			return source;
		}
		if (source.Count == 2) {
			if (myPos == 1) {
				myList.Add (source [1]);
				myList.Add (source [0]);
			}
		} 
		else if (source.Count == 3) {
			if (myPos == 1) {
				myList.Add (source [1]);
				myList.Add (source [2]);
				myList.Add (source [0]);
			}
			else if (myPos == 2) {
				myList.Add (source [2]);
				myList.Add (source [0]);
				myList.Add (source [1]);
			}
		}
		else if (source.Count == 4) {
			if (myPos == 1) {
				myList.Add (source [1]);
				myList.Add (source [2]);
				myList.Add (source [3]);
				myList.Add (source [0]);
			}
			else if (myPos == 2) {
				myList.Add (source [2]);
				myList.Add (source [3]);
				myList.Add (source [0]);
				myList.Add (source [1]);
			}
			else if (myPos == 3) {
				myList.Add (source [3]);
				myList.Add (source [0]);
				myList.Add (source [1]);
				myList.Add (source [2]);
			}
		}
		return myList;
	}


//	[ProtoMember(1)]
//	public List<SingleSettlementInfo> SingleSettlementInfos;
//
//	[ProtoMember(2)]
//	public EndType IsWinner { get; set; }
//
//	/// <summary> 翻开的牌 </summary>
//	[ProtoMember(3)]
//	public List<Card> OpenCards { get; set; }
//
//	/// <summary> 推导出的牌 </summary>
//	[ProtoMember(4)]
//	public List<Card> OtherCards { get; set; }


	GameEndActionResp GetEndResp(){
		GameEndActionResp endResp = new GameEndActionResp ();
		endResp.Type = RoundEndType.RoundEnd;
		endResp.SingleSettlement = new SingleSettlementResp ();

		endResp.SingleSettlement.SingleSettlementInfos = new List<SingleSettlementInfo> ();

		//判断输赢
		bool isHu = false;
		for (int i = 0; i < responsePack.endList.Count; i++) {
			RoundMessageRecord roundRecord = responsePack.endList[i];
			if (roundRecord.Name == mUIMainModel.PlayerInfo.NickName) {
				isHu = roundRecord.IsHu;
			}
		}
		endResp.SingleSettlement.IsWinner = isHu ? EndType.Victory : EndType.Fail; //??? 流局的怎么搞

		for (int i = 0; i < responsePack.endList.Count; i++) {
			SingleSettlementInfo settleInfo = GetSettleInfo(responsePack.endList[i]);
			endResp.SingleSettlement.SingleSettlementInfos.Add (settleInfo);
		}
		return endResp;
	}

	SingleSettlementInfo GetSettleInfo(RoundMessageRecord record){
		SingleSettlementInfo settleInfo = new SingleSettlementInfo();

		//Debug.Log ("############  "+record.CurCard);

		settleInfo.UserID = record.UserID;
		settleInfo.Name = record.Name;
		settleInfo.IsHu = record.IsHu;
		//
		if(string.IsNullOrEmpty(record.HandCardGroups)|| record.CurCard == "null"){
			settleInfo.HandCardGroups = null;
		}
		else{
			settleInfo.HandCardGroups = LitJson.JsonMapper.ToObject<List<CardGroup>>(record.HandCardGroups);	
		}
		//
		settleInfo.HandCards = LitJson.JsonMapper.ToObject<List<Card>> (record.HandCards);
		//
		if(string.IsNullOrEmpty(record.CurCard) || record.CurCard == "null"){
			settleInfo.CurCard = null;
		}
		else{
			settleInfo.CurCard = LitJson.JsonMapper.ToObject<Card> (record.CurCard);
		}
			
		settleInfo.Scores = LitJson.JsonMapper.ToObject<Dictionary<string, int>> (record.Scores);
		settleInfo.TotalScore = record.TotalScore;
		settleInfo.HuFlag = record.HuFlag;
		settleInfo.Flag = record.Flag;

		return settleInfo;
	}
}
