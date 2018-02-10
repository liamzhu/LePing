using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class WordsFilter
{
    public static Dictionary<char, List<string>> wrodsDic = new Dictionary<char, List<string>>();

    static WordsFilter()
    {
        InitWords();
    }

    private static void InitWords()
    {
        wrodsDic.Clear();
        string sb = ((TextAsset)Resources.Load("words.txt")).text;
        string[] words = sb.Split('、');
        foreach (var item in words)
        {
            if (string.IsNullOrEmpty(item))
                continue;

            char value = item[0];
            if (wrodsDic.ContainsKey(value))
                wrodsDic[value].Add(item);
            else
                wrodsDic.Add(value, new List<string>() { item });
        }
        foreach (var item in wrodsDic)
        {
            item.Value.OrderBy(g => g.Length);
        }
    }

    public static string CheckText(string text)
    {
        int count = text.Length;
        StringBuilder sb = new StringBuilder();
        List<string> data = null;

        char word = '1';
        for (int i = 0; i < count; i++)
        {
            word = text[i];
            if (wrodsDic.ContainsKey(word))
            {
                int num = 0;
                data = wrodsDic[word];

                foreach (var wordbook in data)
                {
                    if (i + wordbook.Length <= count)
                    {
                        string result = text.Substring(i, wordbook.Length);
                        if (result == wordbook)
                        {
                            num = 1;
                            sb.Append(GetString(result));
                            i = i + wordbook.Length - 1;
                            break;
                        }
                    }
                }

                if (num == 0)
                    sb.Append(word);

            }
            else
            {
                sb.Append(word);
            }
        }
        return sb.ToString();
    }

    private static string GetString(string value)
    {
        string starNum = string.Empty;
        for (int i = 0; i < value.Length; i++)
        {
            starNum += "*";
        }
        return starNum;
    }

}
