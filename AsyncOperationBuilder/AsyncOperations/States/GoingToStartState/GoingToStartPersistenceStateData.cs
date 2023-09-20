using AsyncOperationBuilder.AsyncOperations.Attributes;

namespace AsyncOperationBuilder.AsyncOperations.States.GoingToStartState;

[AsyncOperationPersistedTypeIdentifier("{D5A21D5D-D36D-4404-B823-F1DDF4F7AB8F}")]
public class GoingToStartPersistenceStateData<TOperationStartData>
{
    public TOperationStartData InitialData { get; set; }
}