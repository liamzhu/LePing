using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

internal enum ErrorCode
{
    Success = 0,
    ConnectError = -1,
    TimeOutError = -2,
}

/// <summary>
///
/// </summary>
/// <param name="package"></param>
public delegate void NetPushCallback(SocketPackage package);

/// <summary>
///
/// </summary>
public class SocketConnect
{
    /// <summary>
    /// push Action的请求
    /// </summary>
    private static readonly List<SocketPackage> ActionPools = new List<SocketPackage>();

    private const int TimeOut = 30;//30秒的超时时间
    private const int HearInterval = 2000;

    private Socket mSocket;
    private volatile bool mIsRunning = false;
    private readonly string mHost;
    private readonly int mPort;
    private readonly IHeadFormater mFormater;
    private bool mIsDisposed;

    private readonly List<SocketPackage> mSendList;
    private readonly Queue<SocketPackage> mReceiveQueue;
    private readonly Queue<SocketPackage> mPushQueue;

    private Thread mRecvThread = null;
    private Timer mHeartbeatThread = null;

    private byte[] mHearbeatPackage;
    public ConnectStatusCallBack mConnectStatusCallback;

    public const int HearbeatRetries = 10;  //原本5
    private int mRetries;

    public int Retries
    {
        get
        {
            return mRetries;
        }
        set
        {
            mRetries = value;
            if (mRetries >= HearbeatRetries)
            {
                OnDisconnected(Net.NetworkState.HearbeatFaild, "HearbeatFaild closed");
            }
        }
    }

    public SocketConnect(string host, int port, IHeadFormater formater)
    {
        this.Retries = 0;
        this.mHost = host;
        this.mPort = port;
        this.mFormater = formater;
        this.mSendList = new List<SocketPackage>();
        this.mReceiveQueue = new Queue<SocketPackage>();
        this.mPushQueue = new Queue<SocketPackage>();
    }

    #region 数据包

    static public void PushActionPool(int actionId, GameAction action)
    {
        RemoveActionPool(actionId);
        SocketPackage package = new SocketPackage();
        package.ActionId = actionId;
        package.Action = action;
        ActionPools.Add(package);
    }

    static public void RemoveActionPool(int actionId)
    {
        foreach (SocketPackage pack in ActionPools)
        {
            if (pack.ActionId == actionId)
            {
                ActionPools.Remove(pack);
                break;
            }
        }
    }

    /// <summary>
    /// 取出回返消息包
    /// </summary>
    /// <returns></returns>
    public SocketPackage Dequeue()
    {
        lock (mReceiveQueue)
        {
            if (mReceiveQueue.Count == 0)
            {
                return null;
            }
            else
            {
                return mReceiveQueue.Dequeue();
            }
        }
    }

    public SocketPackage DequeuePush()
    {
        lock (mPushQueue)
        {
            if (mPushQueue.Count == 0)
            {
                return null;
            }
            else
            {
                return mPushQueue.Dequeue();
            }
        }
    }

    #endregion 数据包

