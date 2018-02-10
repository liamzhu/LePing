using System.Collections;
using UnityEngine;

public class SkeletonRenderer : MonoBehaviour
{

    /// <summary>
    ///
    /// </summary>
    public bool controllerMaterialByOut = false;

    public Material[] repMaterials; /// material for replace

    ///是否需要和NGUI统一层
    public bool needControllerRenderQueue = false;

    public int renderQueue = 3000;

    public virtual void LateUpdate()
    {

        /////如果要替换材质
        //if (this.controllerMaterialByOut)
        //{
        //    for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++)
        //    {
        //        var texture = meshRenderer.sharedMaterials[i].GetTexture("_MainTex");
        //        var alpha_texture = meshRenderer.sharedMaterials[i].GetTexture("_AlphaTex");
        //        this.repMaterials[i].SetTexture("_MainTex", texture);
        //        this.repMaterials[i].SetTexture("_AlphaTex", alpha_texture);
        //    }

        //    meshRenderer.sharedMaterials = this.repMaterials;
        //}

        /////如果在 NGUI 中要动态改层级
        //if (this.needControllerRenderQueue)
        //{
        //    for (int i = 0; i < this.meshRenderer.sharedMaterials.Length; i++)
        //    {
        //        this.meshRenderer.sharedMaterials[i].renderQueue = renderQueue;
        //    }
        //}
    }
}
