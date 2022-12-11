using CardsAgainstDiscord.Model;

namespace CardsAgainstDiscord.Services.Contracts;

public interface IGamesService
{
    public Task<Game> CreateGameAsync(Lobby lobby);

    public Task CreateGameRoundAsync(int gameId);

    public Task<BlackCard> GetCurrentBlackCardAsync(int gameId);

    public Task<List<WhiteCard>> GetAvailableWhiteCardsAsync(int gameId, ulong userId);

    public Task<List<WhiteCard>> GetPickedWhiteCardsAsync(int gameId, ulong userId);

    /// <returns>Whether the player should pick another card</returns>
    public Task<bool> SubmitPickedCardAsync(int gameId, ulong userId, int cardId);

    public Task<(Player, string)> SubmitWinnerAsync(int gameId, ulong playerId, int winnerId);
}