using System.IO;
using System.Security.Cryptography;

public class MD5Util
{
    public static string getStringMd5(string str)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
        byte[] toData = md5.ComputeHash(data);
        md5.Clear();
        return FormatMD5(toData);
    }

    public static string getFileMd5(string url)
    {
        if (File.Exists(url) == false)
            return string.Empty;
        try
        {
            using (FileStream fileStream = File.OpenRead(url))
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] md5Data = md5.ComputeHash(fileStream);//计算指定Stream 对象的哈希值
                return FormatMD5(md5Data);
            }
        }
        catch (System.Exception)
        {
            return string.Empty;
        }
    }

    private static string FormatMD5(byte[] data)
    {
        return System.BitConverter.ToString(data).Replace("-", "").ToLower();//将byte[]装换成字符串
    }
}
