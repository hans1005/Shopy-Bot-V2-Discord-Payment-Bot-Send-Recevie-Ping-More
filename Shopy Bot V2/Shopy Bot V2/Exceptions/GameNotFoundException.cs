namespace CardsAgainstDiscord.Exceptions;

public class GameNotFoundException : EmbeddableException
{
    public GameNotFoundException() : base("Game with this ID was not found in the database")
    {
    }
}