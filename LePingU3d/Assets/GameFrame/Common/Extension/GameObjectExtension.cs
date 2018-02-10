using UnityEngine;
using System.Collections;

public static class GameObjectExtension
{

    #region GameObject相关的拓展
    /// <summary>  
    /// 隐藏按钮，setActive能不用尽量少用，效率问题。  
    /// </summary>  
    /// <value>The material.</value>  
    public static void SetVisible(this GameObject go, bool isVisible)
    {
        if (isVisible)
        {
            go.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            go.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
    /// 获取或创建组件
    /// </summary>
    /// <typeparam name="T">要获取或增加的组件</typeparam>
    /// <param name="obj">目标对象</param>
    /// <returns>获取或增加的组件</returns>
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component == null)
        {
            component = obj.AddComponent<T>();
        }
        return component;
    }

    public static void SetParent(this GameObject go, Transform parent)
    {
        go.transform.parent = parent;
    }
    #endregion
}
