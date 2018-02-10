using System.Collections;
using UnityEngine;

/// <summary>
/// 客户端发送协议
/// </summary>
public enum NetPMSG
{
    AMSG_VERSION = 5100,

    /// <summary>
    /// PMSG_LOGIN：{username:用户名,password:密码}
    /// </summary>
    AMSG_LOGIN = 5101,

    /// <summary>
    /// 心跳
    /// * PMSG_PING:主机ping服务器（4字节）上次延时数据（初始化数据为0）
    /// </summary>
    AMSG_PING = 5102,

    /// <summary>
    /// 修改密码{logonName:用户登录账户,password:原始密码，newPassword：新密码}
    /// </summary>
    AMSG_UPDATE_PWD = 5103,

    /// <summary>
    /// 获取代理商下的用户列表  {pageNo:当前页码,pageSize:每页条数}
    /// </summary>
    AMSG_AGENT_USERLIST = 5104,

    /// <summary>
    /// 获取用户参数 AMSG_USER_PARAM：{username:用户名}
    /// </summary>
    AMSG_USER_PARAM = 5105,

    /// <summary>
    /// 请求用户上下分记录 {username:用户名,pageNo:当前页码,pageSize:每页条数}
    /// </summary>
    AMSG_USER_RECORDS = 5106,

    /// <summary>
    /// 冻结用户账户 {username：用户名}
    /// </summary>
    AMSG_FROZEN_USER_ACCOUNT = 5107,

    /// <summary>
    /// 解冻用户账户 {username：用户名}
    /// </summary>
    AMSG_THAW_USER_ACCOUNT = 5108,

    /// <summary>
    /// 请求上分 {"username":用户名,"upScore":分数}
    /// </summary>
    AMSG_UP_SCORE = 5109,

    /// <summary>
    /// 请求下分 {"username":用户名,"downScore":分数}
    /// </summary>
    AMSG_DOWN_SCORE = 5110,

    AMSG_AGENT_INFO = 5111,

    /// <summary>
    /// {username：用户名}
    /// </summary>
    AMSG_USER_INFO = 5112,

    /// <summary>
    /// 设置用户账目是否入账 {username：用户名}
    /// </summary>
    AMSG_USER_ACCOUNTS_INTO_RECORD = 3412,

    /// <summary>
    /// 获取代理商上下分记录  {pageNo:当前页码,pageSize:每页条数}
    /// </summary>
    AMSG_AGENT_RECORDS = 5113,

    /// <summary>
    /// AMSG_USER_PARAM_CONF:服务器主机响应上分{username:用户名,xianHong:限红,downScoreZore:下分清零,shuYingWarn:输赢报警,hongHeiMeiFangMaxBet：最大押分,wangMaxBet:王最大押分}
    /// </summary>
    AMSG_USER_PARAM_CONF = 5115,
}

/// <summary>
/// 服务器返回协议
/// </summary>
public enum NetSMSG
{
    SMSG_VERSION = 6100,

    /// <summary>
    /// SMSG_LOGIN:服务器主机响应主机登录{"resultCode":1000, agentInfo:代理商信息}  AgentInfo请看最下面
    /// 1000 登录成功 1001 密码错误 1002 用户名不存在
    /// </summary>
    SMSG_LOGIN = 6101,

    /// <summary>
    /// SMSG_PONG：服务器响应pong(4字节)
    /// </summary>
    SMSG_PONG = 6102,

    /// <summary>
    /// 返回代理商下对应页码的用户列表  {count:总条数,userList:[{username:用户名,currentScore：当前分数}]}
    /// </summary>
    SMSG_AGENT_USERLIST = 6104,

    /// <summary>
    ///服务器主机响应用户参数{username:用户名,xianHong:限红,downScoreZore:下分清零,shuYingWarn:输赢报警,hongHeiMeiFangMaxBet：最大押分,wangMaxBet:王最大押分}
    /// </summary>
    SMSG_USER_PARAM = 6105,