    /// <summary>
    /// 打开连接
    /// </summary>
    public void Open()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UIDialogMgr.Instance.ShowDialog(10020);
            return;
        }
        this.mIsRunning = false;
        try
        {
            this.Retries = 0;
            this.mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.mSocket.NoDelay = true;
            this.mSocket.Connect(mHost, mPort);
            mHeartbeatThread = new Timer(SendHeartbeatPackage, null, HearInterval, HearInterval);
            ReBuildHearbeat();
            mRecvThread = new Thread(new ThreadStart(CheckReceive));
            mRecvThread.Start();
            mRecvThread.IsBackground = true;
            this.mIsRunning = true;
        }
        catch (Exception)
        {
            this.mIsRunning = false;
            OnDisconnected(Net.NetworkState.FaildToConnect, "Connect Faild");
        }
    }

    private void OnDisconnected(Net.NetworkState state, string des)
    {
        Debug.Log(des);
        if (mConnectStatusCallback != null)
        {
            mConnectStatusCallback(state);
        }
        Close();
    }

    private void EnsureConnected()
    {
        if (mSocket == null)
        {
            Open();
        }
    }

    public bool IsConnected
    {
        get { return mSocket != null && mSocket.Connected; }
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    public void Close()
    {
        Debug.Log("Begin Close Socket");
        if (mSocket == null) return;

        if (mHeartbeatThread != null)
        {
            mHeartbeatThread.Dispose();
            mHeartbeatThread = null;
        }

        //try
        //{
        //    if (mRecvThread != null)
        //    {
        //        //mRecvThread.Interrupt();
        //        mRecvThread.Abort();
        //    }
        //}
        //catch (Exception)
        //{
        //}

        mRecvThread = null;
        if (this.mSocket != null && this.mSocket.Connected)
        {
            this.mSocket.Shutdown(SocketShutdown.Both);
            this.mSocket.Close();
        }
        this.mSocket = null;
        Debug.Log("End Close Socket");
    }

    /// <summary>
    /// rebuild socket send hearbeat package
    /// </summary>
    public void ReBuildHearbeat()
    {
        mHearbeatPackage = mFormater.BuildHearbeatPackage();
    }

    private void SendHeartbeatPackage(object state)
    {
        try
        {
            if (mHearbeatPackage != null)
            {
                if (!PostSend(mHearbeatPackage))
                {
                    Debug.Log("send heartbeat paketage fail");
                }
                else
                {
                    Retries++;
                }
            }
        }
        catch (Exception ex)
        {
            OnDisconnected(Net.NetworkState.ConnectBreak, "Send Hearbeat Exception Faild");
            UnityEngine.Debug.Log("catch" + ex.ToString());
        }
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="data"></param>
    private bool PostSend(byte[] data)
    {
        //Debug.Log("PostSend");
        EnsureConnected();
        if (this.IsConnected)
        {
            mSocket.Send(data, SocketFlags.None);
            //IAsyncResult asyncSend = mSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(sendCallback), mSocket);
            //bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
            //if (!success)
            //{
            //    OnDisconnected(Net.NetworkState.ConnectBreak, 2);
            //    Debug.Log("asyncSend error close socket");
            //    return false;
            //}
            return true;
        }
        else
        {
            OnDisconnected(Net.NetworkState.ConnectBreak, "Send Faild");
            return false;
        }
    }

    private void sendCallback(IAsyncResult asyncSend)
    {
    }

    public void Send(byte[] data, SocketPackage package)
    {
        if (data == null) { return; }

        lock (mSendList) { mSendList.Add(package); }

        try
        {
            bool success = PostSend(data);
            UnityEngine.Debug.Log("Socket send actionId:" + package.ActionId + ", msgId:" + package.MsgId + ", send result:" + success);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log("Socket send actionId: " + package.ActionId + " error" + ex);
            package.ErrorCode = (int)ErrorCode.ConnectError;
            package.ErrorMsg = ex.ToString();
            lock (mReceiveQueue)
            {
                mReceiveQueue.Enqueue(package);
            }
            lock (mSendList)
            {
                mSendList.Remove(package);
            }
        }
    }

    private void CheckReceive()
    {
        while (true)
        {
            try
            {
                byte[] prefix = new byte[4];
                int recnum = mSocket.Receive(prefix);
                //Debug.Log(recnum);
                if (recnum == 0)
                {
                    OnDisconnected(Net.NetworkState.ConnectBreak, "Receive Closed");
                    return;
                }
                if (recnum == 4)
                {
                    int datalen = BitConverter.ToInt32(prefix, 0);
                    byte[] data = new byte[datalen];
                    int startIndex = 0;
                    recnum = 0;
                    do
                    {
                        //Debug.Log("_socket.Receive data");
                        int rev = mSocket.Receive(data, startIndex, datalen - recnum, SocketFlags.None);
                        //Debug.Log("_socket.Receive data1");
                        recnum += rev;
                        startIndex += rev;
                    } while (recnum != datalen);
                    //判断流是否有Gzip压缩
                    if (data[0] == 0x1f && data[1] == 0x8b && data[2] == 0x08 && data[3] == 0x00)
                    {
                        data = NetReader.Decompression(data);
                    }

                    NetReader reader = new NetReader(mFormater);
                    reader.pushNetStream(data, NetworkType.Socket, NetWriter.ResponseContentType);
                    SocketPackage findPackage = null;

                    Debug.Log("Socket receive ok, revLen:" + recnum
                        + ", actionId:" + reader.ActionId
                        + ", msgId:" + reader.RmId
                        + ", error:" + reader.StatusCode + reader.Description
                        + ", packLen:" + reader.Buffer.Length);
                    if (reader.ActionId == 100)
                    {
                        Retries = 0;
                    }
                    lock (mSendList)
                    {
                        //find pack in send queue.
                        foreach (SocketPackage package in mSendList)
                        {
                            if (package.MsgId == reader.RmId)
                            {
                                package.Reader = reader;
                                package.ErrorCode = reader.StatusCode;
                                package.ErrorMsg = reader.Description;
                                findPackage = package;
                                break;
                            }
                        }
                    }
                    if (findPackage == null)
                    {
                        lock (mReceiveQueue)
                        {
                            //find pack in receive queue.
                            foreach (SocketPackage package in ActionPools)
                            {
                                if (package.ActionId == reader.ActionId)
                                {
                                    package.Reader = reader;
                                    package.ErrorCode = reader.StatusCode;
                                    package.ErrorMsg = reader.Description;
                                    findPackage = package;
                                    break;
                                }
                            }
                        }
                    }
                    if (findPackage != null)
                    {
                        lock (mReceiveQueue)
                        {
                            mReceiveQueue.Enqueue(findPackage);
                        }
                        lock (mSendList)
                        {
                            mSendList.Remove(findPackage);
                        }
                    }
                    else
                    {
                        //server push pack.
                        SocketPackage package = new SocketPackage();
                        package.MsgId = reader.RmId;
                        package.ActionId = reader.ActionId;
                        package.ErrorCode = reader.StatusCode;
                        package.ErrorMsg = reader.Description;
                        package.Reader = reader;
                        lock (mPushQueue)
                        {
                            mPushQueue.Enqueue(package);
                        }
                    }
                }
            }
            catch (SocketException e)
            {
                //10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    //仍然处于连接状态,但是发送可能被阻塞
                }
                else
                {
                    OnDisconnected(Net.NetworkState.ConnectBreak, "Receive SocketException Faild");
                    return;
                }
            }
            catch (Exception)
            {
                OnDisconnected(Net.NetworkState.ConnectBreak, "Receive Exception Faild");
                return;
            }
            //if (!this.IsConnected)
            //{
            //    OnDisconnected(Net.NetworkState.ConnectBreak, "Receive Connect Faild");
            //    break;
            //}
            //try
            //{
            //    if (mSocket.Poll(5, SelectMode.SelectRead))
            //    {
            //        if (mSocket.Available == 0)
            //        {
            //            OnDisconnected(Net.NetworkState.ConnectBreak, "Receive SelectRead Faild");
            //            break;
            //        }
            //        ReadMessage();
            //    }
            //    else if (mSocket.Poll(5, SelectMode.SelectError))
            //    {
            //        UnityEngine.Debug.Log("SelectError Close Socket");
            //        OnDisconnected(Net.NetworkState.ConnectBreak, "Receive SelectError Faild");
            //        break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    UnityEngine.Debug.Log("catch" + ex.ToString());
            //    OnDisconnected(Net.NetworkState.ConnectBreak, "Receive Exception Faild");
            //}
            Thread.Sleep(5);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        try
        {
            if (!this.mIsDisposed)
            {
                if (isDisposing)
                {
                    //if (socket != null) socket.Dispose(true);
                }
            }
        }
        finally
        {
            this.mIsDisposed = true;
        }
    }

    public void ProcessTimeOut()
    {
        SocketPackage findPackage = null;
        lock (mSendList)
        {
            foreach (SocketPackage package in mSendList)
            {
                if (DateTime.Now.Subtract(package.SendTime).TotalSeconds > TimeOut)
                {
                    package.ErrorCode = (int)ErrorCode.TimeOutError;
                    package.ErrorMsg = "TimeOut";
                    findPackage = package;
                    break;
                }
            }
        }
        if (findPackage != null)
        {
            lock (mReceiveQueue)
            {
                mReceiveQueue.Enqueue(findPackage);
            }
            lock (mSendList)
            {
                mSendList.Remove(findPackage);
            }
        }
    }
}

public static class TcpEx
{
    public static bool IsConnected(this Socket socket)
    {
        if (socket.Poll(-1, SelectMode.SelectRead))
        {
            byte[] tmp = new byte[1];
            int nRead = socket.Receive(tmp);
            if (nRead == 0)
            {
                return false;
            }
        }
        return true;

    }

    public static bool IsOnline(this Socket c)
    {
        return !((c.Poll(20, SelectMode.SelectRead) && (c.Available == 0)) || !c.Connected);
    }

    public static bool IsCanWrite(this Socket mSocket)
    {
        return mSocket.Poll(5, SelectMode.SelectRead);
    }

    public static bool IsCanRead(this Socket mSocket)
    {
        return !((mSocket.Poll(20, SelectMode.SelectRead) && (mSocket.Available == 0)));
    }
}
