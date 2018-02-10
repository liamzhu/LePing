using System.Collections;
using UnityEngine;

public class GameTableItem : MonoBehaviour
{
    private UILabel mTimer;
    private TweenAlpha mEastTweenAlpha;
    private TweenAlpha mSouthTweenAlpha;
    private TweenAlpha mWestTweenAlpha;
    private TweenAlpha mNorthTweenAlpha;

    private void Awake()
    {
        mTimer = transform.FindComponent<UILabel>("Timer");
        mEastTweenAlpha = transform.FindComponent<TweenAlpha>("EastDirection");
        mSouthTweenAlpha = transform.FindComponent<TweenAlpha>("SouthDirection");
        mWestTweenAlpha = transform.FindComponent<TweenAlpha>("WestDirection");
        mNorthTweenAlpha = transform.FindComponent<TweenAlpha>("NorthDirection");
		mTimer.text = "0";
    }

    public void CleanUp()
    {
        if (mTimeEvent != null)
        {
            mTimeEvent.Reset();
        }
        mEastTweenAlpha.gameObject.SetVisible(false);
        mSouthTweenAlpha.gameObject.SetVisible(false);
        mWestTweenAlpha.gameObject.SetVisible(false);
        mNorthTweenAlpha.gameObject.SetVisible(false);
        mTimer.text = string.Format("{0}", 0);
    }

    private TimeEvent mTimeEvent;

	bool isPlayTimeAlarm = false;
    public void OperateAction(int pos, int timer = 15)
    {
        if (mTimeEvent != null)
        {
            mTimeEvent.Reset();
        }
			
		if (!GameMgr.Instance.isFromRecord) {
			mTimeEvent = TimerMgr.instance.Subscribe(timer, false, TimeEventType.IngoreTimeScale).OnUpdate(x =>
				{
					mTimer.text = string.Format("{0}", x.surplusTimeRound);

					//******控制时间哒哒哒播放 start
					if(!isPlayTimeAlarm && UIWindowMgr.Instance.mCurrPage.CacheGo.name != "UISingleSettlementWindow(Clone)"
						&& UIWindowMgr.Instance.mCurrPage.CacheGo.name != "UIGameSettlementWindow(Clone)"){
						if(mTimer.text == "5"){
							isPlayTimeAlarm = true;
							//开始播放
							MahjongAudioMgr.Instance.PlayTimeDaDaDa();
						}
					}
					else{
						if(mTimer.text == "15" || mTimer.text == "0"){
							isPlayTimeAlarm = false;
							MahjongAudioMgr.Instance.StopTimeDaDaDa();
						}		
					}
					//******控制时间哒哒哒播放 end

				}).Start();
		}
       
				

        if (pos == 0)
        {
            mEastTweenAlpha.gameObject.SetVisible(true);
            mSouthTweenAlpha.gameObject.SetVisible(false);
            mWestTweenAlpha.gameObject.SetVisible(false);
            mNorthTweenAlpha.gameObject.SetVisible(false);
        }
        else if (pos == 1)
        {
            mEastTweenAlpha.gameObject.SetVisible(false);
            mSouthTweenAlpha.gameObject.SetVisible(true);
            mWestTweenAlpha.gameObject.SetVisible(false);
            mNorthTweenAlpha.gameObject.SetVisible(false);
        }
        else if (pos == 2)
        {
            mEastTweenAlpha.gameObject.SetVisible(false);
            mSouthTweenAlpha.gameObject.SetVisible(false);
            mWestTweenAlpha.gameObject.SetVisible(true);
            mNorthTweenAlpha.gameObject.SetVisible(false);
        }
        else if (pos == 3)
        {
            mEastTweenAlpha.gameObject.SetVisible(false);
            mSouthTweenAlpha.gameObject.SetVisible(false);
            mWestTweenAlpha.gameObject.SetVisible(false);
            mNorthTweenAlpha.gameObject.SetVisible(true);
        }
    }
}
