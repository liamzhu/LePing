using UnityEngine;
using System.Collections;

public class CToggleGroup : MonoBehaviour
{

    private UIToggle[] mToggles;
    private Transform mCacheTransform;
    private int index;

    private void Awake()
    {
        this.mCacheTransform = this.transform;
        mToggles = mCacheTransform.GetComponentsInChildren<UIToggle>();
        //Debug.Log(mToggles.Length);
    }

    public int getIndex()
    {
        if (mToggles.Length == 1)
        {
            index = mToggles[0].value ? 0 : -1;
        }
        else
        {
            for (int i = 0; i < mToggles.Length; i++)
            {
                if (mToggles[i].value)
                {
                    index = i;
                }
            }
        }
        return index;
    }
}
