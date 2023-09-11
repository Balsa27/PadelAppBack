namespace PadelApp.Domain.Primitives;
public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetAtomicValues(); 
    
    //returns true if the atomic values if both of the value objs are the same while respecting the order of values of GetAtomicValues()
    private bool ValuesAreEqual(ValueObject obj) =>GetAtomicValues().SequenceEqual(obj.GetAtomicValues());
    
    public bool Equals(ValueObject? other) => other is not null && ValuesAreEqual(other);

    public override bool Equals(object? obj) => obj is ValueObject other && ValuesAreEqual(other);

    public override int GetHashCode() => GetAtomicValues()
        .Select(x => x.GetHashCode())
        .Aggregate(HashCode.Combine);
}