namespace CardsAgainstDiscord.Exceptions;

public class TooFewPlayersException : EmbeddableException
{
    public TooFewPlayersException() : base("There need to be at least 2 players joined")
    {
    }
}