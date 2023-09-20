using System.Collections.Generic;
using AsyncOperationBuilder.AsyncOperations.Attributes;

namespace AsyncOperationBuilder.AsyncOperations.States.InProgressState;

[AsyncOperationPersistedTypeIdentifier("{B9D0C59E-B277-4398-8E03-34B153F03E34}")]
public class InProgressStatePersistenceModel<TOperationContext, TOperationResult>
{
    public TOperationContext ContextData { get; set; }

    public IEnumerable<TOperationResult> StatusCheckResults { get; set; }
}