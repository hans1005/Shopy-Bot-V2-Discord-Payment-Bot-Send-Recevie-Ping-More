using System.Text.Json.Serialization;
using Shared;

namespace Discord;
public class DiscordConfig : DefaultConfig
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("prod")]
    public bool Production { get; set; }
    [JsonPropertyName("prefix")]
    public string[] Prefix { get; set; }
    [JsonPropertyName("jacobubikey")]
    public string JacobUBIKey { get; set; }
}
