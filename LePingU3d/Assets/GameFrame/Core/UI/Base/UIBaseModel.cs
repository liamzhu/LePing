using System;
using System.Collections;
using UnityEngine;

public class UIBaseModel
{
    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="key"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public void DispatchValueUpdateEvent(string key, object oldValue, object newValue)
    {
        EventHandler<ValueChangeArgs> handler = ValueUpdateEvent;
        if (handler != null)
        {
            handler(this, new ValueChangeArgs(key, oldValue, newValue));
        }
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="args"></param>
    public void DispatchValueUpdateEvent(ValueChangeArgs args)
    {
        EventHandler<ValueChangeArgs> handler = ValueUpdateEvent;
        if (handler != null)
        {
            handler(this, args);
        }
    }

    public event EventHandler<ValueChangeArgs> ValueUpdateEvent;
}

public class ValueChangeArgs : EventArgs
{
    public string key { set; get; }
    public object oldValue { set; get; }
    public object newValue { set; get; }

    public ValueChangeArgs(string key, object oldValue, object newValue)
    {
        this.key = key;
        this.oldValue = oldValue;
        this.newValue = newValue;
    }

    public ValueChangeArgs(string key)
    {
        this.key = key;
    }

    public ValueChangeArgs()
    {
    }
}
