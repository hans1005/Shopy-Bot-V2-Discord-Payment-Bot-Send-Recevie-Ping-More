using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class GeneralBlacklist : CheckBaseAttribute
{
    public GeneralBlacklist() { }
    public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => Task.FromResult(ctx.Channel.Name != "general" || help);
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class DevOnly : CheckBaseAttribute
{
    public DevOnly() { }
    public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => Task.FromResult(ctx.User.Id == 388454632835514380);
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class DevModeOnly : CheckBaseAttribute
{
    public DevModeOnly() { }
    public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => Task.FromResult(!Discord.Program.prod);
}