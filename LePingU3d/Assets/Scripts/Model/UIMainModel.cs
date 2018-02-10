using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainModelConst
{
    public const string KEY_NoticeMsg = "NoticeMsg";
    public const string KEY_UserInfo = "UserInfo";
    public const string KEY_EmailInfo = "EmailInfo";
	public const string KEY_GameRecord = "GameRecord";
}

public class UIMainModel : UIBaseModel
{
    private List<ShareInfo> ShareInfos { get; set; }

    public void SetShareInfos(List<ShareInfo> shares)
    {
        ShareInfos = shares;
    }

    public ShareInfo GetShareInfo(ShareType type)
    {
        if (!ShareInfos.IsNullOrEmpty()) { return ShareInfos.Find(p => p.Type == type); }
        else { return null; }
    }

    private EmailResp emailInfo;

    public EmailResp EmailInfo
    {
        get
        {
            return emailInfo;
        }
        set
        {
            ValueChangeArgs ve = new ValueChangeArgs(UIMainModelConst.KEY_EmailInfo, emailInfo, value);
            emailInfo = value;
            DispatchValueUpdateEvent(ve);
        }
    }

    private string noticeMsg;

    public string NoticeMsg
    {
        get
        {
            return noticeMsg;
        }
        set
        {
            ValueChangeArgs ve = new ValueChangeArgs(UIMainModelConst.KEY_NoticeMsg, noticeMsg, value);
            noticeMsg = value;
            DispatchValueUpdateEvent(ve);
        }
    }

    public WeChatUserResp WeChatUserResp { get; private set; }

    public void SetWeChatUserResp(WeChatUserResp resp)
    {
        WeChatUserResp = resp;
    }

    public SystemInfoReq SystemInfoReq { get; set; }

    public string Pwd
    {
        get { return "W123J8a"; }
    }

    private UserInfo mUserInfo;

    public UserInfo PlayerInfo
    {
        get
        {
            return mUserInfo;
        }
        set
        {
            ValueChangeArgs ve = new ValueChangeArgs(UIMainModelConst.KEY_UserInfo, mUserInfo, value);
            mUserInfo = value;
            DispatchValueUpdateEvent(ve);
        }
    }

	private GameRecordsResp mGameRecordsResp;

	public GameRecordsResp GameRecordRespInfo
	{
		get
		{
			return mGameRecordsResp;
		}
		set
		{
			ValueChangeArgs ve = new ValueChangeArgs(UIMainModelConst.KEY_GameRecord, mGameRecordsResp, value);
			mGameRecordsResp = value;
			DispatchValueUpdateEvent(ve);
		}
	}

}
