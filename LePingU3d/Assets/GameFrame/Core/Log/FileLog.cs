using System;
using System.IO;
using UnityEngine;

public class FileLog
{
    public FileLog()
        : this(DateTime.Now.ToString("yyyy-MM-dd"))
    {
    }

    public FileLog(string fileName)
    {
        this.m_fileName = fileName;
        try
        {
            if (false == Directory.Exists(logRootPath))
            {
                Directory.CreateDirectory(logRootPath);
            }
            m_fileName = m_fileName.Substring(m_fileName.LastIndexOf('\\') + 1);
            m_fileName = m_fileName.Substring(m_fileName.LastIndexOf("/") + 1);
            FullFileName = logRootPath + m_fileName + ".txt";

            m_Writer = WriteLog;

            fs = new FileStream(FullFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            sw = new StreamWriter(fs);

        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }
    }

    private void WriteLog(string msg, LogLevel level)
    {
        lock (m_lock)
        {
            try
            {
                if (null != sw)
                {
                    sw.WriteLine(msg);
                    sw.Flush();
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }
    }

    public void Close()
    {
        lock (m_lock)
        {
            if (sw != null)
            {
                sw.Close();
                sw.Dispose();
                sw = null;
            }
            if (fs != null)
            {
                fs.Close();
                fs.Dispose();
                fs = null;
            }
        }
    }

    private static readonly object m_lock = new object();
    public static readonly string logRootPath = Application.persistentDataPath + "/AkLog/";
    private string m_fileName;

    public string FullFileName
    {
        get;
        set;
    }

    private FileStream fs;
    private StreamWriter sw;
    private Action<string, LogLevel> m_Writer;

    public Action<string, LogLevel> Writer
    {
        get { return m_Writer; }
    }

}
