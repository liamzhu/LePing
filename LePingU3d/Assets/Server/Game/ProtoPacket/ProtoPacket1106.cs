using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1106Packet
{

}

[Serializable, ProtoContract]
public class Response1106Packet
{
    [ProtoMember(1)]
    public int Result { get; set; }

    [ProtoMember(2)]
    public RoomInfo RoomInfo { get; set; }

    public bool Success
    {
        get { return Result == MyActionResult.Success; }
    }
}
