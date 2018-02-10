/*****************************************************
 * 作者: 深红DRed(龙涛) 1036409576@qq.com
 * 创建时间：2015.1.13
 * 版本：1.0.0
 * 描述：智能时间管理类(包含定时触发)
 ****************************************************/

using System;
using UnityEngine;
using Utility;

public enum TimeEventType
{
    /// <summary> Unity的平常时间，会受TimeScale影响 </summary>
    Time,

    /// <summary> 真实时间 </summary>
    IngoreTimeScale,

    /// <summary> 使用服务器的时间 </summary>
    RealServerTime
}

public class TimeEvent
{

    /// <summary> 过去了的时间（可用于普通增量计时） </summary>
    public float elapsedTime;

    /// <summary> 剩余的时间（可用于倒计时） </summary>
    public float surplusTime;

    public int surplusTimeRound
    {
        get { return Mathf.RoundToInt(surplusTime); }   //四舍五入到最近的整数
    }

    public int elapsedTimeRound
    {
        get { return Mathf.RoundToInt(elapsedTime); }   //四舍五入到最近的整数
    }

    /// <summary> 携带的消息事件 </summary>
    public object msgObj;

    /// <summary> 用来标识事件的唯一性，每一次生成的事件都不相同 </summary>
    public int id;

    /// <summary> 是否暂停 </summary>
    private bool mPause = false;

    /// <summary> 起始时间 </summary>
    private float mStartTime;

    /// <summary> 结束的时间 </summary>
    private float mEndTime;

    /// <summary> 下一次触发时间 </summary>
    private float mNextTriggerTime;

    /// <summary> 更新的间隔 </summary>
    private float mUpdateIntervalTime;

    /// <summary> 使用真实时间 </summary>
    private bool mIngoreTimeScale;

    private bool mHasEventCallback;

    private bool mSingle;

    private bool mInit;

    private TimeEventType mTimeEventType = TimeEventType.Time;

    private float currentTime
    {
        get
        {
            if (mTimeEventType == TimeEventType.Time)
            {
                return Time.time;
            }
            return Time.realtimeSinceStartup;
        }
    }

    /// <summary> 结束回调 </summary>
    private Action<TimeEvent> mOnCompleteCallback;

    /// <summary> 更新回调 </summary>
    private Action<TimeEvent> mOnUpdateCallback;

    /// <summary> 结束回调 </summary>
    private Action mOnEmptyCompleteCallback;

    /// <summary> 更新回调 </summary>
    private Action mOnEmptyUpdateCallback;

    public void Init(float callTime, int eventGlobalID, TimeEventType timeType)
    {
        mTimeEventType = timeType;
        mStartTime = currentTime;
        mEndTime = mStartTime + callTime;
        mNextTriggerTime = mEndTime;
        id = eventGlobalID;
    }

    /// <summary>
    /// 执行
    /// </summary>
    /// <returns> 是否需要回收 </returns>
    public bool Execute()
    {
        if (!mHasEventCallback)
        {   //如果没有任何监听函数
            return true;
        }

        bool executeComplete = false;
        if (mNextTriggerTime <= currentTime)
        {
            elapsedTime = currentTime - mStartTime;
            surplusTime = mEndTime - currentTime;

            if (mOnUpdateCallback != null)
            {
                mOnUpdateCallback(this);
            }
            if (mOnEmptyUpdateCallback != null)
            {
                mOnEmptyUpdateCallback();
            }
            if (mNextTriggerTime >= mEndTime)
            {
                executeComplete = true;
                if (mOnCompleteCallback != null)
                {
                    mOnCompleteCallback(this);
                }
                if (mOnEmptyCompleteCallback != null)
                {
                    mOnEmptyCompleteCallback();
                }
            }
            else {
                mNextTriggerTime = mStartTime + elapsedTime + mUpdateIntervalTime;
                mNextTriggerTime = Mathf.Min(mNextTriggerTime, mEndTime);
            }
        }
        return executeComplete;
    }

    public TimeEvent SetIntervalTime(float intervalCallTime, Action<TimeEvent> onUpdate = null)
    {
        mUpdateIntervalTime = intervalCallTime;
        mNextTriggerTime = mStartTime + mUpdateIntervalTime;
        mNextTriggerTime = Mathf.Min(mNextTriggerTime, mEndTime);
        mOnUpdateCallback = onUpdate;
        mHasEventCallback = true;
        return this;
    }

    public TimeEvent SetPause(bool pauseState)
    {
        mPause = pauseState;
        return this;
    }

    public TimeEvent OnUpdate(Action<TimeEvent> onUpdate)
    {
        mHasEventCallback = true;
        mOnUpdateCallback = onUpdate;
        return this;
    }

    public TimeEvent OnUpdate(Action onUpdate)
    {
        mHasEventCallback = true;
        mOnEmptyUpdateCallback = onUpdate;
        return this;
    }

