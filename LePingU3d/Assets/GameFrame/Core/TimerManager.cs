// ******************************************
// 文件名(File Name):             TimerManager.cs
// 创建时间(CreateTime):        20160314
// ******************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TimerCallBack(int id, float time);

public delegate void TimerOverCall(float time);

public delegate void LiamTimeCallBack();

public class TimerManager : MonoSingleton<TimerManager>
{
    private float interval = 0;
    private Dictionary<int, TimerInfo> m_TimerManager = new Dictionary<int, TimerInfo>();

    public float Interval
    {
        get { return interval; }
        set { interval = value; }
    }

    /// <summary>
    /// 启动定时
    /// </summary>
    /// <param name="value"></param>
    public void StartTimer(float value)
    {
        interval = value;
        InvokeRepeating("Run", 0, interval);
    }

    /// <summary>
    /// 停止定时器
    /// </summary>
    public void StopTimer()
    {
        CancelInvoke("Run");
    }

    public bool HasTimer(int timerId)
    {
        return m_TimerManager.ContainsKey(timerId);
    }

    /// <summary>
    /// 创建定时器
    /// </summary>
    /// <param name="timerID">定时器ID</param>
    /// <param name="Repeats">重复次数</param>
    /// <param name="TiemInterval">时间间隔</param>
    /// <param name="call">回调</param>
    /// <returns></returns>
    public bool CreateTimer(int timerID, int Repeats, float TiemInterval, TimerCallBack call, TimerOverCall overCall = null)
    {
        TimerInfo timeInfo = null;
        lock (m_TimerManager)
        {
            if (m_TimerManager.TryGetValue(timerID, out timeInfo) && timeInfo != null)
            {
                timeInfo.m_Repeats = Repeats;
                timeInfo.m_TimeInterval = TiemInterval;
                timeInfo.m_Callback = call;
                timeInfo.m_timeOverCall = overCall;
                timeInfo.m_TempTimeVal = 0;
                return true;
            }
        }
        timeInfo = new TimerInfo(timerID, Repeats, TiemInterval, call, overCall);

        lock (m_TimerManager)
        {
            m_TimerManager.Add(timerID, timeInfo);
        }
        StartTimer(timeInfo.m_TimeInterval);
        return true;
    }

    /// <summary>
    /// 删除定时器
    /// </summary>
    /// <param name="timeID">定时器ID</param>
    /// <returns></returns>
    public bool DelTimer(int timeID)
    {
        TimerInfo timerInfo = null;
        lock (m_TimerManager)
        {
            if (m_TimerManager.TryGetValue(timeID, out timerInfo) && timerInfo != null)
            {
                timerInfo.delete = true;
                return true;
            }
        }
        return true;
    }

    public void CusDelTimer(int timeID)
    {
        lock (m_TimerManager)
        {
            if (m_TimerManager.ContainsKey(timeID))
            {
                m_TimerManager.Remove(timeID);

            }
        }
    }

    /// <summary>
    /// 停止定时器
    /// </summary>
    /// <param name="timeID">定时器ID</param>
    /// <returns></returns>
    public bool StopTimer(int timeID)
    {
        TimerInfo timerInfo = null;
        lock (m_TimerManager)
        {
            if (m_TimerManager.TryGetValue(timeID, out timerInfo) && timerInfo != null)
            {
                timerInfo.stop = true;
                return true;
            }
        }
        return true;
    }

    /// <summary>
    /// 重新开启定时器
    /// </summary>
    /// <param name="timeID">定时器ID</param>
    /// <returns></returns>
    public bool ResumeTimer(int timeID)
    {
        TimerInfo timerInfo = null;
        lock (m_TimerManager)
        {
            if (m_TimerManager.TryGetValue(timeID, out timerInfo) && timerInfo != null)
            {
                timerInfo.stop = false;
                return true;
            }
        }
        return true;
    }

    /// <summary>
    /// 定时器跑
    /// </summary>
    private void Run()
    {
        if (m_TimerManager.Count <= 0) return;
        lock (m_TimerManager)
        {
            List<int> timerKey = new List<int>();
            IEnumerator enumerator = m_TimerManager.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TimerInfo timerInfo = enumerator.Current as TimerInfo;
                if (timerInfo == null)
                    continue;
                // 校验是否到期
                if (timerInfo.stop)
                    continue;
                if (timerInfo.delete)
                {
                    timerKey.Add(timerInfo.timerID);
                    continue;
                }
                //时间++
                timerInfo.m_TempTimeVal += interval;
                //时间还没到一个间隔
                if (timerInfo.m_TempTimeVal < timerInfo.m_TimeInterval)
                    continue;
                // 运行间隔(持续次数)
                --timerInfo.m_Repeats;
                timerInfo.m_TempTimeVal = 0;
                //回调
                if (timerInfo.m_Callback != null)
                {
                    timerInfo.m_Callback(timerInfo.timerID, timerInfo.m_Repeats);
                }
                //运行间隔(持续次数)
                if (timerInfo.m_Repeats <= 0)
                {
                    timerInfo.delete = true;
                    timerKey.Add(timerInfo.timerID);
                    if (timerInfo.m_timeOverCall != null)
                    {
                        timerInfo.m_timeOverCall(timerInfo.m_TimeInterval);
                    }
                }
            }
            //按照key移除
            for (int i = 0; i < timerKey.Count; i++)
            {
                TimerInfo temp = null;
                if (m_TimerManager.TryGetValue(timerKey[i], out temp) && temp != null)
                {
                    m_TimerManager.Remove(timerKey[i]);
                }
            }
            timerKey.Clear();
            timerKey = null;
        }
    }

    /// <summary>
    /// unity 生命周期函数
    /// </summary>
    protected override void OnUnInit()
    {
        m_TimerManager.Clear();
        m_TimerManager = null;
        base.OnUnInit();
    }
		
	public float recordBackDurTime = 2.0f;
	LiamTimeCallBack myCall = null;
	public void LiamTimeStart(LiamTimeCallBack call){
		//InvokeRepeating ("LiamRun", 0, dur);
		myCall = call;
		StartCoroutine(WaitToRunNext(recordBackDurTime));
	}

	IEnumerator WaitToRunNext(float time){
		yield return new WaitForSeconds (time);
		if (myCall != null) {
			myCall ();
		}
		StartCoroutine(WaitToRunNext(recordBackDurTime));
	}
		
	public void LiamTimeCancel(){
		//CancelInvoke ("LiamRun");
		myCall = null;
		StopAllCoroutines ();
		//StopCoroutine("WaitToRunNext");
	} 

//	public void LiamTimeResume(){
//		StartCoroutine(WaitToRunNext(recordBackDurTime));
//	}
}

public class TimerInfo
{
    public bool stop;                       //停止
    public bool delete;                     //删除

    public int timerID;                     //计时间ID
    public int m_Repeats;                   //重复次数
    public float m_TimeInterval;            //时间间隔
    public float m_TempTimeVal;             //临时计算时间
    public TimerCallBack m_Callback;        //回调
    public TimerOverCall m_timeOverCall;    //倒计时结束回调

    public TimerInfo(int itemID, int Repeats, float TimeInterval, TimerCallBack callBack, TimerOverCall overCall = null)
    {
        this.timerID = itemID;
        this.m_Repeats = Repeats;
        this.m_TimeInterval = TimeInterval;
        this.m_Callback = callBack;
        this.m_timeOverCall = overCall;
        m_TempTimeVal = 0;
        stop = false;
        delete = false;
    }

}
