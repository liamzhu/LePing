using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModelMgr : MonoSingleton<UIModelMgr>
{

    /// <summary>
    /// 存储所有的实例化的Model
    /// </summary>
    private Dictionary<string, UIBaseModel> dictionary;

    private UIModelMgr()
    {
        dictionary = new Dictionary<string, UIBaseModel>();
    }

    protected override void OnUnInit()
    {
        dictionary.Clear();
        dictionary = null;
    }

    public T GetModel<T>() where T : UIBaseModel
    {
        Type type = typeof(T);

        if (dictionary.ContainsKey(type.Name))
        {
            return dictionary[type.Name] as T;
        }
        T model = System.Activator.CreateInstance(type) as T;
        dictionary.Add(type.Name, model);
        return model;
    }
}
