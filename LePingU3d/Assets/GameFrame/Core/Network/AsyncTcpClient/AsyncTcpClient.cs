using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class AsyncTcpClient : IDisposable
{
    #region 字段

    private TcpClient tcpClient;
    private int retries = 0;

    #endregion 字段

    #region 属性

    // <summary>
    // 远端服务器的IP地址
    // </summary>
    public IPAddress IP { get; private set; }

    // <summary>
    // 远端服务器的端口
    // </summary>
    public int Port { get; private set; }

    // <summary>
    // 连接重试次数
    // </summary>
    public int Retries { get; set; }

    // <summary>
    // 连接重试间隔
    // </summary>
    public int RetryInterval { get; set; }

    // <summary>
    // 是否已与服务器建立连接
    // </summary>
    public bool Connected { get { return tcpClient != null && tcpClient.Client.Connected; } }

    #endregion 属性

    #region TcpClient 连接 关闭

    // <summary>
    // 异步TCP客户端
    // </summary>
    // <param name = "ip" > 远端服务器ip </ param >
    // < param name="port">远端服务器端口</param>

    public AsyncTcpClient(string ip, int port, bool isDomain)
    {
        if (isDomain)
        {
            this.IP = Dns.GetHostAddresses(ip)[0];
        }
        else
        {
            this.IP = IPAddress.Parse(ip);
        }
        this.Port = port;
        this.tcpClient = new TcpClient();
        Retries = 3;
        RetryInterval = 5;
    }

    // <summary>
    // 连接到服务器
    // </summary>
    // <returns>异步TCP客户端</returns>
    public AsyncTcpClient Connect()
    {
        if ((tcpClient == null) || !Connected)
        {
            tcpClient.SendTimeout = 1000;
            tcpClient.ReceiveTimeout = 1000;
            tcpClient.NoDelay = true;
            try
            {
                tcpClient.BeginConnect(IP, Port, new AsyncCallback(OnConnect), tcpClient);
            }
            catch (Exception e)
            {
                Close();
                Debug.LogError(e.Message);
            }
        }
        return this;
    }

    // <summary>
    // 关闭与服务器的连接
    // </summary>
    // <returns>异步TCP客户端</returns>
    public AsyncTcpClient Close()
    {
        if (Connected)
        {
            retries = 0;
            tcpClient.Close();
            DispatchDisconnectedEventArgs(IP, Port);
        }
        return this;
    }

    #endregion TcpClient 连接 关闭

    private void OnConnect(IAsyncResult ar)
    {
        try
        {
            tcpClient.EndConnect(ar);
            DispatchConnectedEventArgs(IP, Port);
            retries = 0;
        }
        catch (Exception ex)
        {
            if (retries > 0)
            {
                //Logger.Debug(string.Format(CultureInfo.InvariantCulture, "Connect to server with retry {0} failed.", retries));
            }

            retries++;
            if (retries > Retries)
            {
                //we have failed to connect to all the IP Addresses,
                //connection has failed overall.
                DispatchExceptionEventArgs(IP, Port, ex);
                return;
            }
            else
            {
                //Logger.Debug(string.Format(CultureInfo.InvariantCulture, "Waiting {0} seconds before retrying to connect to server.", RetryInterval));
                Thread.Sleep(TimeSpan.FromSeconds(RetryInterval));
                Connect();
                return;
            }
        }
        Debug.Log("BeginRead");

        byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
        tcpClient.GetStream().BeginRead(buffer, 0, buffer.Length, OnReceived, buffer);
    }

    private void OnReceived(IAsyncResult ar)
    {
        Debug.Log("OnReceived");
        NetworkStream stream = tcpClient.GetStream();

        int numberOfReadBytes = 0;
        try
        {
            numberOfReadBytes = stream.EndRead(ar);
        }
        catch
        {
            numberOfReadBytes = 0;
        }

        if (numberOfReadBytes == 0)
        {
            Close();
            return;
        }

        byte[] buffer = (byte[])ar.AsyncState;

        byte[] receivedBytes = new byte[numberOfReadBytes];
        Debug.Log(receivedBytes);
        Buffer.BlockCopy(buffer, 0, receivedBytes, 0, numberOfReadBytes);
        DispatchDatagramReceived(tcpClient, receivedBytes);
        stream.BeginRead(buffer, 0, buffer.Length, OnReceived, buffer);
    }

    #region Send

    public void Send(byte[] data)
    {
        if (data == null)
            throw new ArgumentNullException("data");

        if (!Connected)
        {
            DispatchDisconnectedEventArgs(IP, Port);
            throw new InvalidProgramException(
              "This client has not connected to server.");
        }

        tcpClient.GetStream().BeginWrite(data, 0, data.Length, HandleDatagramWritten, tcpClient);
    }

    private void HandleDatagramWritten(IAsyncResult ar)
    {
        ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);
        Debug.Log("HandleDatagramWritten");
    }

    #endregion Send

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        Close();

        if (tcpClient != null)
        {
            tcpClient = null;
        }
    }

    #region 事件 回调

    // <summary>
    // 接收到数据包事件
    // </summary>
    public event EventHandler<TcpDataPacketReceivedEventArgs<byte[]>> DataPacketReceived;

    private void DispatchDatagramReceived(TcpClient sender, byte[] dataPacket)
    {
        if (DataPacketReceived != null)
        {
            DataPacketReceived(this, new TcpDataPacketReceivedEventArgs<byte[]>(sender, dataPacket));
        }
    }

    /// <summary>
    /// 与服务器的连接已建立事件
    /// </summary>
    public event EventHandler<TcpConnectedEventArgs> ConnectedEventArgs;

    /// <summary>
    /// 与服务器的连接已断开事件
    /// </summary>
    public event EventHandler<TcpDisconnectedEventArgs> DisconnectedEventArgs;

    // <summary>
    // 与服务器的连接发生异常事件
    // </summary>
    public event EventHandler<TcpExceptionOccurredEventArgs> ExceptionEventArgs;

    private void DispatchConnectedEventArgs(IPAddress ip, int port)
    {
        if (ConnectedEventArgs != null)
        {
            ConnectedEventArgs(this, new TcpConnectedEventArgs(ip, port));
        }
    }

    private void DispatchDisconnectedEventArgs(IPAddress ipAddresses, int port)
    {
        if (DisconnectedEventArgs != null)
        {
            DisconnectedEventArgs(this, new TcpDisconnectedEventArgs(ipAddresses, port));
        }
    }

    private void DispatchExceptionEventArgs(IPAddress ip, int port, Exception ex)
    {
        if (ExceptionEventArgs != null)
        {
            ExceptionEventArgs(this, new TcpExceptionOccurredEventArgs(ip, port, ex));
        }
    }

    #endregion 事件 回调
}
