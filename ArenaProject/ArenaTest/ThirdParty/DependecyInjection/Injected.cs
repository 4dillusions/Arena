namespace ArenaTest.ThirdParty.DependecyInjection;

public class Injected : ICloneable
{
    public string? Name { get; set; }

    public readonly Guid Id;

    public Injected()
    {
        Id = Guid.NewGuid();
    }

    public object Clone()
    {
        return new Injected() { Name = Name };
    }

    public override bool Equals(object? obj)
    {
        var other = obj as Injected;
        if (other == null)
            return false;

        return Equals(other);
    }

    protected bool Equals(Injected other)
    {
        return Id.Equals(other.Id) && Name == other.Name;
    }

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode()
    {
        unchecked
        {
            return (Id.GetHashCode() * 397) ^ Name!.GetHashCode();
        }
    }
}