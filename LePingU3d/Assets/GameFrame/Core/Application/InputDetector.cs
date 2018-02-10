using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InputDetector : MonoBehaviour
{

    private void Update()
    {
        #region 安卓返回退出的功能

        InputGetKeyDown(KeyCode.Escape);

        #endregion 安卓返回退出的功能

        if (Input.GetKeyDown(KeyCode.D))
        {
            UIDialogMgr.Instance.ShowDialog(10018, null, null, "fail! error code = " + 204 + "; error msg = " + "dfhbafsbakfbasjfbafnsasdaffffffff");
        }
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    AudioManager.Instance.GlobalVolume = 0;
        //    MahjongAudioMgr.Instance.PlayChiPaiSound(1);
        //    //AudioManager.Instance.StopBGAudio();
        //    //UIWindowMgr.Instance.PushPanel<UISettingWindow>(false);
        //}
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    AudioManager.Instance.GlobalVolume = 1;
        //    MahjongAudioMgr.Instance.PlayChiPaiSound(1);
        //    AudioManager.Instance.PlayCurrBGAudio();
        //    //UIWindowMgr.Instance.PushPanel<UISettingWindow>(false);
        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    string data = "{\"country\":\"CN\", \"province\":\"Guangdong\", \"headimgurl\":\"http://wx.qlogo.cn/mmopen/HmVQlX9WkBuWU6xey4ocGHewjEwC7lr3picwqubjzUibZ8PEiaSlqL9yhfm3y17t5BaaJJYaEqDTiapKQkz2h53GmNY3De6kJlAQ/0\", \"unionid\":\"oOvCQwDImEb6DmYw5LYmu5D5vJXA\", \"openid\":\"omFZUwJ0vUusU38MVkjiZlCp0m9c\", \"nickname\":\"Cavlin\u6234\ud83d\ude04\ud83d\ude01\", \"city\":\"Jiangmen\", \"sex\":1, \"language\":\"zh_CN\", \"privilege\":[]}";
        //    WeChatUserResp resp = JsonUtil.DeserializeObject<WeChatUserResp>(data);
        //    Debug.Log(resp.nickname);
        //}
    }

    private void InputGetKeyDown(KeyCode mKeyCode)
    {
        if (Input.GetKeyDown(mKeyCode))
        {
            switch (mKeyCode)
            {
                case KeyCode.None:
                    break;
                case KeyCode.Return:
                    break;
                case KeyCode.Pause:
                    break;
                case KeyCode.Escape:
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        UIDialogMgr.Instance.ShowDialog(10015, delegate (GameObject go) { Application.Quit(); });
                    }
                    break;
                case KeyCode.A:
                    break;
                case KeyCode.B:
                    break;
                case KeyCode.C:
                    break;
                case KeyCode.D:
                    break;
                case KeyCode.E:
                    break;
                case KeyCode.F:
                    break;
                case KeyCode.G:
                    break;
                case KeyCode.H:
                    break;
                case KeyCode.I:
                    break;
                case KeyCode.J:
                    break;
                case KeyCode.K:
                    break;
                case KeyCode.L:
                    break;
                case KeyCode.M:
                    break;
                case KeyCode.N:
                    break;
                case KeyCode.O:
                    break;
                case KeyCode.P:
                    break;
                case KeyCode.Q:
                    break;
                case KeyCode.R:
                    break;
                case KeyCode.S:
                    break;
                case KeyCode.T:
                    break;
                case KeyCode.U:
                    break;
                case KeyCode.V:
                    break;
                case KeyCode.W:
                    break;
                case KeyCode.X:
                    break;
                case KeyCode.Y:
                    break;
                case KeyCode.Z:
                    break;
                default:
                    break;
            }
        }
    }
}
