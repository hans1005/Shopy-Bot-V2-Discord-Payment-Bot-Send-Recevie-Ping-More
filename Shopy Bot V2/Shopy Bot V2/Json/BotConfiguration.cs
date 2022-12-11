using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lilia.Json;

public class BotConfiguration : BaseJson
{
    public BotConfiguration()
    {
        FilePath = "config.json";
    }

    [JsonProperty("client")] public ClientData Client = new();

    [JsonProperty("credentials")] public CredentialsData Credentials = new();
}

public class ClientData
{
    [JsonProperty("private_guilds")] public List<ulong> PrivateGuildIds = new();

    [JsonProperty("client_invite_link")] public string BotInviteLink = "";

    [JsonProperty("support_server_invite_link")] public string SupportServerInviteLink = "";

    [JsonProperty("slash_commands_for_global")] public bool SlashCommandsForGlobal = true;

    [JsonProperty("activity")] public ClientActivityData Activity = new();
}

public class ClientActivityData
{
    [JsonProperty("type")] public string Type = "Watching";

    [JsonProperty("name")] public string Name = "you";

    [JsonProperty("status")] public string Status = "DoNotDisturb";
}

public class CredentialsData
{
    [JsonProperty("discord_token")] public string DiscordToken;

    [JsonProperty("db_password")] public string DbPassword = "thisisliterallynotapassword";

    [JsonProperty("osu")] public OsuData Osu = new();
}

public class OsuData
{
    [JsonProperty("client_id")] public long ClientId;
    
    [JsonProperty("client_secret")] public string ClientSecret;
}