#define JSONFX

using UnityEngine;

#if JSONFX

using Pathfinding.Serialization.JsonFx;

#elif JsonDotNet

using Newtonsoft.Json;

#endif

using System;
using System.Collections.Generic;

public class JsonUtil
{
    static public List<T> Deserialize2List<T>(string json) where T : class
    {
        T[] ts = DeserializeObject<T[]>(json);
        List<T> list = new List<T>();
        for (int i = 0; i < ts.Length; i++)
        {
            list.Add(ts[i]);
        }
        return list;
    }

    //static public List<T> Deserialize2List<T>(string json) where T : class
    //{
    //    return DeserializeObject<List<T>>(json);
    //}

    public static Dictionary<string, string> Json2Dictionary(string json)
    {
        Dictionary<string, string> _languageNode = new Dictionary<string, string>();
        try
        {
            _languageNode = DeserializeObject<Dictionary<string, string>>(json);
        }
        catch (Exception ex)
        {
            DebugHelper.LogError(ex.Message);
        }
        return _languageNode;
    }

    public static string SerializeObject(object value)
    {
#if JsonDotNet
        return JsonConvert.SerializeObject(value, Formatting.None);
#elif JSONFX
        return JsonWriter.Serialize(value);
#endif
    }

    public static T DeserializeObject<T>(string value) where T : class
    {
#if JsonDotNet
        return JsonConvert.DeserializeObject<T>(value);
#elif JSONFX
        return JsonReader.Deserialize<T>(value);
#endif
    }
}

public static class JsonHelperExpansion
{
    public static T Deserialize<T>(this object value) where T : class
    {
#if JsonDotNet
        return JsonUtil.DeserializeObject<T>(value.ToString());
#elif JSONFX
        return Deserialize<T>(value.ToString());
#endif
    }

    public static T Deserialize<T>(this string value) where T : class
    {
#if JsonDotNet
        return (T)JsonConvert.DeserializeObject(value, typeof(T));
#elif JSONFX
        return JsonReader.Deserialize<T>(value);
#endif
    }
}
