using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// 拓展方法
/// </summary>
public static class ClassExtension
{
    public static bool IsNullOrEmpty<T>(this T[] list)
    {
        if (list != null && list.Length > 0)
            return false;
        return true;
    }

    public static bool IsNullOrEmpty<T>(this List<T> list)
    {
        if (list != null && list.Count > 0)
            return false;
        return true;
    }

    public static T Clone<T>(this T RealObject)
    {
        if (RealObject == null)
        {
            return default(T);
        }
        using (Stream objectStream = new MemoryStream())
        {
            //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, RealObject);
            objectStream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(objectStream);
        }
    }

    #region Vector相关的拓展

    /// <summary>
    /// 取 <see cref="UnityEngine.Vector3" /> 的 (x, z) 转换为 Vector2。
    /// </summary>
    /// <param name="vector3">要转换的 Vector3。</param>
    /// <returns>转换后的 Vector2。</returns>
    public static Vector2 ToVector2(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    /// <summary>
    /// 取 <see cref="UnityEngine.Vector2" /> 的 (x, y) 和给定参数 y 转换为 Vector3。
    /// </summary>
    /// <param name="vector2">要转换的 Vector2。</param>
    /// <returns>转换后的 Vector3。</returns>
    public static Vector3 ToVector3(this Vector2 vector2)
    {
        return new Vector3(vector2.x, 0f, vector2.y);
    }

    /// <summary>
    /// 取 <see cref="UnityEngine.Vector2" /> 的 (x, y) 和给定参数 y 转换为 Vector3。
    /// </summary>
    /// <param name="vector2">要转换的 Vector2。</param>
    /// <param name="y">Vector3 的 y 值。</param>
    /// <returns>转换后的 Vector3。</returns>
    public static Vector3 ToVector3(this Vector2 vector2, float y)
    {
        return new Vector3(vector2.x, y, vector2.y);
    }

    #endregion Vector相关的拓展

}
