using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSettlementItem : MonoBehaviour
{
    private UIGameModel mUIGameModel;
    private Transform mCacheTransform;
    private UISprite mOwnerIcon;
    private UISprite mZhuangIcon;
    private UILabel mLabelName;
    private UILabel mLabelDes;
    private UITexture mHeadIcon;
    private HandCardGroupItem mHandCardItem;
    private List<HandCardGroupItem> mHandCradGroupItems;
    private CardItem mCurrCardItem;
    private UILabel mLabelTotalScore;
    private UILabel mLabelHupai;
    private UILabel mLabelJingPai;
    private UISprite mHuSprite;
    private UISprite mGunnerSprite;
    private SingleSettlementInfo mRoundMessage;

    private void Awake()
    {
        mCacheTransform = this.transform;
        mUIGameModel = UIModelMgr.Instance.GetModel<UIGameModel>();
        mOwnerIcon = mCacheTransform.FindComponent<UISprite>("HeadIcon/OwnerIcon");
        mZhuangIcon = mCacheTransform.FindComponent<UISprite>("HeadIcon/ZhuangIcon");
        mHeadIcon = mCacheTransform.FindComponent<UITexture>("HeadIcon");
        mLabelName = mCacheTransform.FindComponent<UILabel>("HeadIcon/Name");
        mLabelDes = mCacheTransform.FindComponent<UILabel>("PlayerCards/LabelDes");
        mCurrCardItem = mCacheTransform.FindComponent<CardItem>("PlayerCards/CurrCardItem");
        mHandCardItem = mCacheTransform.FindComponent<HandCardGroupItem>("PlayerCards/HandCards");
        if (mHandCradGroupItems == null)
        {
            mHandCradGroupItems = new List<HandCardGroupItem>();
        }
        mHandCradGroupItems.Add(mCacheTransform.FindComponent<HandCardGroupItem>("PlayerCards/HandCardGroup/CardGroup0"));
        mHandCradGroupItems.Add(mCacheTransform.FindComponent<HandCardGroupItem>("PlayerCards/HandCardGroup/CardGroup1"));
        mHandCradGroupItems.Add(mCacheTransform.FindComponent<HandCardGroupItem>("PlayerCards/HandCardGroup/CardGroup2"));
        mHandCradGroupItems.Add(mCacheTransform.FindComponent<HandCardGroupItem>("PlayerCards/HandCardGroup/CardGroup3"));
        mLabelTotalScore = mCacheTransform.FindComponent<UILabel>("LabelTotalScore");
        mLabelHupai = mCacheTransform.FindComponent<UILabel>("LabelHuPai");
        mLabelJingPai = mCacheTransform.FindComponent<UILabel>("LabelJingPai");
        mHuSprite = mCacheTransform.FindComponent<UISprite>("HuSprite");
        mGunnerSprite = mCacheTransform.FindComponent<UISprite>("GunnerSprite");
    }

    public void Refresh(SingleSettlementInfo msg)
    {
        Initialize();
        this.mRoundMessage = msg;
        if (mRoundMessage != null)
        {
            SetVisible(true);
			UserInfo temp = null;
			if (GameMgr.Instance.isFromRecord) {
				temp = mUIGameModel.GetLiamUser(mRoundMessage.UserID);
			}
			else {
				temp = mUIGameModel.GetUser(mRoundMessage.UserID);
			}

            if (temp != null)
            {
                mZhuangIcon.gameObject.SetVisible(temp.IsZhuang);
                mOwnerIcon.gameObject.SetVisible(temp.IsOwner);
                AsyncImageDownload.Instance.SetAsyncImage(temp.HeadUrl, mHeadIcon);
            }
            mLabelName.text = mRoundMessage.Name;
            mLabelDes.text = mRoundMessage.IsHu ? mRoundMessage.HuFlag : string.Empty;
            mCurrCardItem.Refresh(mRoundMessage.CurCard, DirectionType.bottom);
            mHandCardItem.Refresh(mRoundMessage.HandCards, DirectionType.bottom);
            if (!mRoundMessage.HandCardGroups.IsNullOrEmpty())
            {
                for (int i = 0; i < mRoundMessage.HandCardGroups.Count; i++)
                {
                    mHandCradGroupItems[i].Refresh(mRoundMessage.HandCardGroups[i].Cards, DirectionType.bottom);
                }
            }
            mLabelTotalScore.text = mRoundMessage.Scores.TryGetValue(ScoreConst.TotalScore).ToString();
            mLabelHupai.text = mRoundMessage.Scores.TryGetValue(ScoreConst.HuScore).ToString();
            mLabelJingPai.text = mRoundMessage.Scores.TryGetValue(ScoreConst.BaoCardScore).ToString();
            mHuSprite.gameObject.SetVisible(mRoundMessage.IsHu);
            mGunnerSprite.gameObject.SetVisible(mRoundMessage.Flag == 1);
        }
        else
        {
            SetVisible(false);
        }
    }

    private void Initialize()
    {
        mGunnerSprite.gameObject.SetVisible(false);
        mHuSprite.gameObject.SetVisible(false);
        mZhuangIcon.gameObject.SetVisible(false);
        mOwnerIcon.gameObject.SetVisible(false);
        mLabelTotalScore.text = "0";
        mLabelHupai.text = "0";
        mLabelJingPai.text = "0";
        mLabelName.text = string.Empty;
        mLabelDes.text = string.Empty;
        mHandCradGroupItems.ForEach(p => p.SetVisible(false));
        mHandCardItem.SetVisible(false);
        mCurrCardItem.SetVisible(false);
    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetVisible(isVisible);
    }
}
