using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1004Packet
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
public class Response1004Packet
{
    [ProtoMember(1)]
    public int Result { get; set; }

    [ProtoMember(2)]
    public LoginResp LoginResp { get; set; }

    public bool Success
    {
        get { return Result == MyActionResult.Success; }
    }

    public bool SuccessOrUpdate
    {
        get { return Result == MyActionResult.Success || Result == 9002; }
    }
}
