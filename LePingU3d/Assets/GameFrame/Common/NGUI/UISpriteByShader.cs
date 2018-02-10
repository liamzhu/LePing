using UnityEngine;
using System.Collections;

public class UISpriteByShader : UISprite
{
    public bool IsSeperateRGBandAlpha = true;

    protected UIPanel panelObj = null;
    protected Material GrayMaterial;
    /// <summary>  
    /// ngui对Sprite进行渲染时候调用  
    /// </summary>  
    /// <value>The material.</value>  
    public override Material material
    {
        get
        {
            Material mat = base.material;
            if (mat == null)
            {
                mat = (atlas != null) ? atlas.spriteMaterial : null;
            }
            if (GrayMaterial != null) { return GrayMaterial; }
            else { return mat; }
        }
    }

    /// <summary>  
    /// 隐藏按钮，setActive能不用尽量少用，效率问题。  
    /// </summary>  
    /// <value>The material.</value>  
    public void SetVisible(bool isVisible)
    {
        if (isVisible) { transform.localScale = new Vector3(1, 1, 1); }
        else { transform.localScale = new Vector3(0, 0, 0); }
    }

    /// <summary>  
    /// 将按钮置为禁止点击状态，false为禁用状态  
    /// </summary>  
    /// <value>The material.</value>  
    public void SetEnabled(bool isEnabled)
    {
        if (isEnabled)
        {
            BoxCollider lisener = gameObject.GetComponent<BoxCollider>();
            if (lisener) { lisener.enabled = true; }
            SetNormal();
        }
        else
        {
            BoxCollider lisener = gameObject.GetComponent<BoxCollider>();
            if (lisener) { lisener.enabled = false; }
            SetGray();
        }
    }

    /// <summary>  
    /// 将GrayMaterial置为null，此时会调用默认材质，刷新panel才会重绘Sprite  
    /// </summary>  
    /// <value>The material.</value>  
    public void SetNormal()
    {
        GrayMaterial = null;
        RefreshPanel(gameObject);
    }

    /// <summary>  
    /// 调用此方法可将Sprite变灰  
    /// </summary>  
    /// <value>The material.</value>  
    public void SetGray()
    {
        Material mat;
        if (IsSeperateRGBandAlpha)
        {
            if (GrayMaterial != null)
            {
                mat = GrayMaterial;
            }
            else
            {
                mat = new Material(Shader.Find("UI/UI_ETC_Gray"));
                mat.SetTexture("_MainTex", material.GetTexture("_MainTex"));
                mat.SetTexture("_AlphaTex", material.GetTexture("_MainTex_A"));
            }
        }
        else
        {
            mat = new Material(Shader.Find("UI/UI_Gray"));
            mat.mainTexture = material.mainTexture;
        }
        GrayMaterial = mat;
        RefreshPanel(gameObject);
    }

    ///刷新panel，重绘Sprite   
    void RefreshPanel(GameObject go)
    {
        if (panelObj == null)
        {
            panelObj = NGUITools.FindInParents<UIPanel>(go);
        }

        if (panelObj != null)
        {
            panelObj.enabled = false;
            panelObj.enabled = true;
        }
    }
}
