using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AsyncImageDownload : MonoSingleton<AsyncImageDownload>
{
    public string path
    {
        get
        {
            return Application.persistentDataPath + "/ImageCache/";
        }
    }

    private string suffix = ".png";
    private Texture2D mCustomTexture2D;
    private Dictionary<string, Texture2D> mCacheTextures;

    protected override void OnInit()
    {
        mCacheTextures = new Dictionary<string, Texture2D>();
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            base.OnInit();
        }
    }

    public void SetAsyncImage(string url, UITexture image)
    {
        if (mCustomTexture2D == null)
        {
            mCustomTexture2D = Resources.Load<Texture2D>("mask");
        }
        image.mainTexture = mCustomTexture2D;
        if (string.IsNullOrEmpty(url))
        {
            return;
        }
        if (mCacheTextures.ContainsKey(url.getMd5()))
        {
            image.mainTexture = mCacheTextures[url.getMd5()];
            return;
        }
        //判断是否是第一次加载这张图片
        if (!File.Exists(path + url.getMd5() + suffix))
        {
            //如果之前不存在缓存文件
            StartCoroutine(DownloadImage(url, image));
        }
        else {
            StartCoroutine(LoadLocalImage(url, image));
        }
    }

    private void addTexture(string url, Texture2D texture)
    {
        if (mCacheTextures.ContainsKey(url.getMd5()))
        {
            mCacheTextures[url.getMd5()] = texture;
        }
        else
        {
            mCacheTextures.Add(url.getMd5(), texture);
        }
    }

    private IEnumerator DownloadImage(string url, UITexture image)
    {
        Debug.Log("downloading new image:" + path + url);//url转换HD5作为名字
        WWW www = new WWW(url);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            Texture2D texture = www.texture;
            //将图片保存至缓存路径
            byte[] pngData = texture.EncodeToPNG();
            File.WriteAllBytes(path + url.getMd5() + suffix, pngData);
            Debug.Log(texture.name);
            image.mainTexture = texture;
            addTexture(url, texture);
        }
        www.Dispose();
    }

    private IEnumerator LoadLocalImage(string url, UITexture image)
    {
        string filePath = "file:///" + path + url.getMd5() + suffix;
        Debug.Log("getting local image:" + filePath);
        WWW www = new WWW(filePath);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            Texture2D texture = www.texture;
            image.mainTexture = texture;
            addTexture(url, texture);
        }
        www.Dispose();
    }
}
