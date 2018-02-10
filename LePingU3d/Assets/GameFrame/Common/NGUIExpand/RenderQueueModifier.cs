using System.Collections;
using UnityEngine;

public class RenderQueueModifier : MonoBehaviour
{
    public enum RenderType
    {
        Renderer,
        SkeletonRenderer,
        Particles
    }

    public RenderType mRenderType = RenderType.Particles;

    ///必须设置 或者 由代码设置
    public UIPanel mPanel;

    public UIWidget mTarget;
    public bool isForSpine = true;

    public Renderer mRenderer;
    public SkeletonRenderer mSkeletonRender;

    private void Awake()
    {
        switch (mRenderType)
        {
            case RenderType.Renderer:
                mRenderer = this.GetComponent<Renderer>();
                break;
            case RenderType.SkeletonRenderer:
                mSkeletonRender = GetComponent<SkeletonRenderer>();
                break;
            case RenderType.Particles:
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        AddToPanel();
    }

    public void Set(UIPanel m_panel, UIWidget m_target, RenderType _RenderType)
    {
        this.mPanel = m_panel;
        this.mTarget = m_target;
        this.mRenderType = _RenderType;
        AddToPanel();
    }

    private void AddToPanel()
    {
        if (mPanel != null) mPanel.renderQueueModifiers.Add(this);
    }

    private void OnDisable()
    {
        mPanel.renderQueueModifiers.Remove(this);
    }

    private int lasetQueue = int.MinValue;

    public void setQueue(int queue)
    {
        if (this.lasetQueue != queue)
        {
            this.lasetQueue = queue;
            switch (mRenderType)
            {
                case RenderType.Renderer:
                    mRenderer.material.renderQueue = this.lasetQueue;
                    break;
                case RenderType.SkeletonRenderer:
                    mSkeletonRender.needControllerRenderQueue = true;
                    mSkeletonRender.renderQueue = this.lasetQueue;
                    break;
                case RenderType.Particles:
                    break;
                default:
                    break;
            }
        }
    }
}
