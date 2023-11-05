using System.Collections.Generic;

namespace Wallet.Api.Infrastructure.Http;

public class Field<T>
{
    public Field()
    {
    }

    public Field(T value) => Value = value;

    public T Value { get; set; }

    public static implicit operator Field<T>(T value) => new(value);

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        return this == obj || obj.GetType() == GetType() && EqualityComparer<T>.Default.Equals(Value, ((Field<T>)obj).Value);
    }

    public override int GetHashCode() => Value.GetHashCode();
}