    public TimeEvent OnComplete(Action<TimeEvent> onComplete)
    {
        mHasEventCallback = true;
        mOnCompleteCallback = onComplete;
        return this;
    }

    public TimeEvent OnComplete(Action onComplete)
    {
        mHasEventCallback = true;
        mOnEmptyCompleteCallback = onComplete;
        return this;
    }

    public TimeEvent Single()
    {
        mSingle = true;
        if (!mHasEventCallback)
        {
            Debug.LogError("请在设置完回调函数再使用Single功能");
        }
        TimerMgr.instance.SingleSet(this);    //此处调用外部功能有点不爽，但又不想在初始化中设置太多的选项
        return this;
    }

    //延时执行
    public TimeEvent Delay()
    {
        mSingle = true;
        if (!mHasEventCallback)
        {
            Debug.LogError("请在设置完回调函数再使用Single功能");
        }
        TimerMgr.instance.SingleSet(this);    //此处调用外部功能有点不爽，但又不想在初始化中设置太多的选项
        return this;
    }

    //判断回调是否相同
    public bool CallbackEqual(TimeEvent targetTimeEvent)
    {
        if (mOnCompleteCallback == targetTimeEvent.mOnCompleteCallback || mOnEmptyCompleteCallback == targetTimeEvent.mOnEmptyCompleteCallback
            || mOnUpdateCallback == targetTimeEvent.mOnUpdateCallback || mOnEmptyUpdateCallback == targetTimeEvent.mOnEmptyUpdateCallback)
        {
            return true;
        }
        return false;
    }

    //开始执行，否则就要等到下一次条件满足时才执行。Start要放到最后调用
    public TimeEvent Start()
    {
        mNextTriggerTime = mStartTime;
        mNextTriggerTime = Mathf.Min(mNextTriggerTime, mEndTime);
        Execute();
        return this;
    }

    //重置，以供下一次使用
    public void Reset()
    {
        mInit = false;
        id = 0;
        mStartTime = 0;
        mEndTime = 0;
        mPause = false;
        mUpdateIntervalTime = 0;
        mHasEventCallback = false;
        mOnEmptyCompleteCallback = null;
        mOnCompleteCallback = null;
        mOnEmptyUpdateCallback = null;
        mOnUpdateCallback = null;
    }

}

public class TimerMgr : MonoBehaviour
{
    private static TimerMgr _instance;

    public static TimerMgr instance
    {
        get { return _instance ?? (_instance = FindObjectOfType(typeof(TimerMgr)) as TimerMgr); }
        set { _instance = value; }
    }

    private readonly ObjectPoolUtility<TimeEvent> mTimeMonitors = new ObjectPoolUtility<TimeEvent>();

    /// <summary>
    /// 服务器与客户端的秒数差。正数代表服务器比本地时间慢，负数代表快
    /// </summary>
    public float serverTimeDiff { get; set; }

    private float mCurrentTime = 0;

    //用来全局标识事件
    private int mEventId = 0;

    private void Update()
    {
        if (mTimeMonitors.usingObjects.Count > 0)
        {
            for (int i = 0; i < mTimeMonitors.usingObjects.Count; i++)
            {
                TimeEvent timeEvent = mTimeMonitors.usingObjects[i];
                if (timeEvent.Execute())
                {
                    timeEvent.Reset();
                    mTimeMonitors.Recycle(timeEvent);
                    --i;
                }
            }
        }
    }

    /// <summary>
    /// 时间事件
    /// </summary>
    /// <param name="callTimeFromNow">如果useTimestamp=true代表使用时间戳，否则为从现在开始往后执行的时间</param>
    /// <param name="useTimestamp">是否为时间戳，否则为时间间隔差</param>
    /// <param name="eventType">计时时间类型</param>
    public TimeEvent Subscribe(double callTimeFromNow, bool useTimestamp, TimeEventType eventType)
    {
        return Schedule(callTimeFromNow, useTimestamp, null, eventType);
    }

    private TimeEvent Schedule(double callTime, bool useTimestamp, object msg, TimeEventType timeEventType)
    {
        float timeDiff = 0;
        if (timeEventType == TimeEventType.RealServerTime)
        {
            timeDiff = serverTimeDiff;
        }

        if (!useTimestamp)
        {
            callTime = callTime + timeDiff;//不使用时间戳
        }
        else {
            callTime = callTime - TimeUtility.nowToTimestamp + timeDiff;
        }
        var timeMonitor = mTimeMonitors.Spawn();
        timeMonitor.Init((float)callTime, mEventId, timeEventType); //此处已经可以转换成float，是转换后的时间差
        return timeMonitor;
    }

    public void SingleSet(TimeEvent timeEvent)
    {
        for (int i = 0; i < mTimeMonitors.usingObjects.Count; i++)
        {
            var timeEventTmp = mTimeMonitors.usingObjects[i];
            if (timeEventTmp != timeEvent && timeEventTmp.CallbackEqual(timeEvent))
            {
                timeEventTmp.Reset();
            }
        }
    }

}
