using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum DirectionType
{
    bottom,
    right,
    top,
    left,
}

public class PlayerCardGroupItem : MonoBehaviour
{
    [SerializeField]
    private DirectionType mDirection;

    private Transform mCacheTransform;
    private CardItem[] mHandCardList;
    private CardItem[] mDeskCardList;
    private CardItem mCurrCardItem;
    private HandCardGroupItem[] mCardGroupList;
    private OperationItem[] mOperationItems;
    private OperationEffectItem mOperationEffectItem;
    private HandCardGroupItem[] mHandCardGroupItems;

    private Transform mOperationCardGroup;

    private UIGameModel mUIGameModel;
    private UserCardModel mUserCardModel;
    private UserInfo mUserInfo;

    private void Awake()
    {
        mCacheTransform = this.transform;
        if (mUIGameModel == null)
        {
            mUIGameModel = UIModelMgr.Instance.GetModel<UIGameModel>();
            mUIGameModel.ValueUpdateEvent += OnValueUpdateEvent;
        }
        mCurrCardItem = mCacheTransform.FindComponent<CardItem>("CurrCardItem");
        mHandCardList = mCacheTransform.FindChindComponents<CardItem>("HandCards");
        mDeskCardList = mCacheTransform.FindChindComponents<CardItem>("DeskCards");
        mCardGroupList = mCacheTransform.FindChindComponents<HandCardGroupItem>("HandCardGroups");

        mOperationEffectItem = mCacheTransform.FindComponent<OperationEffectItem>("OperationEffectItem");

        if (mDirection == DirectionType.bottom)
        {
            mOperationCardGroup = mCacheTransform.FindComponent<Transform>("Operations/OperationCardGroup");
            mOperationItems = mCacheTransform.FindChindComponents<OperationItem>("Operations");
            mHandCardGroupItems = mCacheTransform.FindChindComponents<HandCardGroupItem>("Operations/OperationCardGroup");
            Array.ForEach<OperationItem>(mOperationItems, p => { UIEventListener.Get(p.gameObject).onClick += OnOperationClick; });  //点击 吃 碰 杠等
            Array.ForEach<HandCardGroupItem>(mHandCardGroupItems, p =>
            {
                p.SetVisible(false);
                UIEventListener.Get(p.gameObject).onClick += OnHandCardGroupClick;
                //UIEventListener.Get(p.gameObject).onDragOver += OnHandCardGroupDrag;
            });
            mOperationCardGroup.SetVisible(false);
            Array.ForEach<CardItem>(mHandCardList, p => { UIEventListener.Get(p.gameObject).onClick += OnClickCardItem; });
            Array.ForEach<CardItem>(mHandCardList, p => { UIEventListener.Get(p.gameObject).onDragOver += OnDragItem; });
            UIEventListener.Get(mCurrCardItem.gameObject).onClick += OnClickCardItem;
            UIEventListener.Get(mCurrCardItem.gameObject).onDragOver += OnDragItem;
        }
    }

    private void OnValueUpdateEvent(object sender, ValueChangeArgs e)
    {
    }

    public void SetUserInfo(UserInfo user)
    {
        this.mUserInfo = user;
    }

    private void SetUserCardModel()
    {
        mUserCardModel = mUIGameModel.getOrAddUserCardModel((int)mDirection);
    }

    /// <summary>
    /// 显示可执行的操作 如：吃 碰 杠 胡 过 听
    /// </summary>
    /// <param name="action"></param>
    public void SetOperationBehavior(OperationBehaviorResp action)
    {
        CleanUpOperationState();
        if (!action.OperationInfos.IsNullOrEmpty())
        {
            for (int i = 0; i < action.OperationInfos.Count; i++)
            {
                mOperationItems[i].Refresh(action.OperationInfos[i]);
            }
        }
        if (!action.ListenCards.IsNullOrEmpty())
        {
            mCardGroupList[mCardGroupList.Length - 1].Refresh(action.ListenCards, mDirection);
        }
        else
        {
            mCardGroupList[mCardGroupList.Length - 1].SetVisible(false);
        }
    }

    private void OnHandCardGroupClick(GameObject go)
    {
        mOperationCardGroup.SetVisible(false);
        Array.ForEach<HandCardGroupItem>(mHandCardGroupItems, p => { p.SetVisible(false); });
        HandCardGroupItem mHandCradGroupItem = go.GetComponent<HandCardGroupItem>();
        if (mHandCradGroupItem != null) { OperationClick(mHandCradGroupItem.getIndex(), mHandCradGroupItem.getOperationType()); }
    }