    /// <summary>
    /// 返回用户上下分记录 {username:xx,count:条数,currentScore:xx,scoreRecord:[{numId:xx, upScore:xxx, downScore:xx, createDate:创建时间}]}
    /// </summary>
    SMSG_USER_RECORDS = 6106,

    /// <summary>
    /// SMSG_OPERATION_STATE：服务返回操作状态（2字节）见
    /// </summary>
    SMSG_SERVER_STATE = 6210,

    /// <summary>
    /// 返还经销商信息{username:用户名,currentScore:当前分数，referralCode：推荐码，totalWin：总赢分}
    /// </summary>
    SMSG_AGENT_INFO = 6111,

    /// <summary>
    /// 返还经销商信息{username:用户名,currentScore:当前分数，referralCode：推荐码，totalUpScore:总上分, totalDownScore: 总下分，totalBetScore:总押分，totalWinScore：总奖分，totalSurplus：总盈余}
    /// </summary>
    SMSG_USER_INFO = 6112,

    /// <summary>
    /// 返回代理商上下分记录 {count:条数,scoreRecord:[{numId:xx,username:用户名,upScore:xxx,downScore:xx,createDate:创建时间}]}
    /// </summary>
    SMSG_AGENT_RECORDS = 6113,

    /// <summary>
    /// SMSG_UPDATE_USERPARS 返回修改状态 {"resultCode":1006}
    /// </summary>
    SMSG_UPDATE_USERPARS = 4406,
}

public enum NetSTATE
{
    NOT_SEARCH_PROTOCOL = 1020,//服务器找不到对应协议
    LOGIN_SUCCESS = 1000,// 登录成功
    LOGIN_PASSWD_ERROR = 1001,// 密码错误
    LOGIN_USERNAME_NOEXIST = 1002,// 用户名不存在

    UPDATE_PWD_SUCCESS = 1110,// 修改密码成功
    UPDATE_PWD_ERROR = 1111,// 原始密码错误
    UPDATE_PWD_FALLURE = 1112,// 修改密码失败

    FROZEN_USER_ACCOUNT_SUCCESS = 1120,//冻结用户账号成功
    FROZEN_USER_ACCOUNT_ILLEGAL = 1121,// 冻结账号错误
    FROZEN_USER_ACCOUNT_FALLURE = 1122,//冻结用户账号失败

    THAW_USER_ACCOUNT_SUCCESS = 1130,//解冻用户账号成功
    THAW_USER_ACCOUNT_ILLEGAL = 1131,// 解冻账号错误
    THAW_USER_ACCOUNT_FALLURE = 1132,//解冻用户账号失败

    UP_SCORE_SUCCESS = 1140,//上分成功
    UP_SCORE_ILLEGAL = 1141,// 上分错误
    UP_SCORE_FALLURE = 1142,//上分失败

    DOWN_SCORE_SUCCESS = 1150,// 下分成功
    DOWN_SCORE_ILLEGAL = 1151,// 下分错误
    DOWN_SCORE_FALLURE = 1152,// 下分失败

    USER_ACCOUNTS_INTO_RECORD_SUCCESS = 1014, //设置用户账号进入账目成功
    USER_ACCOUNTS_INTO_RECORD_FALLURE = 1015,//设置用户账号进入账目失败
    USER_ACCOUNTS_NO_INTO_RECORD_SUCCESS = 1016, //设置用户账号不进入账目成功
    USER_ACCOUNTS_NO_INTO_RECORD_FALLURE = 1017,//设置用户账号不进入账目失败

    UPDATE_USERPARS_SUCCESS = 1006,//更新用户参数成功
    UPDATE_USERPARS_FALLURE = 1007,//更新用户参数失败

    PARAM_CONF_SUCCESS = 1160,// 参数设置成功
    PARAM_CONF_ILLEGAL = 1161,// 参数设置错误
    PARAM_CONF_FALLURE = 1162,// 参数设置失败
}
