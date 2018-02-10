using System.Collections.Generic;

public static class DictionaryExtension
{

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    /// <typeparam name="Tvalue"></typeparam>
    /// <param name="dict">要获取值得字典</param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Tvalue TryGetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
    {
        Tvalue value;
        dict.TryGetValue(key, out value);
        return value;
    }

    /// <summary>
    /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
    /// </summary>
    public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (!dict.ContainsKey(key)) dict.Add(key, value);
        return dict;
    }

    /// <summary>
    /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
    /// </summary>
    public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (!dict.ContainsKey(key)) dict.Add(key, value);
        else dict[key] = value;
        return dict;
    }
}
