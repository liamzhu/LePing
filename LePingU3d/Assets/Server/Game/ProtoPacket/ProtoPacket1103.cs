using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1103Packet
{

}

[Serializable, ProtoContract]
public class Response1103Packet
{
    [ProtoMember(1)]
    public int Result { get; set; }
}
