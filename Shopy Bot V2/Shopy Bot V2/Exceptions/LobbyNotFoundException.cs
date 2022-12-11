namespace CardsAgainstDiscord.Exceptions;

public class LobbyNotFoundException : EmbeddableException
{
    public LobbyNotFoundException() : base("Sorry, but this game no longer exists.",
        "You can create a new game with the **/create-game** command")
    {
    }
}