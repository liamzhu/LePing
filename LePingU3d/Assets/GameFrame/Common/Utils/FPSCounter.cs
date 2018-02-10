using System.Collections;
using UnityEngine;

public class FPSCounter : Singleton<FPSCounter>
{
    // 帧率计算频率
    private const float mCalcRate = 0.5F;

    // 本次计算频率下帧数
    private int mFrameCount = 0;

    // 频率时长
    private float timePassed = 0f;

    // 显示帧率
    private float mFps = 0f;

    public void Initialize()
    {
        this.mFrameCount = 0;
        this.timePassed = 0f;
    }

    public override void OnInit()
    {
        ApplicationMgr.Instance.onUpdate += OnUpdate;
        ApplicationMgr.Instance.onGUI += OnGUI;
        base.OnInit();
    }

    public override void OnUnInit()
    {
        ApplicationMgr.Instance.onUpdate -= OnUpdate;
        ApplicationMgr.Instance.onGUI -= OnGUI;
        base.OnUnInit();
    }

    private void OnUpdate()
    {
        ++this.mFrameCount;
        this.timePassed += Time.deltaTime;
        if (this.timePassed > mCalcRate)
        {
            this.mFps = this.mFrameCount / this.timePassed;
            this.mFrameCount = 0;
            this.timePassed = 0f;
        }
    }

    private void OnGUI()
    {
        GUIStyle bb = new GUIStyle();
        bb.normal.background = null;    //这是设置背景填充的
        bb.normal.textColor = new Color(1.0f, 0.5f, 0.0f);   //设置字体颜色的
        bb.fontSize = 40;       //当然，这是字体大小

        //居中显示FPS
        GUI.Label(new Rect((Screen.width / 2) - 100, 0, 200, 200), string.Format("FPS: {0:N1}", mFps), bb);
    }
}