	//拖拽出牌
    private void OnHandCardGroupDrag(GameObject go)
    {
        Debug.Log("-------------------------------OnHandCardGroupDrag");
        mOperationCardGroup.SetVisible(false);
        Array.ForEach<HandCardGroupItem>(mHandCardGroupItems, p => { p.SetVisible(false); });
        HandCardGroupItem mHandCradGroupItem = go.GetComponent<HandCardGroupItem>();
        if (mHandCradGroupItem != null) { OperationDrag(mHandCradGroupItem.getIndex()); }
    }

    private void OnOperationClick(GameObject go)
    {
        OperationInfo mOperationInfo = go.GetComponent<OperationItem>().getOperationInfo();
        if (mOperationInfo != null)
        {
            if (mOperationInfo.OperateType == OperationType.Chi || mOperationInfo.OperateType == OperationType.Gang)
            {
                Debug.Log(mOperationInfo.CardGroups.Count);
                if (mOperationInfo.CardGroups != null && mOperationInfo.CardGroups.Count > 1)
                {
                    mOperationCardGroup.SetVisible(true);
                    for (int i = 0; i < mOperationInfo.CardGroups.Count; i++)
                    {
                        if (i < mHandCardGroupItems.Length)
                        {
                            mHandCardGroupItems[i].Refresh(mOperationInfo.CardGroups[i].Cards, DirectionType.bottom, i, mOperationInfo.OperateType);
                        }
                    }
                }
                else
                {
                    OperationClick(0, mOperationInfo.OperateType);
                }
            }
            else
            {
                OperationClick(0, mOperationInfo.OperateType);
            }
        }
    }

	//吃 杠 
    private void OperationClick(int index, OperationType mOperationType)
    {
        MahJongGameAction requestPack = new MahJongGameAction()
        {
            Action = (MahJongActionType)mOperationType
        };
        requestPack.Parame = index;
        ActionParam actionParam = new ActionParam(requestPack);
        Net.Instance.Send((int)ActionType.GameAction, onOperationCallBack, actionParam);
    }

	//拖拽出牌
    private void OperationDrag(int index)
    {
        MahJongGameAction requestPack = new MahJongGameAction()
        {
            Action = MahJongActionType.PlayCard
        };
        requestPack.Parame = index;
        ActionParam actionParam = new ActionParam(requestPack);
        Net.Instance.Send((int)ActionType.GameAction, onOperationCallBack, actionParam);
    }

    private void onOperationCallBack(ActionResult actionResult)
    {
        ResponseDefaultPacket responsePack = actionResult.GetValue<ResponseDefaultPacket>();
        if (!responsePack.Success)
        {
            UIDialogMgr.Instance.ShowDialog(responsePack.Result);
        }
    }

    public void ReconnectGameAction(MahJongGameAction action)
    {
        SetUserCardModel();
        switch (action.Action)
        {
            case MahJongActionType.TouchCard:
            case MahJongActionType.PengCard:
            case MahJongActionType.ChiCard:
                SetCardsState(true);
                break;
            case MahJongActionType.StartTouchCard:
            case MahJongActionType.PlayCard:
            case MahJongActionType.GangCard:
                SetCardsState(false);
                break;
            case MahJongActionType.HuCard:
                break;
            case MahJongActionType.Guo:
                break;
            default:
                break;
        }
    }

    public void RefreshCardsInfo()
    {
        SetUserCardModel();
        RefreshHandCards();
        RefreshDeskCards();
        RefreshTouchCard();
        RefreshHandCardGroups();
    }

    /// <summary>
    /// 执行Action
    /// </summary>
    /// <param name="action"></param>
    public void OperateGameAction(MahJongGameAction action)
    {
        SetUserCardModel();
        //Debug.Log(action.Action + "  " + action.Parame + "  " + action.UID);
        switch (action.Action)
        {
            case MahJongActionType.StartTouchCard:  //开局摸牌
                OperateStartTouchCard(action);
                SetCardsState(false);
                break;
            case MahJongActionType.TouchCard: //摸牌
                OperateTouchCard(action);
                SetCardsState(true);
                break;
            case MahJongActionType.PlayCard:  //出牌
                OperatePlayCard(action);
                SetCardsState(false);
                break;
            case MahJongActionType.ChiCard:
                OperateChiCard(action);
                SetCardsState(true);
                break;
            case MahJongActionType.PengCard:
                OperatePengCard(action);
                SetCardsState(true);
                break;
            case MahJongActionType.GangCard:
                OperateGangCard(action);
                SetCardsState(false);
                break;
            case MahJongActionType.HuCard:
                OperateHuCard(action);
                SetCardsState(false);
                break;
            case MahJongActionType.Guo:
                break;
            default:
                break;
        }
    }

