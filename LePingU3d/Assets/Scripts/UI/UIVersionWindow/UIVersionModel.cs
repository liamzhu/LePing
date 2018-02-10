using System.Collections;
using UnityEngine;

public class UIVersionModel : UIBaseModel
{
    private VersionInfoResp mVersionInfo;

    public VersionInfoResp VersionInfo
    {
        get
        {
            return mVersionInfo;
        }

        set
        {
            ValueChangeArgs ve = new ValueChangeArgs("VersionInfo", mVersionInfo, value);
            mVersionInfo = value;
            DispatchValueUpdateEvent(ve);
        }
    }
}
