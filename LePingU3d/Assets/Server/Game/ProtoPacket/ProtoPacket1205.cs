using ProtoBuf;
using System;
using System.Collections.Generic;

[Serializable, ProtoContract]
public class Request1205Packet
{

}

[Serializable, ProtoContract]
public class Response1205Packet
{
    [ProtoMember(1)]
    public int Result { get; set; }

    [ProtoMember(2)]
    public ReConnectDataResp GameData { get; set; }

    public bool Success
    {
        get { return Result == MyActionResult.Success; }
    }
}
