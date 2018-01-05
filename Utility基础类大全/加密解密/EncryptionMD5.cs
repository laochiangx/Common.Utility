using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Common.Utilities
{
    /// <summary>
    /// MD5
    /// 单向加密
    /// </summary>
    public class EncryptionMD5
    {
        /// <summary>
        /// 获得一个字符串的加密密文
        /// 此密文为单向加密，即不可逆(解密)密文
        /// </summary>
        /// <param name="plainText">待加密明文</param>
        /// <returns>已加密密文</returns>
        public static string EncryptString(string plainText)
        {
            return EncryptStringMD5(plainText);
        }
        /// <summary>
        /// 获得一个字符串的加密密文
        /// 此密文为单向加密，即不可逆(解密)密文
        /// </summary>
        /// <param name="plainText">待加密明文</param>
        /// <returns>已加密密文</returns>
        public static string EncryptStringMD5(string plainText)
        {
            string encryptText = "";
            if (string.IsNullOrEmpty(plainText)) return encryptText;
            encryptText = FormsAuthentication.HashPasswordForStoringInConfigFile(plainText, "md5");
            return encryptText;
        }
        /// <summary>
        /// 判断明文与密文是否相符
        /// </summary>
        /// <param name="plainText">待检查的明文</param>
        /// <param name="encryptText">待检查的密文</param>
        /// <returns>bool</returns>
        public static bool EqualEncryptString(string plainText, string encryptText)
        {
            return EqualEncryptStringMD5(plainText, encryptText);
        }
        /// <summary>
        /// 判断明文与密文是否相符
        /// </summary>
        /// <param name="plainText">待检查的明文</param>
        /// <param name="encryptText">待检查的密文</param>
        /// <returns>bool</returns>
        public static bool EqualEncryptStringMD5(string plainText, string encryptText)
        {
            bool result = false;
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(encryptText))
                return result;
            result = EncryptStringMD5(plainText).Equals(encryptText);
            return result;
        }
    }
    /// <summary>
    /// SHA1
    /// 单向加密
    /// </summary>
    public class EncryptionSHA1
    {
        /// <summary>
        /// 获得一个字符串的加密密文
        /// 此密文为单向加密，即不可逆(解密)密文
        /// </summary>
        /// <param name="plainText">待加密明文</param>
        /// <returns>已加密密文</returns>
        public static string EncryptString(string plainText)
        {
            return EncryptStringSHA1(plainText);
        }
        /// <summary>
        /// 获得一个字符串的加密密文
        /// 此密文为单向加密，即不可逆(解密)密文
        /// </summary>
        /// <param name="plainText">待加密明文</param>
        /// <returns>已加密密文</returns>
        public static string EncryptStringSHA1(string plainText)
        {
            string encryptText = "";
            if (string.IsNullOrEmpty(plainText)) return encryptText;
            encryptText = FormsAuthentication.HashPasswordForStoringInConfigFile(plainText, "sha1");
            return encryptText;
        }
        /// <summary>
        /// 判断明文与密文是否相符
        /// </summary>
        /// <param name="plainText">待检查的明文</param>
        /// <param name="encryptText">待检查的密文</param>
        /// <returns>bool</returns>
        public static bool EqualEncryptString(string plainText, string encryptText)
        {
            return EqualEncryptStringSHA1(plainText, encryptText);
        }
        /// <summary>
        /// 判断明文与密文是否相符
        /// </summary>
        /// <param name="plainText">待检查的明文</param>
        /// <param name="encryptText">待检查的密文</param>
        /// <returns>bool</returns>
        public static bool EqualEncryptStringSHA1(string plainText, string encryptText)
        {
            bool result = false;
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(encryptText))
                return result;
            result = EncryptStringSHA1(plainText).Equals(encryptText);
            return result;
        }
    }
    /// <summary>
    /// DES
    /// 双向，可解密
    /// </summary>
    public class EncryptionDES
    {
        /// <summary>
        /// 获得一个字符串的加密密文        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <returns>密文字符串</returns>
        public static string EncryptStringReverse(string plainText)
        {
            return EncryptStringReverse(plainText, VariableName.DefaultEncryptKey);
        }

        /// <summary>
        /// 获得一个字符串的加密密文        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <param name="key">加密/解密密钥</param>
        /// <returns>密文字符串</returns>
        public static string EncryptStringReverse(string plainText, string key)
        {
            string result = "";
            if (string.IsNullOrEmpty(plainText)) return result;

            try
            {
                //如无，取默认值
                string keyStr = string.IsNullOrEmpty(key) ? VariableName.DefaultEncryptKey : key;

                //明文
                //byte[] srcData = System.Text.ASCIIEncoding.ASCII.GetBytes(plainText);
                byte[] srcData = UnicodeEncoding.Unicode.GetBytes(plainText);

                MemoryStream sin = new MemoryStream();
                //将明文写入内存
                sin.Write(srcData, 0, srcData.Length);
                sin.Position = 0;

                MemoryStream sout = new MemoryStream();

                DES des = new DESCryptoServiceProvider();

                //得到密钥
                string sTemp;
                if (des.LegalKeySizes.Length > 0)
                {
                    int lessSize = 0, moreSize = des.LegalKeySizes[0].MinSize;

                    while (keyStr.Length * 8 > moreSize &&
                        des.LegalKeySizes[0].SkipSize > 0 &&
                        moreSize < des.LegalKeySizes[0].MaxSize)
                    {
                        lessSize = moreSize;
                        moreSize += des.LegalKeySizes[0].SkipSize;
                    }

                    if (keyStr.Length * 8 > moreSize)
                        sTemp = keyStr.Substring(0, (moreSize / 8));
                    else
                        sTemp = keyStr.PadRight(moreSize / 8, ' ');
                }
                else
                    sTemp = keyStr;

                //设置密钥
                des.Key = ASCIIEncoding.ASCII.GetBytes(sTemp);


                //设置初始化向量
                if (keyStr.Length > des.IV.Length)
                {
                    des.IV = ASCIIEncoding.ASCII.GetBytes(keyStr.Substring(0, des.IV.Length));
                }
                else
                {
                    des.IV = ASCIIEncoding.ASCII.GetBytes(keyStr.PadRight(des.IV.Length, ' '));
                }

                //加密流
                CryptoStream encStream = new CryptoStream(sout, des.CreateEncryptor(), CryptoStreamMode.Write);

                //明文流程的长度
                long lLen = sin.Length;
                //已经读取长度
                int nReadTotal = 0;

                //读入块
                byte[] buf = new byte[8];

                int nRead;

                //从明文流读到加密流中
                while (nReadTotal < lLen)
                {
                    nRead = sin.Read(buf, 0, buf.Length);
                    encStream.Write(buf, 0, nRead);
                    nReadTotal += nRead;
                }
                encStream.Close();

                //密文
                result = System.Convert.ToBase64String(sout.ToArray());
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 对加密密文进行解密
        /// </summary>
        /// <param name="encryptText">待解密的密文</param>
        /// <returns>明文字符串</returns>
        public static string DecryptStringReverse(string encryptText)
        {
            return DecryptStringReverse(encryptText, VariableName.DefaultEncryptKey);
        }

        /// <summary>
        /// 对加密密文进行解密
        /// </summary>
        /// <param name="encryptText">待解密的密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文字符串</returns>
        public static string DecryptStringReverse(string encryptText, string key)
        {
            string result = "";
            if (string.IsNullOrEmpty(encryptText)) return result;

            try
            {
                //如无，取默认值
                string keyStr = string.IsNullOrEmpty(key) ? VariableName.DefaultEncryptKey : key;

                //密文
                byte[] encData = System.Convert.FromBase64String(encryptText);

                //将密文写入内存
                MemoryStream sin = new MemoryStream(encData);

                MemoryStream sout = new MemoryStream();

                DES des = new DESCryptoServiceProvider();

                //得到密钥
                string sTemp;
                if (des.LegalKeySizes.Length > 0)
                {
                    int lessSize = 0, moreSize = des.LegalKeySizes[0].MinSize;

                    while (keyStr.Length * 8 > moreSize &&
                        des.LegalKeySizes[0].SkipSize > 0 &&
                        moreSize < des.LegalKeySizes[0].MaxSize)
                    {
                        lessSize = moreSize;
                        moreSize += des.LegalKeySizes[0].SkipSize;
                    }

                    if (keyStr.Length * 8 > moreSize)
                        sTemp = keyStr.Substring(0, (moreSize / 8));
                    else
                        sTemp = keyStr.PadRight(moreSize / 8, ' ');
                }
                else
                    sTemp = keyStr;

                //设置密钥
                des.Key = ASCIIEncoding.ASCII.GetBytes(sTemp);


                //设置初始化向量
                if (keyStr.Length > des.IV.Length)
                {
                    des.IV = ASCIIEncoding.ASCII.GetBytes(keyStr.Substring(0, des.IV.Length));
                }
                else
                {
                    des.IV = ASCIIEncoding.ASCII.GetBytes(keyStr.PadRight(des.IV.Length, ' '));
                }

                //解密流
                CryptoStream decStream = new CryptoStream(sin, des.CreateDecryptor(), CryptoStreamMode.Read);

                //密文流的长度
                long lLen = sin.Length;
                //已经读取长度
                int nReadTotal = 0;

                //读入块
                byte[] buf = new byte[8];

                int nRead;

                //从密文流读到解密流中
                while (nReadTotal < lLen)
                {
                    nRead = decStream.Read(buf, 0, buf.Length);
                    if (0 == nRead) break;

                    sout.Write(buf, 0, nRead);
                    nReadTotal += nRead;
                }
                decStream.Close();

                //明文
                //ASCIIEncoding ascEnc = new ASCIIEncoding();
                UnicodeEncoding ascEnc = new UnicodeEncoding();
                result = ascEnc.GetString(sout.ToArray());
            }
            catch { }

            return result;
        }
    }
}
