using UnityEngine;
using System.Collections;

public class GameLogicMgr : Singleton<GameLogicMgr>
{
    private UIGameModel mUIGameModel;
    private UIMainModel mUIMainModel;

    /// <summary>
    /// Action1200 用户行为检测包
    /// </summary>
    /// <param name="action"></param>
    public void OperateCheckAction(OperationBehaviorResp action)
    {
        EventMgr.Instance.MainMgr.Dispatch<OperationBehaviorResp>(EventConst.EventOperationBehavior, action);
    }

    /// <summary>
    /// Action1111 开始游戏
    /// </summary>
    public void StartGame()
    {
        if (mUIGameModel.RoomInfo != null)
        {
            mUIGameModel.RoomInfo.SeqCurrent++;
        }
        EventMgr.Instance.MainMgr.Dispatch(EventConst.EventStartGame);
    }

    /// <summary>
    /// Action1110 用户行为
    /// </summary>
    /// <param name="action"></param>
    public void RoomUserActionOperation(RoomUserActionResp action)
    {
        Debug.Log(action.UserID + "  " + action.ActionType);
        switch (action.ActionType)
        {
            case RoomUserActionType.Join:
                break;
            case RoomUserActionType.Leave:
                mUIGameModel.RemoveUser(action.UserID);
                break;
            case RoomUserActionType.Cull:
                mUIGameModel.RoomActionCull();
                break;
            case RoomUserActionType.Ready:
                mUIGameModel.SetUserReadyState(action.UserID, true);
                break;
            case RoomUserActionType.NoReady:
                mUIGameModel.SetUserReadyState(action.UserID, false);
                break;
            case RoomUserActionType.RequestLeave:
                break;
            case RoomUserActionType.UserOffLine:
                mUIGameModel.SetUserNetState(action.UserID, false);
                break;
            case RoomUserActionType.UserOnLine:
                mUIGameModel.SetUserNetState(action.UserID, true);
                break;
            default:
                break;
        }
    }

    public void NotifyUserInfo(PlayerAction action)
    {
        EventMgr.Instance.MainMgr.Dispatch<PlayerAction>(EventConst.EventPlayerAction, action);
    }

    public void OperateGameEnd(GameEndActionResp resp)
    {
        EventMgr.Instance.MainMgr.Dispatch<GameEndActionResp>(EventConst.EventGameEnd, resp);
    }

    public void LeaveRoom()
    {
        EventMgr.Instance.MainMgr.Dispatch(EventConst.EventLeaveRoom);
    }

    public UserCardModel getUserCardModelByUserId(int id)
    {
        if (mUIGameModel.GetUser(id) != null)
        {
            //Debug.Log(getPlayerIndex(mUIGameModel.GetUser(id).RoomOrder));
            return mUIGameModel.getOrAddUserCardModel(getPlayerIndex(mUIGameModel.GetUser(id).RoomOrder));
        }
        return null;
    }

    public int getPlayerIndex(int roomOrder)
    {
		if (GameMgr.Instance.isFromRecord) {
			return (roomOrder + 4) % 4;
		}
        return (roomOrder - mUIMainModel.PlayerInfo.RoomOrder + 4) % 4;
    }

    public void OperateReconnectGameData(ReConnectDataResp mReConnectDataResp)
    {
        if (mReConnectDataResp != null)
        {
            if (!mReConnectDataResp.Actions.IsNullOrEmpty())
            {
                mReConnectDataResp.Actions.ForEach(p => mUIGameModel.HandleGameAction(p));
            }
            if (!mReConnectDataResp.Players.IsNullOrEmpty())
            {
                mReConnectDataResp.Players.ForEach(p =>
                {
                    if (mUIGameModel.GetUser(p.UID) != null)
                    {
                        UserCardModel user = mUIGameModel.getOrAddUserCardModel(getPlayerIndex(p.Order));
                        if (user != null)
                        {
                            user.setHandCards(p.HandCards);
                            user.setDeskCards(p.DeskCards);
                            if (p.CurCard != null)
                            {
                                user.setTouchCard(p.CurCard);
                            }
                            user.setHandCardGroups(p.HandCardGroups);
                        }
                    }
                });
            }
            mUIGameModel.RefreshCardInfo();
            mUIGameModel.ReconnectGameAction(mReConnectDataResp.LastAction);
        }
    }

    public override void OnInit()
    {
        if (mUIGameModel == null)
        {
            mUIGameModel = UIModelMgr.Instance.GetModel<UIGameModel>();
        }
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        }
        base.OnInit();
    }

    public override void OnUnInit()
    {
        base.OnUnInit();
    }
}
