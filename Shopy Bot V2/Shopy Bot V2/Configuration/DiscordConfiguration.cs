namespace CardsAgainstDiscord.Configuration;

public class DiscordConfiguration
{
    public const string Section = "Discord";

    /// <summary>
    ///     Discord token used for gateway authentication
    /// </summary>
    public string Token { get; init; } = null!;

    /// <summary>
    ///     Id of the testing guild to locally register slash commands to
    /// </summary>
    public ulong? GuildId { get; init; } = null;
}