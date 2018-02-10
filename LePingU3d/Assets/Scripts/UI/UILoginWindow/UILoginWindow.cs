/********************************************************
 *Copyright(C) 2015 by DefaultCompany
 *All rights reserved.
 *FileName: UILoginWindow.cs
 *Author: DefaultCompany
 *Version: 1.0
 *UnityVersion: 5.4.1p4
 *Date: 2016-11-28
 *Description:
 *History:
*********************************************************/

using cn.sharesdk.unity3d;
using System;
using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;

public class UILoginWindow : UIBasePanel
{

    public UILoginWindow() : base(new UIProperty(UIWindowStyle.Normal, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UILoginWindow"))
    {
    }

    private UIToggle mSwitchAccount;
    private UIToggle mCheckAgreement;
    private Transform mLoginGroup;
    private Transform mAgreementGroup;
    private UILabel mLabel;
    private UILabel mAppVersion;
    private UIMainModel mUIMainModel;
    private OperationEffectItem mOperationEffectItem;
    private UIInput mUIInput;

    protected override void OnAwakeInitUI()
    {
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        }
        mUIInput = CacheTrans.FindComponent<UIInput>("Root/ContainerBottom/Input");
        mLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerBottom/LoginGroup/CheckAgreement/Label");
        mAppVersion = CacheTrans.FindComponent<UILabel>("Root/ContainerBottom/AppVersion");
        mSwitchAccount = CacheTrans.FindComponent<UIToggle>("Root/ContainerBottom/LoginGroup/SwitchAccount");
        mCheckAgreement = CacheTrans.FindComponent<UIToggle>("Root/ContainerBottom/LoginGroup/CheckAgreement");
        mLoginGroup = CacheTrans.FindComponent<Transform>("Root/ContainerBottom/LoginGroup");
        mAgreementGroup = CacheTrans.FindComponent<Transform>("Root/ContainerCenter/AgreementGroup");
        mOperationEffectItem = CacheTrans.FindComponent<OperationEffectItem>("Root/Sprite");
        CacheTrans.GetUIEventListener<UIButton>("Root/ContainerBottom/LoginGroup/BtnLogin").onClick += LoginClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/ContainerCenter/AgreementGroup/BtnCanel").onClick += AgreementButtonClick;
        CacheTrans.GetUIEventListener<UIButton>("Root/ContainerCenter/AgreementGroup/BtnConfirm").onClick += AgreementButtonClick;
        ApplicationMgr.Instance.ShareSDK.authHandler += OnAuthResultHandler;
        ApplicationMgr.Instance.ShareSDK.showUserHandler += OnGetUserInfoResultHandler;
    }

    public override void OnRefresh()
    {
        ResetGroupState();
    }

    private void LoginClick(GameObject go)
    {
        if (!mCheckAgreement.value)
        {
            mAgreementGroup.gameObject.SetActive(true);
            return;
        }

        Debug.Log("点击登录按钮");
        Login_PackModeClick();
    }

    private void Login_PackModeClick()
    {
        if (ApplicationMgr.Instance.GamePackMode == ApplicationMgr.PackMode.None)
        {
            string data = "{\"country\":\"CN\",\"province\":\"Guangdong\",\"headimgurl\":\"http://wx.qlogo.cn/mmopen/HmVQlX9WkBuWU6xey4ocGHewjEwC7lr3picwqubjzUibZ8PEiaSlqL9yhfm3y17t5BaaJJYaEqDTiapKQkz2h53GmNY3De6kJlAQ/0\",\"unionid\":\"oOvCQwEr1gmzhw3pFAsPbysCEC-A\",\"openid\":\"omFZUwOVvjp4_QznKyFZjW8tvZLo\",\"nickname\":\"take my hand222\\ue110\\ue329\",\"city\":\"Jiangmen\",\"sex\":2,\"language\":\"zh_CN\",\"privilege\":[]}";
            WeChatLoginClick(data);
            //mUIMainModel.SystemInfoReq = SystemInfoReq.Init();
            //mUIMainModel.PlayerInfo = new UserInfo("黑崎一护");
            //Net.Instance.Send((int)ActionType.Regist, OnRegistCallback, null);
        }
        else if (ApplicationMgr.Instance.GamePackMode == ApplicationMgr.PackMode.Random)
		{   //PlayerPrefsUtil.DeleteAll ();
			string passId = PlayerPrefsUtil.GetString(PassIdKey);
			string nickName = PlayerPrefsUtil.GetString(TempNickNameKey);
			string openID = PlayerPrefsUtil.GetString(OpenIdKey);
			if (string.IsNullOrEmpty (passId)) {
				int num = UnityEngine.Random.Range (10, 999);
				mUIMainModel.SystemInfoReq = new SystemInfoReq ("kkk200" + num);
				mUIMainModel.PlayerInfo = new UserInfo ("Test" + num);
				Net.Instance.Send ((int)ActionType.Regist, OnRegistCallback, null);
			}
			else {
				mUIMainModel.PlayerInfo = new UserInfo (nickName);
				mUIMainModel.SystemInfoReq = new SystemInfoReq (openID);
				mUIMainModel.SystemInfoReq.PassportID = passId;
				Net.Instance.Send((int)ActionType.Login, OnLoginCallback, null);
			}
        }
        else if (ApplicationMgr.Instance.GamePackMode == ApplicationMgr.PackMode.WeChat)
        {
            if (mSwitchAccount.value)
            {
                PlayerPrefsUtil.DeleteKey(UserInfoKey);
                ApplicationMgr.Instance.ShareSDK.CancelAuthorize(PlatformType.WeChat);
            }
            string data = PlayerPrefsUtil.GetString(UserInfoKey);
            if (data.IsNullOrEmpty())
            {
                ApplicationMgr.Instance.ShareSDK.Authorize(PlatformType.WeChat);
            }
            else
            {
                WeChatLoginClick(data);
            }
        }
    }

    private void WeChatLoginClick(string data)
    {
        string result = Regex.Replace(data, @"(\\ud[0-9a-f]{3})|(\\ue[0-9a-f]{3})", "", RegexOptions.IgnoreCase);
        mUIMainModel.SetWeChatUserResp(JsonUtil.DeserializeObject<WeChatUserResp>(result));

        mUIMainModel.SystemInfoReq = new SystemInfoReq(mUIMainModel.WeChatUserResp.openid);
        mUIMainModel.PlayerInfo = new UserInfo(mUIMainModel.WeChatUserResp.nickname, mUIMainModel.WeChatUserResp.headimgurl, mUIMainModel.WeChatUserResp.sex);
        Net.Instance.Send((int)ActionType.Regist, OnRegistCallback, null);
    }

    private void OnRegistCallback(ActionResult actionResult)
    {
        Response1002Packet mResponse1002Packet = actionResult.GetValue<Response1002Packet>();
        if (mResponse1002Packet != null)
        {
            mUIMainModel.SystemInfoReq.PassportID = mResponse1002Packet.PassportID;
            Net.Instance.Send((int)ActionType.Login, OnLoginCallback, null);

			//test
			PlayerPrefsUtil.SetString (PassIdKey, mUIMainModel.SystemInfoReq.PassportID);
			PlayerPrefsUtil.SetString (TempNickNameKey, mUIMainModel.PlayerInfo.NickName);
			PlayerPrefsUtil.SetString (OpenIdKey, mUIMainModel.SystemInfoReq.OpenID);
        }
    }

    private void OnLoginCallback(ActionResult actionResult)
    {
        Response1004Packet mResponse1004Packet = actionResult.GetValue<Response1004Packet>();
        if (mResponse1004Packet != null && mResponse1004Packet.Success)
        {
            mLoginGroup.gameObject.SetActive(false);
            GameMgr.Instance.EnterToLoadingWindow();//跳转界面
            Net.Instance.Send(mResponse1004Packet.LoginResp.GuideId, null, null);
        }
    }

    #region 微信回调

    private const string UserInfoKey = "WeChatUserInfo";

	private const string PassIdKey = "PassIdKey";
	private const string TempNickNameKey = "TempNickName";
	private const string OpenIdKey = "OpenIdKey";

    private void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            UIDialogMgr.Instance.ShowDialog("授权成功");
            ApplicationMgr.Instance.ShareSDK.GetUserInfo(PlatformType.WeChat);
        }
        else if (state == ResponseState.Fail)
        {

#if UNITY_ANDROID
            UIDialogMgr.Instance.ShowDialog(10018, null, null, "fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
            UIDialogMgr.Instance.ShowDialog(10018, null, null, "fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
        }
    }

    private void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            string data = MiniJSON.jsonEncode(result);
            PlayerPrefsUtil.SetString(UserInfoKey, data);
            WeChatLoginClick(data);
        }
        else if (state == ResponseState.Fail)
        {

#if UNITY_ANDROID
            UIDialogMgr.Instance.ShowDialog(10019, null, null, "fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
            UIDialogMgr.Instance.ShowDialog(10019, null, null, "fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
        }
    }

    #endregion 微信回调

    private void AgreementButtonClick(GameObject go)
    {
        mCheckAgreement.value = go.name.Equals("BtnConfirm") ? true : false;
        mAgreementGroup.gameObject.SetActive(false);
    }

    private void ResetGroupState()
    {
        mAppVersion.text = string.Format("V {0}", AppConst.AppVersion);
        mLoginGroup.gameObject.SetActive(true);
        mAgreementGroup.gameObject.SetActive(false);
    }
}
