using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public enum DisType
{
    None,
    Exception,
    Disconnect,
}

public class SocketClient
{
    #region IP和端口号

    private string mHost = "127.0.0.1";
    private int mPort = 10100;

    #endregion IP和端口号

    #region 发送 接收相关字段

    private int retries = 0;
    private Thread mRecvThread = null;
    private volatile bool mIsRunning = false;

    // 接收
    private BufferedStream mRecvStream = null;

    private object mRecvLock = null;
    private Queue<DataPacket> mRecvingMsgQueue = null;
    private Queue<DataPacket> mRecvWaitingMsgQueue = null;

    #endregion 发送 接收相关字段

    private TcpClient mTcpClient;
    private NetworkStream mStream;
    private StructCopyer mStructCopyer;

    // <summary>
    // 连接重试次数
    // </summary>
    public int Retries { get; private set; }

    // <summary>
    // 连接重试间隔
    // </summary>
    public int RetryInterval { get; private set; }

    public bool Connected { get { return mTcpClient != null && mTcpClient.Client.Connected; } }

    #region 构造函数

    public SocketClient()
    {
    }

    public SocketClient(string ipStr, int iPort)
    {
        this.Retries = 3;
        this.RetryInterval = 5;
        this.mHost = ipStr;
        this.mPort = iPort;
    }

    #endregion 构造函数

    #region 事件注册和移除 OnRegister OnRemove

    public void OnRegister()
    {
        ApplicationMgr.Instance.onApplicationQuit += SocketQuit;
        ApplicationMgr.Instance.onUpdate += Update;
    }

    public void OnRemove()
    {
        ApplicationMgr.Instance.onApplicationQuit -= SocketQuit;
        ApplicationMgr.Instance.onUpdate -= Update;
    }

    #endregion 事件注册和移除 OnRegister OnRemove

    public bool Connect()
    {
        DebugHelper.LogInfo("Begin TcpClient Connect");
        this.mStructCopyer = new StructCopyer();
        this.mIsRunning = false;
        this.mRecvLock = new object();
        this.mRecvingMsgQueue = new Queue<DataPacket>();
        this.mRecvWaitingMsgQueue = new Queue<DataPacket>();
        try
        {
            this.mIsRunning = true;
            this.mTcpClient = new TcpClient(mHost, mPort);
            this.mTcpClient.NoDelay = true;
            this.mTcpClient.SendTimeout = 5;
            this.mStream = mTcpClient.GetStream();
            OnRegister();
            this.mRecvThread = new Thread(new ThreadStart(ReceiveMessage));
            this.mRecvThread.Start();
            this.mRecvThread.IsBackground = true;
            DebugHelper.LogInfo("TcpClient Connect Suceess");
        }
        catch (System.Exception)
        {
            this.mIsRunning = false;
            OnDisconnected(DisType.None, "TcpClient Connect--->>Fail");
        }
        return mIsRunning;
    }

    #region 发送数据包 SendNetMsg

    public void SendMessage(DataPacket msg)
    {
        try
        {
            if (this.Connected)
            {
                byte[] data = msg.ToBytes(mStructCopyer);
                //PrintBytes(data);
                //this.mTcpClient.Client.Send(data, SocketFlags.None);
                this.mStream.Write(data, 0, data.Length);
                this.mStream.Flush();

                DebugHelper.LogInfo("Send " + " 包头 :" + msg.Header.HeadSign + " 校验 :" + msg.Header.Crc + " 消息长度 :" + msg.Header.DataSize + " 命令 :" + msg.Header.Command + " 消息体 :" + msg.Data);
                if (msg.Header.Command.Equals((short)NetPMSG.AMSG_PING))
                {
                    NetReceptionMgr.Instance.mSendHeartBeatTimer = DateTime.Now.Millisecond;
                    NetReceptionMgr.Instance.HeartBeatLostTime++;
                }
            }
        }
        catch (System.Exception)
        {
            OnDisconnected(DisType.Disconnect, "TcpClient Send--->>False");
        }
    }

