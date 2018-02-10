using UnityEngine;
using System.Collections;

public static class TransformExtension
{

    #region Transform相关的拓展

    public static T FindComponent<T>(this Transform trans, string name)
    {
        Transform temp = trans.Find(name);
        if (temp != null)
        {
            return temp.GetComponent<T>();
        }
        else
        {
            return default(T);
        }
    }

    public static T[] FindChindComponents<T>(this Transform trans, string name)
    {
        Transform temp = trans.Find(name);
        if (temp != null)
        {
            return temp.GetComponentsInChildren<T>();
        }
        else
        {
            return null;
        }
    }

    public static UIEventListener GetUIEventListener<T>(this Transform trans, string name) where T : Component
    {
        return UIEventListener.Get(trans.FindComponent<T>(name).gameObject);
    }

    public static UIEventListener GetUIEventListener(this Transform trans, string name)
    {
        return UIEventListener.Get(trans.FindComponent<UIButton>(name).gameObject);
    }

    public static void SetParent(this Transform go, Transform parent)
    {
        go.parent = parent;
    }

    public static void ResetTransform(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static void SetPosX(Transform trans, float x)
    {
        var previousPos = trans.localPosition;
        previousPos.x = x;
        trans.position = previousPos;
    }

    public static void SetPosY(Transform trans, float y)
    {
        var previousPos = trans.localPosition;
        previousPos.y = y;
        trans.position = previousPos;
    }

    public static void SetPosXY(this Transform trans, float x, float y)
    {
        var previousPos = trans.localPosition;
        previousPos.x = x;
        previousPos.y = y;
        trans.position = previousPos;
    }

    public static void SetPosZ(Transform trans, float z)
    {
        var previousPos = trans.localPosition;
        previousPos.z = z;
        trans.position = previousPos;
    }

    public static void SetLocalPosX(Transform trans, float x)
    {
        var previousPos = trans.localPosition;
        previousPos.x = x;
        trans.localPosition = previousPos;
    }

    public static void SetLocalPosY(Transform trans, float y)
    {
        var previousPos = trans.localPosition;
        previousPos.y = y;
        trans.localPosition = previousPos;
    }

    public static void SetLocalPosXY(this Transform trans, float x, float y)
    {
        var previousPos = trans.localPosition;
        previousPos.x = x;
        previousPos.y = y;
        trans.localPosition = previousPos;
    }

    public static void SetLocalPosZ(Transform trans, float z)
    {
        var previousPos = trans.localPosition;
        previousPos.z = z;
        trans.localPosition = previousPos;
    }

    /// <summary>
    /// 设置相对尺寸的 x 分量。
    /// </summary>
    /// <param name="t"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">x 分量值。</param>
    public static void SetLocalScaleX(this Transform t, float newValue)
    {
        Vector3 v = t.localScale;
        v.x = newValue;
        t.localScale = v;
    }

    /// <summary>
    /// 设置相对尺寸的 y 分量。
    /// </summary>
    /// <param name="t"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">y 分量值。</param>
    public static void SetLocalScaleY(this Transform t, float newValue)
    {
        Vector3 v = t.localScale;
        v.y = newValue;
        t.localScale = v;
    }

    /// <summary>
    /// 设置相对尺寸的 z 分量。
    /// </summary>
    /// <param name="t"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="newValue">z 分量值。</param>
    public static void SetLocalScaleZ(this Transform t, float newValue)
    {
        Vector3 v = t.localScale;
        v.z = newValue;
        t.localScale = v;
    }

    /// <summary>
    /// 二维空间下使 <see cref="UnityEngine.Transform" /> 指向指向目标点的算法，使用世界坐标。
    /// </summary>
    /// <param name="t"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="lookAtPoint2D">要朝向的二维坐标点。</param>
    /// <remarks>假定其 forward 向量为 <see cref="UnityEngine.Vector3.up" />。</remarks>
    public static void LookAt2D(this Transform t, Vector2 lookAtPoint2D)
    {
        Vector3 vector = lookAtPoint2D.ToVector3() - t.position;
        vector.y = 0f;

        if (vector.magnitude > 0f)
        {
            t.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
        }
    }

    /// <summary>
    /// 递归设置游戏对象的层次。
    /// </summary>
    /// <param name="t"><see cref="UnityEngine.Transform" /> 对象。</param>
    /// <param name="layer">目标层次的编号。</param>
    public static void SetLayerRecursively(this Transform t, int layer)
    {
        foreach (Transform child in t.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = layer;
        }
    }

    /// <summary>
    /// 隐藏按钮，setActive能不用尽量少用，效率问题。
    /// </summary>
    /// <value>The material.</value>
    public static void SetVisible(this Transform trans, bool isVisible)
    {
        if (isVisible)
        {
            trans.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            trans.localScale = new Vector3(0, 0, 0);
        }
    }

    #endregion Transform相关的拓展
}
