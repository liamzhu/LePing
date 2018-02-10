using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 房间 </summary>
[Serializable, ProtoContract]
public class RoomInfo
{
    /// <summary> 房间容纳最大的人数 </summary>
    [ProtoMember(2)]
    public int Capacity { get; set; }

    /// <summary> 总局数 </summary>
    [ProtoMember(3)]
    public int SeqCount { get; set; }

    /// <summary> 当前局数 </summary>
    [ProtoMember(4)]
    public int SeqCurrent { get; set; }

    /// <summary> 房间编号 </summary>
    [ProtoMember(5)]
    public int RoomsNo { get; set; }

    /// <summary> 房间状态 </summary>
    [ProtoMember(6)]
    public RoomState State { get; set; }

    /// <summary> 房间、游戏设置 </summary>
    [ProtoMember(7)]
    public RoomSetting Setting { get; set; }

    /// <summary> 上一个赢得玩家 </summary>
    [ProtoMember(8)]
    public int LastWinner { get; set; }

    /// <summary> 房主ID </summary>
    [ProtoMember(9)]
    public int OwnerId { get; set; }

}

/// <summary> 房间设置 </summary>
[Serializable, ProtoContract]
public class RoomSetting
{
    /// <summary> 所选游戏 </summary>
    [ProtoMember(1)]
    public GameType Game;

    /// <summary> 房间容纳最大的人数序号 </summary>
    [ProtoMember(2)]
    public int PlayerNo;

    /// <summary> 总局数序号 </summary>
    [ProtoMember(3)]
    public int SeqCount;

    /// <summary> 其他参数 </summary>
    public Dictionary<string, int> Others;

    public override string ToString()
    {
        return string.Format("{0},{1} {2}局", (LPGJ_DianPaoHu)Others.TryGetValue(RoomSettingConst.LPGJ_DianPaoHu), LPGJ_ScorePowerStrs[Others.TryGetValue(RoomSettingConst.LPGJ_ScorePower)], SeqCount == 0 ? 8 : 16);
    }

    public readonly string[] LPGJ_ScorePowerStrs = new string[] { "底分1分", "底分2.5分", "底分5分", "底分10分" };

    public enum LPGJ_ScorePower
    {
        底分1分,
        底分2分,
        底分5分,
        底分10分,
        底分20分
    }

    public enum LPGJ_DianPaoHu
    {
        点炮胡,
        自摸胡,
    }

    public enum DGSJ_DianPaoHu
    {
        点炮胡,
        自摸胡,
    }

    public enum DGSJ_YouJingDianPao
    {
        有精点炮不能平胡,
        可平胡,
    }

    public const string DGSJ_JingBiDiao = ",精必钓";
    public const string DGSJ_ChaoZhuang = ",抄庄";

    public enum GZCG_ScorePower
    {
        底分1分,
        底分2分,
    }

    public enum GZCG_EndOpenCard
    {
        上下左右翻精,
        上下翻精,
    }

    public const string GZCG_JingBiDiao = ",精必钓";
    public const string GZCG_YouJingDianPao = ",有精点炮不能平胡";

    public enum CZMJ_DianPaoHu
    {
        吃胡,
        自摸胡,
    }

    public enum CZMJ_EndOpenCard
    {
        无马,
        奖2马,
        奖5马,
        奖8马
    }
}

public class RoomSettingConst
{
    public const string LPGJ_ScorePower = "ScorePower";
    public const string LPGJ_DianPaoHu = "DianPaoHu";

    public const string DGSJ_DianPaoHu = "DianPaoHu";
    public const string DGSJ_YouJingDianPao = "YouJingDianPao";
    public const string DGSJ_JingBiDiao = "JingBiDiao";
    public const string DGSJ_ChaoZhuang = "ChaoZhuang";

    public const string GZCG_EndOpenCard = "EndOpenCardNo";
    public const string GZCG_ScorePower = "ScorePower";
    public const string GZCG_JingBiDiao = "JingBiDiao";
    public const string GZCG_YouJingDianPao = "YouJingDianPao";

    public const string CZMJ_EndOpenCard = "EndOpenCardNo";
    public const string CZMJ_DianPaoHu = "DianPaoHu";
}

public enum GameType
{
    DGSJ,
    GZCG,
    LPGJ,
    CZMJ,
}

public enum GameNoType
{
    DGSJ,
}

public enum RoomState
{
    None,
    Gaming,
    RoundEnd,
    End,
}

/// <summary>
/// 房间消息类
/// </summary>
[Serializable, ProtoContract]
public class RoomMsgInfo
{
    /// <summary> 用户ID </summary>
    [ProtoMember(1)]
    public int UID { get; set; }

    /// <summary> 消息类型 </summary>
    [ProtoMember(2)]
    public RoomMsgType MsgType { get; set; }

    /// <summary> 消息内容 </summary>
    [ProtoMember(3)]
    public byte[] Content { get; set; }

    public RoomMsgInfo()
    {

    }

    public RoomMsgInfo(int id, RoomMsgType type, byte[] bytes)
    {
        this.UID = id;
        this.MsgType = type;
        this.Content = bytes;
    }
}

public class MsgInfo
{
    public int MsgIndex;
    public string MsgContent;

    public MsgInfo()
    {

    }

    public MsgInfo(int index, string msg)
    {
        this.MsgIndex = index;
        this.MsgContent = msg;
    }
}

public enum RoomMsgType
{
    Text,
    Sound,
    Image,
    SimpleText,
    Prop
}

#region 房间玩家行为 RoomUserActionResp

[Serializable, ProtoContract]
public class RoomUserActionResp
{
    [ProtoMember(1)]
    public int UserID;

    [ProtoMember(2)]
    public RoomUserActionType ActionType;
}

public enum RoomUserActionType
{
    /// <summary> 加入房间 </summary>
    Join,

    /// <summary> 离开房间 </summary>
    Leave,

    /// <summary> 被踢出房间 </summary>
    Cull,

    /// <summary> 准备 </summary>
    Ready,

    /// <summary> 取消准备 </summary>
    NoReady,

    /// <summary> 申请解散房间 </summary>
    RequestLeave,

    StartGame,

    /// <summary> 玩家掉线 </summary>
    UserOffLine,

    /// <summary> 玩家上线 </summary>
    UserOnLine,
}

#endregion 房间玩家行为 RoomUserActionResp
