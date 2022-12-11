namespace CardsAgainstDiscord.Exceptions;

public class PlayerNotFoundException : EmbeddableException
{
    public PlayerNotFoundException() : base("Player with this ID was not found in the database")
    {
    }
}