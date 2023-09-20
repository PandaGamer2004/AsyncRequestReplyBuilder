using System.Threading;
using System.Threading.Tasks;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces;

public interface IAsyncOperationStateMachine
{
    public Task PerformStateTransition(CancellationToken ct);
}