    private void PrintBytes(byte[] bytes)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in bytes)
        {
            sb.Append(item + ",");
        }
        DebugHelper.LogInfo(sb.ToString());
    }

    #endregion 发送数据包 SendNetMsg

    #region 接收数据包 ReceiveMessage

    private bool mIsConnect = true;

    private void ReceiveMessage()
    {
        while (this.mTcpClient.IsOnline())
        {
            try
            {
                if (this.mStream.DataAvailable)
                {
                    ReadMessage();
                }
            }
            catch (Exception)
            {
                OnDisconnected(DisType.Disconnect, "TcpClient Receive--->>Fail");
                throw;
            }
        }
    }

    private void ReadMessage()
    {
        DataPacket packet = new DataPacket();
        packet.Header.HeadSign = readInt();
        if (packet.Header.HeadSign == PacketConst.SIGN)
        {
            packet.Header.Crc = readByte();
            packet.Header.DataSize = readShort();
            packet.Header.Command = readShort();

            if (packet.Header.Command.Equals((short)NetSMSG.SMSG_PONG))
            {
                packet.mData = readInt();
            }
            else if (packet.Header.Command.Equals((short)NetSMSG.SMSG_LOGIN) || packet.Header.Command.Equals((short)NetSMSG.SMSG_SERVER_STATE))
            {
                packet.mData = readShort();
            }
            else
            {
                packet.mData = readString((short)(packet.Header.DataSize - (short)NetMsg._HEADERLEN));
            }

            if (packet.Header.Command.Equals((short)NetSMSG.SMSG_PONG))
            {
                NetReceptionMgr.Instance.mRecvHeartBeatTimer = DateTime.Now.Millisecond;
                NetReceptionMgr.Instance.HeartBeatLostTime--;
            }
            SendMsgInMainThread(packet);
        }
        DebugHelper.LogInfo("Receive " + "  包头为：" + packet.Header.HeadSign + "  校验为：" + packet.Header.Crc + "  消息长度为：" + packet.Header.DataSize + "  命令为：" + packet.Header.Command + "  消息体为：" + packet.mData);
        //byte[] buffer = new byte[2000];
        //this.mStream.Read(buffer, 0, buffer.Length);
        //mStructCopyer.
        //NetMsg packet = new NetMsg();
        //packet.Header = readInt();
        //if (packet.Header == NetMsg.SIGN)
        //{
        //    packet.Crccode = readByte();
        //    packet.DataLength = readShort();
        //    packet.Command = readShort();
        //    if (packet.Command.Equals((short)NetSMSG.SMSG_SERVER_STATE) || packet.Command.Equals((short)NetSMSG.SMSG_REG) || packet.Command.Equals((short)NetSMSG.SMSG_LOGON))
        //    {
        //        packet.Data = readShort();
        //    }
        //    else if (packet.Command.Equals((short)NetSMSG.SMSG_PONG) || packet.Command.Equals((short)NetSMSG.SMSG_READY))
        //    {
        //        packet.Data = readInt();
        //    }
        //    else if (packet.Command.Equals((short)NetSMSG.SMSG_END_BET) || packet.Command.Equals((short)NetSMSG.SMSG_START_BET))
        //    {
        //        //SessionInfo info = new SessionInfo();
        //        //info.SessionId = readInt();
        //        //info.ScoreId = readShort();
        //        //info.CountDown = readShort();
        //        //packet.Data = info;
        //        //DebugHelper.LogInfo("倒计时  " + info.CountDown);
        //    }
        //    else if (packet.Command.Equals((short)NetSMSG.SMSG_SCORE_RECORD) || packet.Command.Equals((short)NetSMSG.SMSG_BET_RECORD) || packet.Command.Equals((short)NetSMSG.SMSG_USER_INFO) || packet.Command.Equals((short)NetSMSG.SMSG_RESULT) || packet.Command.Equals((short)NetSMSG.SMSG_UP_DOWN_SCORE) || packet.Command.Equals((short)NetSMSG.SMSG_MAX_WIN) || packet.Command.Equals((short)NetSMSG.SMSG_SERVER_STATE_REMARK))
        //    {
        //        packet.Data = readString((short)(packet.DataLength - (short)NetMsg._HEADERLEN));
        //    }

        //    DebugHelper.LogInfo("Receive " + "  包头为：" + packet.Header + "  校验为：" + packet.Crccode + "  消息长度为：" + packet.DataLength + "  命令为：" + packet.Command + "  消息体为：" + packet.Data);
        //    if (packet.Command.Equals((short)NetSMSG.SMSG_PONG))
        //    {
        //        NetReceptionMgr.Instance.mRecvHeartBeatTimer = DateTime.Now.Millisecond;
        //        NetReceptionMgr.Instance.HeartBeatLostTime--;
        //    }
        //    SendMsgInMainThread(packet);
        //}
    }

    private void SendMsgInMainThread(DataPacket msg)
    {
        lock (this.mRecvLock)
        {
            this.mRecvWaitingMsgQueue.Enqueue(msg);
        }
    }

    #endregion 接收数据包 ReceiveMessage

    public void Close()
    {
        this.mIsRunning = false;
        NetReceptionMgr.Instance.CloseHeartBeat();

        #region 关闭多线程

        if (mRecvThread != null)
        {
            mRecvThread.Interrupt();
            mRecvThread.Abort();
        }

        #endregion 关闭多线程

        OnRemove();
        if (this.mTcpClient != null)
        {
            this.mStream.Close();
            this.mTcpClient.Close();
            this.mTcpClient = null;
        }
        DebugHelper.LogInfo("TcpClient Closed");
    }

    public void SocketQuit()
    {
        Close();
    }

    private void Update()
    {
        #region 把收到的包数据扔进主线程

        if (this.mRecvingMsgQueue.Count == 0)
        {
            lock (this.mRecvLock)
            {
                if (this.mRecvWaitingMsgQueue.Count > 0)
                {
                    Queue<DataPacket> temp = this.mRecvingMsgQueue;
                    this.mRecvingMsgQueue = this.mRecvWaitingMsgQueue;
                    this.mRecvWaitingMsgQueue = temp;
                }
            }
        }
        else
        {
            while (this.mRecvingMsgQueue.Count > 0)
            {
                DataPacket msg = this.mRecvingMsgQueue.Dequeue();
                NetReceptionMgr.Instance.PostNotification(msg);
            }
        }

        #endregion 把收到的包数据扔进主线程

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    OnDisconnected(DisType.Disconnect);
        //}
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            DebugHelper.LogError("没有网络");
            //Do sth.
        }
    }

    /// <summary>
    /// 丢失链接
    /// </summary>
    private void OnDisconnected(DisType dis, string msg = null)
    {
        Close();   //关掉客户端链接

        if (dis == DisType.None)
        {
            UIDialogMgr.Instance.ShowDialog(10014, delegate (GameObject go) { Application.Quit(); });
        }
        else if (dis == DisType.Exception)
        {
            UIDialogMgr.Instance.ShowDialog(10017, delegate (GameObject go) { Application.Quit(); });
        }
        else
        {
            UIDialogMgr.Instance.ShowDialog(10016, delegate (GameObject go) { NetReceptionMgr.Instance.ReConnect(); }, delegate (GameObject go) { Application.Quit(); });

            //retries = 0;
            //DialogController.Instance.ShowDialog(10016, null, null, delegate (Hashtable ha) { NetReceptionMgr.Instance.ReConnect(); }, delegate (Hashtable ha) { Application.Quit(); });
        }
        DebugHelper.LogError(string.Format("DisType:> {0} Msg: {1}", dis, msg));
    }

    #region 数据转换方法

    private short readShort()
    {
        byte[] headData = new byte[NetMsg.COMMAND_LEN];
        this.mStream.Read(headData, 0, headData.Length);
        return IPAddress.NetworkToHostOrder(BitConverter.ToInt16(headData, 0));
    }

    private byte readByte()
    {
        byte[] headData = new byte[NetMsg.CRC_LEN];
        this.mStream.Read(headData, 0, headData.Length);
        return headData[0];
    }

    private int readInt()
    {
        byte[] headData = new byte[NetMsg.SIGN_LEN];
        this.mStream.Read(headData, 0, headData.Length);
        return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(headData, 0));
    }

    private long readLong()
    {
        byte[] headData = new byte[8];
        this.mStream.Read(headData, 0, headData.Length);
        return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(headData, 0));
    }

    private string readString(short num)
    {
        byte[] headData = new byte[num];
        int index = 0;
        while (index < num)
        {
            int no = this.mStream.Read(headData, index, headData.Length - index);
            index += no;
        }
        DebugHelper.LogInfo(num + "   " + headData.Length + "   " + index);
        return Encoding.UTF8.GetString(headData, 0, headData.Length);
    }

    #endregion 数据转换方法
}

/// <summary>
/// 网络状态
/// </summary>
public enum SESSION_STATUS
{
    NO_CONNECT = 0,     //无连接
    CONNECT_SUCCESS = 1,    //连接成功
    CONNECT_FAILED_CONNECT_ERROR = 2,   //连接失败
    CONNECT_FAILED_TIME_OUT = 3,        //连接超时
    CONNECT_EXIT = 4,                   //连接退出
    RE_CONNECT = 5,     //重新连接
}

public static class TcpClientEx
{
    public static bool IsOnline(this TcpClient c)
    {
        return !((c.Client.Poll(1000, SelectMode.SelectRead) && (c.Client.Available == 0)) || !c.Client.Connected);
    }
}
