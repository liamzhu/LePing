public class MyActionResult
{
    /// <summary> 所有Action成功结果 </summary>
    public const int Success = 1000;

    /// <summary> 已在房间中 </summary>
    public const int AlreadyInRoom = 1010;

    /// <summary> 创建房间失败 </summary>
    public const int CreateRoomFailed = 1011;

    /// <summary> 加入房间失败 </summary>
    public const int JoinRoomFailed = 1012;

    /// <summary> 房间不存在 </summary>
    public const int NoThisRoom = 1013;

    /// <summary> 离开房间失败 </summary>
    public const int LeaveRoomFailed = 1014;

    /// <summary> 不在房间中 </summary>
    public const int NotInRoom = 1015;

    /// <summary> 该用户不在房间中 </summary>
    public const int UserNotInRoom = 1016;

    /// <summary> 踢人失败 </summary>
    public const int FireRoomFailed = 1017;

    /// <summary> 不是房主 </summary>
    public const int NotBeOwner = 1018;

    /// <summary> 准备、取消准备失败 </summary>
    public const int ReadyFailed = 1019;

    /// <summary> 获取房间信息失败 </summary>
    public const int RoominfoFailed = 1020;

    /// <summary> 获取房间消息失败 </summary>
    public const int RoomMsgFailed = 1021;

    /// <summary> 获取房间消息失败 </summary>
    public const int UserInfoFailed = 1022;

    /// <summary> 正在游戏中 </summary>
    public const int IsGaming = 1023;

    /// <summary> 开始游戏失败 </summary>
    public const int StartGameFailed = 1024;

    /// <summary> 游戏行为失败 </summary>
    public const int GameActionFailed = 1101;

    /// <summary> 房间不在游戏状态中 </summary>
    public const int NotInGame = 1102;
}
