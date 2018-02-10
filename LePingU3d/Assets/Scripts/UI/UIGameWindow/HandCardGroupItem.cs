using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCardGroupItem : MonoBehaviour
{
    private Transform mCacheTransform;
    private CardItem[] mCardItems;

    private List<Card> mCards;
    private DirectionType mDirectionType;
    private OperationType mOperationType;
    private int mIndex;

    private void Awake()
    {
        mCacheTransform = this.transform;
        mCardItems = mCacheTransform.GetComponentsInChildren<CardItem>();
    }

    public int getIndex()
    {
        return mIndex;
    }

    public OperationType getOperationType()
    {
        return mOperationType;
    }

    public List<Card> getCards()
    {
        return mCards;
    }

    public void Refresh(List<Card> cards, DirectionType type, int index = 0, OperationType operationType = OperationType.Guo)
    {
        this.mCards = cards;
        this.mDirectionType = type;
        this.mIndex = index;
        this.mOperationType = operationType;
        if (mCards != null && mCards.Count > 0)
        {
            Array.ForEach<CardItem>(mCardItems, p => p.SetVisible(false));
            for (int i = 0; i < mCards.Count; i++)
            {
                if (i < mCardItems.Length)
                {
                    mCardItems[i].Refresh(mCards[i], mDirectionType);
                }
            }
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }
    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetVisible(isVisible);
    }
}
