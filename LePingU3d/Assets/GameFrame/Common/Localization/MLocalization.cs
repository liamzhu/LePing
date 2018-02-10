using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MLocalization : MonoSingleton<MLocalization>
{
    /* Language Types */
    public const string CHINESE = "Localization/Chinese.json";
    public const string ENGLISH = "Localization/English.json";


    public SystemLanguage debugLanguage = SystemLanguage.Unknown;
    private string _language;
    public string Language
    {
        get
        {
            return _language;
        }
        set
        {
            _language = value;
            TextAsset asset = Resources.Load<TextAsset>(_language);
            _languageNode = JsonUtil.Json2Dictionary(asset.text);
        }
    }

    private Dictionary<string, string> _languageNode;

    private MLocalization()
    {
        Language = CHINESE;
    }

    public string Get(string key)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return key;
#endif
#if SHOW_REPORT
		if (!mUsed.Contains(key)) mUsed.Add(key);
#endif
        string val;
#if UNITY_IPHONE || UNITY_ANDROID
		if (_languageNode.TryGetValue(key + " Mobile", out val)) return val;
#endif

#if UNITY_EDITOR
        if (_languageNode.TryGetValue(key, out val)) return val;
        Debug.LogWarning("Localization key not found: '" + key + "'");
        return key;
#else
		return (_languageNode.TryGetValue(key, out val)) ? val : key;
#endif
    }
}

