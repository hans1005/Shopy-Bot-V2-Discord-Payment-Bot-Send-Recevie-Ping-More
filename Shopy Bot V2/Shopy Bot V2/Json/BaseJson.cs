namespace Lilia.Json;

public class BaseJson
{
    // remember to override with constructor
    // read BotConfiguration.cs for more information
    protected string FilePath;
    protected string GetFilePath() => FilePath;
}