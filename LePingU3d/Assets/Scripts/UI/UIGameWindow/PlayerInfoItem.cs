using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerInfoItem : MonoBehaviour
{
    private RoomState mRoomState;
    private UserInfo mUserInfo;
    private Transform mCacheTransform;
    private UISprite mNoneIcon;
    private UIWidget mPlayerWidget;
    private UIGameModel mUIGameModel;

    private Transform mReadyState;
    private UIButton mBtnReady;
    private UILabel mName;
    private UILabel mScore;
    private UITexture mHeadIcon;
    private UISprite mOffLineSprite;
    private UISprite mZhuangIcon;
    private UISprite mOwnerIcon;
    private UIChatItem mUIChatItem;
    private GameObject mUIExpressionItem;
    private List<PlayerInfoItem> mPlayerItems = new List<PlayerInfoItem>();

    private void Awake()
    {
        mUIGameModel = UIModelMgr.Instance.GetModel<UIGameModel>();
        mCacheTransform = this.transform;
        mNoneIcon = mCacheTransform.FindComponent<UISprite>("None");
        mPlayerWidget = mCacheTransform.FindComponent<UIWidget>("InfoGroup");

        mOffLineSprite = mCacheTransform.FindComponent<UISprite>("InfoGroup/HeadIcon/OffLine");
        mZhuangIcon = mCacheTransform.FindComponent<UISprite>("InfoGroup/HeadIcon/IconZhuang");
        mOwnerIcon = mCacheTransform.FindComponent<UISprite>("InfoGroup/HeadIcon/IconOwner");
        mHeadIcon = mCacheTransform.FindComponent<UITexture>("InfoGroup/HeadIcon");
        mUIChatItem = mCacheTransform.FindComponent<UIChatItem>("InfoGroup/UIChatItem");
        mBtnReady = mCacheTransform.FindComponent<UIButton>("InfoGroup/StateGroup/BtnReady");
        mReadyState = mCacheTransform.FindComponent<Transform>("InfoGroup/StateGroup/ReadyState");
        mName = mCacheTransform.FindComponent<UILabel>("InfoGroup/Name");
        mScore = mCacheTransform.FindComponent<UILabel>("InfoGroup/Score");
        mUIExpressionItem = mCacheTransform.Find("InfoGroup/UIExpressionItem").gameObject;
        UIEventListener.Get(mBtnReady.gameObject).onClick += OnReadyClick;
        mCacheTransform.GetUIEventListener("InfoGroup/HeadIcon").onClick += OnUserInfoClick;

    }

    private void Start()
    {
        mZhuangIcon.gameObject.SetVisible(false);
        mUIChatItem.SetVisible(false);

		SetVisible(false);
    }

    public void Refresh(UserInfo data)
    {
        this.mUserInfo = data;
        if (mUserInfo != null)
        {
            mOwnerIcon.gameObject.SetVisible(mUserInfo.IsOwner);
            //Debug.Log(mUserInfo.IsZhuang);
            mZhuangIcon.gameObject.SetVisible(mUserInfo.IsZhuang);
            AsyncImageDownload.Instance.SetAsyncImage(mUserInfo.HeadUrl, mHeadIcon);
            mName.text = mUserInfo.NickName;
            mScore.text = mUserInfo.RoomScore.ToString();
            mOffLineSprite.gameObject.SetActive(!mUserInfo.IsOnlining);
            RefreshGameState();
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }
    }

    /// <summary>
    /// 初始化，GamePlaying()时调用ai
    /// </summary>
    /// <param name="state"></param>
    public void Initialize(RoomState state)
    {
        this.mRoomState = state;
        RefreshGameState();
    }

    private void OnUserInfoClick(GameObject go)
    {
        if (mUserInfo != null)
        {
            UIWindowMgr.Instance.PushPanel<UIPlayerInfoWindow>(mUserInfo);
        }
    }

    /// <summary>
    /// 显示聊天信息
    /// </summary>
    /// <param name="mRoomMsgInfo"></param>
    public void ShowRoomMsgInfo(RoomMsgInfo mRoomMsgInfo)
    {
        if (mRoomMsgInfo != null)
        {
            if (mRoomMsgInfo.MsgType == RoomMsgType.SimpleText)
            {
                MsgInfo info = JsonUtil.DeserializeObject<MsgInfo>(Encoding.UTF8.GetString(mRoomMsgInfo.Content));
                mUIChatItem.ShowChatInfo(info.MsgContent);
                MahjongAudioMgr.Instance.PlayChatSound(mUserInfo.Sex, info.MsgIndex, PlayerPrefs.GetInt(PrefsConstant.AudioType, 0));
            }
            else if (mRoomMsgInfo.MsgType == RoomMsgType.Sound)
            {
                MahjongAudioMgr.Instance.PauseBGM(true);
                ViSpeak.Instance.PlayReconding(mRoomMsgInfo.Content, PlayGM);
            }
            else if (mRoomMsgInfo.MsgType == RoomMsgType.Image)
            {
                //显示表情
                Debug.Log("---------------------------------------显示表情---------------------------------");
                GameObject expressionPrefab = Resources.Load<GameObject>("ExpressionPrefab/" + Encoding.UTF8.GetString(mRoomMsgInfo.Content));
                GameObject expressionObject = GameObject.Instantiate(expressionPrefab);
                expressionObject.SetParent(mUIExpressionItem.transform);
                expressionObject.transform.localPosition = new Vector3(0, 0, 0);
                expressionObject.transform.localScale = new Vector3(60, 60, 60);
                Destroy(expressionObject, 3);
            }
            else if (mRoomMsgInfo.MsgType == RoomMsgType.Prop)
            {
                //显示表情
                Debug.Log("---------------------------------------显示道具---------------------------------");
                ShowProp(mRoomMsgInfo);
            }
            else
            {
                mUIChatItem.ShowChatInfo(Encoding.UTF8.GetString(mRoomMsgInfo.Content));
            }
        }
    }

    private void ShowProp(RoomMsgInfo mRoomMsgInfo)
    {
        //获取所有玩家
        mPlayerItems.Clear();
        mPlayerItems.Add(UIGameWindow.Instance.CacheTrans.FindComponent<PlayerInfoItem>("Root/ContainerBottom/PlayerInfo"));
        mPlayerItems.Add(UIGameWindow.Instance.CacheTrans.FindComponent<PlayerInfoItem>("Root/ContainerRight/PlayerInfo"));
        mPlayerItems.Add(UIGameWindow.Instance.CacheTrans.FindComponent<PlayerInfoItem>("Root/ContainerTop/PlayerInfo"));
        mPlayerItems.Add(UIGameWindow.Instance.CacheTrans.FindComponent<PlayerInfoItem>("Root/ContainerLeft/PlayerInfo"));

        PropParameter parameter = JsonUtil.DeserializeObject<PropParameter>(Encoding.UTF8.GetString(mRoomMsgInfo.Content));

        int pos1 = GameLogicMgr.Instance.getPlayerIndex(mUIGameModel.GetUser(parameter.idFrom).RoomOrder);
        int pos2 = GameLogicMgr.Instance.getPlayerIndex(mUIGameModel.GetUser(parameter.idTo).RoomOrder);

        var startPos = mPlayerItems[pos1].transform.Find("InfoGroup/HeadIcon").transform.position;
        var targetPos = mPlayerItems[pos2].transform.Find("InfoGroup/HeadIcon").transform.position;

        var propItem = GameObject.Instantiate(Resources.Load<GameObject>("PropPrefab/" + parameter.propPrefabName)).GetComponent<PropController>();
        propItem.mPosFrom = startPos;
        propItem.mPosTo = targetPos;
        propItem.Play();
    }

    public void PlayGM()
    {
        MahjongAudioMgr.Instance.PauseBGM(false);
    }

    private void OnReadyClick(GameObject go)
    {
        ActionParam actionParam = new ActionParam();
        actionParam["IsGetReady"] = 1;
        Net.Instance.Send((int)ActionType.RoomReady, null, actionParam);
    }

    private void OnReadyCallBack(ActionResult mActionResult)
    {
        ResponseDefaultPacket responsePack = mActionResult.GetValue<ResponseDefaultPacket>();
        if (responsePack != null && responsePack.Success)
        {
            UIModelMgr.Instance.GetModel<UIMainModel>().PlayerInfo.IsReady = true;
            mUserInfo.IsReady = true;
            SetReadyState();
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog(responsePack.Result);
        }
    }

    private void RefreshGameState()
    {
        if (mRoomState == RoomState.Gaming)
        {
            mBtnReady.gameObject.SetActive(false);
            mReadyState.gameObject.SetActive(false);
        }
        else
        {
            SetReadyState();
        }
    }

    public void SetVisible(bool isVisible)
    {
        this.mPlayerWidget.gameObject.SetVisible(isVisible);
        this.mNoneIcon.gameObject.SetVisible(!isVisible);
    }

    private void SetReadyState()
    {
        if (mUserInfo != null)
        {
            if (mUserInfo.IsReady)
            {
                mBtnReady.gameObject.SetActive(false);
                mReadyState.gameObject.SetActive(true);
            }
            else
            {
                if (UIModelMgr.Instance.GetModel<UIGameModel>().IsOneself(mUserInfo.UserId))
                {
                    mBtnReady.gameObject.SetActive(true);
                    mReadyState.gameObject.SetActive(false);
                }
                else
                {
                    mBtnReady.gameObject.SetActive(false);
                    mReadyState.gameObject.SetActive(false);
                }
            }
        }

    }
}
