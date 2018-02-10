public class EventMgr : Singleton<EventMgr>
{
    private EventDispatcher mMainEventMgr;
    private EventDispatcher mLoginEventMgr;

    public override void OnInit()
    {
        mLoginEventMgr = new EventDispatcher();
        mMainEventMgr = new EventDispatcher();
        base.OnInit();
    }

    public override void OnUnInit()
    {
        if (mMainEventMgr != null)
        {
            mMainEventMgr.CleanUp();
            mMainEventMgr = null;
        }
        if (mLoginEventMgr != null)
        {
            mLoginEventMgr.CleanUp();
            mLoginEventMgr = null;
        }
        base.OnUnInit();
    }

    public EventDispatcher MainMgr
    {
        get { return this.mMainEventMgr; }
        private set { }
    }

    public EventDispatcher LoginMgr
    {
        get { return this.mLoginEventMgr; }
        private set { }
    }
}
