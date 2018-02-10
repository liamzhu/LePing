using ProtoBuf;
using System;
using System.Collections.Generic;

[Serializable, ProtoContract]
public class Request1107Packet
{

}

[Serializable, ProtoContract]
public class Response1107Packet
{
    [ProtoMember(1)]
    public int Result { get; set; }

    [ProtoMember(2)]
    public List<UserInfo> UserInfos { get; set; }

    public bool Success
    {
        get { return Result == MyActionResult.Success; }
    }
}
