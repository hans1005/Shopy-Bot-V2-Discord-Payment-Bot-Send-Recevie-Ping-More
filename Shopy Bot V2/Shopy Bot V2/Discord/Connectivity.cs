using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using Shared;
using System.Linq;
using Shared.Models;

namespace Discord.Commands;

[Group("register")]
public class Connectivity : BaseCommandModule
{
    private static Random random = new();
    private const string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890-_~";
    public CocaBotWebContext db { private get; set; }

    [GroupCommand, Aliases("reg", "login")]
    [Description("Gives link for linking your SV account to your discord account")]
    public async Task Register(CommandContext ctx) => Sudo(ctx, ctx.Member);

    [Command("sudo"), Description("Executes a command as another user."), Hidden, DevOnly]
    public async Task Sudo(CommandContext ctx, [Description("Member to execute as.")] DiscordMember member)
    {
        string key = new(Enumerable.Repeat(chars, 20).Select(s => s[random.Next(s.Length)]).ToArray());

        DiscordDmChannel dms;
        try
        {
            dms = await member.CreateDmChannelAsync();
        }
        catch (NullReferenceException)
        {
            ctx.RespondAsync("Cannot do this command in dms!");
            return;
        }
        try
        {
            await dms.SendMessageAsync("https://cocabot.cf/login?key=" + key);
        }
        catch (DSharpPlus.Exceptions.UnauthorizedException)
        {
            ctx.RespondAsync("Please enable direct messages from server members in the server privacy seetings!");
            return;
        }
        await ctx.RespondAsync("Look at your DM!");

        Register register = db.Registers.Where(x => x.Discord == member.Id).SingleOrDefault();
        if (register != null) db.Registers.Remove(register);

        register = new();
        register.VerifKey = key;
        register.Discord = member.Id;

        await db.Registers.AddAsync(register);
        await db.SaveChangesAsync();
    }
}