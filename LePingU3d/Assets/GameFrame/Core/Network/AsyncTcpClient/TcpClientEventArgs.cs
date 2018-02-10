using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// 与服务器的连接已建立事件参数
/// </summary>
public class TcpConnectedEventArgs : EventArgs
{
    /// <summary>
    /// 与服务器的连接已建立事件参数
    /// </summary>
    /// <param name="ip">服务器IP地址</param>
    /// <param name="port">服务器端口</param>
    public TcpConnectedEventArgs(IPAddress ip, int port)
    {
        if (ip == null)
            throw new ArgumentNullException("IP");
        this.IP = ip;
        this.Port = port;
    }

    /// <summary>
    /// 服务器IP地址
    /// </summary>
    public IPAddress IP { get; private set; }

    /// <summary>
    /// 服务器端口
    /// </summary>
    public int Port { get; private set; }

    public override string ToString()
    {
        return string.Format("{0}:{1}", IP, Port.ToString(CultureInfo.InvariantCulture));
    }
}

/// <summary>
/// 与服务器的连接已断开事件参数
/// </summary>
public class TcpDisconnectedEventArgs : EventArgs
{
    /// <summary>
    /// 与服务器的连接已断开事件参数
    /// </summary>
    /// <param name="ip">服务器IP地址</param>
    /// <param name="port">服务器端口</param>
    public TcpDisconnectedEventArgs(IPAddress ip, int port)
    {
        if (ip == null)
            throw new ArgumentNullException("IP");

        this.IP = ip;
        this.Port = port;
    }

    /// <summary>
    /// 服务器IP地址
    /// </summary>
    public IPAddress IP { get; private set; }

    /// <summary>
    /// 服务器端口
    /// </summary>
    public int Port { get; private set; }

    public override string ToString()
    {
        return string.Format("{0}:{1}", IP, Port.ToString(CultureInfo.InvariantCulture));
    }
}

/// <summary>
/// 与服务器的连接发生异常事件参数
/// </summary>
public class TcpExceptionOccurredEventArgs : EventArgs
{
    /// <summary>
    /// 与服务器的连接发生异常事件参数
    /// </summary>
    /// <param name="ip">服务器IP地址</param>
    /// <param name="port">服务器端口</param>
    /// <param name="ex">内部异常</param>
    public TcpExceptionOccurredEventArgs(
      IPAddress ip, int port, Exception ex)
    {
        if (ip == null)
            throw new ArgumentNullException("IP");

        this.IP = ip;
        this.Port = port;
        this.Exception = ex;
    }

    /// <summary>
    /// 服务器IP地址列表
    /// </summary>
    public IPAddress IP { get; private set; }

    /// <summary>
    /// 服务器端口
    /// </summary>
    public int Port { get; private set; }

    /// <summary>
    /// 内部异常
    /// </summary>
    public Exception Exception { get; private set; }

    public override string ToString()
    {
        return string.Format("{0}:{1}", IP, Port.ToString(CultureInfo.InvariantCulture));
    }
}

/// <summary>
/// 接收到数据包事件
/// </summary>
/// <typeparam name="T">报文类型</typeparam>
public class TcpDataPacketReceivedEventArgs<T> : EventArgs
{
    /// <summary>
    /// 接收到数据报文事件参数
    /// </summary>
    /// <param name="tcpClient">客户端</param>
    /// <param name="mDataPacket">数据包</param>
    public TcpDataPacketReceivedEventArgs(TcpClient tcpClient, T mDataPacket)
    {
        TcpClient = tcpClient;
        DataPacket = mDataPacket;
    }

    public TcpClient TcpClient { get; private set; }

    /// <summary>
    /// 数据包
    /// </summary>
    public T DataPacket { get; private set; }
}
