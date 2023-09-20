using System;

namespace AsyncOperationBuilder.AsyncOperations.Exceptions;

public class AsyncStoredItemTypeMismatch: System.Exception
{

    public Type TypeRequestedToDeserialize { get; set; }
    
    public AsyncStoredItemTypeMismatch(string errorMessage, Type typeRequestedToDeserialize):base(errorMessage)
    {
        TypeRequestedToDeserialize = typeRequestedToDeserialize;
    }
}