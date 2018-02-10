using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 权重对象
/// </summary>
public class RandomObject
{
    public int Id { get; set; }

    /// <summary>
    /// 权重
    /// </summary>
    public int Weight { set; get; }
}

public class RandomUtil
{
    public static long GenerateId()
    {
        byte[] buffer = Guid.NewGuid().ToByteArray();
        return BitConverter.ToInt64(buffer, 0);
    }

    public static int getStarRandom(string str)
    {
        if (str == null) { return 0; }
        List<RandomObject> list = new List<RandomObject>();
        int[] datas = IntSplit(str);
        for (int i = 0; i < datas.Length; i++)
        {
            list.Add(new RandomObject { Id = i, Weight = datas[i] });
        }
        return GetRandomList<RandomObject>(list, 1)[0].Id;
    }

    public static int getRankRandom(string str)
    {
        if (str == null) { return 0; }
        List<RandomObject> list = new List<RandomObject>();
        int[] datas = IntSplit(str);
        for (int i = 0; i < datas.Length; i++)
        {
            list.Add(new RandomObject { Id = i + 1, Weight = datas[i] });
        }
        return GetRandomList<RandomObject>(list, 1)[0].Id;
    }

    public static List<T> GetRandomList<T>(List<T> list, int count) where T : RandomObject
    {
        if (list == null || list.Count <= count || count <= 0)
        {
            return list;
        }
        //计算权重总和
        int totalWeights = 0;
        for (int i = 0; i < list.Count; i++)
        {
            totalWeights += list[i].Weight + 0;  //权重+1，防止为0情况。
        }
        //随机赋值权重
        System.Random ran = new System.Random(GetRandomSeed());  //GetRandomSeed()随机种子，防止快速频繁调用导致随机一样的问题
        List<KeyValuePair<int, int>> wlist = new List<KeyValuePair<int, int>>();    //第一个int为list下标索引、第一个int为权重排序值
        for (int i = 0; i < list.Count; i++)
        {
            int w = (list[i].Weight + 0) + ran.Next(0, totalWeights);   // （权重+1） + 从0到（总权重-1）的随机数
            wlist.Add(new KeyValuePair<int, int>(i, w));
        }
        //排序
        wlist.Sort(delegate (KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2)
        {
            return kvp2.Value - kvp1.Value;
        });
        //根据实际情况取排在最前面的几个
        List<T> newList = new List<T>();
        for (int i = 0; i < count; i++)
        {
            T entiy = list[wlist[i].Key];
            newList.Add(entiy);
        }
        //随机法则
        return newList;
    }

    private static int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }

    public static int[] IntSplit(string str)
    {
        string[] strs = str.Split(',');
        int[] prob = new int[strs.Length];
        for (int i = 0; i < strs.Length; i++)
        {
            prob[i] = int.Parse(strs[i]);
        }
        return prob;
    }

    public static List<T> getTList<T>(IList<T> list, int num)
    {
        List<T> resultList = new List<T>();
        if (list == null || list.Count == 0)
        {
            resultList.Add(default(T));
            return resultList;
        }
        while (resultList.Count < num)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            resultList.Add(list[index]);
            list.Remove(list[index]);
        }
        return resultList;
    }

    public static List<T> getMonsterList<T>(IList<T> list, int num)
    {
        List<T> resultList = new List<T>();
        if (list == null || list.Count == 0)
        {
            resultList.Add(default(T));
            return resultList;
        }
        if (list.Count < num)
        {
            num = list.Count;
        }
        while (resultList.Count < num)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            resultList.Add(list[index]);
            list.Remove(list[index]);
        }
        return resultList;
    }

    public static T getRandom<T>(IList<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return default(T);
        }
        int count = list.Count;
        int index = UnityEngine.Random.Range(0, count);
        return list[index];
    }

    //public static ShopData getRandom(List<ShopData> list, List<ShopData> existList)
    //{
    //    if (list == null || list.Count == 0)
    //    {
    //        return default(ShopData);
    //    }
    //    if (existList != null && existList.Count > 0)
    //    {
    //        for (int i = 0; i < list.Count; i++)
    //        {
    //            existList.ForEach(m => { if (m.id.Equals(list[i].id)) { list.Remove(list[i]); } });
    //        }
    //    }
    //    int count = list.Count;
    //    int index = UnityEngine.Random.Range(0, count);
    //    return list[index];
    //}

    public static int getRandom(List<string> strs)
    {
        List<int> resultList = new List<int>();
        if (strs == null || strs.Count <= 0) { return 0; }
        //List<string> strs = Split(str, ',');
        if (strs != null && strs.Count != 0)
        {
            int index = UnityEngine.Random.Range(0, strs.Count);
            return int.Parse(strs[index]);
        }
        else { return 0; }
    }

    public static List<int> getRandom(string str, int num)
    {
        List<int> resultList = new List<int>();
        if (str == null) { return null; }
        List<string> strs = Split(str, ',');
        while (strs != null && strs.Count != 0 && resultList.Count < num)
        {
            int index = UnityEngine.Random.Range(0, strs.Count);
            resultList.Add(int.Parse(strs[index]));
            strs.Remove(strs[index]);
        }
        return resultList;
    }

    /// <summary>
    /// 字符串截取
    /// </summary>
    /// <param name="str">要被截取的字符串</param>
    /// <param name="character">按哪个字符截取</param>
    /// <returns></returns>
    public static List<string> Split(string str, char character)
    {
        return new List<string>(str.Split(character));
    }

    public static List<string> Split(string str)
    {
        List<string> reslut = new List<string>();
        if (str.Contains("%"))
        {
            int start = int.Parse(str.Substring(0, str.IndexOf('-')));
            int end = int.Parse(Regex.Match(str, @"-([\s\S]*?)%").Result("$1"));
            for (int i = start; i <= end; i++)
            {
                reslut.Add(i.ToString());
            }
        }
        else
        {
            reslut.Add(str);
        }
        return reslut;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="arrayCount">总数</param>
    /// <param name="catchCount">要随机出的个数</param>
    /// <returns></returns>
    public static int[] Array_RandomNoRepeatElement(int arrayCount, int catchCount)
    {
        if (catchCount > arrayCount)
        {
            Debug.LogError("The Array count can't less than catchCount.");
            return null;
        }
        int[] resultArray = new int[catchCount],
            originalArray = new int[arrayCount];
        for (int i = 0; i < arrayCount; i++)
        {
            originalArray[i] = i;
        }
        int randomIndex = 0, count = arrayCount, temp = 0;
        for (int i = 0; i < catchCount; i++)
        {
            randomIndex = UnityEngine.Random.Range(0, count);
            resultArray[i] = originalArray[randomIndex];
            if (randomIndex != count - 1)
            {
                temp = originalArray[randomIndex];
                originalArray[randomIndex] = originalArray[count - 1];
                originalArray[count - 1] = temp;
            }
            count--;
        }
        return resultArray;
    }
}
