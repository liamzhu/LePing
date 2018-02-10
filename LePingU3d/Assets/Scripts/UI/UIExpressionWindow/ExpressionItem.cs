using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ExpressionItem : MonoBehaviour
{
    public string expressionPrefabName = "";
    public Action OnCloseClick;

    private UIMainModel mUIMainModel;
    

    private void Awake()
    {
        GetComponent<UIButton>().onClick.Add(new EventDelegate(OnExpressionClick));
        mUIMainModel = UIModelMgr.Instance.GetModel<UIMainModel>();
    }

    private void OnExpressionClick()
    {
        //Send表情请求
        RoomMsgInfo msg = new RoomMsgInfo()
        {
            UID = mUIMainModel.PlayerInfo.UserId,
            MsgType = RoomMsgType.Image,
            Content = Encoding.UTF8.GetBytes(expressionPrefabName)
        };
        ActionParam actionParam = new ActionParam(msg);
        Net.Instance.Send(1108, null, actionParam);

        if (OnCloseClick != null)
            OnCloseClick();
    }
}
