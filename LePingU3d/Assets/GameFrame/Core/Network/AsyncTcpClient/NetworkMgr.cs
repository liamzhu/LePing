using System;
using System.Collections;
using System.Net;
using System.Text;
using UnityEngine;

public class NetworkMgr : Singleton<NetworkMgr>
{
    private AsyncTcpClient mAsyncTcpClient;
    private StructCopyer mStructCopyer;

    public void InitializeSocket(string ip, int port, bool isDomain = false)
    {
        mStructCopyer = new StructCopyer();
        mAsyncTcpClient = new AsyncTcpClient(ip, port, isDomain);
        mAsyncTcpClient.ConnectedEventArgs += OnConnectedEventArgs;
        mAsyncTcpClient.DisconnectedEventArgs += OnDisconnectedEventArgs;
        mAsyncTcpClient.ExceptionEventArgs += OnExceptionEventArgs;
        mAsyncTcpClient.DataPacketReceived += OnDataPacketReceived;
        mAsyncTcpClient.Connect();
    }

    public void SendMsg(short mCommand, byte[] mData)
    {
        PackHeader mPackHeader = new PackHeader();
        mPackHeader.HeadSign = PacketConst.SIGN;
        mPackHeader.Crc = mData.getValidateCode();
        mPackHeader.DataSize = (short)(mData.Length + PacketConst.HeaderLen);

        byte[] totalByte = new byte[mPackHeader.DataSize];

        mPackHeader.DataSize = IPAddress.HostToNetworkOrder(mPackHeader.DataSize);
        mPackHeader.Command = IPAddress.HostToNetworkOrder(mCommand);

        byte[] byte1 = mStructCopyer.StructToBytes(mPackHeader);
        //Debug.Log(mPackHeader.command);
        byte1.CopyTo(totalByte, 0);
        mData.CopyTo(totalByte, PacketConst.HeaderLen);

        StringBuilder sb = new StringBuilder();
        foreach (var item in totalByte)
        {
            sb.Append(item + ",");
        }
        Debug.Log(sb.ToString());
        mAsyncTcpClient.Send(totalByte);
    }

    private void OnDataPacketReceived(object sender, TcpDataPacketReceivedEventArgs<byte[]> e)
    {
        throw new NotImplementedException();
    }

    private void OnExceptionEventArgs(object sender, TcpExceptionOccurredEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnDisconnectedEventArgs(object sender, TcpDisconnectedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnConnectedEventArgs(object sender, TcpConnectedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
