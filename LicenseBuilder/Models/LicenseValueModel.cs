using System;
using System.Text.Json.Serialization;

namespace License.Models;

public class LicenseValueModel
{
    [JsonPropertyName("BuildTime")]
    public DateTime BuildTime { get; set; }

    [JsonPropertyName("ID")]
    public string ID { get; set; }

    [JsonPropertyName("LicenseTime")]
    public DateTimeOffset LicenseTime { get; set; }
}
