using System;
using UnityEngine;

[Flags]
public enum LogLevel
{
    NONE = 0x0,
    Error = 0x10,
    Assert = 0x100,
    Warning = 0x1000,
    Log = 0x10000,
    Debug = 0x100000,
    Exception = 0x1000000,
    ALL = Error | Assert | Warning | Log | Debug | Exception,
}

public static class DebugMgr
{
    static DebugMgr()
    {
        Level = LogLevel.NONE;
        FileLogWriter = new FileLog();
        EnableConsoleLog = false;
        Application.logMessageReceivedThreaded += AppLogCallback;
    }

    public static void AppLogCallback(string condition, string stackTrace, LogType type)
    {

        LogLevel lv = LogLevel.NONE;
        switch (type)
        {
            case LogType.Assert:
                lv = LogLevel.Assert;
                break;
            case LogType.Error:
                lv = LogLevel.Error;
                break;
            case LogType.Exception:
                lv = LogLevel.Exception;
                break;
            case LogType.Log:
                lv = LogLevel.Log;
                break;
            case LogType.Warning:
                lv = LogLevel.Warning;
                break;
        }

        LogFormat(lv, "APP LOG -- condition: {0} , stackTrace {1}", condition, stackTrace);
    }

    public static void Release()
    {
        if (null != m_FileLog)
        {
            DebugMgr.Writer -= m_FileLog.Writer;
            m_FileLog.Close();
            m_FileLog = null;
        }
    }

    private static FileLog m_FileLog;

    public static FileLog FileLogWriter
    {
        set
        {
            Release();
            m_FileLog = value;
            DebugMgr.Writer += m_FileLog.Writer;
        }
        get
        {
            return m_FileLog;
        }

    }

    private static bool m_ConsoleLogEnable;

    public static bool EnableConsoleLog
    {
        set
        {
            m_ConsoleLogEnable = value;
            if (m_ConsoleLogEnable)
                DebugMgr.Writer += ConsoleLogWriter;
            else
                DebugMgr.Writer -= ConsoleLogWriter;
        }
        get
        {
            return m_ConsoleLogEnable;
        }
    }

    private static void ConsoleLogWriter(string str, LogLevel lv)
    {
        var c = getLogLevelColor(lv);
        UnityEngine.Debug.Log("<color=" + c + ">" + str + "</color>");
    }

    public static LogLevel Level
    {
        set;
        get;
    }

    public static Action<string, LogLevel> Writer
    {
        set;
        get;
    }

    public static void Error(object message)
    {
        Log(LogLevel.Error, message);
    }

    public static void Assert(object message)
    {
        Log(LogLevel.Assert, message);
    }

    public static void Warning(object message)
    {
        Log(LogLevel.Warning, message);
    }

    public static void Log(object message)
    {
        Log(LogLevel.Log, message);
    }

    public static void Debug(object message)
    {
        Log(LogLevel.Debug, message);
    }

    public static void LogFormat(LogLevel level, string format, params object[] args)
    {
        string message = string.Format(format, args);
        Log(level, message);
    }

    public static void Log(LogLevel level, object message)
    {
        if (LogLevel.NONE != (level & Level))
        {
            string logTime = getLogTimeStr();
            string strLevel = getLogLevelStr(level);
            string str = logTime + strLevel + " : " + message;
            _Log(str, level);
        }
    }

    private static void _Log(string msg, LogLevel level)
    {
        if (Writer != null)
        {
            var calls = Writer.GetInvocationList();

            foreach (Action<string, LogLevel> call in calls)
            {
                try
                {
                    call(msg, level);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }

            }

        }
    }

    private static string getLogTimeStr()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    private static string getLogLevelStr(LogLevel level)
    {
        string prefix = "UNKNOW";
        switch (level)
        {
            case LogLevel.Error:
                prefix = "Error";
                break;
            case LogLevel.Assert:
                prefix = "Assert";
                break;
            case LogLevel.Warning:
                prefix = "Warning";
                break;
            case LogLevel.Log:
                prefix = "Log";
                break;
            case LogLevel.Debug:
                prefix = "Debug";
                break;
            case LogLevel.Exception:
                prefix = "Exception";
                break;
            default:
                break;
        }
        return " [ " + prefix + " ] ";
    }

    private static string getLogLevelColor(LogLevel level)
    {
        string prefix = "#ffffff";
        switch (level)
        {
            case LogLevel.Error:
                prefix = "#ff0000";
                break;
            case LogLevel.Assert:
                prefix = "#ff0000";
                break;
            case LogLevel.Warning:
                prefix = "#ffff00";
                break;
            case LogLevel.Log:
                prefix = "#0000ff";
                break;
            case LogLevel.Debug:
                prefix = "#888888";
                break;
            case LogLevel.Exception:
                prefix = "#ff0000";
                break;
            default:
                break;
        }
        return prefix;
    }

    private static string getStackInfo()
    {
        System.Diagnostics.Debug.WriteLine(new string('*', 88));
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
        System.Diagnostics.StackFrame sf = null;
        int max = 5;
        int frame = 2;
        string stackInfo = "";
        while (max > 0)
        {
            max--;
            sf = st.GetFrame(frame);
            if (null != sf)
            {
                var method = sf.GetMethod();
                stackInfo += String.Format("[Call Stack {0}  ]{1}:{2}() in file<{3}>:line{4}\n", frame, method.DeclaringType.FullName, method.Name, sf.GetFileName(), sf.GetFileLineNumber());

            }
            frame++;
        }
        return stackInfo;
    }

    public static void Exception(Exception e, object msg = null)
    {
        if (LogLevel.Exception == (LogLevel.Exception & Level))
        {
            Exception inner = e;
            while (inner.InnerException != null)
            {
                inner = inner.InnerException;
            }
            string info = string.Concat(msg != null ? msg : "", " msg: ", e.Message, " ", inner.StackTrace, "\nSTACKINFO: ", getStackInfo());
            Log(LogLevel.Exception, info);
        }
    }

}
