using CardsAgainstDiscord.Model;

namespace CardsAgainstDiscord.Services.Contracts;

public interface ILobbiesService
{
    public Task<Lobby> CreateLobbyAsync(ulong guildId, ulong channelId, ulong messageId, ulong ownerId);

    public Task ToggleJoinLobbyAsync(int lobbyId, ulong userId);

    public Task CancelLobbyAsync(int lobbyId, ulong userId);

    public Task StartGameAsync(int lobbyId, ulong userId);
}