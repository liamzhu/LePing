public enum ActionType
{
    /// <summary> 排行榜 </summary>
    UserRanking = 1001,

    /// <summary> 用户注册 </summary>
    Regist = 1002,

    /// <summary> 用户登录 </summary>
    Login = 1004,

    /// <summary> 创建角色 </summary>
    CreatUser = 1005,

    /// <summary> 游戏公告 </summary>
    Notice = 1007,

    /// <summary> 角色信息 </summary>
    UserInfo = 1008,

    /// <summary> 推荐码 </summary>
    ReferralCode = 1009,

    /// <summary> 创建房间 </summary>
    CreatRoom = 1101,

    /// <summary> 加入房间 </summary>
    JoinRoom = 1102,

    /// <summary> 离开房间 </summary>
    LeaveRoom = 1103,

    /// <summary> 踢出房间 </summary>
    FireRoom = 1104,

    /// <summary> 准备游戏 </summary>
    RoomReady = 1105,

    /// <summary> 获取房间信息 </summary>
    RoomInfo = 1106,

    /// <summary> 获取房间玩家信息 </summary>
    RoomUsers = 1107,

    /// <summary> 房间聊天、通知 </summary>
    RoomChatReq = 1108,

    /// <summary> 房间聊天、通知 </summary>
    RoomChatResp = 1109,

    /// <summary> 房间玩家行为 </summary>
    RoomUserAction = 1110,

    /// <summary> 开始游戏 </summary>
    StartGame = 1111,

    /// <summary> 申请解散房间 </summary>
    DissolveRoom = 1112,

    /// <summary> 用户游戏检测结果推送 </summary>
    GameCheck = 1200,

    /// <summary> 用户游戏行为 </summary>
    GameAction = 1201,

    /// <summary> 打完一局消息推送 </summary>   
    RoundEnd = 1202,

    /// <summary> 游戏结束消息推送 </summary>
    GameEnd = 1203,

    /// <summary> 重连游戏消息推送 </summary>
    ReConnect = 1205,

    /// <summary> Email </summary>
    Email = 1303,

    /// <summary> 回放记录查询 </summary>
    GameRecord = 1208,

    /// <summary> 游戏回放数据 </summary>
    GameRecordData = 1209,

	/// <summary> 游戏回放数据 </summary>
	GameActionRecord = 1210,

    /// <summary> 兑换礼品 </summary>
    GiftExchange = 1301,

    /// <summary> 兑换记录 </summary>
    GiftRecord = 1302,

    /// <summary> 发送位置 </summary>
    SendLocation = 1400,

    /// <summary> 获取位置 </summary>
    GetLocation = 1401

}
