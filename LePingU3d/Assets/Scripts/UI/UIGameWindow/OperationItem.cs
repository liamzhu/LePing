using System;
using System.Collections.Generic;
using UnityEngine;

public class OperationItem : MonoBehaviour
{
    private UISprite mCradSprite;
    private OperationInfo mOperationInfo;

    public OperationInfo getOperationInfo()
    {
        return mOperationInfo;
    }

    private void Awake()
    {
        mCradSprite = this.GetComponentInChildren<UISprite>();
    }

    public void Refresh(OperationInfo info)
    {
        //Debug.Log("OperationItem   " + info.OperateType);
        this.mOperationInfo = info;
        mCradSprite.spriteName = getSpriteName();
        SetVisible(true);
    }

    private string getSpriteName()
    {
        return string.Format("btn_operation_{0}", (int)mOperationInfo.OperateType);
    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetVisible(isVisible);
    }

}
