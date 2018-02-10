using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

[Serializable, ProtoContract]
public class ProtoHeadPack
{
    [ProtoMember(2)]
    public int ActionId { get; set; }

    [ProtoMember(1)]
    public int MsgId { get; set; }

    [ProtoMember(3)]
    public string SessionId { get; set; }

    [ProtoMember(4)]
    public int UserId { get; set; }
}

[ProtoContract]
public class ResponsePack
{
    [ProtoMember(1)]
    public int MsgId { get; set; }

    [ProtoMember(2)]
    public int ActionId { get; set; }

    [ProtoMember(3)]
    public int ErrorCode { get; set; }

    [ProtoMember(4)]
    public string ErrorInfo { get; set; }

    [ProtoMember(5)]
    public string St { get; set; }
}
