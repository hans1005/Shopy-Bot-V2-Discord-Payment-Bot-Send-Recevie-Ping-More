using System;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Lilia.Commons;
using Lilia.Services;

namespace Lilia.Modules;

public class GeneralModule : ApplicationCommandModule
{
    private LiliaClient _client;

    public GeneralModule(LiliaClient client)
    {
        this._client = client;
    }

    [SlashCommand("uptime", "Check the uptime of the bot")]
    public async Task UptimeCheckCommand(InteractionContext ctx)
    {
        await ctx.DeferAsync();

        DiscordEmbedBuilder embedBuilder = ctx.Member.GetDefaultEmbedTemplateForMember()
            .WithTitle("My uptime")
            .AddField("Uptime", $"{DateTime.Now.Subtract(this._client.StartTime):g}", true)
            .AddField("Start since", this._client.StartTime.ToLongDateString(), true);

        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(embedBuilder.Build()));
    }
    
    [SlashCommand("changes", "What's new in this version of \"me\"")]
    public async Task ViewChangelogsCommand(InteractionContext ctx)
    {
        await ctx.DeferAsync();

        DiscordEmbedBuilder embedBuilder = ctx.Member.GetDefaultEmbedTemplateForMember()
            .WithTitle("Here is the changelogs")
            .AddField("All versions", Formatter.MaskedUrl("Click me!", new Uri("https://github.com/Swyreee/Lilia/blob/master/CHANGELOGS.md") , "A GitHub link"), true)
            .AddField("Development version", Formatter.MaskedUrl("Click me!", new Uri("https://github.com/Swyreee/Lilia/blob/master/CHANGELOG.md"), "A GitHub link"), true);

        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(embedBuilder.Build()));
    }

    [SlashCommand("info", "Something about me")]
    public async Task BotInfoCommand(InteractionContext ctx)
    {
        await ctx.DeferAsync(true);

        StringBuilder owners = new();

        ulong memberCount = 0;

        this._client.JoinedGuilds.ForEach(guild => memberCount += Convert.ToUInt64(guild.MemberCount));

        foreach (DiscordUser owner in ctx.Client.CurrentApplication.Owners)
        {
            owners.AppendLine($"{owner.Username}#{owner.Discriminator}");
        }

        TimeSpan timeDiff = DateTime.Now.Subtract(this._client.StartTime);

        bool isValidLink = true;

        DiscordLinkButtonComponent inviteBtn = new DiscordLinkButtonComponent(this._client.BotConfiguration.Client.BotInviteLink, "Invite me!", isValidLink);

        DiscordEmbedBuilder embedBuilder = ctx.Member.GetDefaultEmbedTemplateForMember()
            .WithTitle("Something about me :D")
            .WithThumbnail(ctx.Client.CurrentUser.AvatarUrl)
            .WithDescription($"Hi, I am {ctx.Client.CurrentUser.Username}#{ctx.Client.CurrentUser.Discriminator}, a bot runs under the source code of {Formatter.MaskedUrl("Lilia", new Uri("https://github.com/Swyreee/Lilia"))} made by Swyrin#7193")
            .AddField("Servers count", this._client.JoinedGuilds.Count.ToString(), true)
            .AddField("Members count", memberCount.ToString(), true)
            .AddField("Owners", owners.ToString(), true)
            .AddField("Uptime (d.h\\:m\\:s)", timeDiff.ToString(@"dd\.hh\:mm\:ss"), true)
            .AddField("Start since", this._client.StartTime.ToLongDateString() + " " + this._client.StartTime.ToLongTimeString(), true);

        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(embedBuilder.Build()));
    }
}