// ******************************************
// 文件名(File Name):             DebugHelper.cs
// 创建时间(CreateTime):        20160314
// ******************************************

using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

public class DebugHelper
{
    public static LogLevel mLogLevel = LogLevel.Info;
    private static LogWriter mLogWriter = new LogWriter();
    private static string mInfoColor = "white";
    private static string mWarningColor = "orange";
    private static string mErrorColor = "red";

    static DebugHelper()
    {
        Application.logMessageReceived += OnLogCallBack;
    }

    //public static void LogInfo(object message, UnityEngine.Object sender = null)
    //{
    //    if (mLogLevel >= LogLevel.Info)
    //        LogInfoFormat(LogLevel.Info, message, sender);
    //}

    //public static void LogWarning(object message, UnityEngine.Object sender = null)
    //{
    //    if (mLogLevel >= LogLevel.Warning)
    //        LogWarningFormat(LogLevel.Warning, message, sender);
    //}

    //public static void LogError(object message, UnityEngine.Object sender = null)
    //{
    //    if (mLogLevel >= LogLevel.Error)
    //    {
    //        LogErrorFormat(LogLevel.Error, message, sender);
    //    }
    //}

    public static void LogException(Exception exption, UnityEngine.Object sender = null)
    {
        if (mLogLevel >= LogLevel.Exception)
        {
        }
    }

    private static void LogInfoFormat(LogLevel level, object message, UnityEngine.Object sender)
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
        System.Diagnostics.StackFrame stackFrame = stackTrace.GetFrame(2);
        string stackMessageFormat = Path.GetFileName(stackFrame.GetFileName()) + ":" + stackFrame.GetMethod().Name + "():at line " + stackFrame.GetFileLineNumber();
        StringBuilder msg = new StringBuilder();
        msg.AppendFormat("<color={0}>[{1}] [Msg: {2} ] [{3}]</color>", mInfoColor, level.ToString().ToUpper(), message, stackMessageFormat);
        Debug.Log(msg);
    }

    private static void LogErrorFormat(LogLevel level, object message, UnityEngine.Object sender)
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
        System.Diagnostics.StackFrame stackFrame = stackTrace.GetFrame(2);
        string stackMessageFormat = Path.GetFileName(stackFrame.GetFileName()) + ":" + stackFrame.GetMethod().Name + "():at line " + stackFrame.GetFileLineNumber();
        StringBuilder msg = new StringBuilder();
        msg.AppendFormat("<color={0}>[{1}] [Msg: {2} ] [{3}]</color>", mErrorColor, level.ToString().ToUpper(), message, stackMessageFormat);
        Debug.LogError(msg);
    }

    private static void LogWarningFormat(LogLevel level, object message, UnityEngine.Object sender)
    {
        System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
        System.Diagnostics.StackFrame stackFrame = stackTrace.GetFrame(2);
        string stackMessageFormat = Path.GetFileName(stackFrame.GetFileName()) + ":" + stackFrame.GetMethod().Name + "():at line " + stackFrame.GetFileLineNumber();
        StringBuilder msg = new StringBuilder();
        msg.AppendFormat("<color={0}>[{1}] [Msg: {2} ] [{3}]</color>", mWarningColor, level.ToString().ToUpper(), message, stackMessageFormat);
        Debug.LogWarning(msg);
    }

    private static void OnLogCallBack(string msg, string stackTrace, LogType type)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"));
        switch (type)
        {
            case LogType.Log:
                sb.AppendFormat(" {0}：{1}\n", type, msg);
                break;
            case LogType.Error:
            case LogType.Assert:
            case LogType.Warning:
            case LogType.Exception:
                if (string.IsNullOrEmpty(stackTrace))
                {
                    sb.AppendFormat(" {0}：{1}\n{2}", type, msg, Environment.StackTrace);
                    /// 发布到对应平台后，调用堆栈获取不到。使用 Environment.StackTrace 获取调用堆栈
                }
                else
                {
                    sb.AppendFormat(" {0}：{ 1}\n{2}", type, msg, stackTrace);
                }
                break;
            default:
                break;
        }
        WriteLog(sb.ToString());
    }

    /// <summary>
    /// 加上时间戳
    /// </summary>
    /// <param name="message"></param>
    private static void WriteLog(string message)
    {
        mLogWriter.ExcuteWrite(message);
    }

    public delegate void Func(object obj);

#if UNITY_EDITOR
    public static Func LogInfo = UnityEngine.Debug.Log;
    public static Func LogWarning = UnityEngine.Debug.LogWarning;
    public static Func LogError = UnityEngine.Debug.LogError;
#else

    public static void LogInfo(object obj)
    {
    }

    public static void LogWarning(object obj)
    {
    }

    public static void LogError(object obj)
    {
    }

#endif

    public enum LogLevel : byte
    {
        None = 0,
        Exception = 1,
        Error = 2,
        Warning = 3,
        Info = 4,
    }
}

public class LogWriter
{
    private string m_logPath = Application.persistentDataPath + "/log/";
    private string m_logFileName = "log_{0}.txt";
    private string m_logFilePath = string.Empty;

    public LogWriter()
    {
        if (!Directory.Exists(m_logPath))
        {
            Directory.CreateDirectory(m_logPath);
        }
        this.m_logFilePath = this.m_logPath + string.Format(this.m_logFileName, DateTime.Today.ToString("yyyyMMdd"));
    }

    public void ExcuteWrite(string content)
    {
        using (StreamWriter writer = new StreamWriter(m_logFilePath, true, Encoding.UTF8))
        {
            writer.WriteLine(content);
        }
    }
}
