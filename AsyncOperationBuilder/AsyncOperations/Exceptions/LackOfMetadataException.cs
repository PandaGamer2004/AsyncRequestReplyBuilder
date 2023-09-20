using System;

namespace AsyncOperationBuilder.AsyncOperations.Exceptions;

public class LackOfMetadataException: System.Exception
{
    public Type Participant { get; set; }

    public LackOfMetadataException(string message, Type participant): base(message)
    {
        Participant = participant;
    }
}