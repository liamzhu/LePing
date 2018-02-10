using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 微信分享的结构 ShareInfo

public class ShareInfo
{
    public int ID;
    public ShareType Type;
    public string Title;
    public string Content;
    public string Url;
}

public enum ShareType
{
    Friend = 1,
    Circles = 2,
    Room = 3,
}

#endregion 微信分享的结构 ShareInfo

#region 微信返回的数据结构 WeChatUserResp

public class WeChatUserResp
{
    public string openid;//普通用户的标识，对当前开发者帐号唯一
    public string nickname;
    public int sex;//普通用户性别，1为男性，2为女性
    public string province;//普通用户个人资料填写的省份
    public string city;//普通用户个人资料填写的城市
    public string country;//国家，如中国为CN
    public string headimgurl;//用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
    public string unionid;//用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的。
}

#endregion 微信返回的数据结构 WeChatUserResp

#region 登录返回的数据

[System.Serializable]
public class LoginResp
{
    public string SessionId;
    public string UserId;
    public int UserType;
    public string MathUtils;
    public int GuideId;
    public string PassportId;
    public string AccessToken360;
    public string RefeshToken;
    public string QihooUserID;
    public string Scope;
    public string ApkUpdateWebsite;
    public List<ShareInfo> ShareInfos;
}

#endregion 登录返回的数据

#region 玩家用户数据

[Serializable, ProtoContract]
public class UserInfo
{
    [ProtoMember(1)]
    public int UserId { get; set; }

    [ProtoMember(2)]
    public String NickName { get; set; }

    /// <summary> 性别 </summary>
    [ProtoMember(3)]
    public int Sex { get; set; }

    /// <summary> 体力 </summary>
    [ProtoMember(4)]
    public int Action { get; set; }

    /// <summary> 经验 </summary>
    [ProtoMember(5)]
    public int Exp { get; set; }

    /// <summary> 等级 </summary>
    [ProtoMember(6)]
    public int Lv { get; set; }

    /// <summary> 金币 </summary>
    [ProtoMember(7)]
    public int Gold { get; set; }

    /// <summary> 元宝 </summary>
    [ProtoMember(8)]
    public int Ingot { get; set; }

    [ProtoMember(9)]
    /// <summary> 准备状态 </summary>
    public bool IsReady = false;

    [ProtoMember(10)]
    /// <summary> 座位号 </summary>
    public int RoomOrder { get; set; }

    /// <summary> 房间累计分数 </summary>
    [ProtoMember(11)]
    public int RoomScore { get; set; }

    /// <summary> 登录地址 </summary>
    [ProtoMember(12)]
    public string LoginIP { get; set; }

    /// <summary> 头像 </summary>
    [ProtoMember(13)]
    public string HeadUrl { get; set; }

    /// <summary> 房间号 </summary>
    [ProtoMember(14)]
    public int RoomNumber { get; set; }

    /// <summary> 是否在线 </summary>
    [ProtoMember(15)]
    public bool IsOnlining { get; set; }

    /// <summary> 房卡 </summary>
    [ProtoMember(16)]
    public int RoomCard { get; set; }

    /// <summary> 房卡 </summary>
    [ProtoMember(17)]
    public string AgentID { get; set; }

    /// <summary> 是否是庄 </summary>
    [ProtoMember(18)]
    public bool IsZhuang { get; set; }

    /// <summary> 是否是房主 </summary>
    [ProtoMember(19)]
    public bool IsOwner { get; set; }

    public UserInfo()
    {
    }

    public UserInfo(string name, string imgUrl, int sex = 1)
    {
        this.NickName = name;
        this.HeadUrl = imgUrl;
        this.Sex = sex;
    }

    public UserInfo(string name, int sex = 1)
    {
        this.NickName = name;
        this.Sex = sex;
    }

    public UserInfo getCopy()
    {
        return (UserInfo)MemberwiseClone();
    }
}

#endregion 玩家用户数据

[Serializable, ProtoContract]
public class SystemInfoReq
{
    public string Pwd;
    public string PassportID;
    public int MobileType;
    public string DeviceID;
    public string OpenID;
    public int ScreenX;
    public int ScreenY;
    public string RetailID;
    public int GameType;
    public int ServerID;
    public string RetailUser;
    public string ClientAppVersion;

    public SystemInfoReq()
    {

    }

    public SystemInfoReq(int mMobileType, string mDeviceID, string mOpenID, int mScreenX, int mScreenY, string mRetailID, int mGameType, int mServerID, string mClientAppVersion, string mRetailUser)
    {
        this.MobileType = mMobileType;
        this.DeviceID = mDeviceID;
        this.OpenID = mOpenID;
        this.ScreenX = mScreenX;
        this.ScreenY = mScreenY;
        this.RetailID = mRetailID;
        this.GameType = mGameType;
        this.ServerID = mServerID;
        this.ClientAppVersion = AppConst.AppVersion;
        this.RetailUser = mRetailUser;
    }

    public SystemInfoReq(string mOpenID, string mRetailID = "0000", int mServerID = 1, string mRetailUser = "")
    {
        this.MobileType = 1;
        this.DeviceID = "";
        this.OpenID = mOpenID;
        this.ScreenX = 0;
        this.ScreenY = 0;
        this.RetailID = mRetailID;
        this.GameType = 0;
        this.ServerID = mServerID;
        this.ClientAppVersion = AppConst.AppVersion;
        this.RetailUser = mRetailUser;
    }

    public static SystemInfoReq Init()
    {
        return new SystemInfoReq(1, "", "kkk200", 0, 0, "0000", 1, 1, AppConst.AppVersion, "");
    }
}

#region 邮件 EmailResp

public class EmailResp
{
    public List<EmailInfo> Emails;
}

[Serializable, ProtoContract]
public class EmailInfo
{
    [ProtoMember(1)]
    public int ID { get; set; }

    [ProtoMember(2)]
    public int UserID { get; set; }

    [ProtoMember(3)]
    public DateTime Time { get; set; }

    /// <summary> 简介 </summary>
    [ProtoMember(4)]
    public string Brief { get; set; }

    /// <summary> 内容 </summary>
    [ProtoMember(5)]
    public string Content { get; set; }
}

#endregion 邮件 EmailResp
