using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class ResponseDefaultPacket
{
    [ProtoMember(1)]
    public int Result { get; set; }

    public bool Success
    {
        get { return Result == MyActionResult.Success; }
    }
}
