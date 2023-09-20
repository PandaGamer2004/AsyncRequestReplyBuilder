using System;

namespace AsyncOperationBuilder.AsyncOperations.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AsyncOperationPersistedTypeIdentifier: Attribute
{
    public Guid TypeIdentifier { get; set; }
    
    public AsyncOperationPersistedTypeIdentifier(string typeIdentifier)
    {
        TypeIdentifier = Guid.Parse(typeIdentifier);
    }
}