    private void OperateStartTouchCard(MahJongGameAction action)
    {
        if (mUserCardModel != null)
        {
            mUserCardModel.setHandCards(action.Cards);
            RefreshHandCards();
        }
    }

    private void OperateTouchCard(MahJongGameAction action)
    {
        mUserCardModel.setTouchCard(action.Cards[0]);
        mUserCardModel.addHandCard(action.Cards[0]);
        mCurrCardItem.Refresh(mUserCardModel.getTouchCard(), mDirection);

		//播放摸牌音效
		MahjongAudioMgr.Instance.PlayMoPai();
    }

    private void OperatePlayCard(MahJongGameAction action)
    {
		if (action.Cards [0].IsUniversal) {
			//播放打宝声音
			MahjongAudioMgr.Instance.PlayDaBao (mUserInfo.Sex);
			//播放打宝特效
			mOperationEffectItem.SetEffectType (OperationEffectItem.OperationEffectType.DaBao);
		}
		else {
			MahjongAudioMgr.Instance.PlayChuPai ();
		}

        //设置当前最新的行为
        mUIGameModel.LastGameActionResp = action.getCopy();
        //播放出牌的声音
		if(!action.Cards[0].IsUniversal){
			MahjongAudioMgr.Instance.PlayChuPaiSound(mUserInfo.Sex, action.Cards[0], PlayerPrefs.GetInt(PrefsConstant.AudioType, 0));
		}
        //添加到桌牌
        mUserCardModel.addDeskCard(action.Cards[0]);
        //设置最新的桌牌
        mUIGameModel.LastDeskCard = action.Cards[0];
        //显示桌牌
        mDeskCardList[mUserCardModel.getDeskIndex()].RefreshDeskCard(mUserCardModel.getLastDeskCard(), mDirection);
        if (mUIGameModel.LastDeskCardItem != null)
        {
            mUIGameModel.LastDeskCardItem.SetCurrentDisableCard(false);
        }
        mUIGameModel.LastDeskCardItem = mDeskCardList[mUserCardModel.getDeskIndex()];
//        if (mDirection == DirectionType.bottom)
        {
            mUserCardModel.removeHandCard(mUserCardModel.getLastDeskCard());
        }
//        else
//        {
//            mUserCardModel.removeLastHandCard();
//        }
        //排序牌并刷新显示
        RefreshHandCards();
        mCurrCardItem.SetVisible(false);
    }

    private void OperateChiCard(MahJongGameAction action)
    {
        MahjongAudioMgr.Instance.PlayChiPaiSound(mUserInfo.Sex, PlayerPrefs.GetInt(PrefsConstant.AudioType, 0));
        mOperationEffectItem.SetEffectType(OperationEffectItem.OperationEffectType.Chi);
        mUserCardModel.addHandCardGroup(new CardGroup((int)CardGroupType.Abc, true, action.Cards, 0));
        mCardGroupList[mUserCardModel.getHandCardGroupsIndex].Refresh(mUserCardModel.getHandCardGroup().Cards, mDirection);
        //if (mDirection == DirectionType.bottom)
        {
            Remove(action.Cards, mUIGameModel.LastDeskCard);
            mUserCardModel.removeHandCard(action.Cards[0]);
            mUserCardModel.removeHandCard(action.Cards[1]);
        }
//        else
//        {
//            mUserCardModel.removeLastHandCard();
//            mUserCardModel.removeLastHandCard();
//        }
        RemoveOtherPlayerCard();
        RefreshHandCards();
    }

