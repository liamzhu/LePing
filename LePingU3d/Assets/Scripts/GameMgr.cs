using System.Collections;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
	public bool isFromRecord = false;

    public void EnterToAppUpdatePage(GameObject go)
    {
        Application.OpenURL(AppConst.ApkUpdateWebsite);
    }

    public void Reconnect()
    {
        UIDialogMgr.Instance.ShowDialog(10000, (GameObject go) => { EnterToLoginWindow(); });
    }

    public void PreloadingWindows()
    {
        CoroutineMgr.Instance.StartCoroutine(PreloadingGameWindow());
    }

    private IEnumerator PreloadingGameWindow()
    {
        if (!System.IO.File.Exists(AppConst.ImagePath))
        {
            Texture2D o = Resources.Load("icon") as Texture2D;
            System.IO.File.WriteAllBytes(AppConst.ImagePath, o.EncodeToPNG());
        }
        yield return new WaitForSeconds(0.5f);
        UIWindowMgr.Instance.PrePushPanel<UIMainWindow>();
        yield return new WaitForSeconds(0.5f);
        UIWindowMgr.Instance.PrePushPanel<UITopBarWindow>();
        yield return new WaitForSeconds(0.5f);
        UIWindowMgr.Instance.PrePushPanel<UIGameWindow>();
        yield return new WaitForEndOfFrame();
        EventMgr.Instance.LoginMgr.Dispatch(EventConst.EventPreloading);
    }

    public void EnterToLoginWindow()
    {
        UIWindowMgr.Instance.ClearNavigationStack();
        UIWindowMgr.Instance.PushPanel<UILoginWindow>();
    }

    public void EnterToLoadingWindow()
    {
        UIWindowMgr.Instance.PushPanel<UILoadingWindow>();
        PreloadingWindows();
    }

    public void EnterToMainWindow()
    {
        UIWindowMgr.Instance.ClearNavigationStack();
        UIWindowMgr.Instance.PushPanel<UIMainWindow>();
        UIWindowMgr.Instance.PushPanel<UITopBarWindow>();
        MahjongAudioMgr.Instance.PlayBGM();
    }

    public void EnterToGameWindow()
    {
        UIWindowMgr.Instance.ClearNavigationStack();
        UIWindowMgr.Instance.PushPanel<UIGameWindow>();
    }
}
