using CSharpLicense.Models;
using System.Security.Cryptography;
using System.Text.Json;

namespace CSharpLicense.Helpers;

public class VerifySignHelper
{
    /// <summary>
    /// 构造函数，只传入公钥，在客户机使用
    /// </summary>
    /// <param name="publicKey"></param>
    public VerifySignHelper(string publicKey)
    {
        PublicKey = publicKey;
    }

    public string PublicKey { get; }


    /// <summary>
    /// 进行校验并获取参数类型，这里只获取校验签名的完整性，而不是去检查该许可证是否可用
    /// </summary>
    /// <typeparam name="T">签名时携带数据</typeparam>
    /// <param name="plainText">客户机校验原数据</param>
    /// <param name="signatureBase64">签名数据</param>
    /// <returns></returns>
    public async Task<T?> VerifySignatureAsync<T>(string signatureBase64)
        where T : CurrencyLicense
    {
        return await Task.Run(()=>{
            var rsa = new RSACryptoServiceProvider();
            try
            {
                rsa.FromXmlString(PublicKey);
            }
            catch (ArgumentException)
            {
                return null;
            }
            if (!rsa.PublicOnly)
                return null;
            byte[] licenseDecode;
            try
            {
                licenseDecode = Convert.FromBase64String(signatureBase64);
            }
            catch (Exception e) when (e is FormatException or ArgumentNullException)
            {
                return null;
            }
            //按照移位将签名与数据分开
            var dataLen = (licenseDecode[0] << 8) + licenseDecode[1];
            byte[] data = new byte[dataLen];
            for (int i = 0; i < dataLen; i++)
            {
                data[i] = licenseDecode[i + 2];
            }
            int signatureStartIndex = dataLen + 2;
            byte[] dataSigned = new byte[licenseDecode.Length - signatureStartIndex];
            for (int i = 0; i < dataSigned.Length; i++)
            {
                dataSigned[i] = licenseDecode[signatureStartIndex + i];
            }
            if (!rsa.VerifyData(data, new SHA1CryptoServiceProvider(), dataSigned))
            {
                return null;
            }
            //序列化，只能是Json
            var dataEntity = JsonSerializer.Deserialize<T>(data);
            return dataEntity;
        });
        
    }
}
