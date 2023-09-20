using System;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces;

public class StoredTypeMetadata
{
    public Guid TypeIdentifier { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is StoredTypeMetadata storedTypeMetadata)
        {
            return storedTypeMetadata.TypeIdentifier == TypeIdentifier;
        }

        return false;
    }

    public override int GetHashCode()
        => TypeIdentifier.GetHashCode();

    public override string ToString()
        => $"ObjectIdentifier = {TypeIdentifier}";
}