using UnityEngine;
using System.Collections;
using System.Text;
using DG.Tweening;

public class UIChatItem : MonoBehaviour
{

    private UILabel mChatInfo;
    private TypewriterEffect mTypewriterEffect;
    private Transform mCacheTransform;
    private Tweener mTweener;

    private void Awake()
    {
        mCacheTransform = this.transform;
        mTypewriterEffect = mCacheTransform.FindComponent<TypewriterEffect>("MsgInfo");
        mChatInfo = mCacheTransform.FindComponent<UILabel>("MsgInfo");
        mTweener = mCacheTransform.DOScale(Vector3.zero, 0.5f);
        mTweener.SetAutoKill(false);
        mTweener.SetEase(Ease.InOutCirc);
        mTweener.SetDelay(2f);
    }

    public void ShowChatInfo(string info)
    {
        if (info != null)
        {

            mChatInfo.text = info;
            SetVisible(true);
            mTypewriterEffect.ResetToBeginning();
            mTypewriterEffect.enabled = true;
            mTweener.Restart();
        }
    }

    public void SetVisible(bool isVisible)
    {
        mCacheTransform.gameObject.SetActive(isVisible);
    }
}
