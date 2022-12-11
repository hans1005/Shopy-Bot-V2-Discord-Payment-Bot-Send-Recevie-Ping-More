using DSharpPlus;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using Discord.Commands;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Timers;
using SpookVooper.Api;
using SpookVooper.Api.Entities;
using DSharpPlus.Entities;
using static Discord.TimedEvents;
using static Discord.Program;
using static SpookVooper.Api.SpookVooperAPI;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using DSharpPlus.EventArgs;
using System.Net;
using System.IO;
using System.Net.Http;

namespace Discord;
public class Bot
{
    public static DiscordClient Client { get; private set; }
    public static async Task RunAsync(DiscordConfig ConfigJson)
    {
        DiscordConfiguration config = new()
        {
            Token = ConfigJson.Token,
            TokenType = TokenType.Bot,
            MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
        };

        Client = new DiscordClient(config);

        Client.Intents.AddIntent(DiscordIntents.GuildMembers);

        Client.Ready += async (sender, e) =>
        {
            Task.Run(async () =>
            {
                Console.WriteLine("CocaBot on!");
                ServiceWrapper wrapper = new();
                if (prod)
                {
                    Task.Run(async () => wrapper._ServiceWrapper(Client));
                    await SetTimer();
                }
            });
        };

        if (prod)
        {
            Client.MessageCreated += (s, e) => Task.Run(async () => HandleMessage(ConfigJson.JacobUBIKey, e));
        }

        CommandsNextConfiguration commandsConfig = new()
        {
            StringPrefixes = ConfigJson.Prefix,
            Services = new ServiceCollection().AddDbContextPool<CocaBotWebContext>((serviceProvider, options) =>
            {
                options.UseMySql(CocaBotWebContext.ConnectionString, CocaBotWebContext.version);
            }).BuildServiceProvider()
        };

        var commandsNext = Client.UseCommandsNext(commandsConfig);

        commandsNext.SetHelpFormatter<HelpFormatter>();

        commandsNext.RegisterCommands(Assembly.GetExecutingAssembly());

        await Client.ConnectAsync();
    }

    //private static async Task HandleMessageRemoveReaction(string ubiKey, MessageReactionRemoveEventArgs e)
    //{
    //    if (!prod || e.User.IsBot || e.Guild.Id != 798307000206360588 || e.Message.Author is null || e.User.Id == e.Message.Author.Id || e.Emoji.Name != "⭐") return;

    //    GetData($"https://ubi.vtech.cf/remove_star?id={e.Message.Author.Id}&key={ubiKey}");
    //}

    //private static async Task HandleMessageAddReaction(string ubiKey, MessageReactionAddEventArgs e)
    //{
    //    if (!prod || e.User.IsBot || e.Guild.Id != 798307000206360588 || e.Message.Author is null || e.User.Id == e.Message.Author.Id || e.Emoji.Name != "⭐") return;

    //    GetData($"https://ubi.vtech.cf/new_star?id={e.Message.Author.Id}&key={ubiKey}");
    //}

    private static async Task HandleMessage(string ubiKey, MessageCreateEventArgs e)
    {
        if (e.Author.IsBot) return;

        // send role data too for senator/gov pay & for district level UBI

        DiscordGuild server = await Client.GetGuildAsync(798307000206360588);

        DiscordMember member = await server.GetMemberAsync(e.Author.Id);

        await Task.Delay(10000);

        await e.Channel.GetMessageAsync(e.Message.Id);

        if (member is null)
        {
            GetData($"https://ubi.vtech.cf/new_message?id={e.Author.Id}&name={e.Author.Username}&key={ubiKey}");
            return;
        }

        string end = "";

        foreach (string rolename in member.Roles.Select(x => x.Name))
        {
            end += $"{rolename}|";
        }

        // removes the last "|" symbol
        end = end[0..^1];

        GetData($"https://ubi.vtech.cf/new_message?id={e.Author.Id}&name={e.Author.Username}&key={ubiKey}&roledata={end}");
    }
}

public class ServiceWrapper
{
    HttpClient httpclient { get; set; }

    DiscordChannel channel { get; set; }

    public async Task _ServiceWrapper(DiscordClient client)
    {
        httpclient = new HttpClient();
        channel = await client.GetChannelAsync(908560388923220018);
        while (true)
        {
            await ServerEventOccursAsync(await httpclient.GetStreamAsync(new Uri("https://nvse.vtech.cf/stream_cocabot")));
        }
    }

    private async Task ServerEventOccursAsync(Stream s)
    { 
        using (var sr = new StreamReader(s))
        {
            string message = await sr.ReadLineAsync();

            if (!message.Contains("ping"))
            {
                message = message.Replace("data: ", "");
                channel.SendMessageAsync(message);
            }

        }
    }
}