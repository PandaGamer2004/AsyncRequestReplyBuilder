using System;

namespace AsyncOperationBuilder.AsyncOperations.Attributes;

[AttributeUsage(AttributeTargets.Constructor)]
public class DependencyOnlyStateConstructor: Attribute
{
    
}