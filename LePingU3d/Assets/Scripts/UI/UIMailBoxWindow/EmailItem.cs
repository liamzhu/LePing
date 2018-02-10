using UnityEngine;
using System.Collections;

public class EmailItem : MonoBehaviour
{

    private EmailInfo mEmail;
    private UILabel mLabelDes;
    private UILabel mDateTime;
    private Transform mCacheTransform;

    private void Awake()
    {
        mCacheTransform = this.transform;
        mLabelDes = mCacheTransform.FindComponent<UILabel>("LabelDes");
        mDateTime = mCacheTransform.FindComponent<UILabel>("LabelDateTime");
        mCacheTransform.GetUIEventListener("BtnDetails").onClick += OnDetailsClick;
    }

    public void Refresh(EmailInfo email)
    {
        this.mEmail = email;
        mLabelDes.text = mEmail.Brief;
        mDateTime.text = mEmail.Time.ToString();
    }

    private void OnDetailsClick(GameObject go)
    {
        UIWindowMgr.Instance.PushPanel<UIMailDetailsWindow>(mEmail);
    }
}
