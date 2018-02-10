using System;
using UnityEngine;
using ZyGames.Framework.Common.Serialization;

public class Action1002 : GameAction
{
    private UIMainModel mUIMainModel;
    private Response1002Packet responsePack;
    private ActionResult actionResult;

    public Action1002()
        : base(1002)
    {
        if (mUIMainModel == null)
        {
            mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        }
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        writer.writeInt32("MobileType", mUIMainModel.SystemInfoReq.MobileType);
        writer.writeInt32("GameType", mUIMainModel.SystemInfoReq.GameType);
        writer.writeString("RetailID", mUIMainModel.SystemInfoReq.RetailID);
        writer.writeString("ClientAppVersion", mUIMainModel.SystemInfoReq.ClientAppVersion);
        writer.writeInt32("ScreenX", mUIMainModel.SystemInfoReq.ScreenX);
        writer.writeInt32("ScreenY", mUIMainModel.SystemInfoReq.ScreenY);
        writer.writeString("DeviceID", mUIMainModel.SystemInfoReq.DeviceID);
        writer.writeString("OpenID", mUIMainModel.SystemInfoReq.OpenID);
        writer.writeInt32("ServerID", mUIMainModel.SystemInfoReq.ServerID);
    }

    protected override void DecodePackage(NetReader reader)
    {
        responsePack = new Response1002Packet()
        {
            PassportID = reader.readString()
        };
    }

    public override ActionResult GetResponseData()
    {
        return new ActionResult(responsePack);
    }
}
