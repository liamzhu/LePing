using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1007Packet
{

}

[Serializable, ProtoContract]
public class Response1007Packet
{
    [ProtoMember(1)]
    public string SessionID { get; set; }

    [ProtoMember(2)]
    public string UserID { get; set; }

    [ProtoMember(3)]
    public int UserType { get; set; }

    [ProtoMember(4)]
    public string LoginTime { get; set; }

    [ProtoMember(5)]
    public int GuideID { get; set; }

    [ProtoMember(6)]
    public string PassportId { get; set; }

    [ProtoMember(7)]
    public string RefeshToken { get; set; }

    [ProtoMember(8)]
    public string QihooUserID { get; set; }

    [ProtoMember(9)]
    public string Scope { get; set; }
}
