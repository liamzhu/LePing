using DG.Tweening;
using System.Collections;
using UnityEngine;

public abstract class UIBasePanel
{
    #region Cache gameObject & transfrom

    private Transform _mTrans;

    public Transform CacheTrans
    {
        get { return _mTrans; }
    }

    private GameObject _mGo;

    public GameObject CacheGo
    {
        get { return _mGo; }
    }

    #endregion Cache gameObject & transfrom

    #region 实现接口  RegisterEvent  RemoveEvent

    /// <summary>
    /// 注册事件
    /// </summary>
    protected virtual void RegisterEvent() { }

    /// <summary>
    /// 注销事件
    /// </summary>
    protected virtual void RemoveEvent() { }

    #endregion 实现接口  RegisterEvent  RemoveEvent

    #region 构造函数

    private UIBasePanel()
    {
    }

    public UIBasePanel(UIProperty _UIProperty)
    {
        this.UIPageProperty = _UIProperty;
    }

    protected virtual void InitUIPageProperty()
    {

    }

    #endregion 构造函数

    #region 所有的状态

    public void OnPreloading()
    {
        OnInit();
        OnDeactive();
    }

    public void OnEnter()
    {
        OnInit();
        UILayerMgr.Instance.SetDepthAndRoot(this);
        OnActive();
        OnRefresh();
    }

    //public abstract void OnPause();
    //public abstract void OnResume();

    protected abstract void OnAwakeInitUI();

    public abstract void OnRefresh();

    protected virtual void OnExitBefore()
    {
    }

    public void OnExit()
    {
        OnExitBefore();
        OnDeactive();
    }

    public void OnRelease()
    {
        RemoveEvent();
        OnExit();
        this.mData = null;
        GameObject.Destroy(this.CacheGo);
    }

    protected virtual void OnPlayOpenUIAudio()
    {
    }

    protected virtual void OnPlayCloseUIAudio()
    {
    }

    #endregion 所有的状态

    private void OnInit()
    {
        InitUIPageProperty();
        if (this.CacheGo == null && !string.IsNullOrEmpty(UIPageProperty.UIPath))
        {
            if (!AppConst.IsAsyncUI)
            {
                this._mGo = ResMgr.Instance.LoadUIPrefab(UIPageProperty.UIPath, true);
                AnchorUIGameObject(this.CacheGo);
                OnAwakeInitUI();
            }
            else
            {
                if (ResMgr.Instance.LoadUIPrefabAsync(UIPageProperty.UIPath, delegate (float process, bool isDone, GameObject obj)
                {
                    if (isDone && obj != null)
                    {
                        this._mGo = GameObject.Instantiate(obj);
                    }
                }, true))
                {
                    AnchorUIGameObject(this.CacheGo);
                    OnAwakeInitUI();
                }
            }
            RegisterEvent();
        }
    }

    private void OnActive()
    {
        UILayerMgr.Instance.SetVisible(this.CacheGo, true);
        OnPlayOpenUIAudio();
        isActived = true;
    }

    private void OnDeactive()
    {
        OnPlayCloseUIAudio();
        UILayerMgr.Instance.SetVisible(this.CacheGo, false);
        isActived = false;
    }

    protected void OnReturnClick(GameObject go)
    {
        UIWindowMgr.Instance.PopPanel();
    }

    private void AnchorUIGameObject(GameObject go)
    {
        if (UILayerMgr.Instance == null || go == null) return;
        this._mTrans = go.transform;

        //check if this is ugui or (ngui)?
        Vector3 anchorPos = Vector3.zero;
        Vector2 sizeDel = Vector2.zero;
        Vector3 scale = Vector3.one;
        if (go.GetComponent<RectTransform>() != null)
        {
            anchorPos = go.GetComponent<RectTransform>().anchoredPosition;
            sizeDel = go.GetComponent<RectTransform>().sizeDelta;
            scale = go.GetComponent<RectTransform>().localScale;
        }
        else
        {
            anchorPos = go.transform.localPosition;
            scale = go.transform.localScale;
        }
        UILayerMgr.Instance.SetDepthAndRoot(this);

        if (go.GetComponent<RectTransform>() != null)
        {
            go.GetComponent<RectTransform>().anchoredPosition = anchorPos;
            go.GetComponent<RectTransform>().sizeDelta = sizeDel;
            go.GetComponent<RectTransform>().localScale = scale;
        }
        else
        {
            go.transform.localPosition = anchorPos;
            go.transform.localScale = scale;
        }
    }

    public void SetData(object data)
    {
        this.mData = data;
    }

    public void SetPageDepth(int depth)
    {
        this.Depth = depth;
    }

    public int Depth { get; private set; }

    public bool ActiveSelf
    {
        get { return CacheGo != null && CacheGo.activeSelf; }
    }

    public bool CheckIfNeedBack()
    {
        if (UIPageProperty.WindowStyle == UIWindowStyle.PopUp || UIPageProperty.WindowStyle == UIWindowStyle.GameUI) return false;
        //else if (UIPageProperty.WindowMode == UIWindowMode.NoNeedBack || UIPageProperty.WindowMode == UIWindowMode.DoNothing) return false;
        return true;
    }

    public UIWindowStyle WindowStyle { get; protected set; }
    public UIWindowMode WindowMode { get; protected set; }
    public UIColliderType ColliderType { get; protected set; }
    public UIAnimationType AnimationType { get; protected set; }
    public string UIPath { get; protected set; }
    public UIProperty UIPageProperty { get; protected set; }
    protected object mData = null;
    protected bool isActived = false;
}
