namespace ValourSharp;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class Command : Attribute
{
    public string[] Names { get; set; }

    public Command(params string[] names)
    {
        this.Names = names;
    }
}

//TODO
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class Group : Attribute
{
    public string[] Names { get; set; }

    public Group(params string[] names)
    {
        this.Names = names;
    }
}

//TODO
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class Priority : Attribute
{
    public int Ranking { get; set; }

    public Priority(int ranking)
    {
        this.Ranking = ranking;
    }
}

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public class Remainder : Attribute
{
    public Remainder() { }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class AllowBots : Attribute
{
    public AllowBots() { }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class AllowSelf : Attribute
{
    public AllowSelf() { }
}
