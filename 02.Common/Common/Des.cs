using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Des
    {
        private static readonly Encoding Encoding = Encoding.GetEncoding("gbk");
        public const string KeyString = "12345678";
        public const string QDT_Key = "jUaxcR8K";

        public static string DesEncrypt(string toEncryptString, string keyStr)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                byte[] toEncryptBytes = Encoding.GetBytes(toEncryptString);
                des.Key = Encoding.GetBytes(keyStr);
                des.IV = Encoding.GetBytes(keyStr);
                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(toEncryptBytes, 0, toEncryptBytes.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string encryptedString = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return encryptedString;
            }
        }

        public static string DesDecrypt(string toDecryptString, string keyStr)
        {
            byte[] toDecryptBytes = Convert.FromBase64String(toDecryptString);
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.GetBytes(keyStr);
                des.IV = Encoding.GetBytes(keyStr);
                var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(toDecryptBytes, 0, toDecryptBytes.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string decryptedString = Encoding.GetString(ms.ToArray());
                ms.Close();
                return decryptedString;
            }
        }

    }
}
