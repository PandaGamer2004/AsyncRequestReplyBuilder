using System;

namespace AsyncOperationBuilder.AsyncOperations.Serialization.Models;

public class TypeAwareSerializedModel
{
    //It can have inside some complex rules etc. Now we isolated of which content are actually inside
    public string SerializedContent { get; set; }
    
    public Guid TypeIdentifier { get; set; }
}