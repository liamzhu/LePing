using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardDisplayType
{
    Display,
    Hide,
}

public class CardItem : MonoBehaviour
{
    public enum TouchState
    {
        None,
        Selected,
        Play
    }

    private TouchState mTouchState;
    private Transform mCacheTransform;
	public UISprite mCardSprite;
    private UISprite mJingSprite;
    private TweenPosition mIdentification;

    private Card mCardData;
    private DirectionType mSeatType;
    public CardDisplayType mCardNameType;

    public Card getCard
    {
        get { return mCardData; }
    }

    private void Awake()
    {
        mCacheTransform = this.transform;
        mIdentification = mCacheTransform.FindComponent<TweenPosition>("CardSprite/Identification");
        mCardSprite = mCacheTransform.FindComponent<UISprite>("CardSprite");
        mJingSprite = mCacheTransform.FindComponent<UISprite>("CardSprite/JingSprite");
        Initialize();
    }

    private void Initialize()
    {
        mJingSprite.gameObject.SetActive(false);
        mIdentification.gameObject.SetActive(false);
    }

    public void SetTouchStateNone()
    {
        SetTouchState(TouchState.None);
    }

    public void Click()
    {
        if (mTouchState == TouchState.None)
        {
            SetTouchState(TouchState.Selected);
        }
        else if (mTouchState == TouchState.Selected)
        {
            SetTouchState(TouchState.Play);
        }
        else
        {
            SetTouchState(TouchState.None);
        }
    }

    public void DragOver()
    {
        SetTouchState(TouchState.Play);
    }

    private void SetTouchState(TouchState touchSate)
    {
        mTouchState = touchSate;
        if (mTouchState == TouchState.None)
        {
            mCacheTransform.localPosition = new Vector3(mCacheTransform.localPosition.x, 0, mCacheTransform.localPosition.z);
        }
        else if (mTouchState == TouchState.Selected)
        {
            mCacheTransform.localPosition = new Vector3(mCacheTransform.localPosition.x, 20, mCacheTransform.localPosition.z);
        }
        else if (mTouchState == TouchState.Play)
        {
            //发送出牌请求
            MahJongGameAction requestPack = new MahJongGameAction()
            {
                Action = MahJongActionType.PlayCard
            };
            requestPack.Cards = new List<Card>();
            requestPack.Cards.Add(getCard);
            ActionParam actionParam = new ActionParam(requestPack);
            Net.Instance.Send((int)ActionType.GameAction, null, actionParam);
            //mCacheTransform.localPosition = new Vector3(mCacheTransform.localPosition.x, 0, mCacheTransform.localPosition.z);
        }
    }

    public void RefreshDeskCard(Card data, DirectionType seatType)
    {
        this.mCardData = data;
        this.mSeatType = seatType;
        if (mCardData != null)
        {
            mJingSprite.gameObject.SetActive(mCardData.IsUniversal);
            mCardSprite.spriteName = getCardName();
            SetCurrentDisableCard(true);
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }
    }

    public void Refresh(Card data, DirectionType seatType)
    {
        this.mCardData = data;
        this.mSeatType = seatType;
        if (mCardData != null)
        {
            mJingSprite.gameObject.SetActive(mCardData.IsUniversal);
            mCardSprite.spriteName = getCardName();
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }

		this.mCardSprite.color = new Color(255, 255, 255);
        //如果是宝牌，则加深颜色
		if(data != null){
			if (data.IsUniversal)
			{
				this.mCardSprite.color = new Color(255, 0, 0);
			}
		}
    }

    public void SetCurrentDisableCard(bool active)
    {
        if (mIdentification != null)
        {
            mIdentification.gameObject.SetActive(active);
        }
    }

    private string getCardName()
    {
        if (mSeatType == DirectionType.bottom)
        {
            if (mCardNameType == CardDisplayType.Display)
            {
                if (mCardData.CardValue != 0)
                {
                    return string.Format("{0}{1}_{2}", ((DirectionName)((int)mSeatType)).ToString(), (CardTypeName)mCardData.CardType, mCardData.CardValue);
                }
                else
                {
                    return string.Format("{0}", "tdbgs_4");
                }
            }
            else
            {
                return string.Format("{0}{1}_{2}", ((DirectionName)((int)mSeatType)).ToString(), (CardTypeName1)mCardData.CardType, mCardData.CardValue);
            }
        }
        else if (mSeatType == DirectionType.top)
        {
            if (mCardNameType == CardDisplayType.Display)
            {
                if (mCardData.CardValue != 0)
                {
                    return string.Format("{0}{1}_{2}", "p4", (CardTypeName)mCardData.CardType, mCardData.CardValue);
                }
                else
                {
                    return string.Format("{0}_{1}", "tdbgs", (int)mSeatType);
                }
            }
            else
            {
                return string.Format("{0}_{1}", "tbgs", (int)mSeatType);
            }
        }
        else
        {
            if (mCardNameType == CardDisplayType.Display)
            {
                if (mCardData.CardValue != 0)
                {
                    return string.Format("{0}{1}_{2}", ((DirectionName)((int)mSeatType)).ToString(), (CardTypeName)mCardData.CardType, mCardData.CardValue);
                }
                else
                {
                    return string.Format("{0}_{1}", "tdbgs", (int)mSeatType);
                }
            }
            else
            {
                return string.Format("{0}_{1}", "tbgs", (int)mSeatType);
            }
        }
    }

    //private string getCardName()
    //{
    //    if (mSeatType == DirectionType.bottom)
    //    {
    //        if (mCardNameType == CardDisplayType.Display)
    //        {
    //            if (mCardData.cardValue != 0)
    //            {
    //                return string.Format("{0}_{1}_{2}", "middle", ((CradTypeName)mCardData.cardType).ToString(), mCardData.cardValue);
    //            }
    //            else
    //            {
    //                return string.Format("{0}_{1}_{2}", mSeatType.ToString(), "default", 1);
    //            }
    //        }
    //        else
    //        {
    //            return string.Format("{0}_{1}_{2}", mSeatType.ToString(), ((CradTypeName)mCardData.cardType).ToString(), mCardData.cardValue);
    //        }
    //    }
    //    else
    //    {
    //        if (mCardNameType == CardDisplayType.Display)
    //        {
    //            if (mCardData.cardValue != 0)
    //            {
    //                return string.Format("{0}_{1}_{2}", mSeatType.ToString(), ((CradTypeName)mCardData.cardType).ToString(), mCardData.cardValue);
    //            }
    //            else
    //            {
    //                return string.Format("{0}_{1}_{2}", mSeatType.ToString(), "default", 2);
    //            }
    //        }
    //        else
    //        {
    //            return string.Format("{0}_{1}_{2}", mSeatType.ToString(), "default", 1);
    //        }
    //    }
    //}

    public void SetVisible(bool isVisible)
    {
        gameObject.SetVisible(isVisible);
    }
}

public enum DirectionName
{
    p4,//bottom
    p1,//right
    p2,//top
    p3,//left
}

public enum TableName
{
    tdbgs_2,
    tbgs_1,
    tbgs_2,
    tbgs_3,
    middle
}

public enum CardTypeName
{
    s1, //wan p1s1
    s2, //tong p1s2
    s3,//tiao p1s3
    s4,//feng p1s4
    s5// p1s5
}

public enum CardTypeName1
{
    b1, //wan p1s1
    b2, //tong p1s2
    b3,//tiao p1s3
    b4,//feng p1s4
    b5// p1s5
}
