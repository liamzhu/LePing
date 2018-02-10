using System;
using ProtoBuf;

[Serializable, ProtoContract]
public class BaseRespPacket
{
    [ProtoMember(1)]
    public int ResultCode { get; set; }

    public virtual bool Success
    {
        get { return ResultCode == MyActionResult.Success; }
    }

}
