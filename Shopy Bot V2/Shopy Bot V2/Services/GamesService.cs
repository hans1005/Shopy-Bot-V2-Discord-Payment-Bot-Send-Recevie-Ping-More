using CardsAgainstDiscord.Data;
using CardsAgainstDiscord.Discord;
using CardsAgainstDiscord.Exceptions;
using CardsAgainstDiscord.Extensions;
using CardsAgainstDiscord.Model;
using CardsAgainstDiscord.Services.Contracts;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Game = CardsAgainstDiscord.Model.Game;

namespace CardsAgainstDiscord.Services;

public class GamesService : IGamesService
{
    /// <summary>
    ///     Number of white cards in each player's hand
    /// </summary>
    private const int HandSize = 8;

    private readonly DiscordSocketClient _client;

    private readonly IDbContextFactory<CardsDbContext> _factory;

    public GamesService(DiscordSocketClient client, IDbContextFactory<CardsDbContext> factory)
    {
        _client = client;
        _factory = factory;
    }

    public async Task<Game> CreateGameAsync(Lobby lobby)
    {
        await using var context = await _factory.CreateDbContextAsync();

        var game = new Game
        {
            GuildId = lobby.GuildId,
            ChannelId = lobby.ChannelId,
            Players = lobby.JoinedPlayers.Select(id => new Player {UserId = id}).ToList()
        };

        context.Games.Add(game);
        context.Lobbies.Remove(lobby);

        await context.SaveChangesAsync();
        await CreateGameRoundAsync(game.Id);

        return game;
    }

    public async Task<BlackCard> GetCurrentBlackCardAsync(int gameId)
    {
        // await using var context = await _factory.CreateDbContextAsync();
        //
        // var round = await context.Rounds
        //     .Include(r => r.BlackCard)
        //     .FirstOrDefaultAsync(r => r.GameId == gameId) ?? throw new GameNotFoundException();
        //
        // return round.BlackCard;
        return null!;
    }

    public async Task<List<WhiteCard>> GetAvailableWhiteCardsAsync(int gameId, ulong userId)
    {
        // await using var context = await _factory.CreateDbContextAsync();
        //
        // var round = await context.Rounds
        //                 .Include(r => r.BlackCard)
        //                 .FirstOrDefaultAsync(r => r.GameId == gameId)
        //             ?? throw new GameNotFoundException();
        //
        // var player = await context.Players
        //                  .Include(p => p.WhiteCards)
        //                  .Include(p => p.PickedCards)
        //                  .FirstOrDefaultAsync(p => p.GameId == gameId && p.UserId == userId)
        //              ?? throw new PlayerNotFoundException();
        //
        // // Do not allow judge to pick white cards
        // if (round.JudgeId == player.Id) throw new PlayerIsJudgeException();
        //
        // // Do not allow users to pick more than <black card picks> cards
        // if (player.PickedCards.Count(p => p.RoundId == round.Id) >= round.BlackCard.Picks)
        //     throw new AlreadyPickedAllWhiteCardsException();
        //
        // return player.WhiteCards;
        return null!;
    }

    public async Task<List<WhiteCard>> GetPickedWhiteCardsAsync(int gameId, ulong userId)
    {
        // await using var context = await _factory.CreateDbContextAsync();
        //
        // var player = await context.Players
        //                  .Include(p => p.PickedCards)
        //                  .ThenInclude(p => p.WhiteCard)
        //                  .FirstOrDefaultAsync(p => p.GameId == gameId && p.UserId == userId)
        //              ?? throw new PlayerNotFoundException();
        //
        // return player.PickedCards.Select(p => p.WhiteCard).ToList();
        return null!;
    }

