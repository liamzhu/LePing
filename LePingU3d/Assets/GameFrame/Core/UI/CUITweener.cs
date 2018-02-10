using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class CUITweener
{
    public static Tweener ShowCenterToBig(Transform trans, TweenCallback callback)
    {
        trans.localScale = Vector3.zero;
        trans.gameObject.SetActive(true);
        return trans.DOScale(Vector3.one, 1.5f).SetEase(Ease.InOutSine).SetAutoKill(false).OnComplete(callback);
    }

    public static Tweener ShowFromDir(Transform trans, UIAnimationType mUIShowStyle, TweenCallback callback)
    {
        if (mUIShowStyle == UIAnimationType.FromDown)
        {
            trans.localPosition = new Vector3(0, -1000, 0);
            trans.gameObject.SetActive(true);
            return trans.DOLocalMove(Vector3.one, 1.5f).SetEase(Ease.InOutSine).SetAutoKill(false).OnComplete(callback);
        }
        else if (mUIShowStyle == UIAnimationType.FromLeft)
        {
            trans.localPosition = new Vector3(-1000, 0, 0);
            trans.gameObject.SetActive(true);
            return trans.DOLocalMove(Vector3.one, 1.5f).SetEase(Ease.InOutSine).SetAutoKill(false).OnComplete(callback);
        }
        else if (mUIShowStyle == UIAnimationType.FromRight)
        {
            trans.localPosition = new Vector3(1000, 0, 0);
            trans.gameObject.SetActive(true);
            return trans.DOLocalMove(Vector3.one, 1.5f).SetEase(Ease.InOutSine).SetAutoKill(false).OnComplete(callback);
        }
        else
        {
            trans.localPosition = new Vector3(0, 1000, 0);
            trans.gameObject.SetActive(true);
            return trans.DOLocalMove(Vector3.one, 1.5f).SetEase(Ease.InOutSine).SetAutoKill(false).OnComplete(callback);
        }
    }
}

public static class CUITweenerExtension
{
    public static Tweener TryAddUITween(this Transform trans, UIAnimationType mUIShowStyle, TweenCallback callback = null)
    {
        if (mUIShowStyle == UIAnimationType.CenterToBig)
        {
            return CUITweener.ShowCenterToBig(trans, callback);
        }
        else if (mUIShowStyle == UIAnimationType.FromDown || mUIShowStyle == UIAnimationType.FromLeft || mUIShowStyle == UIAnimationType.FromRight || mUIShowStyle == UIAnimationType.FromTop)
        {
            return CUITweener.ShowFromDir(trans, mUIShowStyle, callback);
        }
        else
        {
            return null;
        }
    }
}
