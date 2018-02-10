using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register
{
    public uint resId;
    public List<INotifier> notifiers = new List<INotifier>();
}

public class GameNotifierMgr : Singleton<GameNotifierMgr>
{
    protected Dictionary<uint, Register> registers;

    public override void OnInit()
    {
        if (registers == null)
        {
            registers = new Dictionary<uint, Register>();
        }
        base.OnInit();
    }

    public override void OnUnInit()
    {
        if (registers != null)
        {
            registers.Clear();
            registers = null;
        }
        base.OnUnInit();
    }

    /// <summary>
    /// 通知该协议已注册的收听器
    /// </summary>
    /// <param name="resId"></param>
    /// <param name="req"></param>
    /// <param name="res"></param>
    public void Notifier(uint resId, object param1, object param2)
    {
        Register register;
        if (!registers.TryGetValue(resId, out register))
        {
            DebugHelper.LogInfo(string.Format("收听器: {0} 不存在", resId));
            return;
        }
        INotifier[] notifiers = registers[resId].notifiers.ToArray();//notifier.OnReceiveData中有可能改变notifiers的长度。所以用notifiers的副本
        for (int i = 0; i < notifiers.Length; i++)
        {
            notifiers[i].OnReceiveData(resId, param1, param2);
        }
    }

    /// <summary>
    /// 注册收听器
    /// </summary>
    /// <param name="resId"></param>
    /// <param name="notifier"></param>
    public void RegisterNotifier(uint resId, INotifier notifier)
    {
        DebugHelper.LogInfo(string.Format("注册收听器: {0}", resId));
        Register register;
        if (!registers.TryGetValue(resId, out register))
        {
            register = new Register();
            register.resId = resId;
            registers.Add(resId, register);
        }
        register.notifiers.Add(notifier);
    }

    /// <summary>
    /// 取消注册收听器
    /// </summary>
    /// <param name="resId"></param>
    /// <param name="notifier"></param>
    public void UnregisterNotifier(uint resId, INotifier notifier)
    {
        DebugHelper.LogInfo(string.Format("取消注册收听器: {0}", resId));
        Register register;
        if (!registers.TryGetValue(resId, out register))
        {
            DebugHelper.LogError(string.Format("试图对没有注册过的收听器取消注册：", resId));
            return;
        }
        if (register.notifiers.Contains(notifier))
        {
            register.notifiers.Remove(notifier);
            if (register.notifiers.Count == 0)
            {
                registers.Remove(resId);
            }
        }
    }

    public void CleanUp()
    {
        OnUnInit();
    }
}