    public async Task<bool> SubmitPickedCardAsync(int gameId, ulong playerId, int cardId)
    {
        // await using var context = await _factory.CreateDbContextAsync();
        //
        // var round = await context.Rounds
        //     .Include(r => r.BlackCard)
        //     .FirstOrDefaultAsync(r => r.GameId == gameId) ?? throw new GameNotFoundException();
        //
        // var player = await context.Players
        //                  .Include(p => p.PickedCards)
        //                  .Include(p => p.WhiteCards)
        //                  .FirstOrDefaultAsync(p => p.GameId == gameId && p.UserId == playerId) ??
        //              throw new PlayerNotFoundException();
        //
        // if (player.PickedCards.Count(p => p.RoundId == round.Id) >= round.BlackCard.Picks)
        //     throw new AlreadyPickedAllWhiteCardsException();
        //
        // var card = player.WhiteCards.First(c => c.Id == cardId);
        // var pick = new PickedCard
        // {
        //     RoundId = round.Id,
        //     PlayerId = player.Id,
        //     WhiteCardId = cardId
        // };
        //
        // round.PickedCards.Add(pick);
        // player.WhiteCards.Remove(card);
        //
        // context.Rounds.Update(round);
        // context.Players.Update(player);
        //
        // await context.SaveChangesAsync();
        // await UpdateGameRoundEmbedAsync(gameId);
        //
        //
        // // If the player needs to pick more cards, early return true
        // if (round.PickedCards.Count(r => r.PlayerId == player.Id) < round.BlackCard.Picks) return true;
        //
        // // Check, whether all players have submitted all picks
        // // There has to be (player - 1 judge) * black picks cards picked
        // var players = await context.Players.CountAsync(p => p.GameId == gameId);
        //
        // if (round.PickedCards.Count == (players - 1) * round.BlackCard.Picks)
        // {
        //     // List all submitted cards in a random order for the judge to pick and delete the original message
        //     await UpdateGameRoundEmbedAsync(gameId);
        //     await SendJudgeSelectionEmbedAsync(gameId);
        // }
        //
        return false;
    }

    public async Task<(Player, string)> SubmitWinnerAsync(int gameId, ulong playerId, int winnerId)
    {
        // await using var context = await _factory.CreateDbContextAsync();
        //
        // var game = await context.Games
        //     .Include(g => g.Players)
        //     .Include(g => g.CurrentRound).ThenInclude(p => p.Judge)
        //     .Include(g => g.CurrentRound).ThenInclude(p => p.BlackCard)
        //     .Include(g => g.CurrentRound)
        //     .ThenInclude(p => p.PickedCards)
        //     .ThenInclude(c => c.WhiteCard)
        //     .FirstOrDefaultAsync(g => g.Id == gameId) ?? throw new GameNotFoundException();
        //
        // if (playerId != game.CurrentRound.Judge.UserId) throw new PlayerIsNotJudgeException();
        //
        // var winner = game.Players.FirstOrDefault(p => p.Id == winnerId)
        //              ?? throw new PlayerNotFoundException();
        //
        // var cards = game.CurrentRound.PickedCards.Where(p => p.PlayerId == winnerId)
        //     .Select(c => c.WhiteCard.Text)
        //     .ToList();
        //
        // var submission = game.CurrentRound.BlackCard.Text.FormatBlackCard(cards);
        //
        // // TODO: Update score
        //
        // await CreateGameRound(game);
        //
        // return (winner, submission);
        return (null!, null!);
    }

    public async Task CreateGameRoundAsync(int gameId)
    {
        await using var context = await _factory.CreateDbContextAsync();

        // Move the judge role to the next player
        var game = await context.Games.Where(g => g.Id == gameId)
            .Include(g => g.UsedBlackCards)
            .Include(g => g.UsedWhiteCards)
            .Include(g => g.PickedCards).ThenInclude(p => p.WhiteCard)
            .Include(g => g.Players).ThenInclude(p => p.WhiteCards)
            .FirstOrDefaultAsync() ?? throw new GameNotFoundException();

        var players = game.Players;
        var previousJudge = players.FindIndex(p => p.Id == game.JudgeId);
        var nextJudge = players[(previousJudge + 1) % players.Count];

        // If there is a black card from the previous round, add it to the used black cards collection
        if (game.BlackCard != null)
        {
            game.UsedBlackCards.Add(game.BlackCard);
        }

        // Similarly, if there are submitted white cards from the previous round, add the to the used white cards collection
        if (game.PickedCards.Any())
        {
            game.UsedWhiteCards.AddRange(game.PickedCards.Select(p => p.WhiteCard));
            game.PickedCards.Clear();
        }

        // Select a card randomly from all black cards that have not been used yet in this game
        var random = new Random();
        var selectedBlackCard = context.BlackCards.Where(c => !game.UsedBlackCards.Contains(c))
            .ToList() // Needed in order to change the context to local evaluation
            .OrderBy(_ => random.Next())
            .First();

        var guild = _client.GetGuild(game.GuildId);
        var channel = guild.GetTextChannel(game.ChannelId);
        var message = await channel.SendMessageAsync("Starting a new round...");

        game.JudgeId = nextJudge.Id;
        game.BlackCardId = selectedBlackCard.Id;
        game.MessageId = message.Id;

        // Add missing white cards to each player from pool of white cards that have not been used yet
        var availableWhiteCards = context.WhiteCards.Where(c => !game.UsedWhiteCards.Contains(c))
            .ToList() // Needed in order to change the context to local evaluation
            .OrderBy(_ => random.Next())
            .ToList();

        // TODO: Maybe write this in a cleaner, more functional approach?
        var takenCards = 0;

        foreach (var player in game.Players)
        {
            var missingCards = HandSize - player.WhiteCards.Count;

            player.WhiteCards.AddRange(availableWhiteCards.Skip(takenCards).Take(missingCards));

            takenCards += missingCards;
        }

        context.Games.Update(game);

        await context.SaveChangesAsync();
        await UpdateGameRoundEmbedAsync(game);
    }

