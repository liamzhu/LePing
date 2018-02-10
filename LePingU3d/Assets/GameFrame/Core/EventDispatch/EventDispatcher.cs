using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher
{
    private Dictionary<string, Delegate> mEventMap = new Dictionary<string, Delegate>();

    public Dictionary<string, Delegate> EventMap
    {
        get
        {
            if (mEventMap == null)
                mEventMap = new Dictionary<string, Delegate>();
            return this.mEventMap;
        }
        private set
        {
        }
    }

    public void CleanUp()
    {
        mEventMap.Clear();
    }

    #region 添加观察者的方法

    public void AddObserver(string eventType, Action listener)
    {
        Delegate evts;
        if (EventMap.TryGetValue(eventType, out evts))
        {
            Action act = evts as Action;
            if (act != null)
            {
                act += listener;
                mEventMap[eventType] = act;
            }
            else
                throw new Exception("EventDispatch evt type is null");
        }
        else
            EventMap.Add(eventType, listener);
    }

    public void AddObserver<T>(string eventType, Action<T> listener)
    {
        Delegate evts;
        if (EventMap.TryGetValue(eventType, out evts))
        {
            Action<T> act = evts as Action<T>;
            if (act != null)
            {
                act += listener;
                mEventMap[eventType] = act;
            }
            else
                throw new Exception("EventDispatch evt type is null");
        }
        else
            EventMap.Add(eventType, listener);
    }

    public void AddObserver<T, U>(string eventType, Action<T, U> listener)
    {
        Delegate evts;
        if (EventMap.TryGetValue(eventType, out evts))
        {
            Action<T, U> act = evts as Action<T, U>;
            if (act != null)
            {
                act += listener;
                mEventMap[eventType] = act;
            }
            else
                throw new Exception("EventDispatch evt type is null");
        }
        else
            EventMap.Add(eventType, listener);
    }

    public void AddObserver<T, U, V>(string eventType, Action<T, U, V> listener)
    {
        Delegate evts;
        if (EventMap.TryGetValue(eventType, out evts))
        {
            Action<T, U, V> act = evts as Action<T, U, V>;
            if (act != null)
            {
                act += listener;
                mEventMap[eventType] = act;
            }
            else
                throw new Exception("EventDispatch evt type is null");
        }
        else
            EventMap.Add(eventType, listener);
    }

    public void AddObserver<T, U, V, W>(string eventType, Action<T, U, V, W> listener)
    {
        Delegate evts;
        if (EventMap.TryGetValue(eventType, out evts))
        {
            Action<T, U, V, W> act = evts as Action<T, U, V, W>;
            if (act != null)
            {
                act += listener;
                mEventMap[eventType] = act;
            }
            else
                throw new Exception("EventDispatch evt type is null");
        }
        else
            EventMap.Add(eventType, listener);
    }

    #endregion 添加观察者的方法

    #region 移除事件

    public void RemoveObserver(string eventType, Action listener)
    {
        if ((mEventMap == null) || (listener == null))
            return;

        Delegate evts;
        if (mEventMap.TryGetValue(eventType, out evts))
        {
            Action act = evts as Action;
            if (act != null)
            {
                act -= listener;
                if (act == null)
                    mEventMap.Remove(eventType);
                else
                    mEventMap[eventType] = act;
            }
            else
                throw new Exception("EventDispatch evt type is null");
        }
    }

    public void RemoveObserver<T>(string eventType, Action<T> listener)
    {
        if ((mEventMap == null) || (listener == null))
            return;

        Delegate evts;
        if (mEventMap.TryGetValue(eventType, out evts))
        {
            Action<T> act = evts as Action<T>;
            if (act != null)
            {
                act -= listener;
                if (act == null)
                    mEventMap.Remove(eventType);
                else
                    mEventMap[eventType] = act;
            }
            else
                throw new Exception("EventDispatch evt type is null");
        }
    }

    public void RemoveObserver<T, U>(string eventType, Action<T, U> listener)
    {
        if ((mEventMap == null) || (listener == null))
            return;

        Delegate evts;
        if (mEventMap.TryGetValue(eventType, out evts))
        {
            Action<T, U> act = evts as Action<T, U>;
            if (act != null)
            {
                act -= listener;
                if (act == null)
                    mEventMap.Remove(eventType);
                else
                    mEventMap[eventType] = act;
            }
            else
                throw new Exception("EventDispatch evt type is null");
        }
    }

    public void RemoveObserver<T, U, V>(string eventType, Action<T, U, V> listener)
    {
        if ((mEventMap == null) || (listener == null))
            return;

        Delegate evts;
        if (mEventMap.TryGetValue(eventType, out evts))
        {
            Action<T, U, V> act = evts as Action<T, U, V>;
            if (act != null)
            {
                act -= listener;
                if (act == null)
                    mEventMap.Remove(eventType);
                else
                    mEventMap[eventType] = act;
            }
            else
                throw new Exception("EventDispatch evt type is null");
        }
    }

    public void RemoveObserver<T, U, V, W>(string eventType, Action<T, U, V, W> listener)
    {
        if ((mEventMap == null) || (listener == null))
            return;

        Delegate evts;
        if (mEventMap.TryGetValue(eventType, out evts))
        {
            Action<T, U, V, W> act = evts as Action<T, U, V, W>;
            if (act != null)
            {
                act -= listener;
                if (act == null)
                    mEventMap.Remove(eventType);
                else
                    mEventMap[eventType] = act;
            }
            else
                throw new Exception("EventDispatch evt type is null");
        }
    }

    #endregion 移除事件

    #region 触发事件

    public void Dispatch(string eventType)
    {
        if (mEventMap == null)
            return;

        Delegate evts;
        if (mEventMap.TryGetValue(eventType, out evts))
        {
            Delegate[] list = evts.GetInvocationList();
            for (int i = 0; i < list.Length; ++i)
            {
                Action act = list[i] as Action;
                if (act != null)
                    act();
            }
        }
    }

    public void Dispatch<T>(string eventType, T params01)
    {
        if (mEventMap == null)
            return;

        Delegate evts;
        if (mEventMap.TryGetValue(eventType, out evts))
        {
            Delegate[] list = evts.GetInvocationList();
            for (int i = 0; i < list.Length; ++i)
            {
                Action<T> act = list[i] as Action<T>;
                if (act != null)
                    act(params01);
            }
        }
    }

    public void Dispatch<T, U>(string eventType, T params01, U params02)
    {
        if (mEventMap == null)
            return;

        Delegate evts;
        if (mEventMap.TryGetValue(eventType, out evts))
        {
            Delegate[] list = evts.GetInvocationList();
            for (int i = 0; i < list.Length; ++i)
            {
                Action<T, U> act = list[i] as Action<T, U>;
                if (act != null)
                    act(params01, params02);
            }
        }
    }

    public void Dispatch<T, U, V>(string eventType, T params01, U params02, V params03)
    {
        if (mEventMap == null)
            return;

        Delegate evts;
        if (mEventMap.TryGetValue(eventType, out evts))
        {
            Delegate[] list = evts.GetInvocationList();
            for (int i = 0; i < list.Length; ++i)
            {
                Action<T, U, V> act = list[i] as Action<T, U, V>;
                if (act != null)
                    act(params01, params02, params03);
            }
        }
    }

    public void Dispatch<T, U, V, W>(string eventType, T params01, U params02, V params03, W params04)
    {
        if (mEventMap == null)
            return;

        Delegate evts;
        if (mEventMap.TryGetValue(eventType, out evts))
        {
            Delegate[] list = evts.GetInvocationList();
            for (int i = 0; i < list.Length; ++i)
            {
                Action<T, U, V, W> act = list[i] as Action<T, U, V, W>;
                if (act != null)
                    act(params01, params02, params03, params04);
            }
        }
    }

    #endregion 触发事件
}
