using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1105Packet
{
    [ProtoMember(1)]
    public int UserID { get; set; }

}

[Serializable, ProtoContract]
public class Response1105Packet
{
    [ProtoMember(1)]
    public int Result { get; set; }
}
