using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1008Packet
{

}

[Serializable, ProtoContract]
public class Response1008Packet
{
    [ProtoMember(1)]
    public int UserId { get; set; }

    [ProtoMember(2)]
    public string NickName { get; set; }

    /// <summary> 性别 </summary>
    [ProtoMember(5)]
    public int Sex { get; set; }

    /// <summary> 体力 </summary>
    [ProtoMember(6)]
    public int Action { get; set; }

    /// <summary> 经验 </summary>
    [ProtoMember(7)]
    public int Exp { get; set; }

    /// <summary> 等级 </summary>
    [ProtoMember(8)]
    public int Lv { get; set; }

    /// <summary> 积分 </summary>
    [ProtoMember(9)]
    public int Gold { get; set; }

    /// <summary> 钻石 </summary>
    [ProtoMember(10)]
    public int Ingot { get; set; }

    [ProtoMember(13)]
    public int PowerLevel { get; set; }

    [ProtoMember(14)]
    public string LoginIP { get; set; }

    [ProtoMember(15)]
    public int Active { get; set; }

    [ProtoMember(16)]
    public int RoomNo { get; set; }

    [ProtoMember(17)]
    public int RoomScore { get; set; }
}
