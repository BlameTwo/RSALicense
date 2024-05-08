using CSharpLicense.Models;
using System.Security.Cryptography;
using System.Text.Json;

namespace CSharpLicense.Helpers;

public class SignHelper
{
    public SignHelper(string privateKey,string publicKey)
    {
        PrivateKey = privateKey;
        PublicKey = publicKey;
    }

    public SignHelper()
    {
        //默认生成
        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = 2048;
            //取出私钥和公钥
            var privateKey = rsa.ToXmlString(true);
            var publicKey = rsa.ToXmlString(false);
            this.PrivateKey = privateKey;
            this.PublicKey = publicKey;
        }
    }

    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }

    /// <summary>
    /// 对数据进行签名
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="param">数据对象</param>
    /// <returns></returns>
    public async Task<string?> SignDataAsync<T>(T param)
       where T : CurrencyLicense
    {
        try
        {
            return await Task.Run(() =>
            {
                //将对象转换成 string UTF8 Bytes 格式
                var data = JsonSerializer.SerializeToUtf8Bytes(param);
                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(PrivateKey);
                // 计算数据长度，并将其转换为2个字节表示，高位在前
                var dataLen = data.Length;
                var dataLenByte = new byte[] { (byte)(dataLen >> 8), (byte)dataLen};
                // 使用SHA1哈希算法对数据进行签名
                var dataSigned = rsa.SignData(data, new SHA1CryptoServiceProvider());
                // 声明一个数据，准备合并签名与原数据
                var dataCombined = new byte[dataLenByte.Length + data.Length + dataSigned.Length];
                // 复制数据长度到组合数组的起始位置
                dataLenByte.CopyTo(dataCombined, 0);
                // 在长度之后复制原始数据
                data.CopyTo(dataCombined, dataLenByte.Length);
                // 在原始数据之后复制签名结果
                dataSigned.CopyTo(dataCombined, dataLenByte.Length + data.Length);
                /*
                 签名是保证原数据的完整性，也就是将要进行校验的的数据是否与私钥签名的数据完全一致（Byte）
                 保存数据是为了防止信息被篡改。
                 实际上签名原数据就是附加的数据，为了保证一致性。
                 */
                var License = Convert.ToBase64String(dataCombined);
                return License;
            });
        }
        catch (Exception ex)
        {
            return null;
        }
    }


}