using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1102Packet
{
    [ProtoMember(1)]
    public int RoomNo { get; set; }
}

[Serializable, ProtoContract]
public class Response1102Packet
{
    [ProtoMember(1)]
    public string Result { get; set; }
}
