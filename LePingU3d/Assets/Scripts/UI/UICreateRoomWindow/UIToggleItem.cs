using System.Collections;
using UnityEngine;

public class UIToggleItem : MonoBehaviour
{
    private UILabel mLabelDes;
    private UIToggle mUIToggle;
    private Transform mCacheTransform;

    private void Awake()
    {
        mCacheTransform = this.transform;
        mLabelDes = mCacheTransform.FindComponent<UILabel>("Label");

        mUIToggle = this.GetComponent<UIToggle>();
    }

    public void Init(string des)
    {
        if (!string.IsNullOrEmpty(des))
        {
            mLabelDes.text = des;
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }

    }

    public void SetVisible(bool isVisble)
    {
        this.gameObject.SetVisible(isVisble);
    }

    public bool GetValue()
    {
        return mUIToggle.value;
    }

}
