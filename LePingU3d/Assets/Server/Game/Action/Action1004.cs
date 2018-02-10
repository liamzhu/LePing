using System;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1004 : GameAction
{
    private UIMainModel mUIMainModel;
    private Response1004Packet responsePack;

    public Action1004()
        : base(1004)
    {
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        }
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        string pwd = new DESAlgorithmNew().EncodePwd(mUIMainModel.Pwd, AppConst.ClientPasswordKey);
        writer.writeString("UserName", mUIMainModel.PlayerInfo.NickName);
        writer.writeInt32("Sex", mUIMainModel.PlayerInfo.Sex);
        writer.writeString("HeadID", mUIMainModel.PlayerInfo.HeadUrl == null ? "" : mUIMainModel.PlayerInfo.HeadUrl);
        writer.writeInt32("MobileType", mUIMainModel.SystemInfoReq.MobileType);
        writer.writeString("Pid", mUIMainModel.SystemInfoReq.PassportID);
        writer.writeString("Pwd", pwd);
        writer.writeString("DeviceID", mUIMainModel.SystemInfoReq.DeviceID);
        writer.writeInt32("ScreenX", mUIMainModel.SystemInfoReq.ScreenX);
        writer.writeInt32("ScreenY", mUIMainModel.SystemInfoReq.ScreenY);
        writer.writeString("RetailID", mUIMainModel.SystemInfoReq.RetailID);
        writer.writeInt32("GameType", mUIMainModel.SystemInfoReq.GameType);
        writer.writeInt32("ServerID", mUIMainModel.SystemInfoReq.ServerID);
        writer.writeString("RetailUser", "");
        writer.writeString("ClientAppVersion", mUIMainModel.SystemInfoReq.ClientAppVersion);
    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new Response1004Packet()
        {
            Result = reader.getInt()
        };
        if (responsePack.SuccessOrUpdate)
        {
            responsePack.LoginResp = JsonUtil.DeserializeObject<LoginResp>(reader.readString());
            if (responsePack.Success)
            {
                UIModelMgr.Instance.GetModel<UIMainModel>().SetShareInfos(responsePack.LoginResp.ShareInfos);
                NetWriter.setUserID(ulong.Parse(responsePack.LoginResp.UserId));
                NetWriter.setSessionID(responsePack.LoginResp.SessionId);

				Debug.Log ("SessionId ================= "+responsePack.LoginResp.SessionId);

                Net.Instance.ReBuildHearbeat();
                UIModelMgr.Instance.GetModel<UIGameModel>().ClearGameEndDatas();
            }
            else
            {
                UIDialogMgr.Instance.ShowDialog(responsePack.Result, delegate (GameObject go) { Application.OpenURL(responsePack.LoginResp.ApkUpdateWebsite); });
            }
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog(responsePack.Result);
        }
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
