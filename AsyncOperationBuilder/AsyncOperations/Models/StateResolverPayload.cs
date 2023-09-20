using System.Collections.Generic;

namespace AsyncOperationBuilder.AsyncOperations.Models;
public class StateResolverPayload<TAsyncOperationContext, TAsyncOperationCheckResult>
{
    public TAsyncOperationContext AsyncOperationContext { get; set; }
    
    public IEnumerable<TAsyncOperationCheckResult> CheckResults { get; set; }
}