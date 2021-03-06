﻿using ProtoBuf;
using System;

[Serializable, ProtoContract]
public class Request1002Packet
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
}

[Serializable, ProtoContract]
public class Response1002Packet
{
    [ProtoMember(1)]
    public string PassportID { get; set; }
}
