namespace CardsAgainstDiscord.Exceptions;

public class PlayerIsJudgeException : EmbeddableException
{
    public PlayerIsJudgeException() : base("You are the judge for this round", "You cannot pick white cards")
    {
    }
}