using AsyncOperationBuilder.AsyncOperations.Attributes;

namespace AsyncOperationBuilder.AsyncOperations.States.Started;

[AsyncOperationPersistedTypeIdentifier("{77DA9CE6-966B-49F5-B3F2-09C82321E6FE}")]
public class StartedOperationStateModel<TContextData>
{
    public TContextData AsyncOperationContextData { get; set; }
}