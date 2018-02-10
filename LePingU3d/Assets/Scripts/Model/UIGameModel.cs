using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameModelConst
{

    public const string KEY_RefreshRoomInfo = "RefreshRoomInfo";
    public const string KEY_RefreshCardInfo = "RefreshCardInfo";
    public const string KEY_GameAction = "MahJongGameAction";
    public const string KEY_RoomMsgInfo = "RoomMsgInfo";
    public const string KEY_ReconnectGameAction = "ReconnectGameAction";

    public const string RoomActionOperation = "RoomActionOp";

    public const string LeaveRoom = "LeaveRoom";
    public const string RoomActionCull = "RoomActionCull";

}

public class UIGameModel : UIBaseModel
{
    private UIMainModel mUIMainModel
    {
        get { return UIModelMgr.Instance.GetModel<UIMainModel>(); }
    }

    public void ClearRoundEndDatas()
    {
        ClearUserCardData();
        LastGameActionResp = null;
        LastDeskCard = null;
        LastDeskCardItem = null;
    }

    public void ClearGameEndDatas()
    {
        ClearUsersData();
        ClearUserCardData();
        LastGameActionResp = null;
        LastDeskCard = null;
        LastDeskCardItem = null;
        RoomInfo = null;
        mUIMainModel.PlayerInfo.RoomNumber = 0;
        mUIMainModel.PlayerInfo.IsReady = false;
    }

    #region 玩家数据相关操作

    private Dictionary<int, UserInfo> mGameUsers = new Dictionary<int, UserInfo>();

    private GameUserModel mGameUserModel = new GameUserModel();

	public List<UserInfo> liamUserList = new List<UserInfo> ();

    public bool IsOneself(int id)
    {
        if (mUIMainModel.PlayerInfo != null)
        {
            return id.Equals(mUIMainModel.PlayerInfo.UserId);
        }
        else
        {
            return false;
        }
    }

	public UserInfo GetLiamUser(int uid){
		for (int i = 0; i < liamUserList.Count; i++) {
			if (uid == liamUserList [i].UserId) {
				return liamUserList[i];
			}
		}
		return null;
	}

    public UserInfo GetUser(int uid)
    {
        if (mGameUsers.ContainsKey(uid))
        {
            return mGameUsers[uid];
        }
        else
        {
            return null;
        }
    }

    public void AddUsers(List<UserInfo> users)
    {
        if (!users.IsNullOrEmpty())
        {
            users.ForEach(p => { AddUser(p); });
        }
    }

    public void AddUser(UserInfo user)
    {
        if (mUIMainModel.PlayerInfo.UserId.Equals(user.UserId))
        {
            mUIMainModel.PlayerInfo = user.getCopy();
        }
        if (mGameUsers.ContainsKey(user.UserId))
        {
            mGameUsers[user.UserId] = user;
        }
        else
        {
            mGameUsers.Add(user.UserId, user);
        }
        //Debug.Log(user.RoomOrder + "  " + mUIMainModel.PlayerInfo.RoomOrder);
        GameLogicMgr.Instance.NotifyUserInfo(new PlayerAction(user.RoomOrder, user));
    }

    public bool RemoveUser(int id)
    {
        if (mGameUsers.ContainsKey(id))
        {
            GameLogicMgr.Instance.NotifyUserInfo(new PlayerAction(mGameUsers[id].RoomOrder, null));
            mGameUsers.Remove(id);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetUserNetState(int id, bool isOnlining)
    {
        if (mGameUsers.ContainsKey(id))
        {
            mGameUsers[id].IsOnlining = isOnlining;
            GameLogicMgr.Instance.NotifyUserInfo(new PlayerAction(mGameUsers[id].RoomOrder, mGameUsers[id]));
        }
    }

    public void SetUserReadyState(int id, bool isReady)
    {

        if (IsOneself(id))
        {
            mUIMainModel.PlayerInfo.IsReady = isReady;
        }
        if (mGameUsers.ContainsKey(id))
        {
            mGameUsers[id].IsReady = isReady;
            GameLogicMgr.Instance.NotifyUserInfo(new PlayerAction(mGameUsers[id].RoomOrder, mGameUsers[id]));
        }
    }

    public void ClearUsersData()
    {
        mGameUsers.Clear();
        mGameUsers = new Dictionary<int, UserInfo>();
    }

    #endregion 玩家数据相关操作

    public void RoomActionCull()
    {
        ValueChangeArgs ve = new ValueChangeArgs(UIGameModelConst.RoomActionCull, null, null);
        DispatchValueUpdateEvent(ve);
    }

    #region 游戏玩家的牌数据

    private Dictionary<int, UserCardModel> mUserCardModelDict = new Dictionary<int, UserCardModel>();

    public UserCardModel getOrAddUserCardModel(int id)
    {
        UserCardModel mUserCardModel;
        if (!mUserCardModelDict.TryGetValue(id, out mUserCardModel))
        {
            mUserCardModel = new UserCardModel();
            mUserCardModelDict.Add(id, mUserCardModel);
        }
        return mUserCardModel;
    }

    public void ClearUserCardData()
    {
        mUserCardModelDict.Clear();
        mUserCardModelDict = new Dictionary<int, UserCardModel>();
    }

    #endregion 游戏玩家的牌数据

    public MahJongGameAction LastGameActionResp { get; set; }
    public Card LastDeskCard { get; set; }
    public CardItem LastDeskCardItem { get; set; }

    public bool IsOwner
    {
        get
        {
            if (mRoomInfo != null)
            {
                return mRoomInfo.OwnerId.Equals(mUIMainModel.PlayerInfo.UserId);
            }
            else
            {
                return false;
            }

        }
    }

    private RoomInfo mRoomInfo;
	//房间信息变化就会触发 事件
    public RoomInfo RoomInfo
    {
        get
        {
            return mRoomInfo;
        }
        set
        {
            ValueChangeArgs ve = new ValueChangeArgs(UIGameModelConst.KEY_RefreshRoomInfo, mRoomInfo, value);
            mRoomInfo = value;
            DispatchValueUpdateEvent(ve);
        }
    }

    public void ReconnectGameAction(MahJongGameAction action)
    {
        ValueChangeArgs ve = new ValueChangeArgs(UIGameModelConst.KEY_ReconnectGameAction, action, action);
        DispatchValueUpdateEvent(ve);
    }

    public void HandleGameAction(MahJongGameAction action)
    {
        ValueChangeArgs ve = new ValueChangeArgs(UIGameModelConst.KEY_GameAction, action, action);
        DispatchValueUpdateEvent(ve);
    }

    public void RefreshCardInfo()
    {
        ValueChangeArgs ve = new ValueChangeArgs(UIGameModelConst.KEY_RefreshCardInfo, null, null);
        DispatchValueUpdateEvent(ve);
    }

    public void RefreshRoomMsgInfo(RoomMsgInfo msg)
    {
        ValueChangeArgs ve = new ValueChangeArgs(UIGameModelConst.KEY_RoomMsgInfo, msg, msg);
        DispatchValueUpdateEvent(ve);
    }
}
