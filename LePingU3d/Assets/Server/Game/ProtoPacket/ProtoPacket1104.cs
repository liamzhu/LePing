using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1104Packet
{
    [ProtoMember(1)]
    public int UserID { get; set; }

}

[Serializable, ProtoContract]
public class Response1104Packet
{
    [ProtoMember(1)]
    public int Result { get; set; }

}
