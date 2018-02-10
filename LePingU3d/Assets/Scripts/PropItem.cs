using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PropItem : MonoBehaviour
{
    public string propPrefabName;

    private int idFrom;
    private int idTo;

    private UIMainModel mUIMainModel;


    private void Awake()
    {
        mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
        transform.GetComponent<UIButton>().onClick.Add(new EventDelegate(OnBtnClick));


    }

    private void OnBtnClick()
    {
        PropParameter propParam = new PropParameter()
        {
            idFrom = this.idFrom,
            idTo = this.idTo,
            propPrefabName = this.propPrefabName
        };

        //发送道具请求
        RoomMsgInfo msg = new RoomMsgInfo()
        {
            UID = mUIMainModel.PlayerInfo.UserId,
            MsgType = RoomMsgType.Prop,
            Content = Encoding.UTF8.GetBytes(JsonUtil.SerializeObject(propParam))
        };
        ActionParam actionParam = new ActionParam(msg);
        Net.Instance.Send(1108, null, actionParam);
    }

    public void SetSendParam(int idFrom,int idTo)
    {
        this.idFrom = idFrom;
        this.idTo = idTo;
    }

}
