using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

public class DataPacket
{
    public PackHeader Header;
    public byte[] Data = null;
    public int Size { get; private set; }
    public object mData;

    public DataPacket()
    {
    }

    public DataPacket(PackHeader packer, byte[] data, int size)
    {
        this.Header = packer;
        this.Data = data;
        this.Size = size;
    }

    public byte[] ToBytes(StructCopyer mStructCopyer)
    {
        byte[] totalByte = new byte[Size];
        Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Header.HeadSign)), 0, totalByte, 0, 4);
        Buffer.BlockCopy(new byte[1] { Header.Crc }, 0, totalByte, 4, 1);
        Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Header.DataSize)), 0, totalByte, 5, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Header.Command)), 0, totalByte, 7, 2);
        //Buffer.BlockCopy(mStructCopyer.StructToBytes(Header), 0, totalByte, 0, PacketConst.HeaderLen);
        Buffer.BlockCopy(Data, 0, totalByte, PacketConst.HeaderLen, Data.Length);
        return totalByte;
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct PackHeader : IEquatable<PackHeader>
{
    public int HeadSign;
    public byte Crc;
    public short DataSize;
    public short Command;

    public void Pack(int head, byte crc, short size, short command)
    {
        this.HeadSign = head;
        this.Crc = crc;
        this.DataSize = size;
        this.Command = command;
    }

    public bool Equals(PackHeader other)
    {
        return (HeadSign == other.HeadSign) && (Crc == other.Crc) && (DataSize == other.DataSize) && (Command == other.Command);
    }
}

public class PacketConst
{
    public const int SIGN = -1;//头部标示
    public const int HeaderLen = 9; // 协议头字节长度
    public const byte InitCode = 0x64;
}

public static class PacketExtension
{
    /// <summary>
    /// 得到验证编码
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public static byte getValidateCode(this byte[] b)
    {
        byte total = 0;
        int i = 0; // 从数据byte[] 第0位开始
        int len = b.Length;
        while (i < len)
        {
            total += b[i++]; // byte[] 数据流
        }
        return (byte)(PacketConst.InitCode ^ total);
    }

    public static byte[] ToBytes(this string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static byte[] ToBytes(this int str)
    {
        return BitConverter.GetBytes(IPAddress.HostToNetworkOrder(str));
    }
}
