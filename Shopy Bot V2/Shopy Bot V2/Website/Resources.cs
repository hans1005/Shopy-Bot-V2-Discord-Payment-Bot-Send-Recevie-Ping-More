using System;

public class Resources
{
    public enum Resource
    {
        Crystallite = 0,
        Platinum = 1,
        Gold = 2,
        Steel = 3,
        Textile = 4,
        Food = 5,
        Fuel = 6,
        Credits = 7
    }

    public static string ResourceToString(Resource resource)
    {
        return resource switch
        {
            Resource.Crystallite => nameof(Resource.Crystallite),
            Resource.Platinum => nameof(Resource.Platinum),
            Resource.Gold => nameof(Resource.Gold),
            Resource.Steel => nameof(Resource.Steel),
            Resource.Textile => nameof(Resource.Textile),
            Resource.Food => nameof(Resource.Food),
            Resource.Fuel => nameof(Resource.Fuel),
            Resource.Credits => nameof(Resource.Credits),
            _ => throw new ArgumentOutOfRangeException(nameof(resource), resource, null)
        };
    }

    public static Resource? StringToResource(string resource)
    {
        if (Enum.TryParse(resource, true, out Resource resourceValue)) return resourceValue;

        switch (resource.ToLower())
        {
            case "crystal" or "cry":
                return Resource.Crystallite;
            case "plat" or "p":
                return Resource.Platinum;
            case "g":
                return Resource.Gold;
            case "s":
                return Resource.Steel;
            case "text" or "t":
                return Resource.Textile;
            case "fo":
                return Resource.Food;
            case "fu":
                return Resource.Fuel;
            case "cre":
                return Resource.Credits;
            default:
                return null;
        }
    }
}