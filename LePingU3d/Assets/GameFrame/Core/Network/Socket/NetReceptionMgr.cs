using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class NetReceptionMgr : Singleton<NetReceptionMgr>
{
    private SocketClient mSocketClient;
    private StructCopyer mStructCopyer;

    public void InitializeSocket(string ip, int port)
    {
        mStructCopyer = new StructCopyer();
        mSocketClient = new SocketClient(ip, port);
        mSocketClient.Connect();
    }

    public void Connect()
    {
        if (mSocketClient != null)
        {
            mSocketClient.Connect();
        }
    }

    public void SendNetMsg(short mCommand, int mData)
    {
        SendNetMsg(mCommand, mData.ToBytes());
    }

    public void SendNetMsg(short mCommand, string mData)
    {
        SendNetMsg(mCommand, mData.ToBytes());
    }

    private void SendNetMsg(short mCommand, byte[] mData)
    {
        if (AppConst.IsNetConnect)
        {
            PackHeader mPackHeader = new PackHeader();
            mPackHeader.Pack(PacketConst.SIGN, mData.getValidateCode(), (short)(mData.Length + PacketConst.HeaderLen), mCommand);
            mSocketClient.SendMessage(new DataPacket(mPackHeader, mData, mData.Length + PacketConst.HeaderLen));
        }
    }

    public void ReConnect()
    {
        if (mSocketClient.Connect())
        {
            ReLogin();
        }
    }

    // 红黑梅方王 分别表示成 1-13,14-26,27-39，40-52,53-54
    public void ReLogin()
    {
        //UIDialogMgr.Instance.ShowDialog(10021);
        //LoginInfoReq info = new LoginInfoReq();
        //info.username = PlayerPrefs.GetString("userName");
        //info.password = PlayerPrefs.GetString("passWord");
        //NetReceptionMgr.Instance.SendNetMsg((short)NetPMSG.AMSG_LOGIN, JsonHelper.SerializeObject(info));
    }

    public void LoginSuccess()
    {
        HeartBeatLostTime = 0;
        NetReceptionMgr.Instance.OpenHeartBeat();
        UIDialogMgr.Instance.HideDialog();
        if (!ApplicationMgr.Instance.GameFlag.HasFlag(GFConst.FirstEnterGame))
        {
            ApplicationMgr.Instance.GameFlag.AddFlag(GFConst.FirstEnterGame);
        }
        else
        {
            //UIMainHelper.SyncGameStatus();
        }
    }

    #region 心跳包相关方法和字段

    public int HeartBeatLostTime = 0;
    public int mSendHeartBeatTimer = 0;
    public int mRecvHeartBeatTimer = 0;

    public int delayTimer
    {
        get { return mRecvHeartBeatTimer - mSendHeartBeatTimer; }
    }

    public const int heartBeatInterval = 2000; //毫秒
    private Thread heartThread = null;

    public void OpenHeartBeat()
    {
        if (this.heartThread == null)
        {
            DebugHelper.LogInfo("****** Open HeartBeat ******");
            this.heartThread = new Thread(HeartBeat);
            this.heartThread.Start();
            this.heartThread.IsBackground = true;
        }
    }

    private void HeartBeat()
    {
        Thread.Sleep(heartBeatInterval);
        SendNetMsg((short)NetPMSG.AMSG_PING, delayTimer);
        HeartBeat();
    }

    public void CloseHeartBeat()
    {
        if (this.heartThread != null)
        {
            DebugHelper.LogInfo("****** Abort HeartBeat ******");
            this.heartThread.Abort();
            this.heartThread = null;
        }
    }

    #endregion 心跳包相关方法和字段

    public void PostNotification(DataPacket packet)
    {
        //if (packet != null)
        //{
        //    if (packet.Header.Command.Equals((short)NetSMSG.SMSG_LOGIN))
        //    {
        //        if (((short)packet.mData).Equals((short)NetSTATE.LOGIN_SUCCESS))
        //        {
        //            LoginSuccess();
        //            UIWindowMgr.Instance.PushPanel<UIAgentWindow>();
        //        }
        //        else
        //        {
        //            UIDialogMgr.Instance.ShowDialog(int.Parse(packet.mData.ToString()));
        //        }
        //    }
        //    else if (packet.Header.Command.Equals((short)NetSMSG.SMSG_AGENT_USERLIST))
        //    {
        //        UIModelMgr.Instance.GetModel<UIAgentModel>().CurrAgentUserList = packet.mData.Deserialize<AgentUserListResp>();
        //    }
        //    else if (packet.Header.Command.Equals((short)NetSMSG.SMSG_USER_RECORDS))
        //    {
        //        UIModelMgr.Instance.GetModel<UIAgentModel>().CurrUserRecords = packet.mData.Deserialize<UserRecordResp>();
        //    }
        //    else if (packet.Header.Command.Equals((short)NetSMSG.SMSG_AGENT_INFO))
        //    {
        //        UIModelMgr.Instance.GetModel<UIAgentModel>().CurrAgentInfo = packet.mData.Deserialize<AgentInfoResp>();
        //    }
        //    else if (packet.Header.Command.Equals((short)NetSMSG.SMSG_USER_INFO))
        //    {
        //        UIModelMgr.Instance.GetModel<UIAgentModel>().CurrUserInfo = packet.mData.Deserialize<UserInfo>();
        //    }
        //    else if (packet.Header.Command.Equals((short)NetSMSG.SMSG_USER_PARAM))
        //    {
        //        UIModelMgr.Instance.GetModel<UIAgentModel>().CurrUserParametersResp = packet.mData.Deserialize<UserParametersResp>();
        //    }
        //    else if (packet.Header.Command.Equals((short)NetSMSG.SMSG_VERSION))
        //    {
        //        UIModelMgr.Instance.GetModel<UIVersionModel>().VersionInfo = packet.mData.Deserialize<VersionInfoResp>();
        //    }
        //    else if (packet.Header.Command.Equals((short)NetSMSG.SMSG_SERVER_STATE))
        //    {
        //        if (((short)packet.mData).Equals((short)NetSTATE.UPDATE_PWD_SUCCESS))
        //        {
        //            NetReceptionMgr.Instance.CloseHeartBeat();
        //            PlayerPrefs.SetString("passWord", PlayerPrefs.GetString("newPassword"));
        //            UIDialogMgr.Instance.ShowDialog(int.Parse(packet.mData.ToString()), OpenLoginWindow);
        //        }
        //        else if (((short)packet.mData).Equals((short)NetSTATE.FROZEN_USER_ACCOUNT_SUCCESS))
        //        {
        //            UIDialogMgr.Instance.ShowDialog(int.Parse(packet.mData.ToString()));
        //        }
        //        else if (((short)packet.mData).Equals((short)NetSTATE.THAW_USER_ACCOUNT_SUCCESS))
        //        {
        //            UIDialogMgr.Instance.ShowDialog(int.Parse(packet.mData.ToString()));
        //        }
        //        else if (((short)packet.mData).Equals((short)NetSTATE.UP_SCORE_SUCCESS))
        //        {
        //            UIDialogMgr.Instance.ShowDialog(int.Parse(packet.mData.ToString()), delegate (GameObject go) { UIWindowMgr.Instance.PopPanel(); });
        //        }
        //        else if (((short)packet.mData).Equals((short)NetSTATE.DOWN_SCORE_SUCCESS))
        //        {
        //            UIDialogMgr.Instance.ShowDialog(int.Parse(packet.mData.ToString()), delegate (GameObject go) { UIWindowMgr.Instance.PopPanel(); });
        //        }
        //        else
        //        {
        //            UIDialogMgr.Instance.ShowDialog(int.Parse(packet.mData.ToString()));
        //        }
        //    }
        //}
    }

    private void OpenLoginWindow(GameObject go)
    {
        GameMgr.Instance.EnterToLoginWindow();
    }
}
