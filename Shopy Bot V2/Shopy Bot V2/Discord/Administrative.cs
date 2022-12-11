using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Discord.Commands;
public class Administrative : BaseCommandModule
{
    [Command("nick"), Hidden, DevOnly]
    public async Task Nick(CommandContext ctx, [RemainingText, Description("The nickname to give to that user.")] string new_nickname)
    {
        var member = await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id);

        await member.ModifyAsync(x =>
        {
            x.Nickname = new_nickname;
            x.AuditLogReason = $"Changed by {ctx.User.Username} ({ctx.User.Id}).";
        });
    }

    [Command("leave"), Hidden, RequireOwner, DevOnly, DevModeOnly]
    public async Task Leave(CommandContext ctx, ulong id)
    {
        var guild = await ctx.Client.GetGuildAsync(id);
        await guild.LeaveAsync();
    }

    [Command("guilds"), Hidden, DevOnly]
    public async Task Guilds(CommandContext ctx)
    {
        foreach (var item in ctx.Client.Guilds) Console.WriteLine(item.Value.Name + " " + item.Key);
    }

    [Command("sudo"), Description("Executes a command as another user."), Hidden, RequireOwner, DevOnly, DevModeOnly]
    public async Task Sudo(CommandContext ctx, 
        [Description("Member to execute as.")] DiscordMember member, 
        [RemainingText, Description("Command text to execute.")] string command)
    {
        if (ctx.Channel.Id != 850578492185509898) return;

        // note the [RemainingText] attribute on the argument.
        // it will capture all the text passed to the command

        await ExecuteCommand(ctx, member, command);
    }

    public static async Task ExecuteCommand(CommandContext ctx, DiscordMember member, string command)
    {
        // let's trigger a typing indicator to let
        // users know we're working
        await ctx.TriggerTypingAsync();

        // get the command service, we need this for
        // sudo purposes
        var cmds = ctx.CommandsNext;

        // retrieve the command and its arguments from the given string
        var cmd = cmds.FindCommand(command, out var customArgs);

        // create a fake CommandContext
        var fakeContext = cmds.CreateFakeContext(member, ctx.Channel, command, ctx.Prefix, cmd, customArgs);

        // and perform the sudo
        await cmds.ExecuteCommandAsync(fakeContext);
    }
}