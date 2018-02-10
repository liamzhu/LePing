using UnityEngine;
using System.Collections;

public class GameSettlementItem : MonoBehaviour
{
    private GameSettlementInfo mEndMessage;
    private Transform mCacheTransform;
    private UISprite mOwnerIcon;
    private UISprite mBestGunner;
    private UISprite mBigWinner;
    private UILabel mLabelName;
    private UILabel mLabelId;
    private UILabel mLabelTotalScore;
    private UITexture mHeadIcon;
    private UIGameModel mUIGameModel;
    private ScrollGrid mScrollGrid;

    private void Awake()
    {
        mCacheTransform = this.transform;
        mUIGameModel = UIModelMgr.Instance.GetModel<UIGameModel>();
        mHeadIcon = mCacheTransform.FindComponent<UITexture>("HeadIcon");
        mOwnerIcon = mCacheTransform.FindComponent<UISprite>("OwnerIcon");
        mBestGunner = mCacheTransform.FindComponent<UISprite>("Group/ScrollView/BestGunner");
        mBigWinner = mCacheTransform.FindComponent<UISprite>("BigWinner");
        mLabelName = mCacheTransform.FindComponent<UILabel>("Name");
        mLabelId = mCacheTransform.FindComponent<UILabel>("ID");
        mLabelTotalScore = mCacheTransform.FindComponent<UILabel>("LabelTotalScore");
        mScrollGrid = mCacheTransform.FindComponent<ScrollGrid>("Group/ScrollView/Grid");
    }

    public void Refresh(GameSettlementInfo msg)
    {
        mScrollGrid.SetGrid(0, setItem);
        mOwnerIcon.gameObject.SetVisible(false);
        mBestGunner.gameObject.SetVisible(false);
        mBigWinner.gameObject.SetVisible(false);
        this.mEndMessage = msg;
        if (mEndMessage != null)
        {
            UserInfo temp = mUIGameModel.GetUser(mEndMessage.UserID);
            if (temp != null)
            {
                mOwnerIcon.gameObject.SetVisible(temp.IsOwner);
                AsyncImageDownload.Instance.SetAsyncImage(temp.HeadUrl, mHeadIcon);
            }
            mBestGunner.gameObject.SetVisible(mEndMessage.Flag == -1);
            mBigWinner.gameObject.SetVisible(mEndMessage.Flag == 1);
            mLabelId.text = string.Format("ID:{0}", mEndMessage.UserID);
            mLabelName.text = mEndMessage.Name;
            mLabelTotalScore.text = mEndMessage.Score.ToString();
            if (!mEndMessage.Scores.IsNullOrEmpty())
            {
                mScrollGrid.SetGrid(mEndMessage.Scores.Count, setItem);
            }
            SetVisible(true);
        }
        else
        {
            mLabelId.text = string.Empty;
            mLabelName.text = string.Empty;
            mLabelTotalScore.text = "0";
            SetVisible(false);
        }
    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetVisible(isVisible);
    }

    private void setItem(Transform[] t, int start, int end)
    {
        for (int i = 0; i < t.Length; i++)
        {
            t[i].GetComponent<ScoreItem>().Refresh(i, mEndMessage.Scores[i]);
        }
    }
}
