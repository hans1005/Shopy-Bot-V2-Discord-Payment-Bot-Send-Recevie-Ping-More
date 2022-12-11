using System.Reflection;
using System.ComponentModel;
using Valour.Shared.Items.Messages.Mentions;
using Valour.Api.Client;
using Valour.Api.Items.Users;
using Valour.Api.Items.Messages;
using Valour.Api.Items.Planets.Members;
using Valour.Api.Items.Planets.Channels;
using System.Collections.Concurrent;
using static ValourSharp.Start;

namespace ValourSharp;

public static class CommandHandler
{
    public static ConcurrentDictionary<string, CommandInfo> Commands = new(StringComparer.InvariantCultureIgnoreCase);
    public static async Task MessageHandler(PlanetMessage ctx)
    {
        var sender = await (await ctx.GetAuthorAsync()).GetUserAsync();

        var matches = Prefixes.Where(prefix => ctx.Content.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase));

        if (!matches.Any()) return;

        int prefixLength = matches.Max().Length;

        List<string> stringArgs = ctx.Content[prefixLength..].Split(null).ToList();

        string commandname = stringArgs[0].ToLowerInvariant();
        stringArgs.RemoveAt(0);

        bool isCommand = Commands.TryGetValue(commandname, out CommandInfo command);

        if (!isCommand ||
           command.Parameters.SkipWhile(x => x.ParameterType == typeof(PlanetMessage)).Count() != stringArgs.Count ||
           (sender.Bot && command.AllowBots) ||
           (ValourClient.Self == sender && command.AllowSelf)) return;

        List<object> args = new(command.Parameters.Length);

        int length = prefixLength + commandname.Length + 1;
        List<Mention> mentions = ctx.Mentions;
        int contexts = 0;
        for (int i = 0; i < command.Parameters.Length; i++)
        {
            ParameterInfo parameter = command.Parameters[i];

            if (parameter.ParameterType == typeof(PlanetMessage))
            {
                contexts++;
                args.Add(ctx);
                continue;
            }

            var position = i - contexts;
            string rawargs = stringArgs[position];

            if (parameter.ParameterType == typeof(string) && parameter.GetCustomAttribute(typeof(Remainder), false) is not null)
            {
                args.Add(string.Join(' ', stringArgs.GetRange(position, stringArgs.Count - position)));
            }
            else if (IsValourType(parameter))
            {
                var mention = mentions.FirstOrDefault();

                if (mention is null || mention.Position != length + 3) continue;

                mentions.RemoveAt(0);

                await mention.ConvertMention(args, parameter);
            }
            else
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(parameter.ParameterType);
                args.Add(typeConverter.ConvertFrom(rawargs));
            }

            length += rawargs.Length + 1;
        }

        command.Method.Invoke(null, args.ToArray());
    }

    private static bool IsValourType(ParameterInfo parameter) => 
        parameter.ParameterType == typeof(User) || parameter.ParameterType == typeof(PlanetMember) || parameter.ParameterType == typeof(PlanetCategory) || parameter.ParameterType == typeof(PlanetChatChannel) || parameter.ParameterType == typeof(PlanetRole);

    private static async Task ConvertMention(this Mention mention, List<object> args, ParameterInfo parameter)
    {
        switch (mention.Type)
        {
            //User
            case MentionType.Member when parameter.ParameterType == typeof(User):
                args.Add(await (await PlanetMember.FindAsync(mention.Target_Id)).GetUserAsync());
                break;
            //Member
            case MentionType.Member when parameter.ParameterType == typeof(PlanetMember):
                args.Add(await PlanetMember.FindAsync(mention.Target_Id));
                break;
            case MentionType.Category when parameter.ParameterType == typeof(PlanetCategory):
                args.Add(await PlanetCategory.FindAsync(mention.Target_Id));
                break;
            case MentionType.Channel when parameter.ParameterType == typeof(PlanetChatChannel):
                args.Add(await PlanetChatChannel.FindAsync(mention.Target_Id));
                break;
            case MentionType.Role when parameter.ParameterType == typeof(PlanetRole):
                args.Add(await PlanetRole.FindAsync(mention.Target_Id));
                break;
        }
    }
}