    private void OperatePengCard(MahJongGameAction action)
    {
        mOperationEffectItem.SetEffectType(OperationEffectItem.OperationEffectType.Peng);
        MahjongAudioMgr.Instance.PlayPengPaiSound(mUserInfo.Sex, PlayerPrefs.GetInt(PrefsConstant.AudioType, 0));
        mUserCardModel.addHandCardGroup(new CardGroup((int)CardGroupType.AAA, true, action.Cards, 0));
        mCardGroupList[mUserCardModel.getHandCardGroupsIndex].Refresh(mUserCardModel.getHandCardGroup().Cards, mDirection);
//        if (mDirection == DirectionType.bottom)
//        {
            mUserCardModel.removeHandCard(action.Cards[0]);
            mUserCardModel.removeHandCard(action.Cards[0]);
//        }
//        else
//        {
//            mUserCardModel.removeLastHandCard();
//            mUserCardModel.removeLastHandCard();
//        }
        RemoveOtherPlayerCard();
        RefreshHandCards();
    }

    private void OperateGangCard(MahJongGameAction action)
    {
        MahjongAudioMgr.Instance.PlayGangPaiSound(mUserInfo.Sex, PlayerPrefs.GetInt(PrefsConstant.AudioType, 0));
        mOperationEffectItem.SetEffectType(OperationEffectItem.OperationEffectType.Gang);
        if (action.Parame == 3 || action.Parame == 4)
        {
            HandCardGroupItem tempItem = Array.Find<HandCardGroupItem>(mCardGroupList, p =>
                     p.getCards() != null && p.getCards()[0].Equals(action.Cards[3])
                );
            if (tempItem != null)
            {
                mUserCardModel.refreshHandCardGroup(new CardGroup((int)CardGroupType.AAAA, true, action.Cards, 0));
                tempItem.Refresh(action.Cards, mDirection);
            }
            else
            {
                mUserCardModel.addHandCardGroup(new CardGroup((int)CardGroupType.AAAA, true, action.Cards, 0));
                mCardGroupList[mUserCardModel.getHandCardGroupsIndex].Refresh(mUserCardModel.getHandCardGroup().Cards, mDirection);
            }
        }
        else
        {
            mUserCardModel.addHandCardGroup(new CardGroup((int)CardGroupType.AAAA, true, action.Cards, 0));
            mCardGroupList[mUserCardModel.getHandCardGroupsIndex].Refresh(mUserCardModel.getHandCardGroup().Cards, mDirection);
        }

//        if (mDirection == DirectionType.bottom)
        {
            //mUserCardModel.removeHandCards(action.Cards[3]);

            if (action.Parame == 0)
            {
                mUserCardModel.removeHandCard(action.Cards[3]);
                mUserCardModel.removeHandCard(action.Cards[3]);
                mUserCardModel.removeHandCard(action.Cards[3]);
                RemoveOtherPlayerCard();
            }
            else if (action.Parame == 1)
            {
                mUserCardModel.removeHandCard(action.Cards[3]);
                mUserCardModel.removeHandCard(action.Cards[3]);
                mUserCardModel.removeHandCard(action.Cards[3]);
                mUserCardModel.removeHandCard(action.Cards[3]);
            }
            else if (action.Parame == 2)
            {
                mUserCardModel.removeHandCard(action.Cards[3]);
                mUserCardModel.removeHandCard(action.Cards[3]);
                mUserCardModel.removeHandCard(action.Cards[3]);
                mUserCardModel.removeHandCard(action.Cards[3]);
            }
            else if (action.Parame == 3)
            {
                mUserCardModel.removeHandCard(action.Cards[3]);
            }
            else if (action.Parame == 4)
            {
                mUserCardModel.removeHandCard(action.Cards[3]);
            }
        }
//        else
//        {
//            //杠牌时 param 0：接杠，1：暗杠，2：自摸暗杠，3：碰杠
//            if (action.Parame == 0)
//            {
//                mUserCardModel.removeLastHandCard();
//                mUserCardModel.removeLastHandCard();
//                mUserCardModel.removeLastHandCard();
//                RemoveOtherPlayerCard();
//            }
//            else if (action.Parame == 1)
//            {
//                mUserCardModel.removeLastHandCard();
//                mUserCardModel.removeLastHandCard();
//                mUserCardModel.removeLastHandCard();
//                mUserCardModel.removeLastHandCard();
//            }
//            else if (action.Parame == 2)
//            {
//                mUserCardModel.removeLastHandCard();
//                mUserCardModel.removeLastHandCard();
//                mUserCardModel.removeLastHandCard();
//                mUserCardModel.removeLastHandCard();
//            }
//            else if (action.Parame == 3)
//            {
//                mUserCardModel.removeLastHandCard();
//            }
//            else if (action.Parame == 4)
//            {
//                mUserCardModel.removeLastHandCard();
//            }
//        }

        mCurrCardItem.SetVisible(false);
        RefreshHandCards();
    }

