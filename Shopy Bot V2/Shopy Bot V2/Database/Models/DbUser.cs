namespace Lilia.Database.Models;

public class DbUser
{
    public ulong DbUserId { get; set; }
    public ulong DiscordUserId { get; set; }
    public string OsuMode { get; set; }
    public string OsuUsername { get; set; }
}