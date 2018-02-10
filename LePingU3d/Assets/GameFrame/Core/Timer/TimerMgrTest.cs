/*****************************************************
 * 作者: 深红DRed(龙涛) 1036409576@qq.com
 * 创建时间：2015.8.20
 * 描述：时间测试类
 ****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerMgrTest : MonoBehaviour
{
    //在项目中因为热更新、避免控件太多管理混乱等原因，都是采用代码初始控件的方式。而不是采用Unity传统的拖拽方法
    private Text mTextAwardCountdown;

    private Text mTextRefrestCountdown;
    private Text mTextChallengeCountdown;

    private Button mButtonChallenge;

    private void Awake()
    {
        mTextAwardCountdown = transform.Find("Award/TextCountdown").GetComponent<Text>();
        mTextRefrestCountdown = transform.Find("ListRefresh/TextCountdown").GetComponent<Text>();
        mTextChallengeCountdown = transform.Find("ButtonChallenge/Text").GetComponent<Text>();

        mButtonChallenge = transform.Find("ButtonChallenge").GetComponent<Button>();
        mButtonChallenge.enabled = false;
    }

    private void Start()
    {
        //奖状倒计时，使用TimeEventType.Time的话，当游戏失去焦点（后台运行）时间会静止不动
        TimerMgr.instance.Subscribe(2389797f, false, TimeEventType.Time).SetIntervalTime(1f).OnUpdate(x =>
        {   //lambda 表达式有着很强大的生产力，但是不要在注重性能的地方滥用
            mTextAwardCountdown.text = TimeUtility.TimeToChineseString(x.surplusTimeRound);
        }).Start();

        //榜单刷新，使用TimeEventType.Time的话，当游戏失去焦点（后台运行）时间依然在正常的流逝
        TimerMgr.instance.Subscribe(3234f, false, TimeEventType.IngoreTimeScale).SetIntervalTime(1f).OnUpdate(x =>
        {
            mTextRefrestCountdown.text = TimeUtility.TimeToChineseString(x.surplusTimeRound);
        }).Start();

        //挑战倒计时
        TimerMgr.instance.Subscribe(10f, false, TimeEventType.IngoreTimeScale).SetIntervalTime(1f).OnUpdate(x =>
        {
            mTextChallengeCountdown.text = string.Format("挑战({0})", x.surplusTimeRound);
        }).OnComplete(() => mButtonChallenge.enabled = true).Start();

    }

    private void OnDestroy()
    {
        //计时器如果自然结束生命周期是不会有问题的，如果中途退出，就需要计得回收计时器。在本人自己的项目是在框架层封装完成的。如果其他同学有兴趣的话再完善此处的功能
    }

}