    private void RemoveOtherPlayerCard()
    {
        if (mUIGameModel.LastGameActionResp != null)
        {
            var temp = GameLogicMgr.Instance.getUserCardModelByUserId(mUIGameModel.LastGameActionResp.UserID);
            if (temp != null)
            {
                temp.removeLastDeskCard();
                mUIGameModel.LastDeskCardItem.SetVisible(false);
            }
        }
    }

	//胡牌
    private void OperateHuCard(MahJongGameAction action)
    {
        MahjongAudioMgr.Instance.PlayHuPaiSound(mUserInfo.Sex, action.Parame, PlayerPrefs.GetInt(PrefsConstant.AudioType, 0));

		if (action.Parame == 1) {//自摸
			//播放胡牌特效
			mOperationEffectItem.SetEffectType (OperationEffectItem.OperationEffectType.ZiMo);
		} 
		else {
			//播放胡牌特效
			mOperationEffectItem.SetEffectType((OperationEffectItem.OperationEffectType)action.Action);
		}
    }

    private void Remove(List<Card> cards, Card card)
    {
        if (card != null && cards != null)
        {
            Card temp = cards.Find(p => p.CardType == card.CardType && p.CardValue == card.CardValue);
            if (temp != null)
            {
                cards.Remove(temp);
            }
        }
    }

    private void SetCardsState(bool enabled)
    {
        if (mDirection == DirectionType.bottom)
        {
            if (enabled)
            {
                //UIDialogMgr.Instance.ShowDialog("该你出牌了");
            }
            //Array.ForEach<CardItem>(mHandCardList, p => { p.GetComponent<BoxCollider>().enabled = enabled; });
            //mCurrCardItem.GetComponent<BoxCollider>().enabled = enabled;
        }
    }

    private CardItem mCurrSelectedCardItem;

    private void OnDragItem(GameObject go)
    {
        CardItem mCardItem = go.GetComponent<CardItem>();

		if (mCurrSelectedCardItem == mCardItem && mCardItem.transform.localPosition.y >= 20.0f) {
//			if (mCardItem == mCurrCardItem) {
//				mCardItem.DragOver ();
//			} 
//			else {
				mCardItem.DragOver ();
				mCurrSelectedCardItem.SetTouchStateNone();
				mCurrSelectedCardItem = null;
//			}
		}
		else {
			Debug.Log ("###############  当前选中的牌不是滑动的牌 ");
		}
    }

    private void OnClickCardItem(GameObject go)
    {
        CardItem mCardItem = go.GetComponent<CardItem>();
        if (mCurrSelectedCardItem != null)
        {
			mCurrSelectedCardItem.SetTouchStateNone();
			mCurrSelectedCardItem = null;
//            if (!mCurrSelectedCardItem.Equals(mCardItem))
//            {
//                mCurrSelectedCardItem.SetTouchStateNone();
//            }
//            mCardItem.Click();
        }
        else
        {
            mCardItem.Click();
			mCurrSelectedCardItem = mCardItem;
        }

       
        //MahJongGameAction requestPack = new MahJongGameAction()
        //{
        //    Action = MahJongActionType.PlayCard
        //};
        //requestPack.Cards = new List<Card>();
        //requestPack.Cards.Add(go.GetComponent<CardItem>().getCard);
        //ActionParam actionParam = new ActionParam(requestPack);
        //Net.Instance.Send((int)ActionType.GameAction, onClickCardCallBack, actionParam);
        //SetCardsState(false);
    }

    private void onClickCardCallBack(ActionResult actionResult)
    {
        ResponseDefaultPacket responsePack = actionResult.GetValue<ResponseDefaultPacket>();
    }

    /// <summary>
    /// 开局发牌时调用
    /// </summary>
    private void RefreshHandCards()
    {
        if (mUserCardModel != null && mUserCardModel.getHandCards().Count > 0)
        {
            //排序手牌
            SortCards(mUserCardModel.getHandCards());
            for (int i = 0; i < mHandCardList.Length; i++)
            {
                if (i < mUserCardModel.getHandCards().Count)
                {
                    mHandCardList[i].Refresh(mUserCardModel.getHandCards()[i], mDirection);
                }
                else
                {
                    mHandCardList[i].Refresh(null, mDirection);
                }
            }
        }
    }

