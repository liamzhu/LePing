using UnityEngine;
using System.Collections;

public class ResUtility {
    public const string PREFAB = ".prefab";
    public const string JSON = ".txt";
    public const string XML = ".xml";
    public const string PNG = ".png";
    public const string MP3 = ".mp3";

    /// <summary>
    /// 配置文件根目录
    /// </summary>
    public const string CONFIG_PATH = "Assets/ResPath/Data/";
    public const string JSON_PATH = "Config/";

    /// <summary>
    /// Prefab资源根目录
    /// </summary>
    public const string PREFAB_PATH = "Assets/ResPath/Prefabs/";

    /// <summary>
    /// 特效路径
    /// </summary>
    public const string FX_PATH = PREFAB_PATH + "Particles/";

    /// <summary>
    /// 图集路径
    /// </summary>
    public const string UIATLAS_PATH = PREFAB_PATH + "UIAtlas/";

    /// <summary>
    /// UI路径
    /// </summary>
    public const string UI_PATH = PREFAB_PATH + "UIPrefab/";

    /// <summary>
    /// Texture路径
    /// </summary>
    public const string TEXTURE_PATH = PREFAB_PATH + "Textures/";

    /// <summary>
    /// Sounds路径
    /// </summary>
    public const string SOUND_PATH = PREFAB_PATH + "Sounds/";

}
