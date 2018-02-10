using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowMgr : MonoSingleton<UIWindowMgr>
{
    [SerializeField]
    private Dictionary<string, UIBasePanel> mOpenedUIList;

    [SerializeField]
    private Stack<UIBasePanel> mNavigationStack;

	public UIBasePanel mCurrPage;

    public void ClearNavigationStack()
    {
        if (mNavigationStack != null && mNavigationStack.Count > 0)
        {
            while (mNavigationStack.Count > 0)
            {
                UIBasePanel top = mNavigationStack.Pop();
                top.OnExit();
            }
        }
        mNavigationStack.Clear();
        Resources.UnloadUnusedAssets();
    }

    public void PrePushPanel<T>() where T : UIBasePanel, new()
    {
        Type t = typeof(T);
        string pageName = t.ToString();

        if (mOpenedUIList != null && mOpenedUIList.ContainsKey(pageName))
        {
            PrePushPanel(pageName, mOpenedUIList[pageName], null, null, false);
        }
        else
        {
            T instance = new T();
            PrePushPanel(pageName, instance, null, null, false);
        }
    }

    private void PrePushPanel(string pageName, UIBasePanel pageInstance, Action callback, object pageData, bool isAsync)
    {
        if (string.IsNullOrEmpty(pageName) || pageInstance == null)
        {
            Debug.LogError("[UI] show page error with :" + pageName + " maybe null instance.");
            return;
        }
        if (mOpenedUIList.ContainsKey(pageName))
        {
            mCurrPage = mOpenedUIList[pageName];
        }
        else
        {
            mOpenedUIList.Add(pageName, pageInstance);
            mCurrPage = pageInstance;
        }
        mCurrPage.OnPreloading();
    }

    public void PushPanel<T>() where T : UIBasePanel, new()
    {
        PushPanel<T>(null, null, false);
    }

    public void PushPanel<T>(object pageData) where T : UIBasePanel, new()
    {
        PushPanel<T>(null, pageData, false);
    }

    public void PushPanel<T>(Action callback) where T : UIBasePanel, new()
    {
        PushPanel<T>(callback, null, true);
    }

    public void PushPanel<T>(Action callback, object pageData) where T : UIBasePanel, new()
    {
        PushPanel<T>(callback, pageData, true);
    }

    private void PushPanel<T>(Action callback, object pageData, bool isAsync) where T : UIBasePanel, new()
    {
        Type t = typeof(T);
        string pageName = t.ToString();

        if (mOpenedUIList != null && mOpenedUIList.ContainsKey(pageName))
        {
            PushPanel(pageName, mOpenedUIList[pageName], callback, pageData, isAsync);
        }
        else
        {
            T instance = new T();
            PushPanel(pageName, instance, callback, pageData, isAsync);
        }
    }

    private void PushPanel(string pageName, UIBasePanel pageInstance, Action callback, object pageData, bool isAsync)
    {
        if (string.IsNullOrEmpty(pageName) || pageInstance == null)
        {
            Debug.LogError("[UI] show page error with :" + pageName + " maybe null instance.");
            return;
        }

        if (mOpenedUIList.ContainsKey(pageName))
        {
            mCurrPage = mOpenedUIList[pageName];
        }
        else
        {
            mOpenedUIList.Add(pageName, pageInstance);
            mCurrPage = pageInstance;
        }
        if (pageData != null)
        {
            mCurrPage.SetData(pageData);
        }

        mCurrPage.OnEnter();
        SetNavigationStack(mCurrPage);

        //if (isAsync)
        //    curPanel.Show(callback);
        //else

        //DebugHelper.LogInfo(mNavigationStack.Count);
    }

    private bool CheckIfNeedBack(UIBasePanel page)
    {
        return page != null && page.CheckIfNeedBack();
    }

    private void SetNavigationStack(UIBasePanel mUIBasePanel)
    {
        if (mUIBasePanel == null)
        {
            Debug.LogError("[UI] page popup is null.");
            return;
        }
        if (!CheckIfNeedBack(mUIBasePanel))
        {
            return;
        }
        //Debug.Log(mUIBasePanel.CacheGo.name);
        if (mNavigationStack.Count < 0) { return; }
        if (mNavigationStack.Count >= 1 && mNavigationStack.Peek().UIPageProperty.UIPath == mUIBasePanel.UIPageProperty.UIPath)
        {
            return;
        }
        if (mUIBasePanel.UIPageProperty.WindowMode == UIWindowMode.HideOther)
        {
            mNavigationStack.Peek().OnExit();
        }
        mNavigationStack.Push(mUIBasePanel);

    }

    #region 关闭窗口

    public void PopPanel()
    {
        if (mNavigationStack.Count <= 0) { return; }
        mNavigationStack.Pop().OnExit();
        mNavigationStack.Peek().OnEnter();
    }

    public void PopPanel<T>() where T : UIBasePanel, new()
    {
        string pageName = typeof(T).ToString();
        UIBasePanel target;
        if (mOpenedUIList.TryGetValue(pageName, out target))
        {
            PopPanel(target);
        }
    }

    public void PopPanel(UIBasePanel target)
    {
        if (target == null && !target.ActiveSelf) return;
        if (mNavigationStack != null && mNavigationStack.Count > 0)
        {
            if (!CheckIfNeedBack(target))
            {
                target.OnExit();
            }
            else
            {
                mNavigationStack.Pop().OnExit();
            }
        }
        else
        {
            target.OnExit();
        }
    }

    #endregion 关闭窗口

    #region 重写 OnInit OnUnInit方法

    protected override void OnInit()
    {
        if (mOpenedUIList == null)
        {
            mOpenedUIList = new Dictionary<string, UIBasePanel>();
        }
        if (mNavigationStack == null)
        {
            mNavigationStack = new Stack<UIBasePanel>();
        }
        base.OnInit();
    }

    protected override void OnUnInit()
    {
        if (mOpenedUIList != null)
        {
            mOpenedUIList.Clear();
            mOpenedUIList = null;
        }
        if (mNavigationStack != null)
        {
            mNavigationStack.Clear();
            mNavigationStack = null;
        }
        base.OnUnInit();
    }

    #endregion 重写 OnInit OnUnInit方法
}
