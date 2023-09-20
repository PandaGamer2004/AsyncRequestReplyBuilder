using System.Collections.Generic;

namespace AsyncOperationBuilder.AsyncOperations.Exceptions;

public class IncompleteAsyncOperationBuildException: System.Exception
{
    public IEnumerable<string> MissedSteps { get; }
    
    public IncompleteAsyncOperationBuildException(string errorMessage, IEnumerable<string> missedSteps)
    {
        MissedSteps = missedSteps; 
    }
    
}