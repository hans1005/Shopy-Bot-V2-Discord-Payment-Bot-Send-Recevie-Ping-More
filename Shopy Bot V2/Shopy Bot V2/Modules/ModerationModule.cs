using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Lilia.Commons;
using Lilia.Database;
using Lilia.Services;

namespace Lilia.Modules;

[SlashCommandGroup("mod", "Moderation commands")]
public class ModerationModule : ApplicationCommandModule
{
    [SlashCommandGroup("general", "General command for moderating members")]
    public class ModerationGeneralModule : ApplicationCommandModule
    {
        private LiliaClient _client;
        private LiliaDatabaseContext _dbCtx;

        public ModerationGeneralModule(LiliaClient client)
        {
            this._client = client;
            this._dbCtx = client.Database.GetContext();
        }
        
        [SlashCommand("ban", "Ban members in batch")]
        [SlashRequirePermissions(Permissions.BanMembers)]
        public async Task BanMembersCommand(InteractionContext ctx,
            [Option("reason", "Reason to ban")] string reason = "Rule violation")
        {
            await ctx.DeferAsync();

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent($"{Formatter.Bold("Mention")} all the people you want to ban"));

            InteractivityExtension interactivity = ctx.Client.GetInteractivity();

            var res = await interactivity.WaitForMessageAsync(x => x.MentionedUsers.Any(), TimeSpan.FromMinutes(5));

            if (res.TimedOut)
            {
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                    .WithContent("Timed out"));

                return;
            }

            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent("Banning mischievous people"));

            StringBuilder stringBuilder = new();

            foreach (DiscordUser user in res.Result.MentionedUsers)
            {
                DiscordMember member = (DiscordMember) user;

                if (member == ctx.Member)
                {
                    stringBuilder.AppendLine("Beaned you");
                }
                else
                {
                    await ctx.Guild.BanMemberAsync(member, 0, $"Banned by {ctx.Member.DisplayName}#{ctx.Member.Discriminator} -> Reason: {reason}");
                    stringBuilder.AppendLine($"Banned {member.DisplayName}#{member.Discriminator}");
                }
            }

            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent(stringBuilder.ToString()));
        }

        [SlashCommand("kick", "Kick members in batch")]
        [SlashRequirePermissions(Permissions.KickMembers)]
        public async Task KickMembersCommand(InteractionContext ctx,
            [Option("reason", "Reason to kick")] string reason = "Rule violation")
        {
            await ctx.DeferAsync();

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent($"{Formatter.Bold("Mention")} all the people you want to kick"));

            InteractivityExtension interactivity = ctx.Client.GetInteractivity();

            var res = await interactivity.WaitForMessageAsync(x => x.MentionedUsers.Any(), TimeSpan.FromMinutes(5));

            if (res.TimedOut)
            {
                await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                    .WithContent("Timed out"));

                return;
            }

            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent("Kicking mischievous people"));

            StringBuilder stringBuilder = new();

            foreach (DiscordUser user in res.Result.MentionedUsers)
            {
                DiscordMember member = (DiscordMember) user;

                if (member == ctx.Member)
                {
                    stringBuilder.AppendLine("Skipped you");
                }
                else
                {
                    await ctx.Guild.BanMemberAsync(member, 0, $"Kicked by {ctx.Member.DisplayName}#{ctx.Member.Discriminator} -> Reason: {reason}");
                    stringBuilder.AppendLine($"Kicked {member.DisplayName}#{member.Discriminator}");
                }
            }

            await ctx.FollowUpAsync(new DiscordFollowupMessageBuilder()
                .WithContent(stringBuilder.ToString()));
        }
    }

    [SlashCommandGroup("notice", "Commands for sending global notices")]
    public class ModerationNoticeModule : ApplicationCommandModule
    {
        [SlashCommand("send", "Send a notification to a channel")]
        [SlashRequirePermissions(Permissions.ManageGuild)]
        public async Task SendNoticeCommand(InteractionContext ctx,
            [Option("target_message_jump_link", "Message jump link to copy, can be from other guild, at least as I am in there")]
            string msgJump,
            [ChannelTypes(ChannelType.Text, ChannelType.News, ChannelType.Store, ChannelType.NewsThread, ChannelType.PublicThread)]
            [Option("target_channel", "Channel to send")]
            DiscordChannel channel)
        {
            await ctx.DeferAsync();

            try
            {
                var resolved = msgJump.ResolveDiscordMessageJumpLink();

                DiscordGuild guild = await ctx.Client.GetGuildAsync(resolved.Item1);
                DiscordChannel chn = guild.GetChannel(resolved.Item2);
                DiscordMessage msg = await chn.GetMessageAsync(resolved.Item3);
                DiscordMessage noticeMsg = await channel.SendMessageAsync(msg.Content);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(ctx.Member.GetDefaultEmbedTemplateForMember()
                        .WithTitle("Notice sent")
                        .AddField("Jump link", Formatter.MaskedUrl("Click me!", noticeMsg.JumpLink))));
            }
            catch (Exception)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("Invalid jump link provided"));
            }
        }

        [SlashCommand("edit", "Edit an existing notification sent by me")]
        [SlashRequirePermissions(Permissions.ManageGuild)]
        public async Task EditNoticeCommand(InteractionContext ctx,
            [Option("old_message_jump_link", "Old message jump link, can be from other guild, at least as I am in there")]
            string msgJumpOld,
            [Option("new_message_jump_link", "New message jump link, can be from other guild, at least as I am in there")]
            string msgJumpNew
        )
        {
            await ctx.DeferAsync();

            try
            {
                var resolved1 = msgJumpOld.ResolveDiscordMessageJumpLink();
                var resolved2 = msgJumpNew.ResolveDiscordMessageJumpLink();
                
                DiscordGuild g1 = await ctx.Client.GetGuildAsync(resolved1.Item1);
                DiscordGuild g2 = await ctx.Client.GetGuildAsync(resolved2.Item1);
                DiscordChannel c1 = g1.GetChannel(resolved1.Item2);
                DiscordChannel c2 = g2.GetChannel(resolved2.Item2);
                
                DiscordMessage oldMsg = await c1.GetMessageAsync(resolved1.Item3);
                DiscordMessage newMsg = await c2.GetMessageAsync(resolved2.Item3);

                await oldMsg.ModifyAsync(newMsg.Content);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("Edited the notification"));
            }
            catch (Exception)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("Invalid jump links provided"));
            }
        }

        [SlashCommand("copy", "Copy a notification sent by me (with formats)")]
        [SlashRequirePermissions(Permissions.ManageGuild)]
        public async Task CopyNoticeCommand(InteractionContext ctx,
            [Option("target_message_jump_link", "Message jump link to copy, can be from other guild, at least as I am in there")]
            string msgJump)
        {
            await ctx.DeferAsync();

            try
            {
                var resolved = msgJump.ResolveDiscordMessageJumpLink();

                DiscordGuild guild = await ctx.Client.GetGuildAsync(resolved.Item1);
                DiscordChannel chn = guild.GetChannel(resolved.Item2);
                DiscordMessage msg = await chn.GetMessageAsync(resolved.Item3);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(ctx.Member.GetDefaultEmbedTemplateForMember()
                        .WithTitle("Notice received")
                        .WithDescription(msg.Content)
                        .AddField("Jump link", Formatter.MaskedUrl("Click me!", msg.JumpLink))));
            }
            catch (Exception)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("Invalid jump link provided"));
            }
        }
    }
}