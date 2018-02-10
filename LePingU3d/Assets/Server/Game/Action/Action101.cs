using UnityEngine;
//using Newtonsoft.Json;
using ZyGames.Framework.Common.Serialization;

public class Action101 : GameAction
{
    private string content;
    private ActionResult result = new ActionResult();

    public Action101()
        : base(101)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {
        RoomMsgInfo msg = new RoomMsgInfo() { Content = System.Text.Encoding.Default.GetBytes(actionParam.Get<string>("Content")), UID = 885999, };
        //writer.writeString("RoomMessage", JsonConvert.SerializeObject(msg));
		writer.writeString("RoomMessage", LitJson.JsonMapper.ToJson(msg));
    }

    protected override void DecodePackage(NetReader reader)
    {
        string msg = reader.readString();
        //RoomMsgInfo roomMessage = JsonConvert.DeserializeObject<RoomMsgInfo>(reader.readString());
		RoomMsgInfo roomMessage = LitJson.JsonMapper.ToObject<RoomMsgInfo>(reader.readString());

        //RoomMessage roomMessage = ProtoBufUtils.Deserialize<RoomMessage>(reader.readBytes());
        Debug.Log(msg);
        Debug.Log(roomMessage.UID);
        Debug.Log(System.Text.Encoding.Default.GetString(roomMessage.Content));
    }

    public override ActionResult GetResponseData()
    {
        return result;
    }
}
