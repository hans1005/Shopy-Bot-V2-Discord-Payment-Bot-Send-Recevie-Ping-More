namespace CardsAgainstDiscord.Exceptions;

public class EmbeddableException : ApplicationException
{
    protected EmbeddableException(string title, string? description = null)
    {
        Title = title;
        Description = description;
    }

    public string Title { get; }

    public string? Description { get; }
}