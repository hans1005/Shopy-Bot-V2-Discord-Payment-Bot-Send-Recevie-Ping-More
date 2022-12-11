using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lilia.Commons
{
    public static class LiliaUtilities
    {
        public static DiscordEmbedBuilder GetDefaultEmbedTemplateForMember(this DiscordMember member)
        {
            return new DiscordEmbedBuilder()
                .WithTimestamp(DateTime.Now)
                .WithColor(DiscordColor.Rose)
                .WithFooter($"Requested by: {member.DisplayName}#{member.Discriminator}", member.AvatarUrl);
        }

        public static Tuple<ulong, ulong, ulong> ResolveDiscordMessageJumpLink(this string str)
        {
            // https://discord.com/channels/guild_id/channel_id/message_id
            Uri link = new Uri(str);
            List<string> segments = link.Segments.ToList();

            ulong guildId = Convert.ToUInt64(segments[2].Replace('/', '\0'));
            ulong channelId = Convert.ToUInt64(segments[3].Replace('/', '\0'));
            ulong messageId = Convert.ToUInt64(segments[4].Replace('/', '\0'));
            return new Tuple<ulong, ulong, ulong>(guildId, channelId, messageId);
        }

        public static bool IsDiscordValidBotInvite(this string str)
        {
            Regex botInvRegex = new Regex(@"(https?:\/\/)?(www\.|canary\.|ptb\.)?discord(app)?\.com\/(api\/)?oauth2\/authorize\?([^ ]+)\/?");
            return !string.IsNullOrWhiteSpace(str) && botInvRegex.IsMatch(str);
        }

        public static bool IsDiscordValidGuildInvite(this string str)
        {
            Regex guildInvRegex = new Regex(@"(https?:\/\/)?(www\.|canary\.|ptb\.)?discord(\.gg|(app)?\.com\/invite|\.me)\/([^ ]+)\/?");
            return !string.IsNullOrWhiteSpace(str) && guildInvRegex.IsMatch(str);
        }
    }
}