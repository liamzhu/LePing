using System.Collections;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public static class FileHelper
{

    #region 文件操作的相关方法

    /// <summary>
    /// 写入数据到对应文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="contents"></param>
    public static void Write(string fileName, string contents, FilePathType type = FilePathType.dataPath)
    {
        string folderPath = GetFullFilePath(fileName, type);
        CreateFolder(folderPath.Substring(0, folderPath.LastIndexOf('\\')));
        using (StreamWriter sw = new StreamWriter(GetFullFilePath(fileName, type)))
        {
            sw.Write(contents);
            sw.Close();
        }
        Refresh();
    }

    /// <summary>
    /// 从对应文件读取数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string Read(string fileName, FilePathType type = FilePathType.dataPath)
    {
        if (IsFileExists(fileName, type))
        {
            using (StreamReader streamReader = new StreamReader(GetFullFilePath(fileName, type)))
            {
                string data = streamReader.ReadToEnd();
                streamReader.Close();
                return data;
            }
            //return File.ReadAllText(GetFullFilePath(fileName, type));
        }
        else {
            return string.Empty;
        }
    }

    public static void CreateFile(string fileName, FilePathType type = FilePathType.dataPath)
    {
        if (!IsFileExists(fileName, type))
        {
            string folderPath = GetFullFilePath(fileName, type);
            CreateFolder(folderPath.Substring(0, folderPath.LastIndexOf('/')));
#if UNITY_4 || UNITY_5
            using (FileStream stream = File.Create(GetFullFilePath(fileName, type)))
            {
                stream.Close();
            }
#else
            File.Create(GetFullFilePath(fileName, type));
#endif
        }
    }

    /// <summary>
    /// 复制文件
    /// </summary>
    /// <param name="srcFileName"></param>
    /// <param name="destFileName"></param>
    public static void CopyFile(string srcFileName, string destFileName, FilePathType type = FilePathType.dataPath)
    {
        if (IsFileExists(srcFileName, type) && !srcFileName.Equals(destFileName))
        {
            string folderPath = GetFullFilePath(destFileName, type);
            CreateFolder(folderPath.Substring(0, folderPath.LastIndexOf('/')));
            File.Copy(GetFullFilePath(srcFileName, type), GetFullFilePath(destFileName, type), true);
            Refresh();
        }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="fileName"></param>
    public static void DeleteFile(string fileName, FilePathType type = FilePathType.dataPath)
    {
        if (IsFileExists(fileName, type))
        {
            File.Delete(GetFullFilePath(fileName, type));
            Refresh();
        }
    }

    /// <summary>
    /// 文件是否存在
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsFileExists(string fileName, FilePathType type = FilePathType.dataPath)
    {
        return File.Exists(GetFullFilePath(fileName, type));
    }

    #endregion 文件操作的相关方法

    #region 文件夹操作相关方法

    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="type"></param>
    public static void CreateFolder(string folderPath, FilePathType type = FilePathType.dataPath)
    {
        if (!IsFolderExists(GetFullFilePath(folderPath, type)))
        {
            Directory.CreateDirectory(GetFullFilePath(folderPath, type));
            Refresh();
        }
    }

    /// <summary>
    /// 删除文件夹
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="type"></param>
    public static void DeleteFolder(string folderPath, FilePathType type = FilePathType.dataPath)
    {
        if (IsFolderExists(GetFullFilePath(folderPath, type)))
        {
            Directory.Delete(GetFullFilePath(folderPath, type), true);
            Refresh();
        }
    }

    /// <summary>
    /// 复制文件夹
    /// </summary>
    /// <param name="srcFolderPath"></param>
    /// <param name="destFolderPath"></param>
    /// <param name="type"></param>
    public static void CopyFolder(string srcFolderPath, string destFolderPath, FilePathType type = FilePathType.dataPath)
    {
        if (!IsFolderExists(srcFolderPath, type))
        {
            DebugHelper.LogInfo(string.Format("文件夹:{0} 不存在", srcFolderPath));
            return;
        }
        CreateFolder(destFolderPath, type);
        srcFolderPath = GetFullFilePath(srcFolderPath, type);
        destFolderPath = GetFullFilePath(destFolderPath, type);

        // 创建所有的对应目录
        foreach (string dirPath in Directory.GetDirectories(srcFolderPath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(srcFolderPath, destFolderPath));
        }
        // 复制原文件夹下所有内容到目标文件夹，直接覆盖
        foreach (string newPath in Directory.GetFiles(srcFolderPath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(srcFolderPath, destFolderPath), true);
        }
        Refresh();
    }

    /// <summary>
    /// 检测文件夹是否存在
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool IsFolderExists(string folderPath, FilePathType type = FilePathType.dataPath)
    {
        return Directory.Exists(GetFullFilePath(folderPath, type));
    }

    #endregion 文件夹操作相关方法

    /// <summary>
    /// 文件全目录
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetFullFilePath(string folderPath, FilePathType type = FilePathType.dataPath)
    {
        if (type == FilePathType.dataPath)
        {
            return Path.Combine(Application.dataPath, folderPath);
        }
        else if (type == FilePathType.persistentDataPath)
        {
            return Path.Combine(Application.persistentDataPath, folderPath);
        }
        else if (type == FilePathType.streamingAssetsPath)
        {
            return Path.Combine(mStreamingAssetsPath, folderPath);
        }
        else
        {
            return Path.Combine(Application.temporaryCachePath, folderPath);
        }
    }

    private static void Refresh()
    {
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public static readonly string mStreamingAssetsPath =
#if UNITY_ANDROID
"jar:file://" + Application.dataPath + "!/assets/";

#elif UNITY_IPHONE
Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
"file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif

    public static string StreamingFilePath()
    {
        string path = string.Empty;

        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer ||
          Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            path = Application.dataPath + "/StreamingAssets/";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            path = Application.dataPath + "/Raw/";
        else if (Application.platform == RuntimePlatform.Android)
            path = "jar:file://" + Application.dataPath + "!/assets/";
        else
            path = Application.dataPath + "/config/";

        return path;
    }

    public static string PersistentFilePath()
    {
        string filepath = string.Empty;
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer ||
          Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            filepath = Application.dataPath + "/StreamingAssets/";
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            filepath = Application.persistentDataPath + "/";
        else
        {
            filepath = Application.persistentDataPath + "/";
        }
#if UNITY_IPHONE
    UnityEngine.iOS.Device.SetNoBackupFlag(filepath);
#endif
        return filepath;
    }

    public static readonly string mRuntimePlatform =
#if UNITY_ANDROID
"Android";

#elif UNITY_IPHONE
"IOS";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
"Windows";
#else
        string.Empty;
#endif

    public enum FilePathType
    {
        persistentDataPath,
        dataPath,
        streamingAssetsPath,
        temporaryCachePath
    }
}
