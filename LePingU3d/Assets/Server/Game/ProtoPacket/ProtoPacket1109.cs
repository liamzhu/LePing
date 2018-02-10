using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1109Packet
{
    [ProtoMember(1)]
    public int MobileType { get; set; }

    [ProtoMember(2)]
    public int GameType { get; set; }

    [ProtoMember(3)]
    public string RetailID { get; set; }

    [ProtoMember(4)]
    public string ClientAppVersion { get; set; }

    [ProtoMember(5)]
    public int ScreenX { get; set; }

    [ProtoMember(6)]
    public int ScreenY { get; set; }

    [ProtoMember(7)]
    public string DeviceID { get; set; }

    [ProtoMember(8)]
    public string OpenID { get; set; }

    [ProtoMember(9)]
    public int ServerID { get; set; }

    [ProtoMember(10)]
    public string PassportID { get; set; }

    [ProtoMember(11)]
    public string Pwd { get; set; }

}

[Serializable, ProtoContract]
public class Response1109Packet
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
