namespace PadelApp.Domain.Primitives;
public abstract class Entity : IEquatable<Entity>
{
    //init - value set once when the object is initialized
    public Guid Id { get; init; } = Guid.NewGuid();

    public static bool operator ==(Entity? first, Entity? second)
    {
        return first is not null && second is not null && first.Equals(second); //Object Equals method used
    }

    public static bool operator !=(Entity? first, Entity? second)
    {
        return !(first == second);
    }
    public bool Equals(Entity? other)
    {
        if (other is null)
            return false;
        if (other.GetType() != GetType())
            return false;
        return other.Id == Id;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != GetType())
            return false;
        if (obj is not Entity entity) 
            return false;
        return entity.Id == Id;
    }
    
    public override int GetHashCode() => Id.GetHashCode();
}