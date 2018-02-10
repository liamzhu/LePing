using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class EncryptUtil
{
    private static readonly string strkeyValue = "12345678901234567890198915689039";

    #region 加密解密

    /// <summary>
    /// DES 解密过程
    /// </summary>
    /// <param name="Message">待解密数据</param>
    /// <returns></returns>
    public static string Decrypt(string Message)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(strkeyValue);
        RijndaelManaged decipher = new RijndaelManaged();
        decipher.Key = keyArray;
        decipher.Mode = CipherMode.ECB;
        decipher.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = decipher.CreateDecryptor();
        byte[] _EncryptArray = Convert.FromBase64String(Message);
        byte[] resultArray = cTransform.TransformFinalBlock(_EncryptArray, 0, _EncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }

    /// <summary>
    /// DES 加密过程
    /// </summary>
    /// <param name="Message">待加密数据</param>
    /// <returns></returns>
    public static string Encrypt(string Message)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(strkeyValue);
        RijndaelManaged encryption = new RijndaelManaged();
        encryption.Key = keyArray;
        encryption.Mode = CipherMode.ECB;
        encryption.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = encryption.CreateEncryptor();
        byte[] _EncryptArray = UTF8Encoding.UTF8.GetBytes(Message);
        byte[] resultArray = cTransform.TransformFinalBlock(_EncryptArray, 0, _EncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    #endregion 加密解密

    private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

    /// <summary>
    /// DES加密字符串
    /// </summary>
    /// <param name="encryptString">待加密的字符串</param>
    /// <param name="encryptKey">加密密钥,要求为8位</param>
    /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
    public static string EncryptDES(string encryptString, string encryptKey)
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            cStream.Close();
            return Convert.ToBase64String(mStream.ToArray());
        }
        catch
        {
            return encryptString;
        }
    }

    /// <summary>
    /// DES解密字符串
    /// </summary>
    /// <param name="decryptString">待解密的字符串</param>
    /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
    /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
    public static string DecryptDES(string decryptString, string decryptKey)
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            cStream.Close();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        catch
        {
            return decryptString;
        }
    }
}
