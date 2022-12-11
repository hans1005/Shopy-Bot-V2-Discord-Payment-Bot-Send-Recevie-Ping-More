namespace CardsAgainstDiscord.Extensions;

public static class UlongExtensions
{
    public static string AsUserMention(this ulong id) => $"<@!{id}>";
    
    public static string AsRoleMention(this ulong id) => $"<@&{id}>";
    
    public static string AsChannelMention(this ulong id) => $"<#{id}>";
}