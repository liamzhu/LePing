using System.Collections.Generic;

public class UserCardModel
{
    private UserInfo mUserInfo;
    private List<Card> mHandCards;
    private List<Card> mDeskCards;
    private List<CardGroup> mHandCardGroups;
    private Card mCurTouchCard;

    public UserCardModel getCopy()
    {
        return (UserCardModel)MemberwiseClone();
    }

    public void ClearCardInfo()
    {
        this.mHandCards = new List<Card>();
        this.mDeskCards = new List<Card>();
        this.mHandCardGroups = new List<CardGroup>();
        this.mCurTouchCard = null;
    }

    #region 构造函数

    public UserCardModel()
    {
        this.mHandCards = new List<Card>();
        this.mDeskCards = new List<Card>();
        this.mHandCardGroups = new List<CardGroup>();
        this.mCurTouchCard = null;
    }

    public UserCardModel(UserInfo info)
    {
        this.mUserInfo = info;
        this.mHandCards = new List<Card>();
        this.mDeskCards = new List<Card>();
        this.mHandCardGroups = new List<CardGroup>();
    }

    public UserCardModel(UserInfo info, List<Card> cards)
    {
        this.mUserInfo = info;
        this.mHandCards = new List<Card>(cards);
        this.mDeskCards = new List<Card>();
        this.mHandCardGroups = new List<CardGroup>();
    }

    #endregion 构造函数

    public void setUserInfo(UserInfo user)
    {
        this.mUserInfo = user;
    }

    public UserInfo getUserInfo()
    {
        return mUserInfo;
    }

    #region 当前摸到的牌

    public void setTouchCard(Card card)
    {
        this.mCurTouchCard = card;
    }

    public Card getTouchCard()
    {
        return mCurTouchCard;
    }

    #endregion 当前摸到的牌

    #region 手里的牌

    public void setHandCards(List<Card> currentCardModels)
    {
        this.mHandCards = currentCardModels;
    }

    public List<Card> getHandCards()
    {
        return mHandCards;
    }

    public void addHandCard(Card model)
    {
        mHandCards.Add(model);
    }

    public void removeLastHandCard()
    {
        if (mHandCards.Count > 0)
        {
            mHandCards.RemoveAt(mHandCards.Count - 1);
        }
    }

    public void removeHandCards(Card model)
    {
        mHandCards.RemoveAll(p => p.CardType == model.CardType && p.CardValue == model.CardValue);
    }

    public void removeHandCard(Card model)
    {
        Card temp = mHandCards.Find(p => p.CardType == model.CardType && p.CardValue == model.CardValue);
        if (temp != null)
        {
            mHandCards.Remove(temp);
        }
    }

    #endregion 手里的牌

    #region 桌面上打出去的牌

    public void setDeskCards(List<Card> cards)
    {
        this.mDeskCards = cards;
    }

    public int getDeskIndex()
    {
        if (mDeskCards.Count > 0)
        {
            return mDeskCards.Count - 1;
        }
        else
        {
            return 0;
        }
    }

    public List<Card> getDeskCards()
    {
        return mDeskCards;
    }

    public Card getLastDeskCard()
    {
        if (mDeskCards != null && mDeskCards.Count > 0)
        {
            return mDeskCards[mDeskCards.Count - 1];
        }
        return null;
    }

    public void addDeskCard(Card model)
    {
        mDeskCards.Add(model);
    }

    public void removeDeskCard(Card model)
    {
        DebugHelper.LogError(mDeskCards.Count);
        Card temp = mDeskCards.Find(p => p.CardType == model.CardType && p.CardValue == model.CardValue);
        if (temp != null)
        {
            mDeskCards.Remove(temp);
        }
        DebugHelper.LogError(mDeskCards.Count);
    }

    public void removeLastDeskCard()
    {
        if (mDeskCards.Count > 0)
        {
            mDeskCards.RemoveAt(mDeskCards.Count - 1);
        }
    }

    #endregion 桌面上打出去的牌

    #region 手上配对过的牌组数据

    public void setHandCardGroups(List<CardGroup> user)
    {
        this.mHandCardGroups = new List<CardGroup>(user);
    }

    public void addHandCardGroup(CardGroup model)
    {
        mHandCardGroups.Add(model);
    }

    public void refreshHandCardGroup(CardGroup model)
    {
        int index = hasHandCardGroup(model.Cards[3]);
        if (index != -1)
        {
            mHandCardGroups[index].Cards = model.Cards;
            mHandCardGroups[index].CardGroupType = model.CardGroupType;
        }
    }

    public int hasHandCardGroup(Card model)
    {
        return mHandCardGroups.FindIndex(p => p.Cards[0].CardType == model.CardType && p.Cards[0].CardValue == model.CardValue);
    }

    //public void removeHandCardGroup(CardGroup model)
    //{
    //    if (mHandCardGroups == null)
    //    {
    //        mHandCardGroups = new List<CardGroup>();
    //    }
    //    Card temp = mHandCardGroups.Find(p => p.Cards.cardType == model.cardType && p.cardValue == model.cardValue);
    //    if (temp != null)
    //    {
    //        pengCardModels.Remove(temp);
    //    }
    //}
    public int getHandCardGroupsIndex
    {
        get { return mHandCardGroups.Count - 1; }
    }

    public CardGroup getHandCardGroup()
    {
        if (mHandCardGroups != null && mHandCardGroups.Count > 0)
        {
            return mHandCardGroups[getHandCardGroupsIndex];
        }
        return null;
    }

    public List<CardGroup> getHandCardGroups()
    {
        return mHandCardGroups;
    }

    #endregion 手上配对过的牌组数据

}
