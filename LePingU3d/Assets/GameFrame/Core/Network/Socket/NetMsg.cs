using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

/**
*
*手机端和游戏服务间使用自定义协议UGP协议进行通信，UGP协议是一款基于TCP封装的通信
*协议。协议分为5部分：
*4字节包头（0xFFFF)+1字节校验(初始化0F64,内容异或)+ 2字节消息长度（2字节）+ 2字节消息命令（2字节）) + 消息体DATA
*
*/

public class NetMsg : System.IDisposable
{
    public const short _VERSION = 0x0001;
    public const int _HEADERLEN = 9; // 协议头字节长度
    public const int MAX_MSG_LEN = 1024 * 10; // 每条消息的最大字节数
    /**
	 * 头部标示
	 */
    public const int SIGN = -1;
    public const int SIGN_LEN = 4;//标示长度
    public const int CRC_LEN = 1;
    public const int COMMAND_LEN = 2;
    public const int DATA_LEN = 2;

    public const byte initCode = 0x64;

    //包头
    public int Header { get; set; }

    //校验
    public byte Crccode { get; set; }

    //消息体长度
    public short DataLength { get; set; }

    //消息命令
    public short Command { get; set; }

    //消息体
    public object Data { get; set; }

    public NetMsg()
    {
    }

    public NetMsg(short mCommand, object mData)
    {
        if (mData == null) { return; }
        this.Header = SIGN;
        //this.Crccode = initCode;
        this.Command = mCommand;
        this.Data = mData;
    }

    /// <summary>
    /// 得到验证编码
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public static byte getValidateCode(byte[] b)
    {
        byte total = 0;
        int i = 0; // 从数据byte[] 第0位开始
        int len = b.Length;
        while (i < len)
        {
            total += b[i++]; // byte[] 数据流
        }
        return (byte)(initCode ^ total);
    }

    public static byte[] BuildDataPackage(NetMsg msg)
    {
        byte[] byte1 = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(msg.Header));
        byte[] byte5;
        if (msg.Command.Equals((short)NetPMSG.AMSG_PING))
        {
            byte5 = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)msg.Data));
        }
        else
        {
            byte5 = Encoding.UTF8.GetBytes(msg.Data.ToString());
        }
        msg.Crccode = NetMsg.getValidateCode(byte5);
        byte[] byte2 = new byte[NetMsg.CRC_LEN] { msg.Crccode };

        msg.DataLength = (short)(byte5.Length + NetMsg._HEADERLEN);
        byte[] byte3 = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(msg.DataLength));
        byte[] byte4 = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(msg.Command));

        byte[] totalByte = new byte[NetMsg.SIGN_LEN + NetMsg.CRC_LEN + NetMsg.COMMAND_LEN + NetMsg.DATA_LEN + byte5.Length];

        Buffer.BlockCopy(byte1, 0, totalByte, 0, byte1.Length);
        Buffer.BlockCopy(byte2, 0, totalByte, NetMsg.SIGN_LEN, byte2.Length);
        Buffer.BlockCopy(byte3, 0, totalByte, NetMsg.SIGN_LEN + NetMsg.CRC_LEN, byte3.Length);
        Buffer.BlockCopy(byte4, 0, totalByte, NetMsg.SIGN_LEN + NetMsg.CRC_LEN + NetMsg.DATA_LEN, byte4.Length);
        Buffer.BlockCopy(byte5, 0, totalByte, NetMsg._HEADERLEN, byte5.Length);
        //byte1.CopyTo(totalByte, 0);
        //byte2.CopyTo(totalByte, NetMsg.SIGN_LEN);
        //byte3.CopyTo(totalByte, NetMsg.SIGN_LEN + NetMsg.CRC_LEN);
        //byte4.CopyTo(totalByte, NetMsg.SIGN_LEN + NetMsg.CRC_LEN + NetMsg.DATA_LEN);
        //byte5.CopyTo(totalByte, NetMsg._HEADERLEN);
        //Debug.Log( "校验码:" + byte2[0] + " 包头 byte1:" + byte1.Length + " 校验 byte2:" + byte2.Length + " 消息长度 byte3:" + byte3.Length + " 命令 byte4:" + byte4.Length + " 消息体 byte5:" + byte5.Length);
        //StringBuilder sb = new StringBuilder();
        //foreach (var item in totalByte)
        //{
        //    sb.Append(item + ",");
        //}
        //DebugHelper.LogInfo(sb.ToString());

        return totalByte;
    }

    #region 数据转换方法

    private short readShort(NetworkStream mStream)
    {
        byte[] headData = new byte[NetMsg.COMMAND_LEN];
        mStream.Read(headData, 0, headData.Length);
        short msgType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(headData, 0));
        return msgType;
    }

    private byte readByte(NetworkStream mStream)
    {
        byte[] headData = new byte[NetMsg.CRC_LEN];
        mStream.Read(headData, 0, headData.Length);
        byte msgType = headData[0];
        return msgType;
    }

    private int readInt(NetworkStream mStream)
    {
        byte[] headData = new byte[NetMsg.SIGN_LEN];
        mStream.Read(headData, 0, headData.Length);
        int msgType = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(headData, 0));
        return msgType;
    }

    private string readString(short num, NetworkStream mStream)
    {
        byte[] headData = new byte[num];
        mStream.Read(headData, 0, headData.Length);
        string msgType = Encoding.UTF8.GetString(headData, 0, headData.Length);
        return msgType;
    }

    #endregion 数据转换方法

    public void Dispose()
    {

    }
}
