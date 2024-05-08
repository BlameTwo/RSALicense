using System.Text.Json.Serialization;

namespace CSharpLicense.Models;

/// <summary>
/// 通用许可证
/// </summary>
public class CurrencyLicense
{
    /// <summary>
    /// 许可证号
    /// </summary>
    [JsonPropertyName(nameof(LincenseID))]
    public string LincenseID { get; set; }

    /// <summary>
    /// 许可证创建时间
    /// </summary>
    [JsonPropertyName(nameof(BuildTime))]
    public DateTime BuildTime { get; set; }

    /// <summary>
    /// User 名称
    /// </summary>
    [JsonPropertyName(nameof(User))]
    public string User { get; set; }

    /// <summary>
    /// 其他附加参数
    /// </summary>
    [JsonPropertyName(nameof(Values))]
    public Dictionary<string,object> Values { get; set; }


    /// <summary>
    /// 许可证剩余可用时间
    /// </summary>
    [JsonPropertyName(nameof(LicenseTime))]
    public DateTime LicenseTime { get; set; }
}
