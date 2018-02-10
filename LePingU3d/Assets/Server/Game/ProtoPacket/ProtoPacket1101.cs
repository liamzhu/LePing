using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1101Packet
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

}

[Serializable, ProtoContract]
public class Response1101Packet
{
    [ProtoMember(1)]
    public int Result { get; set; }

    [ProtoMember(2)]
    public int RoomNo { get; set; }

}
