namespace CardsAgainstDiscord.Exceptions;

public class UserIsNotTheOwnerException : EmbeddableException
{
    public UserIsNotTheOwnerException() : base("Sorry, but only the game owner is allowed to do that",
        "You can create a new game with the **/create-game** command")
    {
    }
}