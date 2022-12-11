using Valour.Api.Client;
using static ValourSharp.CommandHandler;

namespace ValourSharp;

public static class Start
{
    public static string[] Prefixes = null;
    public static async Task Initialize(string email, string password, params string[] prefixes)
    {
        Prefixes = prefixes;

        await ValourClient.InitializeBot(email, password);

        ValourClient.OnMessageRecieved += async (message) =>
            await MessageHandler(message);
    }
}
