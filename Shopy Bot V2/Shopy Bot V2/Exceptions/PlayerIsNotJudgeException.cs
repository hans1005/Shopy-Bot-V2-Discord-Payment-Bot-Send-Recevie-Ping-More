namespace CardsAgainstDiscord.Exceptions;

public class PlayerIsNotJudgeException : EmbeddableException
{
    public PlayerIsNotJudgeException() : base("You can do this only as the judge")

    {
    }
}