    private void RefreshHandCardGroups()
    {
        if (mUserCardModel != null && mUserCardModel.getHandCardGroups().Count > 0)
        {
            for (int i = 0; i < mUserCardModel.getHandCardGroups().Count; i++)
            {
                mCardGroupList[i].Refresh(mUserCardModel.getHandCardGroups()[i].Cards, mDirection);
            }
        }
    }

    private void RefreshTouchCard()
    {
        if (mUserCardModel.getTouchCard() != null)
        {
            mUserCardModel.addHandCard(mUserCardModel.getTouchCard());
            mCurrCardItem.Refresh(mUserCardModel.getTouchCard(), mDirection);
        }
    }

    private void RefreshDeskCards()
    {
        if (mUserCardModel != null && mUserCardModel.getDeskCards().Count > 0)
        {
            for (int i = 0; i < mDeskCardList.Length; i++)
            {
                if (i < mUserCardModel.getDeskCards().Count)
                {
                    mDeskCardList[i].Refresh(mUserCardModel.getDeskCards()[i], mDirection);
                }
                else
                {
                    mDeskCardList[i].Refresh(null, mDirection);
                }
            }
        }
    }

    public void SortCards(List<Card> cards)
    {
        if (cards != null && cards.Count > 0)
        {
            cards.Sort((x, y) =>
            {
                if (x.CardType == y.CardType)
                {
                    if (x.CardValue == y.CardValue)
                    {
                        return x.GetHashCode() - y.GetHashCode();
                    }
                    else
                        return -(x.CardValue - y.CardValue);
                }
                else
                    return -(x.CardType - y.CardType);
            });

            //宝牌放最左
            List<Card> baos = new List<Card>();
            foreach (var item in cards)
            {
                if (item.IsUniversal)
                {
                    baos.Add(item);
                }
            }
            foreach (var item in baos)
            {
                cards.Remove(item);
            }
            if (baos.Count > 0)
                cards.AddRange(baos);
        }
    }

    private void CleanUpOperationState()
    {
        if (mDirection == DirectionType.bottom && !mOperationItems.IsNullOrEmpty())
        {
            Array.ForEach<OperationItem>(mOperationItems, p => { p.SetVisible(false); });
        }
    }

    public void Initialize()
    {
        CleanUp();
        this.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        CleanUp();
        this.gameObject.SetActive(true);
    }

    private void CleanUp()
    {
        SetCardsState(false);
        mOperationEffectItem.Initialize();
        CleanUpOperationState();
        mCurrCardItem.SetVisible(false);
        Array.ForEach<CardItem>(mHandCardList, p => p.SetVisible(false));
        Array.ForEach<CardItem>(mDeskCardList, p => p.SetVisible(false));
        Array.ForEach<HandCardGroupItem>(mCardGroupList, p => p.SetVisible(false));
    }

	public void SetHandCardShow(bool isShow){
		if (mDirection == DirectionType.bottom) {
			return;
		}

		if (isShow) {
			if (mDirection == DirectionType.top) {
				foreach(var card in mHandCardList){
					card.mCardNameType = CardDisplayType.Display;
				}
				mCurrCardItem.mCardNameType = CardDisplayType.Display;
			}
//			else if (mDirection == DirectionType.left) {
//			
//			}
			else if(mDirection == DirectionType.right || mDirection == DirectionType.left){
				foreach(var card in mHandCardList){
					card.mCardNameType = CardDisplayType.Display;
					card.mCardSprite.width = 40+5;
					card.mCardSprite.height = 32+4;
				}
				mCurrCardItem.mCardNameType = CardDisplayType.Display;
				mCurrCardItem.mCardSprite.width = 40+5;
				mCurrCardItem.mCardSprite.height = 32+4;
			}
		} 
		else {
			if (mDirection == DirectionType.top) {
				foreach(var card in mHandCardList){
					card.mCardNameType = CardDisplayType.Hide;
				}
				mCurrCardItem.mCardNameType = CardDisplayType.Hide;
			}
//			else if (mDirection == DirectionType.left) {
//
//			}
			else if(mDirection == DirectionType.right || mDirection == DirectionType.left){
				foreach(var card in mHandCardList){
					card.mCardNameType = CardDisplayType.Hide;
					card.mCardSprite.width = 24;
					card.mCardSprite.height = 60;
				}
				mCurrCardItem.mCardNameType = CardDisplayType.Hide;
				mCurrCardItem.mCardSprite.width = 24;
				mCurrCardItem.mCardSprite.height = 60;
			}
		}
	}
}
