using System;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1005 : GameAction
{
    private Response1105Packet responsePack;
    private UIMainModel mUIMainModel;

    public Action1005()
        : base(1005)
    {
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        }
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeString("UserName", mUIMainModel.PlayerInfo.NickName);
        writer.writeInt32("Sex", mUIMainModel.PlayerInfo.Sex);
        writer.writeString("HeadID", mUIMainModel.PlayerInfo.HeadUrl == null ? "" : mUIMainModel.PlayerInfo.HeadUrl);
        writer.writeString("RetailID", mUIMainModel.SystemInfoReq.RetailID);
        writer.writeString("Pid", mUIMainModel.SystemInfoReq.PassportID);
        writer.writeInt32("MobileType", mUIMainModel.SystemInfoReq.MobileType);
        writer.writeInt32("ScreenX", mUIMainModel.SystemInfoReq.ScreenX);
        writer.writeInt32("ScreenY", mUIMainModel.SystemInfoReq.ScreenY);
        writer.writeString("ClientAppVersion", mUIMainModel.SystemInfoReq.ClientAppVersion);
        writer.writeInt32("GameType", mUIMainModel.SystemInfoReq.GameType);
        writer.writeInt32("ServerID", mUIMainModel.SystemInfoReq.ServerID);
    }

    protected override void DecodePackage(NetReader reader)
    {

    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
