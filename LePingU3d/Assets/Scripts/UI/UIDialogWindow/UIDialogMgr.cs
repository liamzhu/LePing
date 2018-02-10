using System.Collections;
using UnityEngine;

public class UIDialogMgr : Singleton<UIDialogMgr>
{
    private DialogCfg mDialogCfg;

    public void ShowDialog(int Id, UIEventListener.VoidDelegate confirm = null, UIEventListener.VoidDelegate cancel = null, params object[] addition)
    {
        mDialogCfg = DialogCfgHelper.Instance.GetCfg(Id);
        UIWindowMgr.Instance.PushPanel<UIDialogWindow>(new UIDialogWindow.UIDialogInfo(mDialogCfg, Id, confirm, cancel, addition, Color.white));
    }

    public void ShowDialog(string msg)
    {
        UIWindowMgr.Instance.PushPanel<UIDialogWindow>(new UIDialogWindow.UIDialogInfo(null, msg, null, null, null, Color.white));
    }

    public void HideDialog()
    {
        UIWindowMgr.Instance.PopPanel<UIDialogWindow>();
    }
}
