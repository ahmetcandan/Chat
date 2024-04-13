using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Chat.Core.Cryptography
{
    public static class RSA
    {
        public static string Decrypt(this string data, string privateKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            return Encoding.UTF8.GetString(rsa.Decrypt(data.Split(',').Select(c => Convert.ToByte(c)).ToArray(), false));
        }

        public static string Encrypt(this string data, string publicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            return string.Join(",", rsa.Encrypt(Encoding.UTF8.GetBytes(data), false).ToArray());
        }

        public static byte[] _Decrypt(this byte[] data, string privateKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            return rsa.Decrypt(data, false);
        }

        public static byte[] _Encrypt(this byte[] data, string publicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            return rsa.Encrypt(data, false).ToArray();
        }

        public static byte[] ByteArray(this string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        public static string ByteArrayToString(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }
    }
}
