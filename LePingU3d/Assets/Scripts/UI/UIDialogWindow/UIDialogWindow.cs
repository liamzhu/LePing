using System.Collections;
using UnityEngine;

public class UIDialogWindow : UIBasePanel
{
    public UIDialogWindow() : base(new UIProperty(UIWindowStyle.PopUp, UIWindowMode.DoNothing, UIColliderType.Normal, UIAnimationType.Normal, "UIPrefab/UIDialogWindow"))
    {
    }

    private UILabel mTitleLabel;
    private UILabel mDesLabel;
    private UIButton mConfirmButton;
    private UIButton mCanelButton;
    private UIButton mOkButton;
    private HUDText mHUDText;
    private Transform mRoot;
    private Transform mMaskCollider;
    private UIDialogInfo mUIDialogInfo;

    protected override void OnAwakeInitUI()
    {
        mTitleLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerCenter/DialogGroup/Title");
        mDesLabel = CacheTrans.FindComponent<UILabel>("Root/ContainerCenter/DialogGroup/ButtonGroup/DesLabel");
        mConfirmButton = CacheTrans.FindComponent<UIButton>("Root/ContainerCenter/DialogGroup/ButtonGroup/ConfirmButton");
        mCanelButton = CacheTrans.FindComponent<UIButton>("Root/ContainerCenter/DialogGroup/ButtonGroup/CanelButton");
        mOkButton = CacheTrans.FindComponent<UIButton>("Root/ContainerCenter/DialogGroup/ButtonGroup/OkButton");

        mHUDText = CacheTrans.FindComponent<HUDText>("HUDText");
        mRoot = CacheTrans.FindComponent<Transform>("Root");
        mMaskCollider = CacheTrans.FindComponent<Transform>("MaskCollider");

    }

    public override void OnRefresh()
    {
        ResetButtons();
        mUIDialogInfo = mData == null ? null : mData as UIDialogInfo;
        if (mUIDialogInfo != null)
        {
            if (mUIDialogInfo.DialogCfg == null)
            {
                NGUITools.SetActive(mMaskCollider.gameObject, false);
                NGUITools.SetActive(mRoot.gameObject, false);
                mHUDText.Add(string.Format("{0}", mUIDialogInfo.DialogId), Color.red, 0);
            }
            else
            {
                if (mUIDialogInfo.DialogCfg.OutType == 9)
                {
                    NGUITools.SetActive(mMaskCollider.gameObject, false);
                    NGUITools.SetActive(mRoot.gameObject, false);
                    mHUDText.Add(string.Format(mUIDialogInfo.DialogCfg.Contents, mUIDialogInfo.Args), Color.red, 0);
                }
                else
                {
                    NGUITools.SetActive(mMaskCollider.gameObject, true);
                    NGUITools.SetActive(mRoot.gameObject, true);
                    SetButtons();
                    mTitleLabel.text = mUIDialogInfo.DialogCfg.Title != "-1" ? mUIDialogInfo.DialogCfg.Title : string.Empty;
                    if (mDesLabel != null)
                        mDesLabel.text = string.Format(mUIDialogInfo.DialogCfg.Contents, mUIDialogInfo.Args);
                    if (mUIDialogInfo.ConfirmCallback != null)
                    {
                        UIEventListener.Get(mConfirmButton.gameObject).onClick += mUIDialogInfo.ConfirmCallback;
                        UIEventListener.Get(mOkButton.gameObject).onClick += mUIDialogInfo.ConfirmCallback;
                    }
                    if (mUIDialogInfo.CanelCallback != null)
                    {
                        UIEventListener.Get(mCanelButton.gameObject).onClick += mUIDialogInfo.CanelCallback;
                    }
                }
            }
        }
    }

    protected override void OnExitBefore()
    {
        UIEventListener.Get(mConfirmButton.gameObject).onClick = null;
        UIEventListener.Get(mCanelButton.gameObject).onClick = null;
        UIEventListener.Get(mOkButton.gameObject).onClick = null;
        base.OnExitBefore();
    }

    private void SetButtons()
    {
        if (mUIDialogInfo.DialogCfg.OutType == 1)
        {
            NGUITools.SetActive(mConfirmButton.gameObject, false);
            NGUITools.SetActive(mCanelButton.gameObject, false);
            NGUITools.SetActive(mOkButton.gameObject, true);
        }
        else if (mUIDialogInfo.DialogCfg.OutType == 2)
        {
            NGUITools.SetActive(mConfirmButton.gameObject, true);
            NGUITools.SetActive(mCanelButton.gameObject, true);
            NGUITools.SetActive(mOkButton.gameObject, false);
        }
        else if (mUIDialogInfo.DialogCfg.OutType == 9)
        {
            NGUITools.SetActive(mConfirmButton.gameObject, false);
            NGUITools.SetActive(mCanelButton.gameObject, false);
            NGUITools.SetActive(mOkButton.gameObject, false);
        }
    }

    private void OnCloseClick(GameObject go)
    {
        UIWindowMgr.Instance.PopPanel<UIDialogWindow>();
    }

    private void ResetButtons()
    {
        UIEventListener.Get(mConfirmButton.gameObject).onClick += OnCloseClick;
        UIEventListener.Get(mCanelButton.gameObject).onClick += OnCloseClick;
        UIEventListener.Get(mOkButton.gameObject).onClick += OnCloseClick;
        NGUITools.SetActive(mConfirmButton.gameObject, false);
        NGUITools.SetActive(mCanelButton.gameObject, false);
        NGUITools.SetActive(mOkButton.gameObject, false);
    }

    public class UIDialogInfo
    {
        public DialogCfg DialogCfg;
        public object DialogId;
        public UIEventListener.VoidDelegate ConfirmCallback;
        public UIEventListener.VoidDelegate CanelCallback;
        public object[] Args;
        public object Addition;
        public object Addition2;
        public Color Color;

        public UIDialogInfo()
        {
        }

        public UIDialogInfo(DialogCfg cfg, object id, UIEventListener.VoidDelegate mConfirmCallback, UIEventListener.VoidDelegate mCanelCallback, object[] add, Color color)
        {
            this.DialogCfg = cfg;
            this.DialogId = id;
            this.ConfirmCallback = mConfirmCallback;
            this.CanelCallback = mCanelCallback;
            this.Args = add;
            this.Color = color;
        }
    }
}
