using UnityEngine;
using System.Collections;

public class ScoreItem : MonoBehaviour
{
    private UILabel mLabelGameNo;
    private UILabel mLabelGameScore;
    private Transform mCacheTransform;

    private void Awake()
    {
        mCacheTransform = this.transform;
        mLabelGameNo = mCacheTransform.FindComponent<UILabel>("LabelGameNo");
        mLabelGameScore = mCacheTransform.FindComponent<UILabel>("LabelGameScore");
    }

    public void Refresh(int index, int score)
    {
        mLabelGameNo.text = string.Format("第{0}局", index + 1);
        mLabelGameScore.text = string.Format("{0}", score);
    }
}
