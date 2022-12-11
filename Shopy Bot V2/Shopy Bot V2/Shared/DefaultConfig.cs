using System.Text.Json.Serialization;

namespace Shared;
public class DefaultConfig
{
    [JsonPropertyName("server")]
    public string Server { get; set; }
    [JsonPropertyName("userid")]
    public string UserID { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
    [JsonPropertyName("database")]
    public string Database { get; set; }
    [JsonPropertyName("oauth_secret")]
    public string OauthSecret { get; set; }
}
