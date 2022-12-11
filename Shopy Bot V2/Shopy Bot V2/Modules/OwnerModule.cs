using System.Text;
using Lilia.Services;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Serilog;

namespace Lilia.Modules;

public class OwnerModule : ApplicationCommandModule
{
    private LiliaClient _client;

    public OwnerModule(LiliaClient client)
    {
        this._client = client;
    }

    [SlashCommand("shutdown", "Shutdown the bot")]
    [SlashRequireOwner]
    public async Task ShutdownCommand(InteractionContext ctx)
    {
        await ctx.DeferAsync();

        Log.Logger.Warning("Shutting down");

        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .WithContent("Goodbye"));

        this._client.Cts.Cancel();
    }

    [SlashCommand("refresh", "Refreshes slash commands")]
    [SlashRequireOwner]
    public async Task CommandRefreshCommand(InteractionContext ctx)
    {
        await ctx.DeferAsync();
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .WithContent("Processing"));

        StringBuilder response = new();
        SlashCommandsExtension slasher = ctx.Client.GetSlashCommands();
        
        // actually I can do this but I need more "verbose"
        // await slasher.RefreshCommands()
        var guildRegisteredCommands = slasher.RegisteredCommands;
        
        foreach (var (key, value) in guildRegisteredCommands)
        {
            if (key == null)
            {
                Log.Logger.Warning("Refreshing slash commands in global scope");
                response.AppendLine("Refreshing slash commands in global scope");
                await ctx.Client.BulkOverwriteGlobalApplicationCommandsAsync(value);
                continue;
            }
            
            Log.Logger.Warning($"Refreshing slash commands for private guild with ID {key.GetValueOrDefault()}");
            response.AppendLine($"Refreshing slash commands for private guild with ID {key.GetValueOrDefault()}");
            await ctx.Client.BulkOverwriteGuildApplicationCommandsAsync(key.Value, value);
        }
        
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .WithContent(response.ToString()));
    }
}