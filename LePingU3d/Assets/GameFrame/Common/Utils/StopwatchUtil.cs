using System.Diagnostics;

public class StopwatchUtil
{
    private Stopwatch mStopwatch;

    public void ResetAndStart()
    {
        if (mStopwatch == null)
        {
            mStopwatch = new Stopwatch();
        }
        mStopwatch.Reset();
        mStopwatch.Start();
    }

    public void StopAndDebug()
    {
        mStopwatch.Stop();
        DebugHelper.LogInfo("运行时间 = " + mStopwatch.ElapsedMilliseconds.ToString());
    }

    public void StopAndDebug(string des)
    {
        mStopwatch.Stop();
        DebugHelper.LogInfo(string.Format("{0}  : {1}", des, mStopwatch.ElapsedMilliseconds));
    }
}
