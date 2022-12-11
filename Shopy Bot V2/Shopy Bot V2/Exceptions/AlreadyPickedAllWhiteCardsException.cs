namespace CardsAgainstDiscord.Exceptions;

public class AlreadyPickedAllWhiteCardsException : EmbeddableException
{
    public AlreadyPickedAllWhiteCardsException() : base("You already picked all white cards for this round")
    {
    }
}