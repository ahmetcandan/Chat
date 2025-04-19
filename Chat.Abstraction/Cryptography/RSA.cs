using System.Security.Cryptography;
using System.Text;

namespace Chat.Abstraction.Cryptography;

public static class RSA
{
    public static string Decrypt(this byte[] data, string privateKey)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(privateKey);
        return Encoding.UTF8.GetString(rsa.Decrypt(data, false));
    }

    public static byte[] Encrypt(this string message, string publicKey)
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(publicKey);
        return [.. rsa.Encrypt(Encoding.UTF8.GetBytes(message), false)];
    }
}