    private async Task UpdateGameRoundEmbedAsync(int gameId)
    {
        await using var context = await _factory.CreateDbContextAsync();

        var game = await context.Games
            .Include(g => g.Judge)
            .Include(g => g.BlackCard)
            .Include(g => g.Players).ThenInclude(p => p.PickedCards)
            .FirstOrDefaultAsync(g => g.Id == gameId) ?? throw new GameNotFoundException();

        await UpdateGameRoundEmbedAsync(game);
    }

    private async Task UpdateGameRoundEmbedAsync(Game game)
    {
        var guild = _client.GetGuild(game.GuildId);
        var channel = guild.GetTextChannel(game.ChannelId);

        if (await channel.GetMessageAsync(game.MessageId ?? 0) is not IUserMessage message) return;

        var text = game.BlackCard!.FormattedText;
        var picks = game.BlackCard.Picks;
        var judge = game.Judge!.UserId;
        var players = game.Players.Where(p => p.Id != game.JudgeId)
            .Select(p => p.PickedCards.Count < picks
                ? $"⏳ {p.UserId.AsUserMention()}"
                : $"✅ {p.UserId.AsUserMention()}"
            )
            .OrderBy(p => p)
            .ToList();

        var embed = EmbedBuilders.GameRoundEmbed(text, judge, players);
        var components = new ComponentBuilder().WithButton(
            ButtonBuilder.CreatePrimaryButton(
                "Pick your card" + (picks > 1 ? "s" : string.Empty),
                $"game:pick:{game.Id}"
            )
        ).Build();

        await message.ModifyAsync(m =>
        {
            m.Content = "";
            m.Embed = embed;
            m.Components = components;
        });
    }

    private async Task SendJudgeSelectionEmbedAsync(int gameId)
    {
        // await using var context = await _factory.CreateDbContextAsync();
        //
        // var game = await context.Games
        //     .Include(g => g.CurrentRound).ThenInclude(r => r!.Judge)
        //     .Include(g => g.CurrentRound).ThenInclude(r => r!.BlackCard)
        //     .Include(g => g.CurrentRound)
        //     .ThenInclude(r => r!.PickedCards)
        //     .ThenInclude(p => p.WhiteCard)
        //     .FirstOrDefaultAsync(g => g.Id == gameId) ?? throw new GameNotFoundException();
        //
        // var guild = _client.GetGuild(game.GuildId);
        // var channel = guild.GetTextChannel(game.ChannelId);
        //
        // if (await channel.GetMessageAsync(game.CurrentRound!.MessageId) is not IUserMessage message) return;
        //
        // var blackCard = game.CurrentRound.BlackCard.Text;
        //
        // var random = new Random();
        // var submissions = game.CurrentRound.PickedCards
        //     .GroupBy(c => c.PlayerId)
        //     .OrderBy(_ => random.Next())
        //     .ToList();
        //
        // var texts = submissions.Select((cards, i) =>
        //     $"**{i}.** " + blackCard
        //         .FormatBlackCard(cards.Select(c => c.WhiteCard.Text)
        //             .ToList())
        // ).ToList();
        //
        // var embed = new EmbedBuilder()
        //     .WithTitle("Waiting for the judge to select his favorite submission")
        //     .WithDescription("To make the choice, use the button below this message")
        //     .WithColor(DiscordConstants.ColorPrimary)
        //     .WithCurrentTimestamp()
        //     .AddField("Judge", $"<@!{game.CurrentRound!.Judge.UserId}>")
        //     .AddField("Submissions", string.Join("\n\n", texts))
        //     .Build();
        //
        // var components = new ComponentBuilder()
        //     .WithSelectMenu(
        //         $"game:judge:{game.CurrentRound.Id}",
        //         submissions.Select((t, i) => new SelectMenuOptionBuilder
        //         {
        //             Label = $"{i + 1}. " + string.Join(", ", t.Select(c => c.WhiteCard.Text)),
        //             Value = t.Key.ToString()
        //         }).ToList()
        //     )
        //     .Build();
        //
        // await message.DeleteAsync();
        // await channel.SendMessageAsync(
        //     embed: embed,
        //     components: components
        // );
    }
}