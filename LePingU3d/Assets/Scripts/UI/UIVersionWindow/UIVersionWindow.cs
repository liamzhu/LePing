using System;
using System.Collections;
using UnityEngine;

public class UIVersionWindow : UIBasePanel
{
    public UIVersionWindow() : base(new UIProperty(UIWindowStyle.PopUp, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIVersionWindow"))
    {
    }

    private UILabel mTitleLabel;
    private UILabel mDesLabel;
    private UIButton mConfirmButton;
    private UIButton mCanelButton;
    private UIButton mOkButton;
    private VersionInfoResp mVersionInfoResp;
    private UIVersionModel mUIVersiionModel;

    protected override void OnAwakeInitUI()
    {
        if (mUIVersiionModel == null)
            mUIVersiionModel = UIModelMgr.Instance.GetModel<UIVersionModel>();
        mUIVersiionModel.ValueUpdateEvent += OnValueUpdateEvent;

        mTitleLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerCenter/Body/TitleBg/Title");
        mDesLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerCenter/Body/DesLabel");
        mConfirmButton = CacheTrans.FindComponent<UIButton>("Root/ContainerCenter/Body/ButtonGroup/ConfirmButton");
        mCanelButton = CacheTrans.FindComponent<UIButton>("Root/ContainerCenter/Body/ButtonGroup/CanelButton");
        mOkButton = CacheTrans.FindComponent<UIButton>("Root/ContainerCenter/Body/ButtonGroup/OkButton");

        UIEventListener.Get(mConfirmButton.gameObject).onClick += OnCloseClick;
        UIEventListener.Get(mCanelButton.gameObject).onClick += OnCloseClick;
        UIEventListener.Get(mOkButton.gameObject).onClick += OnCloseClick;
        ApplicationMgr.Instance.onUpdate += onUpdate;
        mDesLabel.text = "检查更新中...";
    }

    private void OnValueUpdateEvent(object sender, ValueChangeArgs e)
    {
        switch (e.key)
        {
            case "VersionInfo":
                RefreshVersionInfo((VersionInfoResp)e.newValue);
                break;
            default:
                break;
        }
    }

    private void RefreshVersionInfo(VersionInfoResp newValue)
    {
        mVersionInfoResp = newValue;

        if (mVersionInfoResp != null)
        {
            if (mVersionInfoResp.update)
            {
                mVersionInfoResp.path = getApkUrl();
                mDesLabel.text = "版本需要更新,\n准备下载最新版本";
                Debug.Log(mVersionInfoResp.path);
                CoroutineMgr.Instance.StartCoroutine(BeginLoader());
            }
            else
            {
                NGUITools.SetActive(mOkButton.gameObject, true);
                mDesLabel.text = "检查完成,不需要更新";
            }
        }
    }

    private string getApkUrl()
    {
        return string.Format("http://{0}:{1}{2}", ApplicationMgr.Instance.IP, mVersionInfoResp.port, mVersionInfoResp.path);
    }

    private WWW www;

    private IEnumerator BeginLoader()
    {
        www = new WWW(mVersionInfoResp.path);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            UIEventListener.Get(mOkButton.gameObject).onClick += delegate (GameObject go) { Application.Quit(); };
            mDesLabel.text = string.Format("更新失败\n{0}", www.error);
            NGUITools.SetActive(mOkButton.gameObject, true);
            yield break;
        }

        if (this.www.isDone)
        {
            var bytes = www.bytes;
            string path = Application.persistentDataPath + "/" + System.IO.Path.GetFileName(mVersionInfoResp.path);
            DebugHelper.LogInfo(path);
            System.IO.FileStream cache = new System.IO.FileStream(path, System.IO.FileMode.Create);
            cache.Write(bytes, 0, bytes.Length);
            cache.Close();
            AutoUpdateMgr.Instance.InstallAPP(path);
        }
        yield return new WaitForFixedUpdate();
    }

    private void onUpdate()
    {
        if (www != null && !www.isDone)
        {
            mDesLabel.text = string.Format("已经下载{0}%", (int)(www.progress * 100));
        }
    }

    public override void OnRefresh()
    {
        ResetButtons();
        VersionInfoReq info = new VersionInfoReq();
        info.version = AppConst.AppVersion;
        NetReceptionMgr.Instance.SendNetMsg((short)NetPMSG.AMSG_VERSION, JsonUtil.SerializeObject(info));
    }

    private void OnCloseClick(GameObject go)
    {
        UIWindowMgr.Instance.PopPanel<UIVersionWindow>();
    }

    private void ResetButtons()
    {
        NGUITools.SetActive(mConfirmButton.gameObject, false);
        NGUITools.SetActive(mCanelButton.gameObject, false);
        NGUITools.SetActive(mOkButton.gameObject, false);
    }
}
